# Movie Ticket Booking System – Development Documentation

This document describes the development history of the Movie Ticket Booking System.

The project was developed incrementally using Git commits. Each commit represents a development stage and introduces specific functionality.

The application uses:

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- JWT Bearer Authentication
- Role-Based Authorization
- User Secrets

The project follows the architecture:

**Controller → Service → Repository → DbContext → SQL Server**

---

## Commit 1 – Project Setup

**Commit Hash:** `cef693f`

**Commit Message:**

```text
chore: initialize movie ticket booking web api
```

### Objective

Create the initial ASP.NET Core Web API project and configure the basic application structure.

### Work Completed

- Created the ASP.NET Core Web API project using .NET 10.
- Configured the project for controller-based API development.
- Removed the default WeatherForecast implementation.
- Installed the required packages for OpenAPI, Entity Framework Core and Scalar.
- Configured the initial application settings.
- Created the initial Git repository structure.
- Added the `.gitignore` file.

### Result

The initial Web API project was created and configured successfully.

---

## Commit 2 – Models and DbContext

**Commit Hash:** `7493dc1`

**Commit Message:**

```text
feat: add movie booking models and dbcontext
```

### Objective

Create the entity models and configure the database context.

### Work Completed

Created the eight main entity models:

1. Movie
2. Theatre
3. Screen
4. Seat
5. Show
6. ShowSeat
7. Booking
8. BookingSeat

Implemented the entity properties and navigation properties required for the database relationships.

Created `ApplicationDbContext.cs`.

Configured:

- DbSet properties.
- Primary keys.
- Entity relationships.
- Required properties.
- Maximum string lengths.
- Unique indexes.
- Check constraints.
- Decimal precision.
- Delete behaviours.

### Result

The database entity structure and DbContext configuration were completed.

---

## Commit 3 – Database Creation Using EF Core Migrations

**Commit Hash:** `3458c17`

**Commit Message:**

```text
feat: create database using ef core migrations
```

### Objective

Create the SQL Server database using Entity Framework Core migrations.

### Work Completed

- Configured the SQL Server connection string.
- Created the initial migration named `InitialCreate`.
- Applied the migration using Entity Framework Core.
- Created the database and its tables.
- Verified the database structure.

### Commands Used

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Result

The database was created successfully using EF Core migrations.

---

## Commit 4 – Movie CRUD APIs

**Commit Hash:** `331cc9c`

**Commit Message:**

```text
feat: implement movie CRUD APIs
```

### Objective

Implement the movie management functionality.

### Work Completed

Created the movie request and response DTOs.

Implemented:

- Create movie.
- Retrieve all movies.
- Retrieve movie by ID.
- Update movie.
- Delete movie.
- Search movies by title.
- Pagination.
- Title filtering.

Created:

- `IMovieRepository`
- `MovieRepository`
- `IMovieService`
- `MovieService`
- `MoviesController`

Added validation to prevent deleting movies that have dependent shows.

### Result

Movie management APIs were implemented using the Controller-Service-Repository flow.

---

## Commit 5 – Theatre and Screen APIs

**Commit Hash:** `e6dc6d6`

**Commit Message:**

```text
feat: implement theatre and screen APIs
```

### Objective

Implement theatre and screen management functionality.

### Work Completed

Created theatre and screen DTOs.

Implemented Theatre APIs:

- Create theatre.
- Retrieve all theatres.
- Retrieve theatre by ID.
- Update theatre.
- Delete theatre.

Implemented Screen APIs:

- Create screen.
- Retrieve all screens.
- Retrieve screen by ID.
- Retrieve screens by theatre.
- Update screen.
- Delete screen.

Created:

- `ITheatreRepository`
- `TheatreRepository`
- `IScreenRepository`
- `ScreenRepository`
- `ITheatreService`
- `TheatreService`
- `IScreenService`
- `ScreenService`
- `TheatresController`
- `ScreensController`

Added validation for duplicate screen names within the same theatre.

Added restrictions to prevent deleting theatres or screens that have dependent records.

### Result

Theatre and screen management functionality was completed.

---

## Commit 6 – Seat APIs and Bulk Seat Creation

**Commit Hash:** `cba0f2c`

**Commit Message:**

```text
feat: implement seat APIs and bulk seat creation
```

### Objective

Implement physical seat management for screens.

### Work Completed

Created seat DTOs:

- CreateSeatRequest
- UpdateSeatRequest
- BulkCreateSeatsRequest
- CreateSeatItemRequest
- SeatResponse

Implemented:

- Create individual seat.
- Bulk-create seats.
- Retrieve seats by screen.
- Retrieve seat by ID.
- Update seat.
- Delete seat.

Created:

- `ISeatRepository`
- `SeatRepository`
- `ISeatService`
- `SeatService`
- `SeatsController`

Added validation to prevent duplicate seat coordinates within a screen.

Added restrictions for updating or deleting seats that have dependent show-seat or booking records.

### Result

Seat management APIs and bulk seat creation were implemented.

---

## Commit 7 – Show Scheduling and Validation

**Commit Hash:** `01b320a`

**Commit Message:**

```text
feat: implement show scheduling and validation
```

### Objective

Implement movie show scheduling and its business validations.

### Work Completed

Created show request and response DTOs.

Implemented:

- Create show.
- Retrieve all shows.
- Retrieve show by ID.
- Retrieve shows by movie.
- Retrieve shows by theatre.
- Search shows by date.
- Update show.
- Delete show.

Created:

- `IShowRepository`
- `ShowRepository`
- `IShowService`
- `ShowService`
- `ShowsController`

Added validations:

- Movie must exist.
- Screen must exist.
- End time must be later than start time.
- Ticket price cannot be negative.
- Shows must not overlap on the same screen.

Added restrictions for changing or deleting shows that have dependent records.

### Result

Show scheduling and validation functionality was completed.

---

## Commit 8 – Show Seat Availability and Booking Creation

**Commit Hash:** `d1494f5`

**Commit Message:**

```text
feat: implement show seat availability and booking creation
```

### Objective

Implement show-specific seat availability and booking creation.

### Work Completed

### Show Seat Availability

Updated the show repository and service to generate ShowSeat records when a show is created.

For every physical seat configured for the screen:

- A ShowSeat record is created.
- The initial status is set to `Available`.

Added ShowSeat functionality to retrieve:

- All seats for a show.
- Available seats.
- Booked seats.

Created:

- `ShowSeatResponse`
- ShowSeat repository.
- ShowSeat service.
- ShowSeatsController.

### Booking Creation

Created booking request and response DTOs.

Implemented:

- Booking creation.
- Seat validation.
- Duplicate seat validation.
- Show and seat relationship validation.
- Seat availability validation.
- Total booking amount calculation.
- Booking reference generation.
- Booking record creation.
- BookingSeat record creation.
- ShowSeat status update.

Created:

- `IBookingRepository`
- `BookingRepository`
- `IBookingService`
- `BookingService`
- `BookingsController`

Used a database transaction to ensure that the booking and seat availability changes are saved together.

### Result

Show seat availability and booking creation functionality were implemented.

---

## Commit 9 – Booking Retrieval and Cancellation

**Commit Hash:** `872893d`

**Commit Message:**

```text
feat: implement booking retrieval and cancellation
```

### Objective

Implement booking retrieval and cancellation functionality.

### Work Completed

Updated the BookingResponse DTO with booking and seat information.

Implemented:

- Retrieve all bookings.
- Retrieve booking by ID.
- Retrieve bookings by show.
- Cancel booking.

Cancellation process:

1. Retrieve the booking.
2. Check whether the booking exists.
3. Check whether the booking is confirmed.
4. Change booking status to `Cancelled`.
5. Change associated ShowSeat statuses to `Available`.
6. Save the changes inside a database transaction.

Booking and BookingSeat records are preserved for historical reference.

### Result

Booking retrieval and cancellation functionality were implemented.

---

## Commit 10 – Global Exception Handling and Final Validation

**Commit Hash:** `b4021f7`

**Commit Message:**

```text
feat: add global exception handling and final validation
```

### Objective

Implement centralized exception handling for the application.

### Work Completed

Created:

`Middleware/GlobalExceptionHandler.cs`

Implemented the `IExceptionHandler` interface.

Used:

- IExceptionHandler
- ProblemDetails
- IProblemDetailsService
- ILogger

Configured exception mappings:

| Exception | HTTP Status |
|---|---|
| KeyNotFoundException | 404 Not Found |
| ArgumentException | 400 Bad Request |
| InvalidOperationException | 409 Conflict |
| Other unexpected exceptions | 500 Internal Server Error |

Registered centralized exception handling in Program.cs.

Updated the HTTP request pipeline to use the exception handler.

### Result

Centralized exception handling was implemented to provide consistent API error responses.

---

## Commit 11 – Program.cs Update

**Commit Hash:** `2e16419`

**Commit Message:**

```text
update:program.cs
```

### Objective

Update the application startup configuration.

### Work Completed

Updated the Program.cs application configuration as part of the integration and startup changes following exception handling.

Program.cs is responsible for configuring the application services and HTTP request pipeline.

### Result

The Program.cs changes were committed separately.

---

## Commit 12 – JWT Authentication

**Commit Hash:** `25a2410`

**Commit Message:**

```text
JWT Authentication added
```

### Objective

Implement user registration, login and JWT token authentication.

### Work Completed

Added the User model to store registered user information.

Created authentication DTOs:

- RegisterDto
- LoginDto

Implemented user repository and service functionality.

Created:

- IUserRepository
- UserRepository
- IUserService
- UserService
- IJwtService
- JwtService
- AuthController

Implemented registration:

1. Receive user registration details.
2. Check whether the email already exists.
3. Hash the password using PasswordHasher.
4. Save the user in the database.

Implemented login:

1. Receive email and password.
2. Retrieve the user by email.
3. Verify the password hash.
4. Generate a JWT token when credentials are valid.
5. Return the token to the client.

Configured JWT Bearer authentication in Program.cs.

Configured token validation for:

- Issuer.
- Audience.
- Token lifetime.
- Signing key.

Added the Users table through an EF Core migration.

Configured the JWT signing key using .NET User Secrets instead of committing it to the repository.

### Result

User registration, login and JWT authentication were implemented.

---

## Commit 13 – Role-Based Authorization

**Commit Hash:** `cccb2e9`

**Commit Message:**

```text
feat : Rolebased authorization added
```

### Objective

Implement role-based authorization for Admin and Customer users.

### Work Completed

Created:

`Constants/UserRoles.cs`

Defined two application roles:

- Admin
- Customer

Updated the User model to include a Role property.

Updated user registration so that newly registered users automatically receive the Customer role.

Updated JWT generation to include the user's role as a claim.

Configured role-based authorization using the `[Authorize(Roles = ...)]` attribute.

Implemented role restrictions:

**Admin**

- Create, update and delete movies.
- Manage theatres.
- Manage screens.
- Manage seats.
- Manage shows.
- Retrieve bookings.
- Cancel bookings.

**Customer**

- Register and log in.
- View movies.
- View theatres.
- View screens.
- View seats.
- View shows.
- View show seat availability.
- Create bookings.

Authentication endpoints remain publicly accessible using `[AllowAnonymous]`.

### Result

Role-based authorization was implemented using Admin and Customer roles.

---

## Commit 14 – Admin User Seeding Using User Secrets

**Commit Hash:** `4f4b2c0`

**Commit Message:**

```text
feat: implement admin user seeding using user secrets
```

### Objective

Automatically create an initial Admin account when the application starts in the Development environment.

### Work Completed

Created:

`Data/RoleSeeder.cs`

Implemented the Admin seeding process.

The seeder:

1. Reads the Admin password from User Secrets.
2. Checks whether the Admin account already exists.
3. Creates the Admin account if it does not exist.
4. Hashes the password using PasswordHasher.
5. Assigns the Admin role.
6. Saves the account in the Users table.

Configured the Admin password using:

```powershell
dotnet user-secrets set "SeedUsers:AdminPassword" "YourAdminPassword"
```

The seeded Admin account uses:

- Email: `admin@moviebooking.com`
- Role: `Admin`

Updated Program.cs to call the seeder during application startup in the Development environment.

The seeder checks for an existing Admin account to prevent duplicate records during repeated application startup.

No additional database migration was required because the Users table and Role column already existed.

### Result

Admin account seeding was implemented using .NET User Secrets and password hashing.

---

## Current Project Features

The project currently includes:

### Movie Management

- Create movies.
- Retrieve movies.
- Update movies.
- Delete movies.
- Search movies.
- Pagination.

### Theatre and Screen Management

- Create theatres.
- Retrieve theatres.
- Update theatres.
- Delete theatres.
- Create screens.
- Retrieve screens.
- Update screens.
- Delete screens.

### Seat Management

- Create individual seats.
- Bulk-create seats.
- Retrieve seats.
- Update seats.
- Delete seats.

### Show Management

- Schedule shows.
- Retrieve shows.
- Update shows.
- Delete shows.
- Validate show timings.
- Prevent overlapping shows.

### Show Seat Availability

- Generate ShowSeat records when creating shows.
- Retrieve all show seats.
- Retrieve available seats.
- Retrieve booked seats.

### Booking Management

- Create bookings.
- Validate selected seats.
- Calculate booking amounts.
- Update seat availability.
- Retrieve bookings.
- Cancel bookings.
- Preserve cancelled booking history.
- Use database transactions for booking operations.

### Authentication and Authorization

- User registration.
- User login.
- Password hashing.
- JWT token generation.
- JWT token validation.
- Admin role.
- Customer role.
- Role-based API authorization.
- Admin user seeding using User Secrets.

### Exception Handling

- Centralized exception handling.
- Consistent ProblemDetails responses.
- Exception logging.
- HTTP status mapping.

---

## Git Commit History

The following is the commit history provided for the project:

```text
4f4b2c0 feat: implement admin user seeding using user secrets
cccb2e9 feat : Rolebased authorization added
25a2410 JWT Authentication added
948fa62 docs and readme added
2e16419 update:program.cs
b4021f7 feat: add global exception handling and final validation
872893d feat: implement booking retrieval and cancellation
d1494f5 feat: implement show seat availability and booking creation
01b320a feat: implement show scheduling and validation
cba0f2c feat: implement seat APIs and bulk seat creation
e6dc6d6 feat: implement theatre and screen APIs
331cc9c feat: implement movie CRUD APIs
3458c17 feat: create database using ef core migrations
7493dc1 feat: add movie booking models and dbcontext
cef693f chore: initialize movie ticket booking web api
```

---

## Current Scope and Future Enhancements

The current project focuses on backend API functionality, database operations, authentication and authorization.

Features not currently included:

- Theatre Manager role.
- Frontend application.
- Online payment gateway integration.
- Email or SMS notifications.
- Customer-specific booking ownership and cancellation authorization.

These can be considered for future development.