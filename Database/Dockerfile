FROM mysql:latest

WORKDIR /usr/lib/mysql/scripts
COPY ./bin/initialize_database.sql .

ENTRYPOINT [ ]
CMD /usr/local/bin/docker-entrypoint.sh mysqld