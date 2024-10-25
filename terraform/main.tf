terraform {
    required_providers {
        aws = {
            source  = "hashicorp/aws"
            version = "~> 5.0"
        }
    }
    backend "s3" {
        bucket = "blackcypher-ops-bucket"
        key    = "terraform/state/code-you/cyberpunk/pariah-nexus/terraform.tfstate"
        region = "us-east-2"
        encrypt = true
        
    }
}

# Configure the AWS Provider
provider "aws" {
    region = "us-east-2"
}

# Create network infrastructure
module "pariah_nexus_network_infra" {
    source = "./modules/network-infra"
    vpc_id = "vpc-3771c65c" # Grabbed from the console
    public_subnet_id = "subnet-03595f11b44edacc8"
}

module "pariah_nexus_compute_infra" {
    source = "./modules/compute-infra"
    security_groups = compact(module.pariah_nexus_network_infra.security_groups)
    vpc_id = module.pariah_nexus_network_infra.vpc_id
    subnets = compact(module.pariah_nexus_network_infra.subnets)
    launch_template_key_name = "blackhat_codeyou_demo_keypair" # FIXME: Needs changed later
}
