# Document Storage API
This is a simple document storage API that allows users to upload and download documents with metadata such as posted date, name, description, and category. Users can also create groups and manage other users. The API is built with the following features:

- User authentication
- Group management
- Document access control
- Three user roles: regular user, manager user, and admin
- RESTful API endpoints
- Unit and e2e tests
- PostgreSQL database

# Architecture and Database
This API was built using the Hexagonal architecture and Clean Code principles. We used a Db First approach, which means that we first created the database structure and then generated the code to interact with it.

All business rules were implemented as stored procedures and functions in the PostgreSQL database. This decision was made to ensure that the database is the single source of truth for all data-related operations and to minimize the coupling between the application and the database.

The Hexagonal architecture, also known as Ports and Adapters architecture, was chosen to ensure a clear separation of concerns and to make the code more testable and maintainable. It also allows us to easily swap out the database or any other external service without affecting the application's core logic.

In summary, this API follows the best practices of software design and development to ensure a scalable, maintainable, and extensible solution.

### Technologies
This API was built using the following technologies:

 - .NET Core
 - PostgreSQL
 - Dapper
 - FluentMediator
 - AutoMapper
 - xUnit
 - Moq


### Getting Started
To get started, clone this repository to your local machine:

`git clone https://github.com/alesimoes/document-storage.git`

### Prerequisites
- .NET 5 SDK
- PostgreSQL

### Installation
1. Navigate to the project root directory
2. Create a new PostgreSQL database /DocStorage.Repository/Resources/Database
3. Run all scrips of creation DB structure
4. Update the **appsettings.json** file with your PostgreSQL connection string
5. Run **dotnet restore** to restore the project dependencies
6. Run** dotnet run **to start the API
7. Postman file **DocumentStorage.postman_collection**

### Usage
The API supports the following endpoints:

|Method|Endpoint|Description|
| ------------ | ------------ | ------------ |
|POST|/api/auth|Authenticate a user and obtain an access token|
|POST|/api/user|Create a new user|
|GET|/api/user/{id}| Retrieve a user by ID|
|PATCH|/api/user|Update a user|
|DELETE|/api/user/{id}|Delete a user|
|POST|/api/group|Create a new group|
|GET|/api/group/{id}|Retrieve a group by ID|
|PUT|/api/group/{id}|Update a group|
|DELETE|/api/group/{id}|Delete a group|
|POST|/api/group/{id}/user|Add user in a group|
|DELETE|/api/group/{id}/user|Add user in a group|
|POST|/api/document|Upload a new document|
|GET|/api/document/{id}|Retrieve a document by ID|
|POST|/api/document/{id}/access/user|Grant document access to a user or a group|
|POST|/api/document/{id}/access/group|Grant document access to a user or a group|
|DELETE|/api/document/{id}/access/user|Revoke document access to a user or a group|
|DELETE|/api/document/{id}/access/group|Revoke document access to a user or a group|

### Security

To authenticate a user, send a POST request to /api/auth/ with a JSON payload that includes the username and password:

    {
      "username": "admin",
      "password": "admin"
    }

The response will include an access token that you can use to access protected endpoints:

```json
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyMSIsImp0aSI6ImY4NDUzOGYyLWEyMDMtNDZk....
```
  
Include the access token in the Authorization header of subsequent requests:

`Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIi...`
All roles have a user role validation to grant access to perform the role

#### Database authorization
All functions have a user role validation to grant access to execute the function.

```sql
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role,'manager'::tp_user_role]);
```

### Development
Running Tests
To run the unit tests, run the following command:

`dotnet test`

### Contributing
Contributions to this project are welcome! Please open an issue or a pull request if you find any bugs or have any suggestions for improvement.

### License
This project is licensed under the MIT License.