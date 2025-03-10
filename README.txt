StealCats API

- Overview

This project provides an API for fetching cats information and images from the CaaS Service https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1. 

- Prerequisites

.NET 8.0 SDK
SQL Server
Docker (optional for containerization)

- Setup Instructions

1. Clone the Repository

git clone https://github.com/Tsiftelis-Thanasis/StealCats.git

2. Configure Database

Update appsettings.json with your SQL Server connection string:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CatDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
  }
}

3. Apply Migrations

Run the following command to apply migrations and create the database:
dotnet ef migrations add InitialCreate --project ../StealCatsRepo --startup-project .
dotnet ef database update --project ../StealCatsRepo --startup-project .

4. Run the Application

From the root of StealCatsAPI 
dotnet run  --> for the http profile
dotnet run --launch-profile "https" --> for the https profile

The API will be available at:
http://localhost:5011
or
https://localhost:7191

- API Documentation (Swagger)

Swagger documentation is enabled for API reference. You can access it at:
http://localhost:5011/swagger
or
https://localhost:7191/swagger


- API Endpoints

1. Fetch 25 Cats from the CaaS API

POST /api/cats/fetch
Fetchs 25 cat images from CaaS API and save them to the database. 

Example:
curl -X 'POST' \
  'http://localhost:5011/api/cats/fetch' \
  -H 'accept: */*' \
  -d ''
Request URL
http://localhost:5011/api/cats/fetch
Response body
{
  "message": "Cats fetched successfully."
}

2. Get Cats

GET /api/cats?page=1&pageSize=10
or
GET /api/cats?page=1&pageSize=10&tag=playful
Retrieve cats from DB with paging support

Example:
curl -X 'GET' \
  'http://localhost:5011/api/cats?page=1&pageSize=10' \
  -H 'accept: */*'
Request URL
http://localhost:5011/api/cats?page=1&pageSize=10
Response body
[
  {
    "id": 1,
    "catId": "ozEvzdVM-",
    "width": 1200,
    "height": 800,
    "image": "https://cdn2.thecatapi.com/images/ozEvzdVM-.jpg",
    "created": "2025-03-05T17:42:35.418727",
    "tags": [
      {
        "id": 1,
        "name": "Affectionate",
        "created": "2025-03-05T17:42:35.5139402",
        "cats": []
      },
      {
        "id": 2,
        "name": "Social",
        "created": "2025-03-05T17:42:35.5255856",
        "cats": []
      },
      {
        "id": 3,
        "name": "Intelligent",
        "created": "2025-03-05T17:42:35.5324552",
        "cats": []
      },
      {
        "id": 4,
        "name": "Playful",
        "created": "2025-03-05T17:42:35.5394825",
        "cats": []
      },
      {
        "id": 5,
        "name": "Active",
        "created": "2025-03-05T17:42:35.5461128",
        "cats": []
      }
    ]
  }
}
]
3. Get Cat by ID

GET /api/cats/{id}
Gets a specific cat from DB by ID.

Example:
curl -X 'GET' \
  'http://localhost:5011/api/cats/100' \
  -H 'accept: */*'
Request URL
http://localhost:5011/api/cats/100
Response body
{
  "id": 100,
  "catId": "vVF7hE-Py",
  "width": 1368,
  "height": 855,
  "image": "https://cdn2.thecatapi.com/images/vVF7hE-Py.jpg",
  "created": "2025-03-08T20:58:38.7485134",
  "tags": []
}
  

- Running Unit Tests

Execute unit tests:
cd TestStealCats
dotnet test

- Logging
Serilog, logs, can be found in the ..\StealCatsAPI\logs 


- Docker
I cannot build the docker. I have issue with the SQL server connection.
Build:
docker build -t stealcatsapi -f StealCatsAPI/Dockerfile .

Run:
docker run -d -p 5000:80 stealcatsapi


- Blazor UI.
Simple UI to check the Cats and also check that the API works as intended.

