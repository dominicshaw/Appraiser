# Introduction 

This solution provides a self-service web front-end for users and managers to view and edit their appraisal.

There are lots of moving parts in the solution:

1. `Appraiser.Api` is a windows service hosted API hosted at http://servername:7823/swagger
1. `Appraiser.Client` is a C# client library to access the API. This is generated with NSwag.
1. `Appraiser.Common` is a C# client library with common enums and types that can be used by all projects.
1. `Appraiser.Data` is the data-layer of the application. It uses EntityFrameworkCore to access a Sql database which is at `[LONHSQL01\PROD01].dbo.Appraisals`.
1. `Appraiser.DTOs` is the library for data-transfer-objects. The API exposes objects exclusively from this library, or the common library.
1. `Appraiser.Mapping` is a library for mapping models from `Appraiser.Data` to DTOs from `Appraiser.DTOs`, and back again. It adds some presentation concerns.
1. `Appraiser.Reporting` contains the reports for the project. They use DevExpress provided through their public NuGet. 
1. `Appraiser.Web` is a windows service hosted static file host for an Angular website at http://servername:7825
1. `Appraiser.Web\Appraiser-Web` is the Angular source files for the Angular Single-Page-Application (SPA) which is hosted by the static file host `Appraiser.Web`.
1. `Appraiser.Windows` is a WPF front-end for the administration of the system. This is for use by the appraisal administrator and HR.
1. `Appraiser.Tests` is a set of tests for the C# libraries.

# Getting Started

There are two Windows services setup on `servername`. They are created on the server with `sc create`:

`sc create "Appraiser-Api" binPath= "C:\Program Files (x86)\TT International\Appraiser-Api\Appraiser-Api.exe"`
`sc create "Appraiser-Web-Host" binPath= "C:\Program Files (x86)\TT International\Appraiser-Web-Host\Appraiser-Web.exe"`

After running this you should change the start type to Automatic (Delayed Start).

Once created, the CI/CD pipelines take care of deployment so you should not need to do anything else.

For more information on how to work with the Angular frontend, see the section "Changing the Angular Frontend".

# Build and Test

There is a [CI pipeline](http://lonhgit01:8080/tfs/Main/Appraiser/_build?definitionId=11) that will 

1. build the Angular application;
1. build all of the projects in the C# solution;
1. run all of the tests in the solution.

There is a [CD pipeline](http://lonhgit01:8080/tfs/Main/Appraiser/_release?view=mine&definitionId=1) that will

1. release the API as a Windows service;
1. release the static host which will contain a rebuilt version of the Angular application;
1. release the WPF front-end.

# Changing the Angular Frontend

## Installing Angular & Dependencies

### I will use VS Code to do my editing but the tools to build the Angular application are IDE agnostic.

### npm & node.js

https://www.npmjs.com/get-npm
https://nodejs.org/en/

1. Install Node.js from the site.
1. Open powershell or the terminal in VSCode (but you will need to restart this if it is already running)
1. Enter `npm -v` to check it is installed
1. Check npm is up to date with `npm install npm@latest -g`

### angular

https://angular.io/cli

1. Use npm to install Angular with `npm install -g @angular/cli`
2. Use npm to install Angular build tools with `npm install --save-dev @angular-devkit/build-angular`

You may want to run:

`npm install`
`ng update`
`npm update`
`npm audit fix`

...which will confirm all packages are updated. While these are running they may ask you to run other package updates, please look carefully.

## Running the site

Once you have the tools above you can run the website. All commands to the angular cli are executed with `ng`.

Serve the site locally with `ng serve` which will host on port 4200. 

Enter `ng serve` then navigate to http://servername:4200

Assuming VS Code, the terminal that you were using will now be serving. You can add a new terminal with the + button top left of the current terminal session to run other commands (like `ng build`!)

## Building the site

The site is configured in `angular.json` to put the built site into the wwwroot of the self-serving website.

There are two modes to build - development and production.

For development you can use `ng build` with no parameters. This will produce the javascript un-minified.

For prodution you can use `ng build -c production` which will build and minify the javascript.

The envrionment configurations are in `angular.json` under `projects.configurations`.

## Generating generated-services.ts

To generate the `generated-services.ts` file which provides all the Angular service connectivity, you need to use NSwag Studio.

Download NSwag Studio and load the `Generate-Client-Api.nswag` file in the root of the project. 

On the OpenAPI/Swagger Specification tab, paste http://servername:7823/swagger/appraiser/swagger.json (or your local version) and hit `Create local Copy`.

Go to the TypeScript Client tab on the right, and `Generate Files`. The settings should all be loaded from the `Generate-Client-Api.nswag`.

Once it is generated you will be able to review the changes by comparing your version with the version in source control. 

There is an additional change you need to make to get the site working. You will need to add `withCredentials: true` to all of the options for the `http.get` calls. VS Code has a replace multiple lines which makes this much easier. There are many instances to change.

[Once the latest version of NSwag Studio is released you will not have to do this.](https://github.com/RicoSuter/NSwag/pull/2538)

Make sure all parts where there are options are changed from:

```ts
    getAppraisals(): Observable<Appraisal[]> {
        let url_ = this.baseUrl + "/api/Appraisals";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };
```
to
```ts
    getAppraisals(): Observable<Appraisal[]> {
        let url_ = this.baseUrl + "/api/Appraisals";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };
```
VS Code supports multi-line find-and-replace which makes this very easy to do.
