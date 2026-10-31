# Movie Ticket Booking System

## Project Overview

The **Movie Ticket Booking System** is an ASP.NET Core Web API application developed using C#, .NET 10, Entity Framework Core 10, and SQL Server.

The application provides APIs to manage movies, theatres, screens, seats, shows, and customer bookings.

Customers can view available shows, check seat availability, book seats, retrieve booking details, and cancel bookings. The system automatically updates seat availability when a booking is created or cancelled.

The project follows a layered architecture:

**Controller → Service → Repository → DbContext → SQL Server**

Each layer has a separate responsibility, making the application easier to maintain and understand.

## Technologies Used

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server Express
- LINQ
- Visual Studio
- Scalar / OpenAPI
- Swagger-compatible API documentation
- Git and GitHub

## Project Structure

```text
MovieTicketBooking/
│
├── Controllers/
│   ├── MoviesController.cs
│   ├── TheatresController.cs
│   ├── ScreensController.cs
│   ├── SeatsController.cs
│   ├── ShowsController.cs
│   ├── ShowSeatsController.cs
│   └── BookingsController.cs
│
├── Models/
│   ├── Movie.cs
│   ├── Theatre.cs
│   ├── Screen.cs
│   ├── Seat.cs
│   ├── Show.cs
│   ├── ShowSeat.cs
│   ├── Booking.cs
│   └── BookingSeat.cs
│
├── DTOs/
│   ├── Movies/
│   ├── Theatres/
│   ├── Screens/
│   ├── Seats/
│   ├── Shows/
│   ├── ShowSeats/
│   ├── Bookings/
│   └── PagedResult.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
│
├── Services/
│   ├── Interfaces/
│   └── Implementations/
│
├── Middleware/
│   └── GlobalExceptionHandler.cs
│
├── Migrations/
│
├── Properties/
│
├── appsettings.json
├── Program.cs
├── MovieTicketBooking.csproj
├── README.md
└── docs.md
```

## Folder Responsibilities

### 1. Controllers

The Controllers folder contains the API endpoints.

Controllers receive HTTP requests, call the appropriate service methods, and return HTTP responses.

The controllers do not directly communicate with the database.

| Controller | Responsibility |
|---|---|
| MoviesController | Movie management |
| TheatresController | Theatre management |
| ScreensController | Screen management |
| SeatsController | Seat management |
| ShowsController | Show scheduling and management |
| ShowSeatsController | Show seat availability |
| BookingsController | Booking creation, retrieval and cancellation |

### 2. Models

The Models folder contains the main entity classes representing the database tables.

Each model defines the properties and relationships required for the application.

| Model | Responsibility |
|---|---|
| Movie | Stores movie information |
| Theatre | Stores theatre information |
| Screen | Represents screens belonging to theatres |
| Seat | Represents physical seats belonging to screens |
| Show | Represents a scheduled movie show |
| ShowSeat | Tracks the availability of each seat for a particular show |
| Booking | Stores customer booking information |
| BookingSeat | Stores the seats included in a booking |

### 3. DTOs

DTO stands for Data Transfer Object.

The DTOs folder contains request and response classes used to transfer data between clients and the API.

DTOs separate the API request and response structures from the database entity models.

Subfolders:

- **Movies:** Movie creation, update and response DTOs.
- **Theatres:** Theatre creation, update and response DTOs.
- **Screens:** Screen creation, update and response DTOs.
- **Seats:** Seat creation, update, bulk creation and response DTOs.
- **Shows:** Show creation, update and response DTOs.
- **ShowSeats:** Show seat availability response DTOs.
- **Bookings:** Booking creation and booking response DTOs.

`PagedResult.cs` provides a common structure for paginated API responses.

### 4. Data

The Data folder contains `ApplicationDbContext.cs`.

ApplicationDbContext is the connection between Entity Framework Core and the SQL Server database.

Responsibilities:

- Defines DbSet properties for the eight entities.
- Configures entity relationships.
- Defines primary keys and unique indexes.
- Configures check constraints.
- Configures decimal precision for monetary values.
- Defines delete behaviours.

### 5. Repositories

The Repositories folder contains database access logic.

It is divided into two folders.

**Interfaces**

Contains repository contracts that define the database operations available for each entity.

**Implementations**

Contains the actual implementation of those operations using Entity Framework Core.

Examples include:

- Retrieving records using `ToListAsync()`.
- Retrieving individual records using `FirstOrDefaultAsync()`.
- Checking records using `AnyAsync()`.
- Adding records using `AddAsync()`.
- Saving changes using `SaveChangesAsync()`.

Repositories are responsible for communicating with the DbContext.

### 6. Services

The Services folder contains the application business logic.

It is divided into two folders.

**Interfaces**

Contains service contracts that define the operations available to controllers.

**Implementations**

Contains the actual implementation of the business rules.

Examples:

- Validating movie and theatre information.
- Checking whether a screen already exists.
- Preventing overlapping shows.
- Generating ShowSeat records when a show is created.
- Checking seat availability before booking.
- Calculating booking amounts.
- Updating seat availability during cancellation.
- Mapping entity models into response DTOs.

Services use repository interfaces to access database operations.

### 7. Middleware

The Middleware folder contains `GlobalExceptionHandler.cs`.

It provides centralized exception handling for exceptions that are not handled by individual controllers.

It uses:

- `IExceptionHandler`
- `ProblemDetails`
- `IProblemDetailsService`
- `ILogger`

Expected exception mappings include:

| Exception | HTTP Status |
|---|---|
| KeyNotFoundException | 404 Not Found |
| ArgumentException | 400 Bad Request |
| InvalidOperationException | 409 Conflict |
| Other unexpected exceptions | 500 Internal Server Error |

Unexpected internal exception details are logged rather than returned to the client.

### 8. Migrations

The Migrations folder contains Entity Framework Core migration files.

Migrations track database schema changes based on the entity models and DbContext configuration.

They are used to create and update the SQL Server database.

### 9. Program.cs

Program.cs is the application's entry point.

Responsibilities:

- Registers controllers.
- Configures the SQL Server database connection.
- Registers repository and service implementations with dependency injection.
- Configures OpenAPI and Scalar.
- Registers centralized exception handling.
- Configures the HTTP request pipeline.
- Maps API controllers.

### 10. appsettings.json

Stores application configuration, including the SQL Server connection string.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Database Design

The application contains eight main database tables.

| Table | Description |
|---|---|
| Movies | Movie information |
| Theatres | Theatre information |
| Screens | Screens belonging to theatres |
| Seats | Physical seats belonging to screens |
| Shows | Scheduled movie shows |
| ShowSeats | Seat availability for individual shows |
| Bookings | Customer booking information |
| BookingSeats | Seats associated with individual bookings |

### Entity Relationships

- One Theatre can have multiple Screens.
- One Screen can have multiple Seats.
- One Movie can have multiple Shows.
- One Screen can have multiple Shows.
- One Show can have multiple ShowSeats.
- One Seat can be associated with multiple ShowSeats across different shows.
- One Show can have multiple Bookings.
- One Booking can have multiple BookingSeats.
- One Seat can be associated with multiple BookingSeats across different bookings.

### Important Database Constraints

- Screen names are unique within a theatre.
- Seat coordinates are unique within a screen.
- A seat can appear only once for a particular show.
- Booking references are unique.
- Show end time must be greater than show start time.
- Ticket prices and booking amounts cannot be negative.
- Foreign key relationships protect dependent records from unsafe deletion.

## Application Workflow

### 1. Movie Management

The administrator can create, retrieve, update, search and delete movies.

Movie information includes:

- Title
- Description
- Duration
- Release date
- Language
- Genre

Movie deletion is restricted when dependent shows exist.

### 2. Theatre Management

The administrator can create and manage theatres.

Each theatre stores its name and location.

A theatre can contain multiple screens.

### 3. Screen Management

Screens are created under existing theatres.

Each screen has a screen name and theatre association.

Duplicate screen names within the same theatre are not allowed.

### 4. Seat Management

Physical seats are created under screens.

Seats can be created individually or through the bulk seat creation endpoint.

Each seat contains:

- Row name
- Seat number
- Seat type
- Screen association

Duplicate seat coordinates within a screen are prevented.

### 5. Show Scheduling

A show connects a movie with a screen and a scheduled time.

The system validates:

- Movie existence.
- Screen existence.
- End time must be later than start time.
- Ticket price must not be negative.
- Another show must not overlap on the same screen.

When a show is created, the system generates a ShowSeat record for every physical seat configured for that screen.

Initially, every generated ShowSeat has the status `Available`.

### 6. Show Seat Availability

ShowSeats represent the availability of physical seats for a particular show.

The same physical seat can be available for one show and booked for another show.

The application provides endpoints to retrieve:

- All seats for a show.
- Available seats.
- Booked seats.

### 7. Booking Creation

The customer provides:

- Customer name.
- Email.
- Phone number.
- Show ID.
- Selected seat IDs.

The application:

1. Checks whether the show exists.
2. Checks whether at least one seat is selected.
3. Checks for duplicate seat IDs in the request.
4. Verifies that the selected seats belong to the requested show.
5. Checks that all selected seats are available.
6. Calculates the total amount using the show's ticket price.
7. Creates the Booking record.
8. Creates the corresponding BookingSeat records.
9. Changes the selected ShowSeat statuses to `Booked`.
10. Saves the changes inside a database transaction.

The total amount is calculated by the server and is not accepted from the customer request.

### 8. Booking Retrieval

The application supports:

- Retrieving all bookings.
- Filtering bookings by show.
- Filtering bookings by status.
- Retrieving a booking using its ID.
- Retrieving bookings associated with a particular show.

### 9. Booking Cancellation

When a confirmed booking is cancelled:

1. The application retrieves the booking.
2. It checks whether the booking exists.
3. It checks whether the booking is currently confirmed.
4. It changes the booking status to `Cancelled`.
5. It changes the associated ShowSeat statuses back to `Available`.
6. It saves the changes inside a database transaction.

The booking and its booking-seat records are preserved for historical reference.

## API Endpoints

### Movies

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/movies` | Create a movie |
| GET | `/api/movies` | Retrieve movies with pagination and optional title filter |
| GET | `/api/movies/{id}` | Retrieve a movie |
| PUT | `/api/movies/{id}` | Update a movie |
| DELETE | `/api/movies/{id}` | Delete a movie |
| GET | `/api/movies/search?title=leo` | Search movies by title |

### Theatres

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/theatres` | Create a theatre |
| GET | `/api/theatres` | Retrieve theatres |
| GET | `/api/theatres/{id}` | Retrieve a theatre |
| PUT | `/api/theatres/{id}` | Update a theatre |
| DELETE | `/api/theatres/{id}` | Delete a theatre |

### Screens

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/screens` | Create a screen |
| GET | `/api/screens` | Retrieve screens, optionally filtered by theatre |
| GET | `/api/screens/{id}` | Retrieve a screen |
| GET | `/api/screens/theatre/{theatreId}` | Retrieve screens of a theatre |
| PUT | `/api/screens/{id}` | Update a screen |
| DELETE | `/api/screens/{id}` | Delete a screen |

### Seats

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/seats` | Create a seat |
| POST | `/api/seats/bulk` | Create multiple seats |
| GET | `/api/seats/screen/{screenId}` | Retrieve seats of a screen |
| GET | `/api/seats/{id}` | Retrieve a seat |
| PUT | `/api/seats/{id}` | Update a seat |
| DELETE | `/api/seats/{id}` | Delete a seat |

### Shows

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/shows` | Create a show |
| GET | `/api/shows` | Retrieve shows with optional filters |
| GET | `/api/shows/{id}` | Retrieve a show |
| GET | `/api/shows/movie/{movieId}` | Retrieve shows of a movie |
| GET | `/api/shows/theatre/{theatreId}` | Retrieve shows at a theatre |
| GET | `/api/shows/search?date=YYYY-MM-DD` | Search shows by date |
| PUT | `/api/shows/{id}` | Update a show |
| DELETE | `/api/shows/{id}` | Delete a show |

### Show Seats

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/shows/{showId}/seats` | Retrieve all seats for a show |
| GET | `/api/shows/{showId}/seats/available` | Retrieve available seats |
| GET | `/api/shows/{showId}/seats/booked` | Retrieve booked seats |

### Bookings

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/bookings` | Create a booking |
| GET | `/api/bookings` | Retrieve bookings with optional filters |
| GET | `/api/bookings/{id}` | Retrieve a booking |
| GET | `/api/bookings/show/{showId}` | Retrieve bookings for a show |
| PUT | `/api/bookings/{id}/cancel` | Cancel a booking |

## Running the Project

### Prerequisites

- Visual Studio with .NET 10 support.
- .NET 10 SDK.
- SQL Server Express.
- SQL Server Management Studio (SSMS).
- Entity Framework Core tools.

### Step 1: Clone the Repository

```powershell
git clone <your-github-repository-url>
```

### Step 2: Open the Project

Open `MovieTicketBooking.sln` or the project folder in Visual Studio.

### Step 3: Configure the Database

Update the `DefaultConnection` value in `appsettings.json` according to your SQL Server instance.

### Step 4: Restore Packages

```powershell
dotnet restore
```

### Step 5: Apply Migrations

```powershell
dotnet ef database update
```

### Step 6: Build the Project

```powershell
dotnet build
```

### Step 7: Run the Project

```powershell
dotnet run
```

### Step 8: Open API Documentation

Open the Scalar URL displayed by the application.

Usually:

```text
https://localhost:<port>/scalar/v1
```

Use Scalar or Postman to test the endpoints.

## Git Workflow

The project was developed incrementally using Git commits.

Each commit represents a separate development stage. Refer to `docs.md` for the detailed implementation history of all commits.

## Scope and Limitations

This is Version 1 of the Movie Ticket Booking System.

The current implementation focuses on the backend Web API and database operations.

The following are outside the current scope:

- User authentication and authorization.
- JWT token implementation.
- Online payment gateway integration.
- Frontend application.
- Email or SMS notifications.

## Conclusion

The Movie Ticket Booking System demonstrates a layered ASP.NET Core Web API application with Entity Framework Core and SQL Server.

It covers entity relationships, CRUD operations, business validation, show scheduling, seat availability, booking creation, booking cancellation, database transactions, and centralized exception handling.