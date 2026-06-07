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
`https://localhost:port/api` (Lokalt under utveckling)

### 🧑‍💻 Användare (User)
Hanterar passagerare och användarkonton i systemet.

| Metod | Endpoint | Beskrivning | Body / Query |
|-------|----------|-------------|--------------|
| `POST` | `/User/CreateUser` | Skapar en ny användare. | `CreateUser` DTO |
| `GET` | `/User/GetUsersInAlphabeticalOrder` | Hämtar alla användare sorterade på efternamn. | - |
| `GET` | `/User/{id}` | Hämtar en specifik användare via ID. | - |
| `PUT` | `/User/{id}` | Uppdaterar en befintlig användares uppgifter. | `UpdateUser` DTO |
| `DELETE` | `/User/{id}` | Tar bort en användare från systemet. | - |

### ✈️ Flyg (Flight)
Sökning och information om tillgängliga flygresor.

| Metod | Endpoint | Beskrivning | Body / Query |
|-------|----------|-------------|--------------|
| `GET` | `/Flight/search` | Söker efter lediga flyg. Filtrerar bort fullbokade flyg. | `?city={cityName}` (Frivillig) |
| `GET` | `/Flight/{id}` | Hämtar detaljerad information om ett specifikt flyg. | - |

### 🎟️ Bokningar (Booking)
Kärnfunktionaliteten för att skapa, hantera och avboka resor.

| Metod | Endpoint | Beskrivning | Body / Query |
|-------|----------|-------------|--------------|
| `POST` | `/Booking/CreateBooking` | Skapar en ny bokning. Tilldelar auto-genererat säte och referensnummer. | `CreateBookingRequest` DTO |
| `GET` | `/Booking/all` | Hämtar alla bokningar i systemet. | - |
| `GET` | `/Booking/weekly` | Hämtar alla bokningar för veckan utifrån ett specifikt datum. | `?date={YYYY-MM-DD}` |
| `GET` | `/Booking/monthly` | Hämtar alla bokningar för månaden utifrån ett specifikt datum. | `?date={YYYY-MM-DD}` |
| `GET` | `/Booking/by-date-range` | Hämtar bokningar inom ett specifikt tidsspann. | `?startDate={date}&endDate={date}` |
| `GET` | `/Booking/user/{id}` | Hämtar alla bokningar tillhörande en specifik användare. | - |
| `GET` | `/Booking/reference/{bookingReference}` | Hämtar en bokning via dess unika referensnummer (t.ex. BKG-1001). | - |
| `PUT` | `/Booking/{bookingReference}` | Ombokar en resa till ett nytt flyg (om > 1h kvar till avgång). | `UpdateBooking` DTO |
| `PATCH` | `/Booking/{bookingReference}/cancel` | Avbokar en resa (Ändrar status till *Cancelled*). | - |
| `DELETE` | `/Booking/{bookingReference}` | Tar bort en bokning och dess detaljer permanent från databasen. | - |

### 🏢 Flygplatser (Airport)
*Notering: Grundstruktur (Controller & Service) är implementerad och förberedd för framtida CRUD-operationer av flygplatser.*

---

## 🧪 Beskrivning av Teststrategi och Resultat

För att säkerställa hög kodkvalitet, pålitlighet och att affärsreglerna följs, använder projektet en omfattande testsvit baserad på **MSTest**. Teststrategin är uppdelad i två huvudsakliga nivåer: **Controller-tester** och **Service-tester**.

### Verktyg & Ramverk
*   **MSTest**: Som primärt testramverk för att driva och exekvera testerna.
*   **Moq**: Används i controller-testerna för att "mocka" (simulera) beteendet hos Services och Validators, vilket garanterar isolerade enhetstester.
*   **Entity Framework Core In-Memory Database**: Används i Service-testerna för att snabbt och säkert testa databaslogik, relationer och LINQ-frågor utan att påverka en riktig databas.

### 1. Controller-tester (Enhetstester)
Fokuserar på att API:et hanterar HTTP-förfrågningar korrekt, returnerar rätt statuskoder och skickar rätt data vidare.
*   **Happy Paths**: Säkerställer att giltiga anrop returnerar t.ex. `200 OK`, `201 Created` eller `204 NoContent`.
*   **Edge Cases & Felhantering**: Kontrollerar att systemet fångar upp felaktig input och returnerar rätt statuskod (t.ex. `400 Bad Request` vid ogiltig validering eller `404 Not Found` när en resurs saknas).
*   **Verifikation av anrop**: Via *Moq Verify* säkerställs att underliggande tjänster anropas exakt det antal gånger som förväntas (eller `Times.Never` vid fel).

### 2. Service-tester (Integrations-/Logiktester)
Fokuserar på affärslogiken och interaktionen med databasen (In-Memory).
*   **Affärsregler**: Säkerställer att logiken håller. Exempelvis testas att:
    *   Det *inte* går att boka ett flyg som är fullsatt.
    *   Det *inte* går att omboka ett flyg om det är mindre än en timme kvar till avgång.
    *   En bokning som redan är avbokad inte kan avbokas igen.
*   **Databasoperationer**: Kontrollerar att kaskadborttagningar fungerar (t.ex. att `BookingDetails` tas bort när en `Booking` raderas) och att sökfiltrering på datum och städer fungerar korrekt.

### Testresultat
*   **Täckning:** Testerna täcker samtliga kritiska flöden (Skapa användare, Sök flyg, Boka, Omboka, Avboka, Radera). 
*   **Tillförlitlighet:** Genom att använda `Parallelize(Scope = ExecutionScope.MethodLevel)` och unika In-Memory databasnamn för varje testmetod, exekveras testerna snabbt, parallellt och helt isolerat från varandra utan "State leakage".
*   **Status:** Samtliga skrivna tester i projektet passerar (grönt), vilket bekräftar att applikationens arkitektur, valideringsflöden (FluentValidation) och ServiceResult-mönster fungerar integrerat och exakt som designat.
