#!/bin/sh
set -eu

for database_name in $DATABASES; do
  exists="$(psql -h postgres -U "$POSTGRES_USER" -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='$database_name'")"
  if [ "$exists" != "1" ]; then
    psql -h postgres -U "$POSTGRES_USER" -d postgres -v ON_ERROR_STOP=1 -c "CREATE DATABASE \"$database_name\""
  fi
done
