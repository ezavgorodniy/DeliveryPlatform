provider "mongodbatlas" {
  version = "0.4.2"
  public_key  = "${var.mongodbatlas_public_key}"
  private_key = "${var.mongodbatlas_private_key}"
}