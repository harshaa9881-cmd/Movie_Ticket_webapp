# Movie Ticket Booking System – Web API

## 1. Project Overview

The **Movie Ticket Booking System** is an ASP.NET Core Web API developed using C#, .NET 10, Entity Framework Core, and SQL Server.

The application provides APIs to manage movies, theatres, screens, seats, shows, and movie ticket bookings. It also implements JWT authentication and role-based authorization to control access to protected endpoints.

The project follows a layered architecture to separate API controllers, business logic, database operations, and data models.

## 2. Technologies Used

- C#
- .NET 10 – ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server Express
- JWT Authentication
- Role-Based Authorization
- Custom Authorization
- Data Annotations and Fluent API
- Dependency Injection
- Scalar / OpenAPI
- Visual Studio
- Postman
- Angular frontend

## 3. Project Architecture

The application follows this request flow:

```text
Client (Postman / Angular)
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
      DbContext
          |
          v
     SQL Server
```

### Layer Responsibilities

**Controllers**
- Handle incoming HTTP requests.
- Call the appropriate service methods.
- Return HTTP responses.

**Services**
- Implement business logic.
- Perform validations.
- Coordinate repository operations.

**Repositories**
- Handle database operations.
- Communicate with Entity Framework Core.

**DbContext**
- Manages the database connection.
- Defines entity relationships and configurations.
- Maps model classes to database tables.

**Models**
- Define the application's data entities.

**DTOs**
- Define the data transferred between the client and API.

**Middleware**
- Handles global exception management.
- Provides authentication, authorization, and endpoint diagnostics.

## 4. Project Structure

```text
MovieTicketBooking/
│
├── Authorization/
│   ├── Handlers/
│   │   └── BookingOwnerOrAdminHandler.cs
│   │
│   └── Requirements/
│       └── BookingOwnerOrAdminRequirement.cs
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
├── Data/
│   ├── ApplicationDbContext.cs
│   └── RoleSeeder.cs
│
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   └── LoginDto.cs
│   │
│   ├── Movies/
│   ├── Theatres/
│   ├── Screens/
│   ├── Seats/
│   ├── Shows/
│   ├── ShowSeats/
│   └── Bookings/
│
├── Middleware/
│   ├── GlobalExceptionHandler.cs
│   ├── AuthenticationDiagnosticsMiddleware.cs
│   ├── AuthorizationDiagnosticsMiddleware.cs
│   └── EndpointMetadataMiddleware.cs
│
├── Migrations/
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
├── Repositories/
│   ├── Interfaces/
│   │   ├── IMovieRepository.cs
│   │   ├── ITheatreRepository.cs
│   │   ├── IScreenRepository.cs
│   │   ├── ISeatRepository.cs
│   │   ├── IShowRepository.cs
│   │   ├── IShowSeatRepository.cs
│   │   ├── IBookingRepository.cs
│   │   └── IUserRepository.cs
│   │
│   ├── MovieRepository.cs
│   ├── TheatreRepository.cs
│   ├── ScreenRepository.cs
│   ├── SeatRepository.cs
│   ├── ShowRepository.cs
│   ├── ShowSeatRepository.cs
│   ├── BookingRepository.cs
│   └── UserRepository.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IMovieService.cs
│   │   ├── ITheatreService.cs
│   │   ├── IScreenService.cs
│   │   ├── ISeatService.cs
│   │   ├── IShowService.cs
│   │   ├── IShowSeatService.cs
│   │   ├── IBookingService.cs
│   │   ├── IUserService.cs
│   │   └── IJwtService.cs
│   │
│   ├── MovieService.cs
│   ├── TheatreService.cs
│   ├── ScreenService.cs
│   ├── SeatService.cs
│   ├── ShowService.cs
│   ├── ShowSeatService.cs
│   ├── BookingService.cs
│   ├── UserService.cs
│   └── JwtService.cs
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── MovieTicketBooking.csproj
└── README.md
```

## 5. Database Design

The application currently uses the following main entities:

| Entity | Description |
|---|---|
| Movie | Stores movie information. |
| Theatre | Stores theatre details. |
| Screen | Represents screens within theatres. |
| Seat | Stores seat information for each screen. |
| Show | Stores movie show schedules. |
| ShowSeat | Maintains seat availability for individual shows. |
| Booking | Stores customer booking details. |
| BookingSeat | Stores the seats associated with a booking. |
| User | Stores registered user and authentication details. |

### Database Relationships

- A Theatre can have multiple Screens.
- A Screen can contain multiple Seats.
- A Movie can have multiple Shows.
- A Screen can have multiple Shows.
- A Show has multiple ShowSeats.
- A Booking belongs to a Show.
- A Booking can contain multiple BookingSeats.
- A User can have multiple Bookings.

## 6. Features Implemented

### Movie Management
- Add a movie.
- Retrieve all movies.
- Retrieve a movie by ID.
- Update movie details.
- Delete a movie.

### Theatre Management
- Add a theatre.
- Retrieve theatres.
- Retrieve theatre details.
- Update theatre information.
- Delete a theatre.

### Screen Management
- Add screens to theatres.
- Retrieve screens.
- Retrieve screen details.
- Update screen information.
- Delete screens.

### Seat Management
- Add individual seats.
- Create seats in bulk.
- Retrieve seats.
- Retrieve seat details.
- Update seat information.
- Delete seats.

### Show Management
- Schedule movie shows.
- Retrieve show details.
- Validate show scheduling.
- Manage show information.

### Show Seat Management
- Retrieve seat availability for a show.
- Track available and booked seats.
- Maintain show-specific seat status.

### Booking Management
- Create movie ticket bookings.
- Validate selected seats.
- Prevent duplicate seat selection.
- Calculate booking amounts.
- Retrieve booking details.
- Retrieve bookings by show.
- Cancel bookings.
- Release seats when a booking is cancelled.

Booking cancellation updates the booking status and releases the associated seats instead of permanently deleting the booking record.

## 7. Authentication and Authorization

### JWT Authentication

JWT (JSON Web Token) is used to authenticate users.

Implemented functionality:
- User registration.
- User login.
- Password hashing.
- JWT token generation.
- JWT token validation.
- Claims included in generated tokens.

The JWT includes user-related claims such as:

- User ID (`ClaimTypes.NameIdentifier`)
- Full Name (`ClaimTypes.Name`)
- Email (`ClaimTypes.Email`)
- Role (`ClaimTypes.Role`)

### Role-Based Authorization

The application currently supports two roles:

| Role | Access |
|---|---|
| Admin | Manages movies, theatres, screens, seats, shows, and administrative booking operations. |
| Customer | Registers, logs in, and accesses permitted customer operations. |

Newly registered users are assigned the Customer role.

An Admin account is seeded during Development using a password configured through User Secrets.

### Custom Authorization

The booking authorization requirement is designed to support resource-based access:

- Admin can access and manage any booking.
- Customer can access or cancel only their own booking.
- The booking owner is identified using the authenticated user's ID claim.
- User ID is obtained from the JWT rather than accepting it from the client.

The booking-owner requirement and handler are used with an authorization policy.

## 8. Middleware Implementation

The project uses middleware to handle exceptions and inspect the authentication and authorization pipeline.

### GlobalExceptionHandler

- Handles unhandled application exceptions.
- Provides a centralized exception-handling mechanism.
- Avoids repeating exception-handling code in every controller.

### AuthenticationDiagnosticsMiddleware

- Checks whether the current request is authenticated.
- Displays the authentication type.
- Logs the claims available in `HttpContext.User`.
- Helps debug JWT authentication.

### AuthorizationDiagnosticsMiddleware

- Logs the authorization process.
- Displays the final HTTP response status code.
- Helps identify authorization-related failures.

### EndpointMetadataMiddleware

- Retrieves the selected endpoint.
- Displays the request method and path.
- Displays endpoint information.
- Checks for `[AllowAnonymous]`.
- Displays the number of authorization metadata entries.

Diagnostic middleware is intended for Development only. Claim values should not be exposed in production logs.

### Middleware Execution Order

```text
Exception Handling
        |
        v
HTTPS Redirection
        |
        v
Routing
        |
        v
CORS
        |
        v
Endpoint Metadata Diagnostics
        |
        v
Authentication
        |
        v
Authentication Diagnostics
        |
        v
Authorization Diagnostics
        |
        v
Authorization
        |
        v
Controllers
```

## 9. CORS Configuration

CORS is configured to allow requests from the Angular development server.

Allowed origin:

```text
http://localhost:4200
```

The `AllowAngular` policy allows the required HTTP methods and headers.

## 10. Configuration

### SQL Server Connection String

The application uses SQL Server Express.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### User Secrets

Sensitive configuration such as the JWT signing key and Admin seed password is stored using .NET User Secrets during development.

Do not commit actual passwords, JWT signing keys, or other secrets to GitHub.

## 11. Running the Project

### Step 1: Clone the Repository

```powershell
git clone https://github.com/harshaa9881-cmd/Movie_Ticket_webapp.git
```

### Step 2: Open the Project

Open the solution or project in Visual Studio.

### Step 3: Configure the Database

Update the SQL Server connection string in the application configuration.

### Step 4: Configure User Secrets

Configure the required JWT settings and Admin seed password using .NET User Secrets.

### Step 5: Apply EF Core Migrations

```powershell
dotnet ef database update
```

### Step 6: Run the API

```powershell
dotnet run
```

### Step 7: Test the API

Use Scalar/OpenAPI or Postman to test the available endpoints.

For protected endpoints, authenticate and provide the JWT using the Bearer Token authorization option.

## 12. Testing

The API can be tested using Postman or Scalar.

Important test scenarios include:

- Movie, theatre, screen, and seat CRUD operations.
- Show creation and validation.
- Show seat availability.
- Successful booking creation.
- Duplicate seat selection validation.
- Booking cancellation and seat release.
- User registration and login.
- JWT authentication.
- Admin and Customer role authorization.
- Booking ownership authorization.
- Global exception handling.
- Authentication and authorization diagnostic logs.

## 13. Current Project Scope

The current implementation focuses on the Web API, database operations, booking workflow, authentication, authorization, and middleware diagnostics.

The following are outside the current implemented scope:

- Payment gateway integration.
- Email and SMS notifications.
- Online payment processing.
- Advanced reporting and analytics.

## 14. Repository

GitHub Repository:

https://github.com/harshaa9881-cmd/Movie_Ticket_webapp

