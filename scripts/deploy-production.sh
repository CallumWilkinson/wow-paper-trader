#!/usr/bin/env bash

set -Eeuo pipefail

cd /opt/wow-paper-trader

echo "Waiting for job lock..."
exec 9>/opt/wow-paper-trader/.job.lock
flock 9
echo "Job lock acquired."

compose=(
    docker compose
    --env-file .env.production
    -f compose.yml
    -f compose.production.yml
)

echo "Validating Docker Compose configuration..."
"${compose[@]}" config --quiet

# If Caddy is already running, validate the newly pulled Caddyfile
# before changing the application.
if "${compose[@]}" ps --status running --services | grep -qx caddy; then
    echo "Validating Caddy configuration..."

    "${compose[@]}" exec -T caddy \
        caddy validate \
        --config /etc/caddy/Caddyfile \
        --adapter caddyfile
fi

echo "Pulling application images..."
"${compose[@]}" pull \
    api \
    migrator \
    ingestor

echo "Starting PostgreSQL..."
"${compose[@]}" up -d --wait postgres

echo "Stopping the API before changing the schema..."
"${compose[@]}" stop api

echo "Applying database migrations..."
"${compose[@]}" run \
    --rm \
    --interactive=false \
    --no-tty \
    migrator

echo "Starting updated API and Caddy..."
"${compose[@]}" up \
    -d \
    --wait \
    api \
    caddy

echo "Reloading Caddy configuration..."
"${compose[@]}" exec -T caddy \
    caddy reload \
    --config /etc/caddy/Caddyfile \
    --adapter caddyfile

echo "Deployment complete."
"${compose[@]}" ps