## Documentation for Dockerfiles and Docker Compose in CyberpunkPariahNexusApi

This document provides an overview and explanation of the **Dockerfiles** and **Docker Compose** configuration used in the `CyberpunkPariahNexusApi` project. These configurations are integral for building, running, and testing the application in containerized environments, enabling a robust DevOps workflow.

### 1. Dockerfile for `pariah-nexus-api`

The **Dockerfile** defines the steps needed to build and run the **`pariah-nexus-api`**. It uses multi-stage builds to create a production-ready Docker image.

#### Breakdown of Stages
1. **Build Stage**
   - **Base Image**: Uses the `.NET SDK 8.0` image (`mcr.microsoft.com/dotnet/sdk:8.0`) to build the application.
   - **Working Directory**: Sets the working directory to `/source`.
   - **Expose Port**: Exposes **port 8080** for the application.
   - **Copy and Restore**: Copies the API project files (`CyberpunkPariahNexusApi/`) and runs `dotnet restore` to restore dependencies. Using **distinct layers** improves caching and speeds up subsequent builds.

2. **Publish Stage**
   - **Base Image**: Continues from the build stage.
   - **Working Directory**: Moves into the project directory.
   - **Publish Application**: Runs `dotnet publish` to publish the application without restoring again (`--no-restore`). The output is directed to `/app`.

3. **Runtime Stage**
   - **Base Image**: Uses the `.NET ASP.NET runtime 8.0` image (`mcr.microsoft.com/dotnet/aspnet:8.0`) to keep the final container lightweight.
   - **Working Directory**: Sets the working directory to `/app`.
   - **Copy Published Application**: Copies the published output from the **publish stage**.
   - **Install Additional Tools**: Updates apt and installs `curl` for additional functionality.
   - **Entry Point and Command**: Uses **CMD** to run the application (`dotnet CyberpunkPariahNexusApi.dll`).

### 2. Dockerfile.Test for `pariah-nexus-api`

The **Dockerfile.Test** is used to build and run **tests** for the `pariah-nexus-api`. This is important for CI/CD pipelines where tests are executed before deployment.

#### Breakdown of Steps
1. **Base Image**
   - Uses the `.NET SDK 8.0` image (`mcr.microsoft.com/dotnet/sdk:8.0`) to build and run tests.

2. **Working Directory and Port**
   - Sets the working directory to `/source` and **exposes port 8080**.

3. **Copy and Restore**
   - **Copy Project Files**: Copies the project files for both the API (`CyberpunkPariahNexusApi/`) and **test project** (`CyberpunkPariahNexusApi.Tests/`).
   - **Restore Dependencies**: Runs `dotnet restore` on both the API and test projects to restore all dependencies.

4. **Run Tests**
   - **Command**: Executes `dotnet test` on the test project (`CyberpunkPariahNexusApi.Tests/CyberpunkPariahNexusApi.Tests.csproj`) without restoring (`--no-restore`), ensuring all tests are executed.

### 3. Docker Compose Configuration (`docker-compose.yml`)

The **Docker Compose** file defines the services involved in running the **`pariah-nexus-api`** and its corresponding **MySQL database**.

#### Overview of Services
1. **pariah-nexus-api**
   - **Build Configuration**: Uses `Dockerfile.Test` to build the container. This ensures that the image contains everything necessary for both running and testing.
   - **Container Name**: The container is named `pariah-nexus-api`.
   - **Ports**: Exposes **port 8080** to the host, allowing access to the web API.
   - **Dependencies**: Depends on the **`pariah-nexus-db`** service, with a condition of `service_healthy` to ensure that the API only starts after the database is ready.
   - **Restart Policy**: Uses a **restart policy** to restart the service if needed.

2. **pariah-nexus-db**
   - **Build Configuration**: Uses the **Dockerfile** found in the `./Database` directory.
   - **Container Name**: Named `pariah-nexus-db`.
   - **Ports**: Exposes **port 3306** for the MySQL database and **port 4444** (presumably for SSH or other services).
   - **Environment Variables**: Defines **environment variables** to set up the **database** with a default name, root password, user, and user password.
   - **Healthcheck**: Uses `mysqladmin ping` to check if the MySQL server is running. This ensures other services only start after the database is fully operational.
     - **Timeout**: Waits **20 seconds** per check.
     - **Retries**: Retries the health check up to **10 times**.

#### Networks
- **Bridge Network**: Uses a **bridge network** as the default, allowing the containers to communicate with each other securely.

### 4. Dockerfile for `pariah-nexus-db`

The **Dockerfile** for `pariah-nexus-db` defines the steps needed to set up the **MySQL database** container, along with initializing data and installing necessary tools.

#### Breakdown of Steps
1. **Base Image**
   - Uses the official **MySQL** image (`mysql:latest`) as the base to leverage MySQL server functionalities.

2. **Working Directory**
   - Sets the working directory to `/docker-entrypoint-initdb.d`, which is the default directory for executing initialization scripts in MySQL Docker images.

3. **Copy Initialization Script**
   - **Copy SQL File**: Copies the `initialize_database.sql` file into the working directory. This script is used to initialize the database with required data and tables.

4. **Install Additional Tools**
   - **Netcat Installation**: Installs `netcat` (`nc`) using `microdnf` to provide a mechanism to check if the MySQL server is ready.
   - **Cleanup**: Runs `microdnf clean all` to remove cached files and reduce image size.

5. **Entry Point and Command**
   - **Custom Startup Command**: Uses a custom command to start the MySQL server and perform readiness checks:
     - Starts the MySQL server using the default entrypoint script (`docker-entrypoint.sh mysqld`).
     - Uses `netcat` to repeatedly check if **port 3306** is open, indicating that the MySQL server is ready.
     - After MySQL is ready, the command keeps the container running (`tail -f /dev/null`), which is often used to prevent container exit after initialization.

### Summary
- The **Dockerfiles** are used to create both **production** and **test** images for the `pariah-nexus-api`, with a focus on multi-stage builds for efficient image creation.
- The **Dockerfile for `pariah-nexus-db`** sets up the MySQL database, initializes data using an SQL script, and ensures readiness using `netcat`.
- The **Docker Compose** file orchestrates the **API** and **MySQL database** services, ensuring they are built, configured, and run in tandem with the proper dependencies.
- The **pariah-nexus-api** is built using the **Dockerfile.Test**, which not only runs the application but also **executes tests** as part of the CI/CD process.
- The **`pariah-nexus-db`** service is set up with a health check to ensure the database is fully operational before dependent services start, providing **reliable and synchronized startup** of services.

This configuration allows for **seamless local development**, **testing**, and **deployment** of the `pariah-nexus-api` application, enabling both developers and CI/CD pipelines to work effectively with the containerized environment.

