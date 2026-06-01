# SaddleHeroesAirWays - Flight Booking System

En modern flygbokningsplattform byggd med C# och ASP.NET Core.

---

## 📋 Projektbeskrivning och Arkitekturöversikt

### Projektöversikt

**SaddleHeroesAirWays** är ett API-baserat flygbokningssystem som möjliggör användare att söka, boka och hantera flygbookningar. Systemet är byggt med ett lagrat arkitektur-mönster och följer SOLID-principerna för maintainability och skalbarhet.

### Systemarkitektur

Projektet är organiserat i tre huvudkomponenter:

#### 1. **SaddleHeroesAirWays.API** (ASP.NET Core API)
Huvudapplikationen som exponerar REST-endpoints för frontend-applikationer. Denna skikt innehåller:

- **Controllers**: Hanterar HTTP-förfrågningar och svar
  - `UserController`: Hantering av användarprofiler (CRUD-operationer)
  - `FlightController`: Flygöversökning och hämtning
  - `BookingController`: Bokningshantering (skapande, uppdatering, hämtning)
  - `AirportController`: Flygplatshantering
  
- **Services**: Affärslogik och dataaccessmönster
  - `IUserService`: Användarhantering
  - `IFlightService`: Flygöversökning och filtrering
  - `IBookingService`: Bokningslogik inklusive tidsbegränsade frågor
  - `IAirportService`: Flygplatshantering
  
- **DTOs (Data Transfer Objects)**: 
  - `CreateUser`, `UpdateUser`, `UserResponse`
  - `CreateBookingRequest`, `UpdateBooking`, `BookingResponse`
  - `FlightResponse`
  
- **Validering**: Använder FluentValidation för robust datakontroll

#### 2. **SaddleHeroesAirWays.Library** (Klassbibliotek)
Innehåller alla domänmodeller och affärslogik som delas mellan komponenter:

- **Models**:
  - `User`: Användarinformation (namn, e-post, telefon, personnummer)
  - `Flight`: Fluminformation (flightnummer, tider, kapacitet, pris, status)
  - `Booking`: Bokningsdetaljer (referensnummer, användare, flygning)
  - `Airport`: Flygplatsdata (namn, stad, IATA-kod)
  - `ServiceResult<T>`: Generisk resultatwrapper för konsistent felhantering
  
- **Databaskontexten**: `DbContextAPI` (Entity Framework Core)

#### 3. **SaddleHeroesAirWays.MSTest** (Testprojekt)
Omfattande testsvit med unit- och integrationstester.

### Teknologisk Stack

| Komponent | Teknik |
|-----------|--------|
| **Språk** | C# 12 |
| **Framework** | ASP.NET Core 8.x |
| **Databas** | Entity Framework Core (In-Memory för tester) |
| **Validering** | FluentValidation |
| **Testning** | MSTest, Moq |
| **API Format** | REST/JSON |

---

## 📡 API-Endpoints Dokumentation

### Base URL
