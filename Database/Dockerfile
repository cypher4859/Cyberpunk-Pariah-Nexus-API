FROM mysql:latest

WORKDIR /docker-entrypoint-initdb.d
COPY ./bin/initialize_database.sql .
COPY ./bin/mysql.conf /etc/mysql/conf.d/mysql.cnf

RUN microdnf install nc

ENTRYPOINT [ ]
CMD [ "bash", "-c", "/usr/local/bin/docker-entrypoint.sh mysqld & \
    until nc -zv localhost 3306; do \
        echo 'waiting for mysqld to be ready...'; \
        sleep 5; \
    done && \
    mysql -u root --password=arasakaOperator123 -h localhost --protocol=TCP < /docker-entrypoint-initdb.d/initialize_database.sql && \
    tail -f /dev/null"]
