#!/usr/bin/env bash

set -Eeuo pipefail

: "${POSTGRES_DB:?POSTGRES_DB is required}"
: "${POSTGRES_USER:?POSTGRES_USER is required}"
: "${API_DB_PASSWORD:?API_DB_PASSWORD is required}"
: "${INGESTOR_DB_PASSWORD:?INGESTOR_DB_PASSWORD is required}"

if [[ "$POSTGRES_USER" != "postgres_admin" ]]; then
    echo "POSTGRES_USER must be postgres_admin."
    exit 1
fi

psql \
    --username "$POSTGRES_USER" \
    --dbname "$POSTGRES_DB" \
    --set=ON_ERROR_STOP=1 \
    --set=database_name="$POSTGRES_DB" \
    --set=api_password="$API_DB_PASSWORD" \
    --set=ingestor_password="$INGESTOR_DB_PASSWORD" <<'SQL'

-- Create the API role if it does not already exist.

SELECT 'CREATE ROLE wow_api_user'
WHERE NOT EXISTS (
    SELECT 1
    FROM pg_roles
    WHERE rolname = 'wow_api_user'
)
\gexec

-- Create the ingestor role if it does not already exist.

SELECT 'CREATE ROLE wow_ingestor_user'
WHERE NOT EXISTS (
    SELECT 1
    FROM pg_roles
    WHERE rolname = 'wow_ingestor_user'
)
\gexec


-- Configure the API user.

ALTER ROLE wow_api_user
WITH
    LOGIN
    PASSWORD :'api_password'
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOREPLICATION
    NOBYPASSRLS;


-- Configure the ingestor user.

ALTER ROLE wow_ingestor_user
WITH
    LOGIN
    PASSWORD :'ingestor_password'
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOREPLICATION
    NOBYPASSRLS;


-- Allow both application users to connect.

GRANT CONNECT
ON DATABASE :"database_name"
TO wow_api_user, wow_ingestor_user;


-- Stop ordinary users from creating objects in the public schema.

REVOKE CREATE
ON SCHEMA public
FROM PUBLIC;


-- Both users need to access objects inside the public schema.

GRANT USAGE
ON SCHEMA public
TO wow_api_user, wow_ingestor_user;


-- Remove any existing table permissions before applying the intended model.

REVOKE ALL PRIVILEGES
ON ALL TABLES IN SCHEMA public
FROM wow_api_user, wow_ingestor_user;

REVOKE ALL PRIVILEGES
ON ALL SEQUENCES IN SCHEMA public
FROM wow_api_user, wow_ingestor_user;


-- API: read-only access.

GRANT SELECT
ON ALL TABLES IN SCHEMA public
TO wow_api_user;


-- Ingestor: data read/write access.

GRANT SELECT, INSERT, UPDATE, DELETE
ON ALL TABLES IN SCHEMA public
TO wow_ingestor_user;


-- Required when inserted IDs use PostgreSQL sequences.

GRANT USAGE, SELECT, UPDATE
ON ALL SEQUENCES IN SCHEMA public
TO wow_ingestor_user;


-- Configure permissions for future tables created by postgres_admin.
--
-- This works because migrations will also run as postgres_admin.

ALTER DEFAULT PRIVILEGES
IN SCHEMA public
GRANT SELECT
ON TABLES
TO wow_api_user;

ALTER DEFAULT PRIVILEGES
IN SCHEMA public
GRANT SELECT, INSERT, UPDATE, DELETE
ON TABLES
TO wow_ingestor_user;

ALTER DEFAULT PRIVILEGES
IN SCHEMA public
GRANT USAGE, SELECT, UPDATE
ON SEQUENCES
TO wow_ingestor_user;

SQL

echo "PostgreSQL application roles configured successfully."