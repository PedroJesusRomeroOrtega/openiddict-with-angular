# OpenIddict with Angular

An authentication and authorization example with two identity servers, an Angular client and a resource api.
The two servers are based in [OpenIddict](https://github.com/openiddict/openiddict-core).

## Why use _OpenIddict_ for the identity servers?

There are several identity servers, some of them being made specifically for the .NET ecosystem, but the language used doesn't matter since the identity server should be an independent server for an SSO (Single Sign On) approach.

If there is a chance the identity server needs to be customized it is safer to use a solution based on the stack you are comfortable with.
For .NET, the most popular identity solutions are _AAD (Azure Active Directory)_, _Identity Server_ and _OpenIddict_.

* **AAD**: It's hosted (SaaS) and free for the first 50,000 active users. The caveat is that the solution is dependent on Microsoft Azure.
* **Identity Server**: This is the most popular identity solution in .NET. It is no more free after 4 clients in an enterprise environment.
* **OpenIddict**: It's a library used to create identity servers. It's very popular since it's free. The [OrchardCore](https://github.com/orchardcms/orchardcode) (a SaaS application framework and CMS) uses it.

This repository provides sample solutions using the _OpenIddict_ library for the following reasons:
* Not tied to a cloud provider, host the samples where you want.
* It's free, whatever the number of clients and users you need to support.
* It's highly customizable.
* It's .NET ecosytem friendly.

## Structure

The solution has 4 projects:

### OrchardOpenId

It's an identity server based on the Orchard Core modular framework and the [OpenId module](https://docs.orchardcore.net/en/dev/docs/reference/modules/OpenId/) already configured.
The module uses [OpenIddict-core](https://github.com/openiddict/openiddict-core) and as the [author recommends](https://github.com/openiddict/openiddict-core#i-want-something-simple-and-easy-to-configure) is a good way to configure a simple and easy identity server.

When you run the project, you can select the __Identity__ recipe, which will load the `identity.recipe.json` containing all the necessary default configuration for an identity server. Once the setup is done, the `/admin` url gives you access to the dashboard.

### OpenIdServer

It's a custom identity server using [OpenIddict-core](https://github.com/openiddict/openiddict-core) and the [Velusia sample](https://github.com/openiddict/openiddict-samples/tree/dev/samples/Velusia)

It uses the .NET self-contained UI for Identity with automatically generated views. _login_ and _registry_ are customized using generated views in the _Areas_ folder.

### AngularOpenId

It's a simple _Angular_ app to test how to login and logout with a _code flow + PKCE and silent refresh_ approach.
For authentication and authorization it uses the [angular-oauth2-oidc](https://github.com/manfredsteyer/angular-oauth2-oidc) library by Manfred Steyer.

The project has two guards that only let the _principal-feature_ and _optional-feature_ modules be lazy loaded if the user is authenticated.

#### _principal-feature_ module

The _principal-feature_ module will redirect to the login page in the identity server if the user is not authenticated.

A button will be shown if the user has an _administrator_ role. For this, a _hasRole_ structural directive is used.

#### _optional-feature_ module

The _optional-feature_ module will redirect to a _noauth_ page if the user is not authenticated.

When the user is authenticated a request to the _example api_ will be made in order to retrieve the weather forecast. 
The request will be returned with the response only if the user id contains the _forecast_ role

#### ssl

Follow these steps to setup HTTPS in development:

1. Execute the _CreateAngularDevelopmentCertificate_ to generate the certificate files (dev_localhost.key, dev_localhost.pem, dev_localhost.pfx).
2. Copy the generated files to the _ssl_ folder at root level in _angular-openid_ project
3. Add the ssl configuration to the _angular.json_ file
   ``` diff
    "serve": {
        "builder": "@angular-devkit/build-angular:dev-server",
        "configurations": {
        "production": {
            "browserTarget": "angular-openid:build:production"
        },
        "development": {
            "browserTarget": "angular-openid:build:development",
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
5. Modify the CORS configuration in the _Startup.cs_ file in _OrchardOpenId_ project
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
6. In the Orchard Core dashboard (`/admin`), open the _OpenID Connect_ menu, choose _Applications_ and edit _angularClient_ app. Update the _Redirect and Post Logout Redirect_ URIs.

### Example API

The sample API is configured to work with the two identity server projects.

A _JWT bearer_ schema is used with _OrchardOpenId_. For _OpendIdServer_it's the _OpenIddict_ schema which is used.

In my opinion the _OpenIddict_ schema is a better solution because it uses introspection to secure the connection which is the recommeded approach with the _OpenIddict_ library.

A _forecastPolicy_ is used to authorize the user if _forecast_ is a contained scope.

### CreateAngularDevelopmentCertificate

It's a small console program that generates the certificate files to secure the _Angular SPA_ application.
It follows the [DamienBod approach](https://damienbod.com/2020/02/04/creating-certificates-in-net-core-for-vue-js-development-using-https/)

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
