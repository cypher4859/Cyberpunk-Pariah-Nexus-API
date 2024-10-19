terraform {
    required_providers {
        aws = {
            source  = "hashicorp/aws"
            version = "~> 5.0"
        }
    }
    backend "s3" {
        bucket = "codeyou-demo-cyberpunk-pariah-nexus-terraform-bucket"
        key    = "terraform/state/code-you/cyberpunk/pariah-nexus/terraform.tfstate"
        region = "us-east-2"
        encrypt = true
        
    }
}

# Configure the AWS Provider
provider "aws" {
    region = "us-east-2"
    profile = "blackhat-user"
}

# Create network infrastructure
module "pariah_nexus_network_infra" {
    source = "./modules/network-infra"
    vpc_id = "vpc-3771c65c" # Grabbed from the console
}

module "pariah_nexus_compute_infra" {
    source = "./modules/compute-infra"
    security_groups = compact(module.pariah_nexus_network_infra.security_groups)
    vpc_id = module.pariah_nexus_network_infra.vpc_id
    subnets = compact(module.pariah_nexus_network_infra.subnets)
    launch_template_key_name = "cyberpunk_pariah-nexus_keypair"
    kali_subnet = module.pariah_nexus_network_infra.public_subnet
}
