# SaddleHeroesAirways - Flight Booking System

A modern flight booking platform built with C# and ASP.NET Core.

---

## 📋 Project Description and Architecture Overview

### Project Overview

**SaddleHeroesAirWays** is an API-based flight booking system that enables users to search, book and manage flight reservations. The system is built with a layered architecture pattern and follows the SOLID principles for maintainability and scalability.

### System Architecture

The project is organized into three main components:

#### 1. SaddleHeroesAirWays.API (ASP.NET Core API)
The main application exposing REST endpoints for frontend applications. This layer contains:

**Controllers** — Handle HTTP requests and responses
- `UserController`: User profile management (CRUD operations)
- `FlightController`: Flight search and retrieval
- `BookingController`: Booking management (create, update, retrieve)

**Services** — Business logic and data access patterns
- `IUserService`: User management
- `IFlightService`: Flight search and filtering
- `IBookingService`: Booking logic including time-based queries

**DTOs (Data Transfer Objects)**
- `CreateUser`, `UpdateUser`, `UserResponse`
- `CreateBookingRequest`, `UpdateBooking`, `BookingResponse`
- `FlightResponse`

**Validation** — Uses FluentValidation for robust data control

#### 2. SaddleHeroesAirWays.Library (Class Library)
Contains all domain models and business logic shared between components:

**Models**
- `User`: User information (name, email, phone, social security number)
- `Flight`: Flight information (flight number, times, capacity, price, status)
- `Booking`: Booking details (reference number, user, flight)
- `Airport`: Airport data (name, city, IATA code)
- `ServiceResult<T>`: Generic result wrapper for consistent error handling

**Database Context**: `DbContextAPI` (Entity Framework Core)

#### 3. SaddleHeroesAirWays.MSTest (Test Project)
Comprehensive test suite with unit and integration tests.

### Technology Stack

| Component | Technology |
|-----------|------------|
| Language | C# 12 |
| Framework | ASP.NET Core 8.x |
| Database | Entity Framework Core (In-Memory for tests) |
| Validation | FluentValidation |
| Testing | MSTest, Moq |
| API Format | REST/JSON |

---

## 📡 API Endpoints Documentation

### Base URL
`https://localhost:port/api` (Local during development)

### Users

| Method | Endpoint | Description | Body / Query |
|--------|----------|-------------|--------------|
| POST | /User/CreateUser | Creates a new user. | CreateUser DTO |
| GET | /User/GetUsersInAlphabeticalOrder | Retrieves all users sorted by last name. | - |
| GET | /User/{id} | Retrieves a specific user by ID. | - |
| PUT | /User/{id} | Updates an existing user's details. | UpdateUser DTO |
| DELETE | /User/{id} | Removes a user from the system. | - |

### Flights

| Method | Endpoint | Description | Body / Query |
|--------|----------|-------------|--------------|
| GET | /Flight/search | Searches for available flights. Filters out fully booked flights. | ?city={cityName} (Optional) |
| GET | /Flight/{id} | Retrieves detailed information about a specific flight. | - |

### Bookings

| Method | Endpoint | Description | Body / Query |
|--------|----------|-------------|--------------|
| POST | /Booking/CreateBooking | Creates a new booking. Assigns auto-generated seat and reference number. | CreateBookingRequest DTO |
| GET | /Booking/all | Retrieves all bookings in the system. | - |
| GET | /Booking/weekly | Retrieves all bookings for the week based on a specific date. | ?date={YYYY-MM-DD} |
| GET | /Booking/monthly | Retrieves all bookings for the month based on a specific date. | ?date={YYYY-MM-DD} |
| GET | /Booking/by-date-range | Retrieves bookings within a specific time span. | ?startDate={date}&endDate={date} |
| GET | /Booking/user/{id} | Retrieves all bookings belonging to a specific user. | - |
| GET | /Booking/reference/{bookingReference} | Retrieves a booking via its unique reference number (e.g. BKG-1001). | - |
| PUT | /Booking/{bookingReference} | Rebooks a trip to a new flight (if more than 1h until departure). | UpdateBooking DTO |
| PATCH | /Booking/{bookingReference}/cancel | Cancels a trip (changes status to Cancelled). | - |
| DELETE | /Booking/{bookingReference} | Permanently removes a booking and its details from the database. | - |

---

## 🧪 Test Strategy and Results

### Tools & Frameworks
- **MSTest**: Primary test framework for driving and executing tests.
- **Moq**: Used in controller tests to mock the behavior of Services and Validators, ensuring isolated unit tests.
- **Entity Framework Core In-Memory Database**: Used in service tests to quickly and safely test database logic, relationships and LINQ queries without affecting a real database.

### 1. Controller Tests (Unit Tests)
Focuses on ensuring the API handles HTTP requests correctly, returns the right status codes and passes the right data forward.
- **Happy Paths**: Ensures valid calls return e.g. 200 OK, 201 Created or 204 NoContent.
- **Edge Cases & Error Handling**: Checks that the system catches invalid input and returns the correct status code (e.g. 400 Bad Request for invalid validation or 404 Not Found when a resource is missing).
- **Call Verification**: Via Moq Verify, ensures that underlying services are called exactly the expected number of times.

### 2. Service Tests (Integration/Logic Tests)
Focuses on business logic and interaction with the database (In-Memory).
- **Business Rules**: Ensures the logic holds. For example:
  - It is not possible to book a fully booked flight.
  - It is not possible to rebook a flight with less than one hour until departure.
  - A booking that is already cancelled cannot be cancelled again.
- **Database Operations**: Checks that cascade deletions work and that date and city search filtering works correctly.

### Test Results
- **Coverage**: Tests cover all critical flows (Create user, Search flight, Book, Rebook, Cancel, Delete).
- **Reliability**: By using unique In-Memory database names for each test method, tests execute completely isolated from each other without state leakage.
- **Status**: All written tests pass (green), confirming that the application's architecture, validation flows (FluentValidation) and ServiceResult pattern work together exactly as designed.
