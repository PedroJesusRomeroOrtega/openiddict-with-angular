# openiddict-with-angular

An authentication and authorization example with two identity servers, an Angular client and a resource api.
The two servers are based in [OpenIdDict](https://github.com/openiddict/openiddict-core).

## Why use _OpenIdDict_ for the identity servers?

There are a lot of identity servers, ones of them are made specifically for the .NET ecosystem and others are based in other languages, but can be used too because the identity server should be an independent server specially if we want a SSO (single sign on) approach.

If you think that you will need to personalize the Identity, is better to use a Identity solution based in your principal stack.
For .NET, the principals Identity solutions are _AAD(Azure Active Directory)_, _Identity Server_ and _OpenIdDict_.

* **AAD**: Is SaaS hosted and free for the first 50000 active user. The const is that you will be tied to Azure.
* **Identity Server**: Is the most popular, but actually you will have to pay is you need more than 4 client in an enterprise environment. If you think in microservices and a free framework, this isn´t a good solution.
* **OpenIdDict**: Is a library used to create identity servers. In these days has become more popular because is free. Popular frameworks like OrchardCore used it.

So, for this sample I choose _OpenIdDict_ library for the below reasons:
* Not tied to a cloud provider.
* It's free.
* It's highly customizable.
* It's .NET ecosytem friendly.

## Structure

The solution has 4 projects:

### OrchardOpenId

It's an identity server. 
It's an Orchard Core project with the [OpenId module](https://docs.orchardcore.net/en/dev/docs/reference/modules/OpenId/) configured.
It's based on [openiddict-core](https://github.com/openiddict/openiddict-core) and as the [author recommends](https://github.com/openiddict/openiddict-core#i-want-something-simple-and-easy-to-configure) is a good way to configure a simple and easy identity server.

When you run the project, you can load the identity.recipe.json choosing it in _configuration-Recipes_. In this way all the configuration related with _OpenId_ will be configured automatically.

### OpenIdServer

It's an other identity server.
I've used [openiddict-core](https://github.com/openiddict/openiddict-core) and the [Velusia sample](https://github.com/openiddict/openiddict-samples/tree/dev/samples/Velusia)

Used .Net self-contained UI for Identity. In this way the default views are generated automatically.

For personalization of _login_ and _registry_ I have used the identity scaffold that generates the views in _Areas_ folder.

### angular-openId

It's a simple _Angular_ app to test how to login and logout with a _code flow + PKCE and silent refresh_ approach.
For the authentication and authorization process I use [angular-oauth2-oidc](https://github.com/manfredsteyer/angular-oauth2-oidc) library by Manfred Steyer.

The project has two guards that only let the _principal-feature_ and _optional-feature_ modules be lazy loaded if the user is authenticated.

#### _principal-feature_ module

The _principal-feature_ module will redirect to the login page in the identity server if the user is not authenticated.

A button will be shown if the user has an _administrator_ role. For this, a _hasRole_ structural directive is used.

#### _optional-feature_ module

The _optional-feature_ module will redirect to a _noauth_ page if the user is not authenticated.

When the user is authenticated, a request to the _example api_ will be made in order to retrieve the weather forecast. 
Only if the user id contains the _forecast_ role, the request will be returned with the response.

#### ssl

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

### Example API

The example API is prepaired to work with the two identity server projects.

For _OrchardOpenId_ the _JWT bearer_ schema is used and for _OpendIdServer_, the _openIdDict_ schema is used.

In my opinion the _openIdDict_ schema is a better solution because use introspection to secure the connection and is the recommeded approach with the _OpenIdDict_ library.

A _forecastPolicy_ is used to authorize the user if _forecast_ is a contained scope.

### CreateAngularDevelopmentCertificate

It's an small console program to generate the certificate files to secure the _Angular SPA_ application.
I've followed the [DamienBod approach](https://damienbod.com/2020/02/04/creating-certificates-in-net-core-for-vue-js-development-using-https/)

## Run the example

1. Open a terminal with _OpenIdServer_ or _OrchardServer_ root folder.
2. Run `dotnet watch run`
3. Open a terminal with _ExampleApi_ root folder.
4. Run `dotnet watch run`
5. Follow the [steps to generate the certificates](#ssl)
6. Open a terminal with _angular-openId_ root folder.
7. Run `npm install`
8. Run `npm run start-with-ssl`

## Links

- [OpenID Connect Flows](https://www.scottbrady91.com/OpenID-Connect/OpenID-Connect-Flows)
- [OAuth is Not User Authorization](https://www.scottbrady91.com/OAuth/OAuth-is-Not-User-Authorization)
- [Identity vs Permissions](https://leastprivilege.com/2016/12/16/identity-vs-permissions/): Identity is universal, whereas permissions are application specific
- [SPAs are dead](https://leastprivilege.com/2020/03/31/spas-are-dead/)
- [SPA necromancy](https://infi.nl/nieuws/spa-necromancy/)
- [Security in Angular series](https://www.codemag.com/Article/1805021/Security-in-Angular-Part-1)
- [Implementing openId code flow with PKCE using OpenIdDict and angular](https://damienbod.com/2017/04/11/implementing-openid-implicit-flow-using-openiddict-and-angular/)
- [angular-oauth2-oidc](https://github.com/manfredsteyer/angular-oauth2-oidc)
- [Example angular-oauth2-oidc with AuthGuard](https://github.com/jeroenheijmans/sample-angular-oauth2-oidc-with-auth-guards/)