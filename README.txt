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
Server response
Code	Details
200	
Response body
Download
{
  "$id": "1",
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
Server response
Code	Details
200	
Response body
Download
{
  "$id": "1",
  "$values": [
    {
      "$id": "2",
      "id": 1,
      "catId": "ozEvzdVM-",
      "width": 1200,
      "height": 800,
      "image": "https://cdn2.thecatapi.com/images/ozEvzdVM-.jpg",
      "created": "2025-03-05T17:42:35.418727",
      "tags": {
        "$id": "3",
        "$values": [
          {
            "$id": "4",
            "id": 1,
            "name": "Affectionate",
            "created": "2025-03-05T17:42:35.5139402",
            "cats": {
              "$id": "5",
              "$values": [
                {
                  "$ref": "2"
                }
              ]
            }
          },
		  ...
		  
		}
	}]
}

3. Get Cat by ID

GET /api/cats/{id}
Gets a specific cat from DB by ID.

Example:
curl -X 'GET' \
  'http://localhost:5011/api/cats/1' \
  -H 'accept: */*'
Request URL
http://localhost:5011/api/cats/1
Server response
Code	Details
200	
Response body
Download
{
  "$id": "1",
  "id": 1,
  "catId": "ozEvzdVM-",
  "width": 1200,
  "height": 800,
  "image": "https://cdn2.thecatapi.com/images/ozEvzdVM-.jpg",
  "created": "2025-03-05T17:42:35.418727",
  "tags": {
    "$id": "2",
    "$values": []
  }
  

- Running Unit Tests

Execute unit tests:
cd TestStealCats
dotnet test

- Logging
Serilog, logs, can be found in the ..\StealCatsAPI\logs 


- Running in Docker (pending)

1. Build Docker Image

docker build -t stealcats-api .

2. Run the Container

docker run -p 5000:5000 -e "ConnectionStrings__DefaultConnection=Server=YOUR_SERVER;Database=CatDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;" stealcats-api


