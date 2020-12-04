Architecturally I'd like to separate auth concern from api and busines logic. So, overally system will have two major microservice with their own database:

- Identity - which will be responsible to authenticate user and produce a JWT with claims (User or Partner)
- DeliveryApi - which will be responsible to deal with deliveries

As they logically different and serving different purposes I'd like to separate them as two different application. The code is flexible enough to combine them together if it's absolutely necessary. 

![Architecture](images/Architecture.png)

Identity will be standard Id v4 ASP.NET core project.
Delivery.Api will be standard ASP.NET Core WebAPI project.
Authentication will be based on JWT token.