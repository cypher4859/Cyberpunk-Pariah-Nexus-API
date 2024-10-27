## Documentation for GitHub Actions Workflows in CyberpunkPariahNexusApi

This document provides an overview and explanation of the **GitHub Actions workflows** used in the `CyberpunkPariahNexusApi` project. These workflows automate the process of building, testing, deploying, and managing infrastructure for the application.

### 1. (Midnight) Scheduled Redeploy for Pariah-Nexus

This workflow is responsible for **redeploying the Pariah-Nexus service** on a schedule or when manually triggered. It helps to ensure that the service is consistently refreshed, which can be useful for updating configurations or pulling new Docker images.

#### Workflow Triggers
- **Scheduled Trigger**: Runs every day at **04:00 UTC** (`cron: '0 4 * * *'`). This ensures the service is redeployed regularly.
- **Manual Trigger**: Supports **workflow dispatch**, allowing a manual trigger from the GitHub UI.

#### Environment Variables
- **`SERVICE_NAME`**: Set to `pariah-nexus-service`, representing the ECS service name that will be redeployed.

#### Jobs
1. **build-and-deploy**
   - **Runs-on**: Uses `ubuntu-latest` for the environment.
   - **Permissions**: Requires `id-token: write` for AWS credential exchange and `contents: read` for repository access.

   ##### Steps
   - **Checkout Repository**: Uses `actions/checkout@v2` to clone the repository.
   - **Configure AWS Credentials**: Uses `aws-actions/configure-aws-credentials@v1` to authenticate with AWS using an IAM role.
   - **Log in to DockerHub**: Uses the DockerHub username and password from GitHub secrets to authenticate Docker operations.
   - **Build API Docker Image**: Builds the Docker image for the API using the Dockerfile in the root directory.
   - **Download SQL from S3**: Downloads the `initialize_database.sql` from an S3 bucket, providing data for initializing the database.
   - **Build DB Docker Image**: Builds the Docker image for the MySQL database using the Dockerfile in the `./Database` directory.
   - **Push Docker Images**: Pushes both the **API** and **DB** Docker images to DockerHub.
   - **Force ECS Deployment**: Forces a new deployment of the ECS service (`pariah-nexus-service`) to ensure the new Docker images are used.

### 2. On Push, Build and Terraform Apply

This workflow is responsible for **running tests**, **building and deploying** the Docker images, and applying changes using **Terraform** to manage the infrastructure. It is triggered on changes to specific branches or manually from the GitHub UI.

#### Workflow Triggers
- **Push to Branch**: Triggered on pushes to the `develop` branch.
- **Pull Request to Main**: Triggered when a pull request is made to the `main` branch.
- **Manual Trigger**: Supports **workflow dispatch** for manual execution.

#### Jobs
1. **run-tests** (E2E Run Dotnet Tests)
   - **Runs-on**: Uses `ubuntu-latest` as the environment.
   - **Permissions**: Requires `id-token: write` for AWS credential exchange and `contents: read` for repository access.

   ##### Steps
   - **Checkout Repository**: Uses `actions/checkout@v2` to clone the repository.
   - **Configure AWS Credentials**: Configures AWS credentials using a specific role for accessing S3.
   - **Log in to DockerHub**: Uses credentials from GitHub secrets to authenticate with DockerHub.
   - **Download SQL from S3**: Downloads the initialization SQL file from S3.
   - **Start Docker Compose (Run Tests)**: Runs `docker-compose up` to start all services, build the containers, and run the tests defined in the `Dockerfile.Test`. The `--abort-on-container-exit` flag ensures the job stops if any container exits with an error.
   - **Tear Down Docker Compose**: Runs `docker compose down` to stop and remove the containers after the tests are completed, ensuring the environment is cleaned up.

2. **build-deploy** (Build and Deploy Code)
   - **Runs-on**: Uses `ubuntu-latest`.
   - **Depends on**: Needs `run-tests` job to complete successfully before starting.
   - **Permissions**: Requires `id-token: write` for AWS credential exchange and `contents: read` for repository access.

   ##### Steps
   - **Checkout Repository**: Clones the repository.
   - **Configure AWS Credentials**: Authenticates with AWS using credentials from GitHub secrets.
   - **Log in to DockerHub**: Uses the DockerHub credentials from GitHub secrets.
   - **Build and Push Docker Images**: Builds and pushes **API** and **DB** Docker images to DockerHub.

3. **terraform-plan** (Terraform Plan)
   - **Runs-on**: Uses `ubuntu-latest`.
   - **Depends on**: Needs `build-deploy` job to complete successfully.
   - **Permissions**: Requires `id-token: write` for AWS credential exchange and `contents: read` for repository access.

   ##### Steps
   - **Checkout Repository**: Uses `actions/checkout@v2` to clone the repository.
   - **Configure AWS Credentials**: Configures AWS credentials to manage infrastructure.
   - **Setup Terraform**: Uses `hashicorp/setup-terraform@v2` to set up the Terraform CLI (version `1.5.1`).
   - **Terraform Init**: Initializes Terraform in the `./terraform` directory.
   - **Terraform Plan**: Runs `terraform plan` and saves the output to `tfplan`.
   - **Upload Plan File**: Uploads the plan file as an artifact for later use.

4. **terraform-apply** (Terraform Apply)
   - **Runs-on**: Uses `ubuntu-latest`.
   - **Depends on**: Needs `terraform-plan` to complete successfully.
   - **Permissions**: Requires `id-token: write` for AWS credential exchange and `contents: read` for repository access.

   ##### Steps
   - **Checkout Repository**: Clones the repository.
   - **Configure AWS Credentials**: Configures AWS credentials for managing infrastructure.
   - **Setup Terraform**: Uses `hashicorp/setup-terraform@v2` to set up the Terraform CLI.
   - **Terraform Init**: Initializes Terraform.
   - **Download Plan Artifact**: Downloads the saved Terraform plan artifact (`tfplan`).
   - **Terraform Apply**: Applies the Terraform plan with `-auto-approve` to make the infrastructure changes.

### Summary
- The **Midnight Scheduled Redeploy** workflow runs every day at **04:00 UTC** or manually and redeploys the `pariah-nexus-service` on ECS, ensuring the service stays up-to-date.
- The **On Push, Build and Terraform Apply** workflow runs on pushes to `develop`, pull requests to `main`, or manually. It consists of four jobs:
  - **run-tests**: Runs integration tests using Docker Compose.
  - **build-deploy**: Builds and pushes Docker images to DockerHub.
  - **terraform-plan**: Runs a Terraform plan to prepare infrastructure changes.
  - **terraform-apply**: Applies the Terraform changes, updating infrastructure based on the plan.

These workflows automate the CI/CD process, ensuring the application is thoroughly tested, built, deployed, and infrastructure is updated with minimal manual intervention.

