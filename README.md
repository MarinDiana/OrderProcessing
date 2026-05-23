# OrderProcessing — Lab 4

## Descriere

Aplicatie ASP.NET Core Web API + SPA vanilla JavaScript care demonstreaza pattern-urile:

- Chain of Responsibility
- State Pattern

Aplicatia permite:
- creare comenzi
- validare pipeline
- tranzitii de stare
- vizualizare state machine
- afisare istoric tranzitii

---

# Pattern 1 — Chain of Responsibility

Pipeline-ul de validare este:

```mermaid
flowchart LR

StockValidation --> PriceValidation
PriceValidation --> FraudDetection
FraudDetection --> AgeVerification
```

Primul handler care detecteaza o problema opreste pipeline-ul si returneaza:

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

Comanda trece prin urmatoarele stari:

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

Starile implementate:

- PendingState
- ConfirmedState
- ProcessingState
- ShippedState
- DeliveredState
- CancelledState

---

# Diagrama de secventa

Flux complet de procesare comanda:

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

# Diagrama de clase

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

# Structura proiect

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
| POST | /orders | Creeaza comanda |
| POST | /orders/{id}/pay | Pending → Confirmed |
| POST | /orders/{id}/process | Confirmed → Processing |
| POST | /orders/{id}/ship | Processing → Shipped |
| POST | /orders/{id}/deliver | Shipped → Delivered |
| POST | /orders/{id}/cancel | Cancel comanda |
| GET | /orders/{id} | Detalii comanda |
| GET | /orders | Toate comenzile |

---

# Screenshot-uri

## Screenshot 1 — Comanda creata
![Screenshot 1](docs/screenshots/Screenshot1.png)
## Screenshot 2 — Tranzitii de stare
![Screenshot 2](docs/screenshots/Screenshot2.png)
Comanda trece prin:
- Pending
- Confirmed
- Processing
- Shipped
- Delivered

## Screenshot 3 — Tranzitie invalida
![Screenshot 3](docs/screenshots/Screenshot3.png)
Mesaj de eroare la încercarea unui Cancel invalid.

## Screenshot 4 — Validare esuata
![Screenshot 4](docs/screenshots/Screenshot4.png)
Exemplu de eroare returnata de pipeline-ul Chain of Responsibility.

---

# Tehnologii folosite

- ASP.NET Core Minimal API
- C#
- JavaScript
- HTML
- CSS
- Mermaid diagrams