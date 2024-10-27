# Cyberpunk - Pariah Nexus API

## Architecture
### Arasaka Data Models
- Cluster
- Device
- Device Memory Mapping
- Device Process
- Athena Security Data Events
- Mikoshi
- Net Runner Administration

Documentation on Models, how they interact, and the interactions with the database can be found at [DataAndModels.md](./Docs/DataAndModels.md).

- **ArasakaCluster** has **many** `ArasakaDevice` instances.
- **ArasakaDevice** has **many** `ArasakaDeviceProcess`, `ArasakaDeviceMemoryMapping`, and `ArasakaAthenaDataEvent` instances.
- Relationships are configured in the **`OnModelCreating`** method of `DataContext`, defining foreign keys and navigation properties.

The **Arasaka models** define the core entities for managing **clusters**, **devices**, and their associated data in the **CyberpunkPariahNexusApi**. Relationships are configured using **Entity Framework Core** to ensure data integrity, with many-to-one and one-to-many relationships between clusters, devices, and their respective processes, memory mappings, and events.

### Endpoints and Controllers
Each data model has a corresponding controller that has support for multiple HTTP methods. Documentation on these controllers, supported HTTP methods, and more can be found at [ControllersAndEndpoints.md](./Docs/ControllersAndEndpoints.md)

The **Arasaka Controllers** manage various entities related to **clusters**, **devices**, **processes**, **memory mappings**, and **administrative information** within the **CyberpunkPariahNexusApi**. Each controller provides standard **CRUD** operations, with **authorization checks** to ensure secure access. Relationships between entities are handled using **eager-loading** or **lazy-loading** as appropriate, providing flexibility and efficiency in data retrieval. Special controllers like **Mikoshi** and **NetRunnerAdministration** are intended for internal administrative purposes.

### Terraform
Details can be found at [TeraformAndAWS.md](./Docs/TerraformAndAWS.md)

The Terraform modules used in the **CyberpunkPariahNexusApi** project follow a modular approach, with each module responsible for a specific part of the infrastructure. Below is a summary of the modules and what they provide:

- **`network-infra` Module**: Manages the **network infrastructure**, including subnets, route tables, internet gateways, and security groups.
- **`compute-infra` Module**: Deploys **compute resources**, including EC2 instances, autoscaling groups, and load balancers.
- **`ecs-infra` Module**: Sets up **Elastic Container Service (ECS)**, including the ECS cluster, task definitions, and ECS services.
- **`autoscaling-and-template` Module**: Configures the **Auto Scaling Group (ASG)** and EC2 **launch template**.
- **`alb` Module**: Manages the **Application Load Balancer (ALB)** to distribute traffic across ECS tasks.

These modules provide a reusable and scalable way to define the infrastructure required for the Pariah-Nexus API, allowing for **automated deployment**, **managed scaling**, and **load balancing** of the application.

### CI/CD and Pipelines
Details can be found at [Github Pipelines And Automation](./Docs/GithubPipelinesAndAutomation.md).

- The **Midnight Scheduled Redeploy** workflow runs every day at **04:00 UTC** or manually and redeploys the `pariah-nexus-service` on ECS, ensuring the service stays up-to-date.
- The **On Push, Build and Terraform Apply** workflow runs on pushes to `develop`, pull requests to `main`, or manually. It consists of four jobs:
  - **run-tests**: Runs integration tests using Docker Compose.
  - **build-deploy**: Builds and pushes Docker images to DockerHub.
  - **terraform-plan**: Runs a Terraform plan to prepare infrastructure changes.
  - **terraform-apply**: Applies the Terraform changes, updating infrastructure based on the plan.

These workflows automate the CI/CD process, ensuring the application is thoroughly tested, built, deployed, and infrastructure is updated with minimal manual intervention.

### Containers, Docker, and Testing
- The **Dockerfiles** are used to create both **production** and **test** images for the `pariah-nexus-api`, with a focus on multi-stage builds for efficient image creation.
- The **Dockerfile for `pariah-nexus-db`** sets up the MySQL database, initializes data using an SQL script, and ensures readiness using `netcat`.
- The **Docker Compose** file orchestrates the **API** and **MySQL database** services, ensuring they are built, configured, and run in tandem with the proper dependencies.
- The **pariah-nexus-api** is built using the **Dockerfile.Test**, which not only runs the application but also **executes tests** as part of the CI/CD process.
- The **`pariah-nexus-db`** service is set up with a health check to ensure the database is fully operational before dependent services start, providing **reliable and synchronized startup** of services.

This configuration allows for **seamless local development**, **testing**, and **deployment** of the `pariah-nexus-api` application, enabling both developers and CI/CD pipelines to work effectively with the containerized environment.

## Ways to use this repo
### Techniques
You can reference this project to investigate techniques such as:
- Models,
- Controllers,
- Database interactions, 
- Docker deployment, 
- EF Fluent API rules, 
- Fluent Validation rules, 
- EFCore + DbContext, 
- connection strings, 
- DTOs (needs improvement)
- CI/CD
- Terraform
- AWS Deployment
- Robust build+test+deploy with automatic scaling+nightly-resets
- *Autism
