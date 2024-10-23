data "aws_caller_identity" "current" {}
data "aws_region" "current" {}

locals {
    acct_id = data.aws_caller_identity.current.account_id
    region  = data.aws_region.current.name
}

resource "aws_cloudwatch_log_group" "logs" {
  name = "ecs-pariah-nexus-logs"
}

resource "aws_ecs_cluster" "ecs_cluster" {
    name = "pariah-nexus-managed-ecs"
}

resource "aws_ecs_capacity_provider" "ecs_capacity_provider" {
    name = "pariah-nexus_ec2_capacity_provider"

    auto_scaling_group_provider {
        auto_scaling_group_arn = var.asg_arn

        managed_scaling {
            maximum_scaling_step_size = 1000
            minimum_scaling_step_size = 1
            status                    = "ENABLED"
            target_capacity           = 3
        }
    }
}

resource "aws_ecs_cluster_capacity_providers" "example" {
    cluster_name = aws_ecs_cluster.ecs_cluster.name

    capacity_providers = [aws_ecs_capacity_provider.ecs_capacity_provider.name]

    default_capacity_provider_strategy {
        base              = 1
        weight            = 100
        capacity_provider = aws_ecs_capacity_provider.ecs_capacity_provider.name
    }
}

resource "aws_ecs_task_definition" "pariah_nexus_ecs_task_definition" {
    family             = "pariah-nexus-ecs-task" # TODO: Swap this
    network_mode       = "awsvpc"
    execution_role_arn = "arn:aws:iam::${local.acct_id}:role/ecsTaskExecutionRole"
    cpu                = 512
    runtime_platform {
        operating_system_family = "LINUX"
        cpu_architecture        = "X86_64"
    }
    container_definitions = jsonencode([
        {
            name      = "pariah-nexus"
            image     = "docker.io/cypher4859/pariah-nexus-api:latest" # TODO: Swap this for the actual
            container_name = "pariah-nexus-app"
            cpu       = 256
            memory    = 1024
            essential = true
            portMappings = [
                {
                    containerPort = 8080
                    hostPort      = 8080
                    protocol      = "tcp"
                }
            ],
            dependsOn = [
                {
                    containerName   = "pariah-nexus-db",
                    condition       = "HEALTHY"
                }
            ],
            logConfiguration = {
                logDriver = "awslogs",
                options   = {
                    awslogs-group         = aws_cloudwatch_log_group.logs.name
                    awslogs-region        = local.region
                    awslogs-stream-prefix = "ecs-pariah-nexus"
                }
            },
            "healthCheck": {
                "command": [
                    "CMD-SHELL",
                    "curl -f http://localhost:8080/swagger/index.html || exit 1"
                ],
                "interval": 30,
                "timeout": 5,
                "retries": 3,
                "startPeriod": 60
            }
        },
        {
            name      = "pariah-nexus-db"
            image     = "docker.io/cypher4859/pariah-nexus-db:latest" # TODO: Swap this for the actual 
            container_name = "pariah-nexus-db"
            cpu       = 256
            memory    = 1024
            essential = true
            environment = [
                {"name": "MYSQL_DATABASE", "value": "pariahnexus"},
                {"name": "MYSQL_ROOT_PASSWORD", "value": "arasakaOperator123"},
                {"name": "MYSQL_USER", "value": "arasakaOperator"},
                {"name": "MYSQL_PASSWORD", "value": "arasakaOperator123"}
            ]
            portMappings = [
                {
                    containerPort = 3306
                    hostPort      = 3306
                    protocol      = "tcp"
                }
            ],
            healthCheck = {
                "command": [
                    "CMD-SHELL",
                    "mysqladmin ping -h localhost -u root --password=arasakaOperator123 || exit 1"
                ],
                "interval": 30,
                "timeout": 5,
                "retries": 3,
                "startPeriod": 30
            }
            logConfiguration = {
                logDriver = "awslogs",
                options   = {
                    awslogs-group         = aws_cloudwatch_log_group.logs.name
                    awslogs-region        = local.region
                    awslogs-stream-prefix = "ecs-pariah-nexus-db"
                }
            }
        }
    ])
}

resource "aws_ecs_service" "pariah_nexus_ecs_service" {
    name            = "pariah-nexus-service"
    cluster         = aws_ecs_cluster.ecs_cluster.id
    task_definition = aws_ecs_task_definition.pariah_nexus_ecs_task_definition.arn
    desired_count   = 1

    network_configuration {
        subnets         = var.subnets
        security_groups = var.security_groups
    }

    force_new_deployment = true
    placement_constraints {
        type = "distinctInstance"
    }

    triggers = {
        redeployment = plantimestamp()
    }

    capacity_provider_strategy {
        capacity_provider = aws_ecs_capacity_provider.ecs_capacity_provider.name
        weight            = 100
    }

    load_balancer {
        target_group_arn = var.alb_target_group_arn
        container_name   = "pariah-nexus"
        container_port   = 8080
    }
}
