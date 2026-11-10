# 🏃 RaceDay – Event Management System

## 📌 Project Overview

RaceDay is a full-stack web-based event management system designed for the South African road running, walking, and cycling community.

The system allows Event Organisers to create and manage sporting events, categories, participant enrolments, and race results. Participants can create accounts, browse upcoming events, enter events, view their enrolments, and track their personal race results.

The project was developed progressively across three parts:
- **Part 1:** System Planning and Database Design
- **Part 2:** RESTful API Development and Testing
- **Part 3:** MVC Web Application, Azure Blob Storage, and Docker Containerisation

---

## 👥 User Roles

### 🏢 Organiser
Organisers are responsible for managing sporting events on the RaceDay platform.

**Organisers can:**
- Log into the RaceDay system
- View an organiser dashboard
- Create, edit, and delete or deactivate events
- Upload event banner images
- Create, edit, and delete or deactivate event categories
- View participant enrolments and manage enrolment statuses
- Capture, edit, and delete participant results

### 🏃 Participant
Participants use RaceDay to register and participate in events.

**Participants can:**
- Create an account, log in, and manage their profile
- Upload a profile picture
- Browse upcoming events, apply filters, and view event details and available categories2
- Enrol in events and select a category
- View their enrolments and enrolment statuses
- View their personal race results and complete race history

---

## 📚 Part 1 – System Planning and Database

Part 1 focused on planning the RaceDay system before API development began.

**The following documents were produced:**
1. Entity Relationship Diagram (ERD)
2. API Endpoint Plan
3. SQL Server Database Script

### Database Entities
The RaceDay database contains the following main entities:
1. `Users`
2. `Organisers`
3. `Events`
4. `Categories`
5. `Enrolments`
6. `Results`

---

## 🔌 Part 2 – RESTful API Development and Testing

Part 2 implemented the backend RESTful Web API using **ASP.NET Core Web API (C#)** and **Entity Framework Core** with a **SQL Server** database, following the endpoint plan created during Part 1.

### Part 2 Features
- User registration, login, password hashing, and role-based authentication
- User profile management
- Event and category management
- Participant enrolments and status management
- Race result management
- Swagger / OpenAPI documentation
- Unit testing and GitHub Actions CI/CD workflows

Role-based access is strictly enforced at the API level for Organisers and Participants.

---

## 🌐 Part 3 – Automation of Web Application

Part 3 brings the RaceDay system together by adding the web-based frontend developed using **ASP.NET Core MVC**. It communicates entirely with the Part 2 RESTful API for all application data operations.

### System Architecture

```text
                         USER
                           |
                           v
              +-------------------------+
              |   ASP.NET Core MVC      |
              |    Web Application      |
              +-----------+-------------+
                          |
                          | HTTP / REST
                          v
              +-------------------------+
              |      RaceDay API        |
              |   ASP.NET Core Web API  |
              +-----------+-------------+
                          |
                  +-------+-------+
                  |               |
                  v               v
          +-------------+   +-------------------+
          | SQL Server  |   | Azure Blob        |
          |  Database   |   | Storage           |
          +-------------+   +-------------------+

## System Architecture

- **MVC Application Responsibilities:** User interface, session management, role-based navigation, presentation of data, and communication with the API.
- **API Responsibilities:** Business operations, authentication, data access, database operations, file uploads, and Azure Blob Storage integration.

---

## 🚀 Part 3 Features

### 🖥️ MVC Web Application
- Consumes the Part 2 RESTful API using controllers and views
- Provides role-specific navigation, responsive layouts, and authenticated sessions
- Displays validation messages and handles API errors gracefully

### 🔐 Authentication and Session Management
Authentication is performed by communicating with the Part 2 API, storing authenticated user information and roles via ASP.NET Core session management.

- **Organiser Navigation:** Dashboard | Events | Categories | Enrolments | Results | Profile | Logout
- **Participant Navigation:** Home | Events | My Enrolments | My Results | Profile | Logout

### 🏢 Organiser Interface
- **Organiser Dashboard:** Overview of total events created, total participant enrolments, upcoming event dates, and enrolment summaries.
- **Event Management:** Create, edit, delete, or deactivate events (Name, Description, Date, Location, Distance, Event Type, Banner Image).
- **Category Management:** Manage age, distance, or event-specific categories.
- **Enrolments:** View participants, selected categories, and update enrolment statuses.
- **Results:** Capture, edit, or delete finish times and finishing positions after an event takes place.

### 🏃 Participant Interface
- **Home Page & Event Browsing:** Filter upcoming events by type, location, distance, and date.
- **Event Details & Enrolment:** View complete event info and banners, choose categories, and submit enrolments.
- **My Enrolments & Statuses:** Track entry states using status color coding:
  - **Confirmed** → Green
  - **Pending** → Amber
  - **Rejected** → Red
- **My Results:** View personal race history (Event name, Date, Category, Finish time, Position).
- **Profile Management:** View/update personal info and upload profile pictures.

---

## ☁️ Azure Blob Storage

Azure Blob Storage is integrated into the Part 2 API for cloud-based image storage (event banner images and participant profile pictures).

### File Upload Architecture

```text
+---------------------+
|   MVC Application   |
+----------+----------+
           |
           v
+---------------------+
|    RaceDay API      |
+----------+----------+
           |
           v
+---------------------+
| Azure Blob Storage  |
+---------------------+

## 🐳 Docker Containerisation

The RaceDay MVC application includes a multi-stage `Dockerfile` optimizing build and runtime environments.

### Build and Run Docker Container
Execute the following commands from the Part 3 MVC project directory:

```bash
# Build the Docker image
docker build -t raceday-mvc .

# Run the Docker container
docker run -d -p 8080:8080 --name raceday-mvc raceday-mvc ## 📦 GitHub and Version Control

- Maintained in the official RaceDay GitHub repository with a minimum of 20 meaningful commits for Part 3.
- **GitHub Actions CI/CD:** Automated workflow verifying .NET SDK setup, solution restoration, project compilation, unit tests, and structural integrity.

---

## 📁 Repository Structure

```text
RaceDay/
│
├── .github/
│   └── workflows/
│       ├── part1-ci.yml
│       ├── part2-ci.yml
│       └── part3-ci.yml
│
├── docs/
│   ├── RaceDay_ERD.png
│   ├── RaceDay_API_Endpoint_Plan.md
│   ├── RaceDay_Database.sql
│   ├── ci-green-build.png
│   └── part3-screenshots/
│
├── src/
│   ├── RaceDay.Api/
│   ├── RaceDay.Core/
│   ├── RaceDay.Tests/
│   └── RaceDay.Web/
│
├── RaceDay.sln
└── README.md
## 🛠️ Technologies Used

| Technology | Purpose |
| :--- | :--- |
| **C#** | Application programming language |
| **ASP.NET Core MVC** | Web frontend |
| **ASP.NET Core Web API** | Backend REST API |
| **Entity Framework Core** | Database access in the API |
| **SQL Server** | Relational database |
| **Bootstrap** | Responsive UI styling |
| **Swagger / OpenAPI** | API documentation and testing |
| **Azure Blob Storage** | Cloud image storage |
| **Docker** | Application containerisation |
| **GitHub Actions** | CI/CD automation |

---

## 🎥 Part 3 Demonstration Video

- **YouTube Video Link:** `🔗 PASTE YOUR PART 3 UNLISTED YOUTUBE LINK HERE`

---

## 🏁 Final Summary

RaceDay successfully combines database planning, a robust RESTful backend API, and a dynamic ASP.NET Core MVC frontend into a complete, containerized platform for managing sporting events.
