# Home Library

A simple book import application built with **.NET 10**, **Angular**, **PostgreSQL**, **RabbitMQ**, and **Docker Compose**.

The application follows an asynchronous producer/consumer architecture where the API parses uploaded CSV files and publishes one message per book to RabbitMQ. A dedicated background worker consumes these messages and persists the data into PostgreSQL, keeping the API responsive and allowing imports to be processed asynchronously.

---

## Architecture

![System Design](docs/images/system-design.png)

---

## Project Structure

```text
.
├── backend
│   ├── HomeLibrary.slnx
│   ├── HomeLibrary.Api
│   ├── HomeLibrary.Worker
│   ├── HomeLibrary.Shared
│   └── HomeLibrary.Tests
│
├── frontend
│   └── home-library-ui
│
├── docs
│   └── images
│       └── system-design.png
│
├── docker-compose.yml
├── .env
└── README.md
```

### Backend Projects

| Project | Description |
|----------|-------------|
| **HomeLibrary.Api** | REST API responsible for parsing CSV files, publishing messages to RabbitMQ and exposing book endpoints. |
| **HomeLibrary.Worker** | Background worker responsible for consuming RabbitMQ messages and persisting books into PostgreSQL. |
| **HomeLibrary.Shared** | Shared entities, Entity Framework Core context, DTOs, messaging contracts and common models. |
| **HomeLibrary.Tests** | Unit tests covering the application's business logic. |

---

## Prerequisites

Before running the application, make sure the following software is installed:

- .NET 10 SDK
- Node.js
- Docker Desktop

---

# Local Development

All Docker commands should be executed from the repository root.

```text
home-library-wa/
```

Start PostgreSQL and RabbitMQ:

```bash
docker compose up -d
```

Verify running containers:

```bash
docker ps
```

Stop the infrastructure:

```bash
docker compose down
```

Rebuild the infrastructure:

```bash
docker compose up -d --build
```

---

## Entity Framework

All Entity Framework commands should be executed from:

```text
home-library-wa/backend
```

### Create a migration

```bash
dotnet ef migrations add <MigrationName> --project HomeLibrary.Shared --startup-project HomeLibrary.Api
```

### Apply migrations

```bash
dotnet ef database update --project HomeLibrary.Shared --startup-project HomeLibrary.Api
```

### Remove the last migration

```bash
dotnet ef migrations remove --project HomeLibrary.Shared --startup-project HomeLibrary.Api
```

---

## Running Tests

Execute all tests from the `backend` folder:

```bash
dotnet test
```

---

## Running the Application

### Backend

Open the solution:

```text
backend/HomeLibrary.slnx
```

Run the following startup projects:

- HomeLibrary.Api
- HomeLibrary.Worker

---

### Frontend

```bash
cd frontend/home-library-ui

npm install

ng serve
```

---

## Docker Deployment

Once the Dockerfiles are added for the API, Worker and Angular application, the complete application can be started using:

```bash
docker compose up -d --build
```

This will start:

- PostgreSQL
- RabbitMQ
- ASP.NET Core API
- Background Worker
- Angular application

---

## Default Services

| Service | URL |
|----------|-----|
| Swagger | https://localhost:5001/swagger |
| RabbitMQ Management | http://localhost:15672 |
| PostgreSQL | localhost:5432 |

### RabbitMQ

```text
Username: guest
Password: guest
```

### PostgreSQL

```text
Host=localhost
Port=5432
Database=library
Username=library
Password=library
```

---

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- Angular
- Tailwind CSS
- Docker Compose
- xUnit
- Moq
- FluentAssertions