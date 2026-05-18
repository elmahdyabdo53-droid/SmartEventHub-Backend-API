# 🎯 SmartEventHub Backend API

Welcome to the **SmartEventHub** backend repository! This is a robust, production-ready RESTful API built for managing events, sessions, speakers, attendees, and automated notifications.

This project goes beyond simple CRUD operations. It implements strict business rules, role-based access control, and a clean layered architecture, demonstrating real-world backend engineering practices.

---

## 🚀 Key Features

* **Layered Architecture:** Strict separation of concerns (API, Services, Domain, Repositories, DTOs).
* **JWT Authentication & Authorization:** Secure endpoints using JSON Web Tokens with Role-Based Access Control (Admin, Speaker, Attendee).
* **Business Logic & Validation:** * Enforces session capacity limits (prevents overbooking).
  * Prevents duplicate registrations.
* **Soft Delete:** Implemented for Session Registrations to maintain data integrity without permanently losing records.
* **Pagination:** Efficient data retrieval for events and sessions.
* **Notifications System:** Automated inbox for users to track session updates (e.g., Room changes, cancellations).

---

## 🛠️ Tech Stack

* **Framework:** ASP.NET Core Web API
* **Language:** C#
* **ORM:** Entity Framework Core (EF Core)
* **Database:** MS SQL Server
* **Authentication:** JWT Bearer Authentication
* **Documentation:** Swagger / OpenAPI

---

## 📂 API Modules Overview

The API is divided into 5 main modules:

1. **Auth (`/api/auth`):** Registration and JWT Login.
2. **Events (`/api/events`):** CRUD operations for events (Admins only for creation/updates).
3. **Sessions (`/api/sessions`):** Managing sessions within events, including speaker assignments.
4. **Registrations (`/api/sessions/{id}/register`):** Attendee booking system with capacity checks and soft-delete cancellation.
5. **Notifications (`/api/notifications`):** User inbox for system alerts (Mark as read / read-all).

*(Check the Swagger UI for detailed request/response schemas)*

---

## 📸 Screenshots

*(Here is a look at the Swagger Documentation for the API endpoints)*

<img width="1366" height="768" alt="image" src="https://github.com/user-attachments/assets/a064ddf5-399d-4b13-bcaf-f8f4206c83b1" />
<img width="1366" height="768" alt="image" src="https://github.com/user-attachments/assets/82697c82-7485-4005-97f7-792ee0be193c" />
<img width="1366" height="768" alt="image" src="https://github.com/user-attachments/assets/1ea201c7-78fa-4e53-a982-2607297a6fde" />
<img width="1366" height="768" alt="image" src="https://github.com/user-attachments/assets/692f2780-42d7-44fe-ae9f-331a512e85c1" />


---

## ⚙️ How to Run Locally

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/YOUR_GITHUB_USERNAME/SmartEventHub-Backend-API.git](https://github.com/YOUR_GITHUB_USERNAME/SmartEventHub-Backend-API.git)
