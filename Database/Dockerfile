FROM mysql:latest

# Set up environment variables for AWS, these can be replaced with build secrets
ARG TARGET_S3_BUCKET_NAME

# Optionally expose these as environment variables for the build process
ENV TARGET_S3_BUCKET_NAME=$TARGET_S3_BUCKET_NAME

WORKDIR /docker-entrypoint-initdb.d
RUN aws s3 cp s3://${TARGET_S3_BUCKET_NAME}/projects/cyberpunk/pariah-nexus-api/initialization_data/initialize_database.sql ./initialize_database.sql
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
