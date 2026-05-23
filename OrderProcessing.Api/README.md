# OrderProcessing — Lab 4

## Descriere

Aplicație ASP.NET Core Web API + SPA vanilla JavaScript care demonstrează pattern-urile:

- Chain of Responsibility
- State Pattern

Aplicația permite:
- creare comenzi
- validare pipeline
- tranziții de stare
- vizualizare state machine
- afișare istoric tranziții

---

# Pattern 1 — Chain of Responsibility

Pipeline-ul de validare este:

```mermaid
flowchart LR

StockValidation --> PriceValidation
PriceValidation --> FraudDetection
FraudDetection --> AgeVerification
```

Primul handler care detectează o problemă oprește pipeline-ul și returnează:

```text
ValidationResult.Failed()
```

Handler-ele implementate:

- StockValidationHandler
- PriceValidationHandler
- FraudDetectionHandler
- AgeVerificationHandler

---

# Pattern 2 — State Pattern

Comanda trece prin următoarele stări:

```mermaid
stateDiagram-v2

[*] --> Pending

Pending --> Confirmed : Pay()
Pending --> Cancelled : Cancel()

Confirmed --> Processing : Process()
Confirmed --> Cancelled : Cancel()

Processing --> Shipped : Ship()
Processing --> Cancelled : Cancel()

Shipped --> Delivered : Deliver()

Delivered --> [*]
Cancelled --> [*]
```

Stările implementate:

- PendingState
- ConfirmedState
- ProcessingState
- ShippedState
- DeliveredState
- CancelledState

---

# Diagramă de secvență

Flux complet de procesare comandă:

```mermaid
sequenceDiagram

actor User

User->>API: POST /orders
API->>OrderService: CreateOrder()

OrderService->>StockValidation: Handle()
StockValidation-->>OrderService: OK

OrderService->>PriceValidation: Handle()
PriceValidation-->>OrderService: OK

OrderService->>FraudDetection: Handle()
FraudDetection-->>OrderService: OK

OrderService->>AgeVerification: Handle()
AgeVerification-->>OrderService: OK

OrderService->>Order: Create()

User->>API: POST /orders/{id}/pay
API->>OrderService: PayOrder()
OrderService->>Order: Pay()

User->>API: POST /orders/{id}/process
API->>OrderService: ProcessOrder()
OrderService->>Order: Process()

User->>API: POST /orders/{id}/ship
API->>OrderService: ShipOrder()
OrderService->>Order: Ship()

User->>API: POST /orders/{id}/deliver
API->>OrderService: DeliverOrder()
OrderService->>Order: Deliver()
```

---

# Diagramă de clase

```mermaid
classDiagram

class Order {
    +OrderId Id
    +Customer Customer
    +Address ShippingAddress
    +Money Total
    +string CurrentState
    +Pay()
    +Process()
    +Ship()
    +Deliver()
    +Cancel()
}

class IOrderState
class PendingState
class ConfirmedState
class ProcessingState
class ShippedState
class DeliveredState
class CancelledState

IOrderState <|.. PendingState
IOrderState <|.. ConfirmedState
IOrderState <|.. ProcessingState
IOrderState <|.. ShippedState
IOrderState <|.. DeliveredState
IOrderState <|.. CancelledState

class IOrderValidationHandler

class StockValidationHandler
class PriceValidationHandler
class FraudDetectionHandler
class AgeVerificationHandler

IOrderValidationHandler <|.. StockValidationHandler
IOrderValidationHandler <|.. PriceValidationHandler
IOrderValidationHandler <|.. FraudDetectionHandler
IOrderValidationHandler <|.. AgeVerificationHandler
```

---

# Structură proiect

```text
Domain/
States/
Validation/
Services/
Endpoints/
wwwroot/
```

---

# Endpoint-uri API

| Method | Endpoint | Descriere |
|---|---|---|
| POST | /orders | Creează comandă |
| POST | /orders/{id}/pay | Pending → Confirmed |
| POST | /orders/{id}/process | Confirmed → Processing |
| POST | /orders/{id}/ship | Processing → Shipped |
| POST | /orders/{id}/deliver | Shipped → Delivered |
| POST | /orders/{id}/cancel | Cancel comandă |
| GET | /orders/{id} | Detalii comandă |
| GET | /orders | Toate comenzile |

---

# Screenshot-uri

## Screenshot 1 — Comandă creată
![Screenshot 1](docs/screenshots/screenshot1.png)
## Screenshot 2 — Tranziții de stare
![Screenshot 2](docs/screenshots/screenshot2.png)
Comanda trece prin:
- Pending
- Confirmed
- Processing
- Shipped
- Delivered

## Screenshot 3 — Tranziție invalidă
![Screenshot 3](docs/screenshots/screenshot3.png)
Mesaj de eroare la încercarea unui Cancel invalid.

## Screenshot 4 — Validare eșuată
![Screenshot 4](docs/screenshots/screenshot4.png)
Exemplu de eroare returnată de pipeline-ul Chain of Responsibility.

---

# Tehnologii folosite

- ASP.NET Core Minimal API
- C#
- JavaScript
- HTML
- CSS
- Mermaid diagrams