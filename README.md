# openiddict-with-angular

An authentication and authorization example with _Orchard Core OpenID_ as a identity server and an Angular client.

The solution has 3 projects:

## OrchardOpenId

It's the identity server. 
It's an Orchard Core project with the [OpenId module](https://docs.orchardcore.net/en/dev/docs/reference/modules/OpenId/) configured.
It's based on [openiddict-core](https://github.com/openiddict/openiddict-core) and as the [author recommends](https://github.com/openiddict/openiddict-core#i-want-something-simple-and-easy-to-configure) is a good way to configure a simple and easy identity server.

When you run the project, you can load the identity.recipe.json choosing it in _configuration-Recipes_. In this way all the configuration related with _OpenId_ will be configured automatically.

## angular-openId

It's a simple _Angular_ app to test how to login and logout with a _code flow + PKCE and silent refresh_ approach.
For the authentication and authorization process I use [angular-oauth2-oidc](https://github.com/manfredsteyer/angular-oauth2-oidc) library by Manfred Steyer.

### ssl

Follow the below steps to setup the HTTPS in development:

1. Execute the _CreateAngularDevelopmentCertificate_ to generate the certificate files (dev_localhost.key, dev_localhost.pem, dev_localhost.pfx).
2. Copy the generated files to _ssl_ folder at root level in _angular-openId_ project
3. Add ssl configuration to _angular.json_ file
   ``` diff
    "serve": {
        "builder": "@angular-devkit/build-angular:dev-server",
        "configurations": {
        "production": {
            "browserTarget": "angular-openId:build:production"
        },
        "development": {
            "browserTarget": "angular-openId:build:development",
   +        "ssl": true,
   +        "sslKey": "ssl/dev_localhost.key",
   +        "sslCert": "ssl/dev_localhost.pem"
        }
    },
    "defaultConfiguration": "development"
    },
   ```
4. Add a script to _package.json_ to have the posibility of execute the app without ssl
   ``` json
    "start": "ng serve --ssl=false --open",
    "start-with-ssl": "ng serve --open",
   ```
5. Modify the cors configuration in _startup.cs_ file in _OrchardOpenId_ project
   ``` c#
    services.AddCors(options =>
    {
        options.AddPolicy(name: CORS_POLICY,
                            builder =>
                            {
                                builder.WithOrigins("https://localhost:4200");
                                builder.AllowAnyMethod();
                                builder.AllowAnyHeader();
                            });
    });
   ``` 
6. In Orchard dashboard, inside _OpenID Connect_ option, choose _applications_ and edit _angularClient_ app. Update the _Redirect and Post Logout Redirect_ uris.

## CreateAngularDevelopmentCertificate

It's an small console program to generate the certificate files to secure the _Angular SPA_ application.
I follow the [DamienBod approach](https://damienbod.com/2020/02/04/creating-certificates-in-net-core-for-vue-js-development-using-https/)