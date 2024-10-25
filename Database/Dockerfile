FROM mysql:latest

# Set up environment variables for AWS, these can be replaced with build secrets


# Optionally expose these as environment variables for the build process
ARG TARGET_S3_BUCKET_NAME=blackcypher-ops-bucket
ARG AWS_ACCESS_KEY_ID
ARG AWS_SECRET_ACCESS_KEY
ARG AWS_DEFAULT_REGION

ENV TARGET_S3_BUCKET_NAME=$TARGET_S3_BUCKET_NAME
ENV AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID
ENV AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY
ENV AWS_DEFAULT_REGION=$AWS_DEFAULT_REGION
# Install the necessary tools (curl, unzip) and install AWS CLI using microdnf
RUN microdnf install -y curl unzip nc && \
    curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
    unzip awscliv2.zip && \
    ./aws/install && \
    rm -rf awscliv2.zip aws

WORKDIR /docker-entrypoint-initdb.d
RUN aws s3 cp s3://${TARGET_S3_BUCKET_NAME}/projects/cyberpunk/pariah-nexus-api/initialization_data/initialize_database.sql ./initialize_database.sql
RUN aws s3 cp s3://${TARGET_S3_BUCKET_NAME}/projects/cyberpunk/pariah-nexus-api/initialization_data/mysql.conf /etc/mysql/conf.d/mysql.cnf


ENTRYPOINT [ ]
CMD [ "bash", "-c", "/usr/local/bin/docker-entrypoint.sh mysqld & \
    until nc -zv localhost 3306; do \
        echo 'waiting for mysqld to be ready...'; \
        sleep 5; \
    done && \
    mysql -u root --password=arasakaOperator123 -h localhost --protocol=TCP < /docker-entrypoint-initdb.d/initialize_database.sql && \
    tail -f /dev/null"]
