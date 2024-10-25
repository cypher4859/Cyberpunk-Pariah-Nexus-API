FROM mysql:latest

WORKDIR /docker-entrypoint-initdb.d

# Copy the downloaded files into the image
COPY initialize_database.sql ./initialize_database.sql

# Install netcat for port checking
RUN microdnf install -y nc
RUN microdnf clean all

ENTRYPOINT [ ]
CMD [ "bash", "-c", "/usr/local/bin/docker-entrypoint.sh mysqld & \
    until nc -zv localhost 3306; do \
        echo 'waiting for mysqld to be ready...'; \
        sleep 5; \
    done && \
    tail -f /dev/null"]
