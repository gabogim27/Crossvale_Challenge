# xv_dotnet_demo

In this exercise, you will develop a RESTful API microservice using .NET SDK 7.0. This microservice will provide an API capable of performing fundamental CRUD (Create, Read, Update, Delete) operations on a resource. Additionally, you will work with JSON Web Tokens (JWT) for authentication and authorization, make HTTP requests to third-party services, and document the process of building, configuring, and deploying your API as a Docker image for microservice deployment.

This microservice will expose a REST API, and we will provide the API definition using OpenAPI/Swagger. You can run the app and navigate to http://localhost:5152/swagger/v1/swagger.json to get API specs once the exercise have been completed.

This exercise will utilize an SQLite3 database for data storage: `demodb.db`.

## Prerequisites

To get the most out of this exercise, you should have a basic understanding of programming concepts, C# programming language, and familiarity with RESTful API principles. You'll need .NET SDK 7.0 and an Integrated Development Environment (IDE) like Visual Studio or Visual Studio Code (we recommend to use this one).

__1. MAC: Install Homebrew__
```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```

__2. MAC: Install dotnet command__
```
brew install --cask dotnet
```

__3. Install .Net SDK__
https://dotnet.microsoft.com/en-us/download/dotnet/7.0

__4. SQLite3__
https://www.sqlite.org/download.html

## Running the app

```sh
dotnet run
```

## Exercises

To complete this exercise, please conduct a screen sharing session for exercises 1 and 2. Once the session is finished, send both files, `ApiMessageController.cs` and `ApiJwtController.cs`, via email (**compressed and attached, shared links are not allowed**). fterwards, you will have one day to complete the entire test, including any unfinished parts of exercises 1 and 2. Please compress the project folder and send it back.

### Exercise 1  (EASY)

**To send by mail during the sreen sharing**: Add the insert, update and delete to the message API controller (`ApiMessageController.cs`).

Example of requests:
```sh
# Message API
curl -X GET -vvv http://localhost:5152/api/message/all
curl -X GET -vvv http://localhost:5152/api/message/1
curl -X DELETE -vvv http://localhost:5152/api/message/1
curl -X POST -vvv http://localhost:5152/api/message  -H 'Content-Type: application/json; charset=utf-8' -d '{"message":"your new message"}'
curl -X PUT -vvv http://localhost:5152/api/message/4  -H 'Content-Type: application/json; charset=utf-8' -d '{"id": 4, "message":"your message"}'
```


### Exercise 2 (MEDIUM)
Complete the JWT API controller (`ApiJwtController.cs`): 
* `GET /jwt/build?issuer={issuer}&subject={subject}`: this service builds a JWT with the incoming issuer and subject. If those params are not received, it will build a token with the default values issuer=anonymous, and subject=anonymous. Token must be valid for X seconds
* `GET /jwt/validate`: this service validates the JWT token received in Authorization header.
* Default values must be included in the application configuration `appsettings.json`.

Provided info:
* Algorithm: SecurityAlgorithms.HmacSha256
* Key: appsettings.json > jwt:key
* Valid Issuer: appsettings.json > jwt:valid:issuer
* Token validity in seconds: appsettings.json > jwt:tokenValiditySeconds

Example of requests:
```sh
# 1. Build a JWT using default values
curl -X GET -vvv http://localhost:5152/jwt/build
# 2. Build a JWT using the provided issuer and subject
curl -X GET -vvv 'http://localhost:5152/jwt/build?issuer=xv&subject=test'
# 3. Validate a JWT:
curl -X GET -vvv http://localhost:5152/jwt/validate -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0IiwibmJmIjoxNjk1ODI4MTYyLCJleHAiOjE2OTU4MjgxODIsImlzcyI6Inh2In0.wmOdX_y-xLcKop_PljJagDJZtB7Ydsq31B7TtQvxZNA'
```

### Exercise 3 (COMPLEX)

**To send by mail during the sreen sharing**: Create the Rick & Morty API controller (`ApiRickAndMortyController.cs`) to:

* Expose an API (`/richandmorty/character`) that retrieves the same info as https://rickandmortyapi.com/api/character API.
* Add a configuration to define the rickAndMorty API uri: `RickAndMortyAPI:characterUri`.
* Handle internal errors (500) showing a description of the error.
* If the requested API replies with and error, our API must reply with a message to explain it: if response != success, reply with the error code and a message description like this one: `Request to the external API failed with status 404-NotFound`.

Example of requests:
```sh
curl -X GET -vvv http://localhost:5152/api/richandmorty/character/50
# this request must do a request to https://rickandmortyapi.com/api/character/50 and reply with the same output.
```

### Exercise 4 (EASY)

* There is a table in the SQLite DB called "names": you must expose this resource as an API. 
* Create a new Controller for this new resource (`ApiNameControlller.cs`)
* Rename the existing controller to work only for messages (ApiMessageController -> ApiMessageControlller).

Example of requests:
```sh
# Name API
curl -X GET -vvv http://localhost:5152/api/name/all
curl -X GET -vvv http://localhost:5152/api/name/1
curl -X DELETE -vvv http://localhost:5152/api/name/1
curl -X POST -vvv http://localhost:5152/api/name  -H 'Content-Type: application/json; charset=utf-8' -d '{"name":"your new name"}'
curl -X PUT -vvv http://localhost:5152/api/name/3  -H 'Content-Type: application/json; charset=utf-8' -d '{"id": 3, "name":"write your name here"}'
```

### Exercise 5 (COMPLEX)

Build the microservice image and run it using docker (you don't need to attach the image):
* Base Image: `mcr.microsoft.com/dotnet/aspnet:7.0` 
* Exponse the API using port `5002` instead of the default ones (`5152` or `80`).
* Complete the sections bellow `Build image` and `Running the docker image`: explain how to build the image and how to run it using docker.
* EXTRA: Create a default deployment yaml file `deployment.yml` file to deploy the microservice in Kubernetes / Openshift.
* EXTRA: Complete the section `Deploying the microservice to Kubernetes / Openshift`.

## Build & deploy microservice image

This section describes how to build the .NET image and how to run it.

Base image: `mcr.microsoft.com/dotnet/aspnet:7.0`.

### Build image

TODO: explain here what is the process to build the image and how to publish the image to a Docker Registry.

Below are the steps:

1 - Open a commnd prompt, go to the folder where the dockerfile is present and run the following commands:

2 - docker build -t gabogim27/xv_dotnet_demo:v1 . (This command will create the docker image)

3 - docker login 

4 - docker tag gabogim27/xv_dotnet_demo:v1 gabogim27/xv_dotnet_demo:latest

5 - docker push gabogim27/xv_dotnet_demo:v1


```sh
TODO: command to build the image using port 5002

command: docker build -t gabogim27/xv_dotnet_demo:v1 .

TODO: what is the image (name and version) you built?
```
Image name: gabogim27/xv_dotnet_demo
Version: v1

### Running the docker image

To run the image using port 5002, you must use this command:

```sh
TODO: command to run the image using port 5002
```
docker run -p 5002:5002 gabogim27/xv_dotnet_demo:v1

### Deploying the microservice to Kubernetes / Openshift

```sh
TODO: how to deploy & run this microservice in Kubernetes or Openshift
```

Project changes:

I have created 3 new projects in order to decouple the business logic, database access from controller. 

xv_dotnet_demo_v2 => Contains the controllers, dtos, settings and mappers.

xv_dotnet_demo_v2_domain => Contains the entities classes.

xv_dotnet_demo_v2_infrastructure => Contains the repositories.

xv_dotnet_demo_v2_services => Contains the business logic.

I created a new folder named xv_dotnet_demo_tests that contains all the controller unit tests. 