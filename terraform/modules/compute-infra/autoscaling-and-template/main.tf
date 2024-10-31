resource "aws_launch_template" "ecs_lt" {
  name_prefix   = "ecs-pariah-nexus-template"
  image_id      = "ami-0fc9e89bb58985752" # Amazon ECS-Optimized Amazon Linux 2 (AL2) x86_64 AMI AMI, pulled from AWS -> EC2 -> AMI Catalog
  instance_type = "t3.large"

  key_name               = var.launch_template_key_name
  vpc_security_group_ids = var.security_groups
  iam_instance_profile {
    name = "ecsInstanceRole"
  }

  block_device_mappings {
    device_name = "/dev/xvda"
    ebs {
      volume_size = 30
      volume_type = "gp2"
    }
  }

  tag_specifications {
    resource_type = "instance"
    tags = {
      Name = "ecs-instance"
    }
  }

  user_data = filebase64("${path.module}/assets/ecs.sh")
}

resource "aws_autoscaling_group" "ecs_asg" {
  vpc_zone_identifier = var.subnets
  desired_capacity    = 1
  max_size            = 3
  min_size            = 1

  launch_template {
    id      = aws_launch_template.ecs_lt.id
    version = "$Latest"
  }

  tag {
    key                 = "AmazonECSManaged"
    value               = true
    propagate_at_launch = true
  }

  lifecycle {
    ignore_changes = [desired_capacity]
  }
}

resource "aws_autoscaling_policy" "scale_down" {
  name                   = "ecs-asg-scale-down"
  scaling_adjustment     = -1
  adjustment_type        = "ChangeInCapacity"
  cooldown               = 300
  autoscaling_group_name = aws_autoscaling_group.ecs_asg.name
}

resource "aws_autoscaling_policy" "scale_up" {
  name                   = "ecs-asg-scale-up"
  scaling_adjustment     = 1
  adjustment_type        = "ChangeInCapacity"
  cooldown               = 300
  autoscaling_group_name = aws_autoscaling_group.ecs_asg.name
}

resource "aws_cloudwatch_metric_alarm" "high_cpu" {
  alarm_name          = "ecs-asg-high-cpu"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "CPUUtilization"
  namespace           = "AWS/EC2"
  period              = "60"
  statistic           = "Average"
  threshold           = "75"
  alarm_description   = "This metric monitors EC2 CPU utilization and triggers scale up."
  dimensions = {
    AutoScalingGroupName = aws_autoscaling_group.ecs_asg.name
  }
  alarm_actions = [aws_autoscaling_policy.scale_up.arn]
}

resource "aws_cloudwatch_metric_alarm" "low_cpu" {
  alarm_name          = "ecs-asg-low-cpu"
  comparison_operator = "LessThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "CPUUtilization"
  namespace           = "AWS/EC2"
  period              = "120"
  statistic           = "Average"
  threshold           = "25"
  alarm_description   = "This metric monitors EC2 CPU utilization and triggers scale down."
  dimensions = {
    AutoScalingGroupName = aws_autoscaling_group.ecs_asg.name
  }
  alarm_actions = [aws_autoscaling_policy.scale_down.arn]
}

resource "aws_cloudwatch_metric_alarm" "high_memory" {
  alarm_name          = "ecs-asg-high-memory"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "MemoryUtilization"
  namespace           = "AWS/ECS"
  period              = "60"
  statistic           = "Average"
  threshold           = "75"
  alarm_description   = "This metric monitors ECS memory utilization and triggers scale up."
  dimensions = {
    AutoScalingGroupName = aws_autoscaling_group.ecs_asg.name
  }
  alarm_actions = [aws_autoscaling_policy.scale_up.arn]
}

resource "aws_cloudwatch_metric_alarm" "low_memory" {
  alarm_name          = "ecs-asg-low-memory"
  comparison_operator = "LessThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "MemoryUtilization"
  namespace           = "AWS/ECS"
  period              = "120"
  statistic           = "Average"
  threshold           = "25"
  alarm_description   = "This metric monitors ECS memory utilization and triggers scale down."
  dimensions = {
    AutoScalingGroupName = aws_autoscaling_group.ecs_asg.name
  }
  alarm_actions = [aws_autoscaling_policy.scale_down.arn]
}
