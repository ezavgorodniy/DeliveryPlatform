resource "mongodbatlas_cluster" "delivery" {
  project_id = "${var.mongodbatlas_project}"
  name       = "${var.mongodbatlas_cluster_name}"
  num_shards = 1

  mongo_db_major_version       = "4.0"
  auto_scaling_disk_gb_enabled = true

  //Provider Settings "block"
  provider_name               = "AZURE"
  provider_disk_type_name     = "P4"
  provider_instance_size_name = "${var.mondodbatlas_size}"
  provider_region_name        = "${var.mongodbatlas_region}"
}

resource "random_string" "mongodb_accesspassword" {
  length  = 16
  special = false
  upper   = false
}

resource "mongodbatlas_database_user" "user-af" {
  username      = "yezavh-test"
  password      = "${random_string.mongodb_accesspassword.result}"
  project_id    = "${var.mongodbatlas_project}"
  auth_database_name = "admin"

  roles {
    role_name     = "readWriteAnyDatabase"
    database_name = "admin"
  }
}

resource "mongodbatlas_project_ip_whitelist" "all" {
  project_id = "${var.mongodbatlas_project}"
  cidr_block = "${var.mongodbatlas_whitelistip}"
  comment    = "for azure functions access"
}


locals {
  mongodb_connectionstring = join("", [replace(mongodbatlas_cluster.delivery.srv_address, "mongodb+srv://", "mongodb+srv://${mongodbatlas_database_user.user-af.username}:${random_string.mongodb_accesspassword.result}@"), "/test?retryWrites=true&w=majority"])
}

output "mongodb_connectionstring" {
  value = local.mongodb_connectionstring
}