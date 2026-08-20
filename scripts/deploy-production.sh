#!/usr/bin/env bash

set -Eeuo pipefail

cd /opt/wow-paper-trader

compose=(
    docker compose
    --env-file .env.production
    -f compose.yml
    -f compose.production.yml
)

echo "Pulling production images..."
"${compose[@]}" pull

echo "Starting PostgreSQL..."
"${compose[@]}" up -d postgres

echo "Stopping the API before changing the schema..."
"${compose[@]}" stop api

echo "Applying database migrations..."
"${compose[@]}" run \
    --rm \
    --interactive=false \
    --no-tty \
    migrator

echo "Starting the updated API and Caddy..."
"${compose[@]}" up -d api caddy

echo "Deployment complete."
"${compose[@]}" ps