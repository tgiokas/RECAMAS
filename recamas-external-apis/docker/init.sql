SELECT 'CREATE DATABASE recamas_ars' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'recamas_ars')\gexec
SELECT 'CREATE DATABASE recamas_cass' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'recamas_cass')\gexec
SELECT 'CREATE DATABASE recamas_arrivals' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'recamas_arrivals')\gexec
SELECT 'CREATE DATABASE recamas_stoplist' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'recamas_stoplist')\gexec
