# Movie Management System API

A lightweight, robust, and clean-architecture-compliant ASP.NET Core Web API for managing movies, genres, actors, and biographies. Built using **Entity Framework Core (SQLite)**, the **Repository & Unit of Work Patterns**, and secured with **JWT Bearer Authentication**.

---

## 🏗️ Architecture Overview

The project is structured following the principles of **Clean Architecture** (or Onion Architecture) to ensure separation of concerns, testability, and high maintainability.

```
                  +----------------------------+
                  |    MovieManagement.WebAPI  | (Controllers, DTOs, JWT Authentication)
                  +-------------+--------------+
                                |
                                v
                  +----------------------------+
                  | MovieManagement.DataAccess | (DbContext, EF Core, Repositories, Migrations)
                  +-------------+--------------+
                                |
                                v
                  +----------------------------+
                  |   MovieManagement.Domain   | (Entities, Repository Interfaces)
                  +----------------------------+
```

### Layer Breakdown

1. **[MovieManagement.Domain](file:///d:/MovieManagement/MovieManagement.Domain)**:
   * **Core Entities**: Defines data structures without external framework dependencies (except standard C# collections).
   * **Repository Interfaces**: Abstracts database access patterns, specifying contracts for data CRUD operations.

2. **[MovieManagement.DataAccess](file:///d:/MovieManagement/MovieManagement.DataAccess)**:
   * **EF Core Context**: Houses [ApplicationDbContext.cs](file:///d:/MovieManagement/MovieManagement.DataAccess/Data/ApplicationDbContext.cs) which configures schema constraints, cascading rules, and seeds initial data.
   * **Repository Implementations**: Implements domain contracts using EF Core and generic repository patterns.
   * **Unit of Work**: Groups repositories into a single transaction wrapper to ensure atomic database writes.

3. **[MovieManagement.WebAPI](file:///d:/MovieManagement/MovieManagement.WebAPI)**:
   * **API Controllers**: Exposes RESTful JSON endpoints.
   * **Data Transfer Objects (DTOs)**: Prevents circular JSON serialization and separates internal entity structures from API contracts.
   * **Security**: Integrates JWT authentication and validation.
   * **Swagger Docs**: Configured with JWT authorization header support for convenient API testing.

---

## 📂 Project Structure

```
d:\MovieManagement
├── MovieManagement.slnx                      # Solution definition file
├── MovieManagement.Domain/                   # Core business logic and interfaces
│   ├── Entities/                             # Domain Models
│   │   ├── Actor.cs
│   │   ├── Biography.cs
│   │   ├── Genre.cs
│   │   ├── Movie.cs
│   │   └── User.cs
│   └── Repositories/                         # Data access contracts
│       ├── IActorRepository.cs
│       ├── IBiographyRepository.cs
│       ├── IGenericRepository.cs
│       ├── IGenreRepository.cs
│       ├── IMovieRepository.cs
│       ├── IUnitOfWork.cs
│       └── IUserRepository.cs
│
├── MovieManagement.DataAccess/               # Infrastructure & EF Core implementations
│   ├── Data/
│   │   └── ApplicationDbContext.cs           # DbContext with relations and seed data
│   └── Repositories/
│       ├── ActorRepository.cs
│       ├── BiographyRepository.cs
│       ├── GenericRepository.cs
│       ├── GenreRepository.cs
│       ├── MovieRepository.cs
│       ├── UnitOfWork.cs
│       └── UserRepository.cs
│
└── MovieManagement.WebAPI/                   # API layer & Startup configuration
    ├── Controllers/
    │   ├── ActorController.cs
    │   ├── AuthController.cs
    │   ├── GenreController.cs
    │   └── MovieController.cs
    ├── DTOs/                                 # API contracts and payloads
    │   ├── ActorCreateDto.cs
    │   ├── ActorDto.cs
    │   ├── ActorShortDto.cs
    │   ├── GenreDto.cs
    │   ├── MovieCreateDto.cs
    │   ├── MovieDto.cs
    │   └── UserDtos.cs
    ├── Program.cs                            # Application startup and service registration
    └── appsettings.json                      # Connection strings and JWT configurations
```

---

## 🗄️ Database Relationships & Seed Data

The database uses SQLite, automatically created and populated during application startup (`dbContext.Database.EnsureCreated()`).

### Relationship Configuration
* **Actor & Biography (1-to-1)**: Configured with a cascade delete constraint in EF. Deleting an actor automatically deletes their biography.
* **Movie & Actor (Many-to-Many)**: Linked through a junction table `MovieActor` (`MovieId` and `ActorId`).
* **Movie & Genre (Many-to-Many)**: Linked through a junction table `MovieGenre` (`MovieId` and `GenreId`).

### Seed Data
The database is pre-seeded with the following records:
* **Genres**: Drama, Sci-Fi, Action
* **Actors**: Tom Hanks, Leonardo DiCaprio, Morgan Freeman
* **Biographies**: Associated biographies for each seeded actor
* **Movies**: Forrest Gump, Inception, The Shawshank Redemption
* **Default Users**:
  * **Admin**: Username: `admin` | Password: `password123`
  * **User**: Username: `user` | Password: `password123`

---

## 🔑 Authentication & Authorization

Secured endpoints require a `Bearer` token in the HTTP `Authorization` header. You can obtain a token by sending a login request using the default credentials.

* **Admin Role**: Required for creating, updating, or deleting resources.
* **User Role / Anonymous**: Allowed to read resource data.

---

## 📋 API Endpoints Reference

### 🔐 Authentication

| Endpoint | Method | Authentication | Payload | Description |
| :--- | :--- | :--- | :--- | :--- |
| `/api/Auth/register` | `POST` | None | `UserRegisterDto` | Registers a new user. Roles can be specified as `Admin` or `User`. |
| `/api/Auth/login` | `POST` | None | `UserLoginDto` | Log in and retrieve a JWT bearer token. |

### 🎬 Movies

| Endpoint | Method | Authentication | Role Required | Description |
| :--- | :--- | :--- | :--- | :--- |
| `/api/Movie` | `GET` | None | None | Retrieves all movies. Supports query search parameter `?name=...`. |
| `/api/Movie/{id}` | `GET` | None | None | Retrieves a single movie by its ID, loading its genre(s) and cast list. |
| `/api/Movie` | `POST` | JWT | `Admin` | Creates a new movie. Allows linking actor IDs and genre IDs. |
| `/api/Movie/{id}` | `DELETE` | JWT | `Admin` | Deletes a movie. |

### 🎭 Actors

| Endpoint | Method | Authentication | Role Required | Description |
| :--- | :--- | :--- | :--- | :--- |
| `/api/Actor` | `GET` | None | None | Retrieves all actors, including their biography and movie lists. Optional query search parameter `?name=...`. |
| `/api/Actor/{id}` | `GET` | None | None | Retrieves a single actor by their ID. |
| `/api/Actor` | `POST` | JWT | `Admin` | Creates a new actor and creates their biography. |
| `/api/Actor/{id}` | `PUT` | JWT | `Admin` | Updates actor name and/or biography text. |
| `/api/Actor/{id}` | `DELETE` | JWT | `Admin` | Deletes an actor and their associated biography. |

### 🏷️ Genres

| Endpoint | Method | Authentication | Role Required | Description |
| :--- | :--- | :--- | :--- | :--- |
| `/api/Genre` | `GET` | None | None | Retrieves all genres. Supports query parameter `?name=...`. |
| `/api/Genre` | `POST` | JWT | `Admin` | Creates a new genre. |

---

## 🚀 Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Steps to Run

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

3. **Run the API application**:
   ```bash
   dotnet run --project MovieManagement.WebAPI
   ```

4. **Access the Application**:
   * The application starts listening by default. Once it's running, you will be redirected to the Swagger documentation page:
     `https://localhost:7119/swagger` or `http://localhost:5262/swagger` (verify ports in output logs).

---

## 🔒 Security & Optimization Recommendations

1. **Password Hashing (Production)**:
   Currently, [User.cs](file:///d:/MovieManagement/MovieManagement.Domain/Entities/User.cs) uses simple, unsalted SHA256 hashing. It is strongly recommended to upgrade this to a work-factor secure hasher (such as PBKDF2, BCrypt, or Argon2) before deploying to production.
2. **Missing Update Endpoint for Movies**:
   Add a `PUT` endpoint inside [MovieController.cs](file:///d:/MovieManagement/MovieManagement.WebAPI/Controllers/MovieController.cs) to allow updating movie details (such as title, description, cast, and genres) after creation.
3. **Repository Instantiations**:
   The current [UnitOfWork.cs](file:///d:/MovieManagement/MovieManagement.DataAccess/Repositories/UnitOfWork.cs) instantiates repositories directly using the `new` keyword. A cleaner, more modular approach is injecting individual repositories or using a dependency injection container.
4. **Validation Layer**:
   DTO payloads can benefit from structural validations using libraries like `FluentValidation` to validate input length, range, and format prior to reaching the domain models.
