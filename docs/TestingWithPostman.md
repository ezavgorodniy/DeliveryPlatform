If you're using PostMan (https://www.postman.com/downloads/) you can download requests collection and environment setup from [collection](postman/Delivery.Tests) and [environment](TestDelivery.Environment).

After this you just need to setup identityHost with URL of deployed IdentityServer and deliveryHost with URL of deployed Delivery api. See example below:

![Postman](images/PostmanExample.png)

As soon a you make a request "Authenticate Partner" or "Authenticate User" jwt will be set in environment variables and you'll be able to start using REST api calls. 

As soon as you make successful call to "Create" request "lastCreatedDelivery" variable will be instantiated and you'll be able to use GetById, DeleteById and Update as it is without changing request url.