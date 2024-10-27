## Documentation for Terraform Modules in CyberpunkPariahNexusApi

This document provides an overview and explanation of the **Terraform modules** used in the `CyberpunkPariahNexusApi` project. These modules help manage the infrastructure in AWS, such as networking, compute, load balancers, ECS, and autoscaling resources.

### 1. Overview

The Terraform configuration is structured to define AWS infrastructure for **networking**, **compute resources**, **load balancing**, and **Elastic Container Service (ECS)**. It utilizes modules to encapsulate different parts of the infrastructure and makes it easier to manage and reuse.

### 2. Terraform Configuration File (`main.tf`)

#### Terraform Block
- **Providers**: The **AWS provider** (`hashicorp/aws`) is used, specifying version `~> 5.0`. It allows Terraform to interact with AWS services.
- **Backend**: Uses **S3** as the backend to store the **Terraform state**. The state file is stored in `blackcypher-ops-bucket` under the given path, and encryption is enabled for secure state management.

#### AWS Provider Configuration
- Configures the **AWS provider** to use the `us-east-2` region.

### 3. Network Infrastructure Module (`pariah_nexus_network_infra`)

- **Source**: Located in `./modules/network-infra`, this module is responsible for creating the **networking resources** required for the infrastructure.
- **Inputs**:
  - **`vpc_id`**: Specifies the VPC ID where the infrastructure should be deployed.
  - **`public_subnet_id`**: Refers to an existing public subnet within the specified VPC.

#### Resources
- **VPC Data Source**: Fetches information about the VPC using `aws_vpc` data source.
- **Private Subnet**: Creates a private subnet within the VPC (`aws_subnet.private_subnet`) with a specific CIDR block.
- **Internet Gateway**: Creates an **Internet Gateway** to provide internet connectivity for resources within the VPC.
- **Route Tables**:
  - Configures **route tables** for both private and public subnets, enabling routing through the internet gateway.
- **Security Group**: Creates a **security group** (`aws_security_group.pariah_nexus_security_group`) allowing all inbound and outbound traffic for the ECS infrastructure.

### 4. Compute Infrastructure Module (`pariah_nexus_compute_infra`)

- **Source**: Located in `./modules/compute-infra`, this module is responsible for managing **compute resources**.
- **Inputs**:
  - **`security_groups`**: Security groups applied to compute resources.
  - **`vpc_id`** and **`subnets`**: Specify where the compute resources will be deployed.
  - **`launch_template_key_name`**: Specifies the key pair used to SSH into the instances.

#### Sub-Modules
- **`autoscaling-and-template` Module**: Manages the **Auto Scaling Group** (ASG) and **launch template** for EC2 instances.
- **`alb` Module**: Creates the **Application Load Balancer** (ALB) to distribute traffic among ECS containers.
- **`ecs-infra` Module**: Deploys **ECS** infrastructure, linking it with the **ASG** and **ALB** for load balancing.

### 5. ECS Infrastructure (`ecs-infra` Module)

- **Source**: Located in `./ecs-infra`, this module sets up the ECS infrastructure.
- **Inputs**: 
  - **`security_groups`**, **`subnets`**, **`alb_target_group_arn`**, and **`asg_arn`**.
- **Dependencies**: Depends on the **autoscaling** and **app_load_balancer** modules.

#### Resources
- **CloudWatch Log Group**: Defines a log group for storing ECS logs (`aws_cloudwatch_log_group.logs`).
- **ECS Cluster**: Creates an **ECS cluster** named `pariah-nexus-managed-ecs`.
- **ECS Capacity Provider**: Creates a **capacity provider** for ECS with managed scaling enabled.
- **ECS Task Definition**: Defines the **task** with containers for both **pariah-nexus API** and **pariah-nexus DB**:
  - **API Container**: Configured with **health checks** and **logging** to CloudWatch.
  - **DB Container**: Defines MySQL with environment variables for database setup.
- **ECS Service**: Deploys the **ECS service** using the task definition, with an ALB used for routing traffic.

### 6. Autoscaling and Launch Template Module (`autoscaling-and-template`)

- **Launch Template**: Defines an **EC2 launch template** for the instances used in the ECS cluster. It includes:
  - **Instance Type**: Uses `t3.medium`.
  - **AMI**: Uses an Amazon ECS-optimized Amazon Linux 2 AMI.
  - **Security Groups**: Specifies security groups and IAM instance profile (`ecsInstanceRole`).
  - **Block Device Mapping**: Configures an EBS volume for the instances.
  - **User Data**: Includes a user data script for configuring instances on startup.
- **Auto Scaling Group**: Defines an **ASG** for managing instance scaling based on resource usage.

### 7. Application Load Balancer Module (`alb`)

- **ALB Resource**: Creates an **Application Load Balancer** (`aws_lb.ecs_alb`) that listens on port 80 and forwards traffic to the ECS tasks.
- **Load Balancer Listener**: Configures the **listener** to forward traffic to the target group.
- **Target Group**: Creates a **target group** (`aws_lb_target_group.ecs_tg`) for routing traffic to the ECS containers on port 8080.

### Summary

The Terraform modules used in the **CyberpunkPariahNexusApi** project follow a modular approach, with each module responsible for a specific part of the infrastructure. Below is a summary of the modules and what they provide:

- **`network-infra` Module**: Manages the **network infrastructure**, including subnets, route tables, internet gateways, and security groups.
- **`compute-infra` Module**: Deploys **compute resources**, including EC2 instances, autoscaling groups, and load balancers.
- **`ecs-infra` Module**: Sets up **Elastic Container Service (ECS)**, including the ECS cluster, task definitions, and ECS services.
- **`autoscaling-and-template` Module**: Configures the **Auto Scaling Group (ASG)** and EC2 **launch template**.
- **`alb` Module**: Manages the **Application Load Balancer (ALB)** to distribute traffic across ECS tasks.

These modules provide a reusable and scalable way to define the infrastructure required for the Pariah-Nexus API, allowing for **automated deployment**, **managed scaling**, and **load balancing** of the application.

