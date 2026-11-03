# Movie Ticket Booking System

## Project Overview

The **Movie Ticket Booking System** is an ASP.NET Core Web API application developed using C#, .NET 10, Entity Framework Core 10, and SQL Server.

The application provides APIs to manage movies, theatres, screens, seats, shows, and customer bookings.

Customers can view available movies and shows, check seat availability, book seats, retrieve booking details, and cancel bookings. The system automatically updates seat availability when a booking is created or cancelled.

The application also implements JWT authentication and role-based authorization with two roles:

- **Admin:** Manages movies, theatres, screens, seats, and shows.
- **Customer:** Registers, logs in, views available information, and creates bookings.

An Admin account is automatically created using the user seeding process when the application starts in the Development environment.

The project follows a layered architecture:

**Controller → Service → Repository → DbContext → SQL Server**

Each layer has a separate responsibility, making the application easier to maintain and understand.

---

## Technologies Used

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server Express
- LINQ
- JWT Bearer Authentication
- ASP.NET Core PasswordHasher
- User Secrets
- Visual Studio
- Scalar / OpenAPI
- Git and GitHub

---

## Project Structure

```text
MovieTicketBooking/
│
├── Constants/
│   └── UserRoles.cs
│
├── Controllers/
│   ├── AuthController.cs
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
│   ├── BookingSeat.cs
│   └── User.cs
│
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   └── LoginDto.cs
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
│   ├── ApplicationDbContext.cs
│   └── RoleSeeder.cs
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

---

## Folder Responsibilities

### 1. Controllers

The Controllers folder contains the API endpoints.

Controllers receive HTTP requests, call the appropriate service methods, and return HTTP responses.

The controllers do not directly communicate with the database.

| Controller | Responsibility |
|---|---|
| AuthController | User registration and login |
| MoviesController | Movie management |
| TheatresController | Theatre management |
| ScreensController | Screen management |
| SeatsController | Seat management |
| ShowsController | Show scheduling and management |
| ShowSeatsController | Show seat availability |
| BookingsController | Booking creation, retrieval and cancellation |

### 2. Models

The Models folder contains the main entity classes representing the database tables.

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
| User | Stores registered user information, password hash and role |

### 3. DTOs

DTO stands for Data Transfer Object.

The DTOs folder contains request and response classes used to transfer data between clients and the API.

DTOs separate API request and response structures from database entity models.

Subfolders:

- **Auth:** Registration and login request DTOs.
- **Movies:** Movie creation, update and response DTOs.
- **Theatres:** Theatre creation, update and response DTOs.
- **Screens:** Screen creation, update and response DTOs.
- **Seats:** Seat creation, update, bulk creation and response DTOs.
- **Shows:** Show creation, update and response DTOs.
- **ShowSeats:** Show seat availability response DTOs.
- **Bookings:** Booking creation and booking response DTOs.

`PagedResult.cs` provides a common structure for paginated API responses.

### 4. Constants

The Constants folder contains reusable constant values.

`UserRoles.cs` defines the roles used by the application:

- Admin
- Customer

Using constants avoids repeatedly writing role names throughout the application.

### 5. Data

The Data folder contains:

**ApplicationDbContext.cs**

ApplicationDbContext is the connection between Entity Framework Core and the SQL Server database.

Responsibilities:

- Defines DbSet properties for the application entities.
- Configures entity relationships.
- Defines primary keys and unique indexes.
- Configures check constraints.
- Configures decimal precision for monetary values.
- Defines delete behaviours.

**RoleSeeder.cs**

Responsible for creating the initial Admin account.

It:

- Reads the Admin password from User Secrets.
- Checks whether the Admin account already exists.
- Hashes the password using PasswordHasher.
- Saves the Admin account in the Users table.
- Avoids inserting duplicate Admin accounts.

### 6. Repositories

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

### 7. Services

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
- Registering users and verifying login credentials.
- Generating JWT tokens.

Services use repository interfaces to access database operations.

### 8. Middleware

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

### 9. Migrations

The Migrations folder contains Entity Framework Core migration files.

Migrations track database schema changes based on the entity models and DbContext configuration.

They are used to create and update the SQL Server database.

### 10. Program.cs

Program.cs is the application's entry point.

Responsibilities:

- Registers controllers.
- Configures the SQL Server database connection.
- Registers repository and service implementations with dependency injection.
- Configures OpenAPI and Scalar.
- Registers centralized exception handling.
- Configures JWT Bearer authentication.
- Registers authorization services.
- Configures the HTTP request pipeline.
- Calls the Admin seeder in the Development environment.
- Maps API controllers.

### 11. appsettings.json

Stores application configuration, including the SQL Server connection string and JWT settings such as issuer, audience and expiry.

The JWT signing key and Admin seed password are stored using .NET User Secrets during development. They should not be committed to the repository.

---

## Database Design

The application contains nine main database tables:

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
| Users | Registered user information and authentication details |

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
- User email addresses are unique.

---

## Application Workflow

### 1. User Registration

A new customer registers through the registration API by providing:

- Full name.
- Email.
- Password.

The application checks whether the email already exists.

The password is hashed using `PasswordHasher<User>` before storing the user in the database.

Public registration automatically assigns the `Customer` role. Users cannot select the Admin role through the registration request.

### 2. User Login and JWT Authentication

Users log in using their registered email and password.

The application:

1. Retrieves the user using the email address.
2. Verifies the entered password against the stored password hash.
3. Generates a JWT token when the credentials are valid.
4. Includes user information and role claims in the token.
5. Returns the token to the client.

The token is used to access protected API endpoints.

### 3. Role-Based Authorization

The application supports two roles.

| Operation | Admin | Customer |
|---|---|---|
| Register and login | Yes | Yes |
| View movies | Yes | Yes |
| Create, update and delete movies | Yes | No |
| View theatres | Yes | Yes |
| Manage theatres | Yes | No |
| View screens | Yes | Yes |
| Manage screens | Yes | No |
| View seats | Yes | Yes |
| Manage seats | Yes | No |
| View shows | Yes | Yes |
| Manage shows | Yes | No |
| View show seat availability | Yes | Yes |
| Create bookings | Yes | Yes |
| Retrieve all bookings | Yes | No |
| Retrieve a booking by ID | Yes | No |
| Retrieve bookings by show | Yes | No |
| Cancel bookings | Yes | No |

A valid JWT is required for protected endpoints.

The application uses role claims in the JWT to determine whether the logged-in user is authorized to perform an operation.

A missing or invalid token returns `401 Unauthorized`.

A valid token without the required role returns `403 Forbidden`.

**Note:** Customer-specific booking ownership and customer-only booking cancellation are not implemented in the current version. Booking retrieval and cancellation are restricted to Admin.

### 4. Admin User Seeding

The application automatically creates an initial Admin account using the seeding process.

The Admin password is configured through .NET User Secrets.

Example:

```powershell
dotnet user-secrets set "SeedUsers:AdminPassword" "YourAdminPassword"
```

The seeder:

1. Reads the configured Admin password.
2. Checks whether the Admin account already exists.
3. Creates the Admin user if it does not exist.
4. Hashes the password before saving.
5. Assigns the Admin role.
6. Saves the account in the Users table.

The seeded account uses:

- Email: `admin@moviebooking.com`
- Role: `Admin`

The password is not stored as plain text.

The seeder runs in the Development environment and does not create duplicate Admin accounts on repeated application startup.

Customer accounts continue to be created through the registration API.

### 5. Movie Management

The administrator can create, retrieve, update, search and delete movies.

Movie information includes:

- Title
- Description
- Duration
- Release date
- Language
- Genre

Movie deletion is restricted when dependent shows exist.

### 6. Theatre Management

The administrator can create and manage theatres.

Each theatre stores its name and location.

A theatre can contain multiple screens.

### 7. Screen Management

Screens are created under existing theatres.

Each screen has a screen name and theatre association.

Duplicate screen names within the same theatre are not allowed.

### 8. Seat Management

Physical seats are created under screens.

Seats can be created individually or through the bulk seat creation endpoint.

Each seat contains:

- Row name
- Seat number
- Seat type
- Screen association

Duplicate seat coordinates within a screen are prevented.

### 9. Show Scheduling

A show connects a movie with a screen and a scheduled time.

The system validates:

- Movie existence.
- Screen existence.
- End time must be later than start time.
- Ticket price must not be negative.
- Another show must not overlap on the same screen.

When a show is created, the system generates a ShowSeat record for every physical seat configured for that screen.

Initially, every generated ShowSeat has the status `Available`.

### 10. Show Seat Availability

ShowSeats represent the availability of physical seats for a particular show.

The same physical seat can be available for one show and booked for another show.

The application provides endpoints to retrieve:

- All seats for a show.
- Available seats.
- Booked seats.

### 11. Booking Creation

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

### 12. Booking Retrieval

The application supports:

- Retrieving all bookings.
- Filtering bookings by show.
- Filtering bookings by status.
- Retrieving a booking using its ID.
- Retrieving bookings associated with a particular show.

These retrieval operations are currently restricted to Admin.

### 13. Booking Cancellation

When a confirmed booking is cancelled:

1. The application retrieves the booking.
2. It checks whether the booking exists.
3. It checks whether the booking is currently confirmed.
4. It changes the booking status to `Cancelled`.
5. It changes the associated ShowSeat statuses back to `Available`.
6. It saves the changes inside a database transaction.

The booking and its booking-seat records are preserved for historical reference.

---

## API Endpoints

### Authentication

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/auth/register` | Register a customer | Anonymous |
| POST | `/api/auth/login` | Login and receive JWT | Anonymous |

### Movies

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/movies` | Create a movie | Admin |
| GET | `/api/movies` | Retrieve movies with pagination and optional title filter | Admin, Customer |
| GET | `/api/movies/{id}` | Retrieve a movie | Admin, Customer |
| PUT | `/api/movies/{id}` | Update a movie | Admin |
| DELETE | `/api/movies/{id}` | Delete a movie | Admin |
| GET | `/api/movies/search?title=leo` | Search movies by title | Admin, Customer |

### Theatres

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/theatres` | Create a theatre | Admin |
| GET | `/api/theatres` | Retrieve theatres | Admin, Customer |
| GET | `/api/theatres/{id}` | Retrieve a theatre | Admin, Customer |
| PUT | `/api/theatres/{id}` | Update a theatre | Admin |
| DELETE | `/api/theatres/{id}` | Delete a theatre | Admin |

### Screens

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/screens` | Create a screen | Admin |
| GET | `/api/screens` | Retrieve screens, optionally filtered by theatre | Admin, Customer |
| GET | `/api/screens/{id}` | Retrieve a screen | Admin, Customer |
| GET | `/api/screens/theatre/{theatreId}` | Retrieve screens of a theatre | Admin, Customer |
| PUT | `/api/screens/{id}` | Update a screen | Admin |
| DELETE | `/api/screens/{id}` | Delete a screen | Admin |

### Seats

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/seats` | Create a seat | Admin |
| POST | `/api/seats/bulk` | Create multiple seats | Admin |
| GET | `/api/seats/screen/{screenId}` | Retrieve seats of a screen | Admin, Customer |
| GET | `/api/seats/{id}` | Retrieve a seat | Admin, Customer |
| PUT | `/api/seats/{id}` | Update a seat | Admin |
| DELETE | `/api/seats/{id}` | Delete a seat | Admin |

### Shows

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/shows` | Create a show | Admin |
| GET | `/api/shows` | Retrieve shows with optional filters | Admin, Customer |
| GET | `/api/shows/{id}` | Retrieve a show | Admin, Customer |
| GET | `/api/shows/movie/{movieId}` | Retrieve shows of a movie | Admin, Customer |
| GET | `/api/shows/theatre/{theatreId}` | Retrieve shows at a theatre | Admin, Customer |
| GET | `/api/shows/search?date=YYYY-MM-DD` | Search shows by date | Admin, Customer |
| PUT | `/api/shows/{id}` | Update a show | Admin |
| DELETE | `/api/shows/{id}` | Delete a show | Admin |

### Show Seats

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| GET | `/api/shows/{showId}/seats` | Retrieve all seats for a show | Admin, Customer |
| GET | `/api/shows/{showId}/seats/available` | Retrieve available seats | Admin, Customer |
| GET | `/api/shows/{showId}/seats/booked` | Retrieve booked seats | Admin, Customer |

### Bookings

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/bookings` | Create a booking | Admin, Customer |
| GET | `/api/bookings` | Retrieve bookings with optional filters | Admin |
| GET | `/api/bookings/{id}` | Retrieve a booking | Admin |
| GET | `/api/bookings/show/{showId}` | Retrieve bookings for a show | Admin |
| PUT | `/api/bookings/{id}/cancel` | Cancel a booking | Admin |

---

## Authentication and Configuration

### JWT Configuration

The application uses JWT Bearer authentication.

The JWT configuration contains:

- Issuer
- Audience
- Signing key
- Token expiry time

The signing key should be stored securely using User Secrets during development.

### Configure User Secrets

Open PowerShell in the project folder containing the `.csproj` file.

Initialize User Secrets once:

```powershell
dotnet user-secrets init
```

Set the JWT signing key:

```powershell
$jwtKey = [Convert]::ToBase64String(
    [Security.Cryptography.RandomNumberGenerator]::GetBytes(32)
)

dotnet user-secrets set "Jwt:Key" $jwtKey
```

Set the Admin seed password:

```powershell
dotnet user-secrets set "SeedUsers:AdminPassword" "YourAdminPassword"
```

Verify the configuration:

```powershell
dotnet user-secrets list
```

Do not commit passwords or signing keys to GitHub.

---

## Running the Project

### Prerequisites

- Visual Studio with .NET 10 support.
- .NET 10 SDK.
- SQL Server Express.
- SQL Server Management Studio (SSMS).
- Entity Framework Core tools.

### Step 1: Clone the Repository

```powershell
git clone https://github.com/harshaa9881-cmd/Movie_Ticket_webapp.git
```

### Step 2: Open the Project

Open the solution or project folder in Visual Studio.

### Step 3: Configure the Database

Update the `DefaultConnection` value in `appsettings.json` according to your SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Step 4: Restore Packages

```powershell
dotnet restore
```

### Step 5: Configure User Secrets

Initialize User Secrets and configure the JWT key and Admin password as explained in the Authentication and Configuration section.

### Step 6: Apply Migrations

```powershell
dotnet ef database update
```

### Step 7: Build the Project

```powershell
dotnet build
```

### Step 8: Run the Project

```powershell
dotnet run
```

During Development startup, the application checks whether the initial Admin account exists and creates it if necessary.

### Step 9: Open API Documentation

Open the Scalar URL displayed by the application.

Usually:

```text
https://localhost:<port>/scalar/v1
```

Use Scalar or Postman to test the endpoints.

### Step 10: Login and Use JWT

Use the seeded Admin credentials to log in through:

```text
POST /api/auth/login
```

Copy the returned JWT and use it in Postman:

**Authorization → Bearer Token**

Customer accounts can be registered through:

```text
POST /api/auth/register
```

---

## Git Workflow

The project was developed incrementally using Git commits.

Each commit represents a separate development stage.

Refer to `docs.md` for the detailed implementation history of all 15 commits.

---

## Scope and Limitations

The current implementation focuses on the backend Web API, database operations, authentication and role-based authorization.

Implemented features:

- Movie management.
- Theatre and screen management.
- Seat management and bulk seat creation.
- Show scheduling and validation.
- Show seat availability.
- Booking creation and cancellation.
- Database transactions for booking operations.
- Centralized exception handling.
- JWT registration and login.
- Admin and Customer role-based authorization.
- Admin account seeding using User Secrets.

The following are outside the current scope:

- Frontend application.
- Online payment gateway integration.
- Email or SMS notifications.
- Theatre Manager role.
- Customer-specific booking ownership and cancellation authorization.

---

## Conclusion

The Movie Ticket Booking System demonstrates a layered ASP.NET Core Web API application using Entity Framework Core and SQL Server.

It covers entity relationships, CRUD operations, business validation, show scheduling, seat availability, booking creation, booking cancellation, database transactions, centralized exception handling, JWT authentication, role-based authorization and Admin user seeding.

The project was developed incrementally using Git commits, with each stage adding functionality to the application.