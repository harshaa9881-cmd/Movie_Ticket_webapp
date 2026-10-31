# Movie Ticket Booking System – Development Documentation

## Project Development History

This document describes the development process of the Movie Ticket Booking System commit by commit.

The project was implemented incrementally, following the Controller → Service → Repository → DbContext architecture.

Each commit introduced a specific group of functionalities while maintaining the coding style and structure of the reference Web API project.

---

# Commit 1 – Project Setup

**Commit message:**

```text
chore: initialize movie ticket booking web api
```

### Objective

Create the initial ASP.NET Core Web API project and configure the required packages and database connection.

### Changes Made

1. Created an ASP.NET Core Web API project using .NET 10.
2. Selected the controller-based Web API template.
3. Removed the default WeatherForecast example files.
4. Installed the required NuGet packages.
5. Configured the SQL Server connection string.
6. Configured Scalar and OpenAPI.
7. Added a `.gitignore` file.
8. Verified that the application could build and run.

### Packages Installed

| Package | Purpose |
|---|---|
| Microsoft.AspNetCore.OpenApi | OpenAPI documentation support |
| Microsoft.EntityFrameworkCore.Design | EF Core design-time operations |
| Microsoft.EntityFrameworkCore.SqlServer | SQL Server database provider |
| Microsoft.EntityFrameworkCore.Tools | EF Core migration tools |
| Scalar.AspNetCore | API documentation interface |

### Configuration

The connection string was added to `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Result

The initial Web API project was created and configured with the required development dependencies.

---

# Commit 2 – Models and DbContext

**Commit message:**

```text
feat: add movie booking models and dbcontext
```

### Objective

Create the entity classes required for the Movie Ticket Booking System and configure their database relationships.

### Changes Made

Created eight model classes.

#### 1. Movie.cs

Stores movie information.

Properties include:

- MovieId
- Title
- Description
- DurationMinutes
- ReleaseDate
- Language
- Genre
- Shows

#### 2. Theatre.cs

Stores theatre information.

Properties include:

- TheatreId
- Name
- Location
- Screens

#### 3. Screen.cs

Represents a screen belonging to a theatre.

Properties include:

- ScreenId
- TheatreId
- ScreenName
- Theatre
- Seats
- Shows

#### 4. Seat.cs

Represents a physical seat belonging to a screen.

Properties include:

- SeatId
- ScreenId
- RowName
- SeatNumber
- SeatType
- Screen
- ShowSeats
- BookingSeats

#### 5. Show.cs

Represents a scheduled movie show.

Properties include:

- ShowId
- MovieId
- ScreenId
- StartTime
- EndTime
- TicketPrice
- Movie
- Screen
- ShowSeats
- Bookings

#### 6. ShowSeat.cs

Tracks the availability of a physical seat for a particular show.

Properties include:

- ShowSeatId
- ShowId
- SeatId
- Status
- Show
- Seat

The default status is `Available`.

#### 7. Booking.cs

Stores customer booking information.

Properties include:

- BookingId
- BookingReference
- ShowId
- CustomerName
- CustomerEmail
- CustomerPhone
- BookingDate
- TotalAmount
- Status
- Show
- BookingSeats

The default booking status is `Confirmed`.

#### 8. BookingSeat.cs

Stores the seats included in a booking.

Properties include:

- BookingSeatId
- BookingId
- SeatId
- Price
- Booking
- Seat

### ApplicationDbContext.cs

Created the DbContext inside the Data folder.

Configured:

- Eight DbSet properties.
- Primary key relationships.
- One-to-many relationships.
- Unique indexes.
- Check constraints.
- Decimal precision for monetary values.
- Required properties and maximum string lengths.
- Restrictive delete behaviour for dependent records.

### Important Constraints

- Screen name must be unique within a theatre.
- Row name and seat number must be unique within a screen.
- ShowSeat must be unique for each ShowId and SeatId combination.
- BookingReference must be unique.
- Show EndTime must be greater than StartTime.
- TicketPrice and booking amounts must not be negative.
- BookingSeat price must not be negative.

### Result

All eight entities and their database relationships were defined.

---

# Commit 3 – Database Creation Using EF Core Migrations

**Commit message:**

```text
feat: create database using ef core migrations
```

### Objective

Create the SQL Server database using the entity models and DbContext configuration.

### Changes Made

1. Installed the EF Core command-line tool if required.
2. Generated the initial migration.
3. Applied the migration to SQL Server.
4. Verified the created database and tables.

### Commands Used

Install the EF Core tool if it is not already installed:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.14
```

Create the migration:

```powershell
dotnet ef migrations add InitialCreate
```

Apply the migration:

```powershell
dotnet ef database update
```

### Database Created

```text
MovieTicketBookingDB
```

### Tables Created

1. Movies
2. Theatres
3. Screens
4. Seats
5. Shows
6. ShowSeats
7. Bookings
8. BookingSeats

The migration history is maintained in `__EFMigrationsHistory`.

### Result

The database schema was generated from the EF Core models and configured successfully.

---

# Commit 4 – Movie CRUD APIs

**Commit message:**

```text
feat: implement movie CRUD APIs
```

### Objective

Implement the complete movie management functionality using the Controller, Service and Repository layers.

### Changes Made

#### 1. Movie DTOs

Created:

- `CreateMovieRequest.cs`
- `UpdateMovieRequest.cs`
- `MovieResponse.cs`
- `PagedResult.cs`

Added request validation attributes such as:

- Required
- StringLength
- Range

#### 2. Movie Repository

Created:

- `IMovieRepository.cs`
- `MovieRepository.cs`

Implemented database operations:

- Retrieve all movies.
- Retrieve movies using pagination.
- Retrieve a movie by ID.
- Add a movie.
- Update a movie.
- Delete a movie.
- Search movies by title.
- Check whether a movie has dependent shows.

Used asynchronous EF Core methods.

#### 3. Movie Service

Created:

- `IMovieService.cs`
- `MovieService.cs`

Implemented:

- Business logic.
- DTO mapping.
- Movie creation.
- Movie retrieval.
- Movie update.
- Movie search.
- Movie deletion validation.

Movie deletion is prevented if dependent shows exist.

#### 4. MoviesController

Created endpoints for movie management.

| Method | Endpoint | Function |
|---|---|---|
| POST | `/api/movies` | Create movie |
| GET | `/api/movies` | Get paginated movies |
| GET | `/api/movies/{id}` | Get movie by ID |
| PUT | `/api/movies/{id}` | Update movie |
| DELETE | `/api/movies/{id}` | Delete movie |
| GET | `/api/movies/search?title=leo` | Search by title |

The GET endpoint supports `pageNumber`, `pageSize`, and an optional title filter.

### Dependency Injection

Registered the movie repository and service in Program.cs.

### Result

Movie CRUD operations, pagination, search and dependency validation were implemented.

---

# Commit 5 – Theatre and Screen APIs

**Commit message:**

```text
feat: implement theatre and screen APIs
```

### Objective

Implement theatre and screen management functionality.

### Changes Made

#### 1. Theatre DTOs

Created:

- `CreateTheatreRequest.cs`
- `UpdateTheatreRequest.cs`
- `TheatreResponse.cs`

#### 2. Screen DTOs

Created:

- `CreateScreenRequest.cs`
- `UpdateScreenRequest.cs`
- `ScreenResponse.cs`

#### 3. Theatre Repository

Created:

- `ITheatreRepository.cs`
- `TheatreRepository.cs`

Implemented:

- Retrieve all theatres.
- Retrieve theatre by ID.
- Add theatre.
- Update theatre.
- Delete theatre.
- Check whether a theatre has screens.

#### 4. Screen Repository

Created:

- `IScreenRepository.cs`
- `ScreenRepository.cs`

Implemented:

- Retrieve all screens.
- Filter screens by theatre.
- Retrieve screen by ID.
- Check theatre existence.
- Check duplicate screen names.
- Add screen.
- Update screen.
- Delete screen.
- Check whether seats or shows depend on a screen.

#### 5. Services

Created:

- `ITheatreService.cs`
- `TheatreService.cs`
- `IScreenService.cs`
- `ScreenService.cs`

Implemented business validation.

Examples:

- A screen cannot be created for a nonexistent theatre.
- Duplicate screen names within a theatre are prevented.
- A theatre cannot be deleted when screens exist.
- A screen cannot be deleted when seats or shows depend on it.

#### 6. Controllers

Created:

- `TheatresController.cs`
- `ScreensController.cs`

Implemented CRUD endpoints and screen retrieval by theatre.

### Result

Theatre and screen management functionality was implemented with validation and dependency checks.

---

# Commit 6 – Seat APIs and Bulk Seat Creation

**Commit message:**

```text
feat: implement seat APIs and bulk seat creation
```

### Objective

Implement physical seat management and support creating multiple seats in one request.

### Changes Made

#### 1. Seat DTOs

Created:

- `CreateSeatRequest.cs`
- `UpdateSeatRequest.cs`
- `BulkCreateSeatsRequest.cs`
- `CreateSeatItemRequest.cs`
- `SeatResponse.cs`

The bulk request contains a ScreenId and a collection of seat details.

#### 2. Seat Repository

Created:

- `ISeatRepository.cs`
- `SeatRepository.cs`

Implemented:

- Retrieve seats by screen.
- Retrieve seat by ID.
- Check screen existence.
- Check duplicate seat coordinates.
- Add a seat.
- Add multiple seats.
- Update a seat.
- Delete a seat.
- Check whether bookings or show-seat records depend on a seat.

#### 3. Seat Service

Created:

- `ISeatService.cs`
- `SeatService.cs`

Implemented:

- Validate screen existence.
- Validate duplicate seat coordinates.
- Validate duplicate entries within a bulk request.
- Create an individual seat.
- Create multiple seats.
- Update seat information.
- Prevent unsafe changes to seat coordinates after show or booking references exist.
- Prevent deletion when dependent records exist.
- Map entity models to response DTOs.

#### 4. SeatsController

Created endpoints:

| Method | Endpoint | Function |
|---|---|---|
| POST | `/api/seats` | Create one seat |
| POST | `/api/seats/bulk` | Create multiple seats |
| GET | `/api/seats/screen/{screenId}` | Get seats by screen |
| GET | `/api/seats/{id}` | Get seat by ID |
| PUT | `/api/seats/{id}` | Update seat |
| DELETE | `/api/seats/{id}` | Delete seat |

### Result

Physical seat management and bulk seat creation were implemented.

---

# Commit 7 – Show Scheduling and Validation

**Commit message:**

```text
feat: implement show scheduling and validation
```

### Objective

Implement movie show scheduling and validate show details before saving them.

### Changes Made

#### 1. Show DTOs

Created:

- `CreateShowRequest.cs`
- `UpdateShowRequest.cs`
- `ShowResponse.cs`

The request DTOs contain:

- MovieId
- ScreenId
- StartTime
- EndTime
- TicketPrice

#### 2. Show Repository

Created:

- `IShowRepository.cs`
- `ShowRepository.cs`

Implemented:

- Retrieve all shows.
- Retrieve show by ID.
- Retrieve shows by movie.
- Retrieve shows by theatre.
- Retrieve shows by date.
- Check movie existence.
- Check screen existence.
- Check overlapping shows.
- Check show dependencies.
- Add show.
- Update show.
- Delete show.

#### 3. Show Overlap Validation

Implemented overlap checking for shows scheduled on the same screen.

The repository checks:

```csharp
return await _context.Shows.AnyAsync(s =>
    s.ScreenId == screenId &&
    s.ShowId != excludeShowId &&
    startTime < s.EndTime &&
    endTime > s.StartTime);
```

This prevents overlapping show schedules on the same screen.

#### 4. Show Service

Created:

- `IShowService.cs`
- `ShowService.cs`

Implemented:

- Validate show time.
- Validate ticket price.
- Check movie existence.
- Check screen existence.
- Prevent overlapping shows.
- Map show information into response DTOs.
- Prevent changing the screen after dependent show-seat or booking records exist.
- Prevent deleting shows with dependent records.

#### 5. ShowsController

Implemented:

| Method | Endpoint | Function |
|---|---|---|
| POST | `/api/shows` | Create show |
| GET | `/api/shows` | Get shows with optional filters |
| GET | `/api/shows/{id}` | Get show by ID |
| GET | `/api/shows/movie/{movieId}` | Get shows by movie |
| GET | `/api/shows/theatre/{theatreId}` | Get shows by theatre |
| GET | `/api/shows/search?date=YYYY-MM-DD` | Search shows by date |
| PUT | `/api/shows/{id}` | Update show |
| DELETE | `/api/shows/{id}` | Delete show |

### Result

Show scheduling, filtering and overlap validation were implemented.

---

# Commit 8 and 9 – Show Seat Availability and Booking Creation

**Combined commit message:**

```text
feat: implement show seat availability and booking creation
```

### Objective

Generate show-specific seat records, provide seat availability APIs and implement customer booking creation.

The original ShowSeat and Booking Creation tasks were combined into one commit.

## Part A – ShowSeat Generation

### Changes Made

Extended the show repository with a method to retrieve all physical seats belonging to a screen.

Added the method:

```csharp
Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId);
```

Updated `ShowService.AddShowAsync()`.

After validating the show details:

1. Retrieve all physical seats belonging to the selected screen.
2. Check whether the screen contains seats.
3. Generate one ShowSeat record for every physical seat.
4. Set every generated ShowSeat status to `Available`.
5. Save the show and its ShowSeat records.

This means every show has its own independent seat availability records.

## Part B – ShowSeat APIs

Created:

- `ShowSeatResponse.cs`
- `IShowSeatRepository.cs`
- `ShowSeatRepository.cs`
- `IShowSeatService.cs`
- `ShowSeatService.cs`
- `ShowSeatsController.cs`

Implemented:

- Retrieve all seats for a show.
- Retrieve available seats.
- Retrieve booked seats.
- Map seat details into ShowSeatResponse.

The response includes:

- SeatId
- RowName
- SeatNumber
- SeatLabel
- SeatType
- Price
- Status

### Endpoints

| Method | Endpoint | Function |
|---|---|---|
| GET | `/api/shows/{showId}/seats` | Get all seats |
| GET | `/api/shows/{showId}/seats/available` | Get available seats |
| GET | `/api/shows/{showId}/seats/booked` | Get booked seats |

## Part C – Booking Creation

### Booking DTOs

Created:

- `CreateBookingRequest.cs`
- `BookingResponse.cs`

The booking request accepts:

- CustomerName
- CustomerEmail
- CustomerPhone
- ShowId
- SeatIds

### Booking Repository

Created:

- `IBookingRepository.cs`
- `BookingRepository.cs`

Implemented booking creation.

The repository:

1. Begins a database transaction.
2. Retrieves the show and selected ShowSeat records.
3. Verifies that all requested seats belong to the show.
4. Checks that the selected seats are available.
5. Calculates the total amount using the show ticket price.
6. Creates the Booking entity.
7. Creates the BookingSeat entities.
8. Changes selected ShowSeat statuses to `Booked`.
9. Saves all changes.
10. Commits the transaction.

### Booking Service

Created:

- `IBookingService.cs`
- `BookingService.cs`

Implemented:

- Show existence validation.
- Seat selection validation.
- Duplicate seat ID validation.
- Booking reference generation.
- Booking entity creation.
- Booking response mapping.

The booking reference is generated by the application.

The booking total is calculated on the server:

```text
Total Amount = Ticket Price × Number of Selected Seats
```

### BookingsController

Added:

```text
POST /api/bookings
```

The endpoint creates a booking and returns the created booking response.

### Result

ShowSeat generation, seat availability retrieval and booking creation were implemented.

Booking creation uses a database transaction to keep the booking, booking-seat and show-seat updates together.

---

# Commit 10 – Booking Retrieval and Cancellation

**Commit message:**

```text
feat: implement booking retrieval and cancellation
```

### Objective

Extend the booking functionality to retrieve booking records and cancel confirmed bookings.

### Changes Made

## 1. Updated BookingResponse

Expanded the response DTO to include:

- BookingId
- BookingReference
- ShowId
- CustomerName
- CustomerEmail
- CustomerPhone
- BookingDate
- TotalAmount
- Status
- Seats

## 2. Updated Booking Repository

Extended:

- `IBookingRepository.cs`
- `BookingRepository.cs`

Added the following operations:

```csharp
Task<List<Booking>> GetAllAsync(int? showId, string? status);

Task<Booking?> GetByIdAsync(int bookingId);

Task<List<Booking>> GetByShowIdAsync(int showId);

Task<Booking> CancelBookingAsync(int bookingId);
```

### Booking Retrieval

Implemented:

- Retrieve all bookings.
- Filter bookings by show ID.
- Filter bookings by booking status.
- Retrieve a booking by ID.
- Retrieve all bookings for a particular show.

BookingSeat and Seat information is included when retrieving bookings.

### Booking Cancellation

Implemented cancellation using a database transaction.

The cancellation process:

1. Begins a database transaction.
2. Retrieves the booking and its associated BookingSeat records.
3. Checks whether the booking exists.
4. Checks whether the booking status is `Confirmed`.
5. Changes the booking status to `Cancelled`.
6. Retrieves the associated ShowSeat records.
7. Changes their status back to `Available`.
8. Saves the changes.
9. Commits the transaction.
10. Returns the updated booking.

Cancelled bookings are preserved for historical reference.

## 3. Updated Booking Service

Extended:

- `IBookingService.cs`
- `BookingService.cs`

Added:

- Retrieve all bookings.
- Retrieve booking by ID.
- Retrieve bookings by show ID.
- Cancel booking.
- Map booking entities to BookingResponse.

## 4. Updated BookingsController

Added endpoints:

| Method | Endpoint | Function |
|---|---|---|
| GET | `/api/bookings` | Retrieve bookings |
| GET | `/api/bookings/{id}` | Retrieve booking by ID |
| GET | `/api/bookings/show/{showId}` | Retrieve bookings by show |
| PUT | `/api/bookings/{id}/cancel` | Cancel booking |

The existing POST booking endpoint was retained.

### Result

Booking retrieval and cancellation functionality were implemented.

The system now releases seats when a confirmed booking is cancelled, without deleting the booking history.

---

# Commit 11 – Global Exception Handling and Final Validation

**Commit message:**

```text
feat: add global exception handling and final validation
```

### Objective

Add centralized exception handling and perform final integration testing of the application.

### Changes Made

## 1. GlobalExceptionHandler.cs

Created the Middleware folder and added:

```text
Middleware/GlobalExceptionHandler.cs
```

The class implements:

```csharp
IExceptionHandler
```

It handles unhandled application exceptions centrally.

### Exception Mapping

| Exception | HTTP Status |
|---|---|
| KeyNotFoundException | 404 Not Found |
| ArgumentException | 400 Bad Request |
| InvalidOperationException | 409 Conflict |
| Unexpected exceptions | 500 Internal Server Error |

### Problem Details

Used `ProblemDetails` to return structured error responses.

The response contains:

- Status
- Title
- Detail
- Instance

Unexpected exceptions are logged using `ILogger`.

Internal exception details are not returned to the client for unexpected server errors.

### 2. Program.cs Updates

Registered:

```csharp
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
```

Configured the middleware:

```csharp
app.UseExceptionHandler();
```

This enables centralized exception handling for unhandled exceptions.

### 3. Final Build and Integration Testing

The final validation stage includes:

- Building the application.
- Testing movie creation and retrieval.
- Testing theatre and screen creation.
- Testing individual and bulk seat creation.
- Testing show scheduling.
- Testing show overlap validation.
- Verifying ShowSeat generation.
- Checking available and booked seats.
- Testing booking creation.
- Checking booking amount calculation.
- Testing booking retrieval.
- Testing booking cancellation.
- Verifying that cancelled seats become available again.
- Testing invalid requests and missing records.

### Result

Centralized exception handling was added, and the application was prepared for end-to-end validation.

---

# Overall Development Summary

The project was developed through 11 planned commits.

| Commit | Feature | Main Deliverables |
|---|---|---|
| 1 | Project Setup | .NET Web API, packages, configuration |
| 2 | Models and DbContext | Eight entity models and relationships |
| 3 | Database Creation | EF Core migration and SQL Server database |
| 4 | Movie APIs | Movie CRUD, search and pagination |
| 5 | Theatre and Screen APIs | Theatre and screen CRUD |
| 6 | Seat APIs | Seat CRUD and bulk seat creation |
| 7 | Show APIs | Show scheduling, filtering and overlap validation |
| 8 + 9 | ShowSeats and Booking Creation | Seat generation, availability and booking |
| 10 | Booking Retrieval and Cancellation | Booking queries and cancellation |
| 11 | Final Validation | Global exception handling and integration testing |

## Important Implementation Concepts

### Layered Architecture

The application separates responsibilities into four main layers:

```text
Client
  |
  v
Controller
  |
  v
Service
  |
  v
Repository
  |
  v
ApplicationDbContext
  |
  v
SQL Server
```

### Dependency Injection

Repository and service implementations are registered in Program.cs using `AddScoped()`.

Controllers depend on service interfaces, and services depend on repository interfaces.

### Entity Framework Core

EF Core is used for:

- Database communication.
- Entity relationships.
- CRUD operations.
- Filtering and searching.
- Asynchronous queries.
- Database migrations.
- Transactions.

### DTO Mapping

Request DTOs receive client data, while response DTOs control the information returned by the API.

Services perform the mapping between entities and response DTOs.

### Database Transactions

Transactions are used for multi-step booking operations.

This ensures that the booking, booking-seat records and seat availability updates are committed together.

Transactions are also used during cancellation to update the booking and release the associated seats together.

### Business Validation

The application includes validations for:

- Duplicate movie-independent identifiers and unique database fields.
- Theatre and screen relationships.
- Duplicate seat coordinates.
- Show scheduling conflicts.
- Seat availability.
- Duplicate seat selections.
- Booking cancellation rules.
- Unsafe deletion of records with dependent data.

## Current Project Scope

The completed Version 1 implementation focuses on backend API functionality.

It does not include:

- Authentication.
- Authorization.
- JWT.
- Payment gateway.
- Frontend UI.
- Email notifications.
- SMS notifications.

## Final Conclusion

The Movie Ticket Booking System was developed incrementally using ASP.NET Core Web API, Entity Framework Core and SQL Server.

The implementation covers the complete backend workflow, beginning with movie and theatre management and continuing through show scheduling, seat availability, booking creation, booking retrieval and cancellation.

The project demonstrates layered architecture, dependency injection, repository and service patterns, DTO mapping, database relationships, validation, EF Core migrations, asynchronous operations and database transactions.