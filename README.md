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
- Browse upcoming events
- Apply filters and view event details and available categories
- Enrol in events and select a category
- View their enrolments and enrolment statuses
- View their personal race results and complete race history

---

# 📚 Part 1 – System Planning and Database

Part 1 focused on planning the RaceDay system before API development began.

### Deliverables

The following documents were produced:

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

# 🔌 Part 2 – RESTful API Development and Testing

Part 2 implemented the backend RESTful Web API using **ASP.NET Core Web API (C#)** and **Entity Framework Core** with a **SQL Server** database.

The API follows the endpoint plan created during Part 1.

### Part 2 Features

- User registration and login
- Password hashing
- Role-based authentication
- User profile management
- Event management
- Category management
- Participant enrolments
- Enrolment status management
- Race result management
- Swagger / OpenAPI documentation
- Unit testing
- GitHub Actions CI/CD workflows

Role-based access is enforced at the API level for **Organisers** and **Participants**.

---

# 🌐 Part 3 – MVC Web Application

Part 3 brings the RaceDay system together by adding a web-based frontend developed using **ASP.NET Core MVC**.

The MVC application communicates with the Part 2 RESTful API for application data and business operations.

## 🏗️ System Architecture

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
```

### System Responsibilities

**MVC Application Responsibilities:**

- User interface
- Session management
- Role-based navigation
- Presentation of data
- Communication with the API
- Validation and display of API responses

**API Responsibilities:**

- Business operations
- Authentication
- Data access
- Database operations
- File uploads
- Azure Blob Storage integration

---

# 🚀 Part 3 Features

## 🖥️ MVC Web Application

The MVC application:

- Consumes the Part 2 RESTful API using controllers and views
- Provides role-specific navigation
- Provides responsive layouts
- Handles authenticated sessions
- Displays validation messages
- Handles API errors gracefully

---

## 🔐 Authentication and Session Management

Authentication is performed by communicating with the Part 2 API.

Authenticated user information and roles are stored using **ASP.NET Core session management**.

### Organiser Navigation

```text
Dashboard
Events
Categories
Enrolments
Results
Profile
Logout
```

### Participant Navigation

```text
Home
Events
My Enrolments
My Results
Profile
Logout
```

---

# 🏢 Organiser Interface

### 📊 Organiser Dashboard

The dashboard provides an overview of:

- Total events created
- Total participant enrolments
- Upcoming event dates
- Enrolment summaries

### 🏁 Event Management

Organisers can:

- Create events
- Edit events
- Delete events
- Deactivate events
- Upload event banner images

Event information includes:

- Event Name
- Description
- Date
- Location
- Distance
- Event Type
- Banner Image

### 🏷️ Category Management

Organisers can:

- Create categories
- Edit categories
- Delete categories
- Deactivate categories

### 👥 Enrolment Management

Organisers can:

- View participants
- View selected categories
- View enrolment information
- Update enrolment statuses

### 🏆 Results Management

Organisers can:

- Capture race results
- Edit results
- Delete results
- Record finish times
- Record finishing positions

---

# 🏃 Participant Interface

## 🏠 Home Page and Event Browsing

Participants can:

- Browse upcoming events
- Filter events by type
- Filter events by location
- Filter events by distance
- Filter events by date
- View event details

## 📝 Event Enrolment

Participants can:

- View complete event information
- View event banners
- View available categories
- Select a category
- Submit an enrolment

## 📋 My Enrolments

Participants can track their enrolment status using colour-coded statuses:

| Status | Meaning |
|---|---|
| 🟢 **Confirmed** | Enrolment has been approved |
| 🟠 **Pending** | Enrolment is awaiting processing |
| 🔴 **Rejected** | Enrolment has been rejected |

## 🏆 My Results

Participants can view their personal race history, including:

- Event name
- Event date
- Category
- Finish time
- Finishing position

## 👤 Profile Management

Participants can:

- View their personal information
- Update their profile
- Upload a profile picture

---

# ☁️ Azure Blob Storage

Azure Blob Storage is integrated into the RaceDay API for cloud-based image storage.

It is used for:

- Event banner images
- Participant profile pictures

## 📤 File Upload Architecture

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
```

The MVC application sends upload requests to the API, and the API handles the communication with Azure Blob Storage.

---

# 🐳 Docker Containerisation

The RaceDay MVC application includes a multi-stage `Dockerfile` to optimise the build and runtime environments.

## Build the Docker Image

Run the following command from the Part 3 MVC project directory:

```bash
docker build -t raceday-mvc .
```

## Run the Docker Container

```bash
docker run -d -p 8080:8080 --name raceday-mvc raceday-mvc
```

The application can then be accessed through the configured local Docker port.

---

# 📦 GitHub and Version Control

The RaceDay project is maintained in a GitHub repository.

Part 3 includes a minimum of **20 meaningful commits** documenting the development process.

## GitHub Actions CI/CD

Automated GitHub Actions workflows are used to verify:

- .NET SDK setup
- Solution restoration
- Project compilation
- Unit tests
- Project structure
- Build integrity

---

# 📁 Repository Structure

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
```

---

# 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| **C#** | Application programming language |
| **ASP.NET Core MVC** | Web frontend |
| **ASP.NET Core Web API** | Backend REST API |
| **Entity Framework Core** | Database access |
| **SQL Server** | Relational database |
| **Bootstrap** | Responsive UI styling |
| **Swagger / OpenAPI** | API documentation and testing |
| **Azure Blob Storage** | Cloud image storage |
| **Docker** | Application containerisation |
| **GitHub Actions** | CI/CD automation |

---

# 🎥 Part 3 Demonstration Video

**YouTube Video:**  
`🔗 PASTE YOUR PART 3 UNLISTED YOUTUBE LINK HERE`

---

# 🏁 Final Summary

RaceDay successfully combines database planning, a RESTful backend API, and a dynamic ASP.NET Core MVC frontend into a complete web-based sporting event management system.

The system provides separate functionality for **Organisers** and **Participants**, supports secure authentication and role-based access, manages sporting events and enrolments, records race results, stores images using Azure Blob Storage, and supports Docker containerisation.

The project demonstrates the use of modern software development practices including:

- RESTful API development
- MVC architecture
- Entity Framework Core
- SQL Server database integration
- Authentication and session management
- Cloud storage
- Unit testing
- GitHub Actions CI/CD
- Docker containerisation
- Git version control

---

## 👨‍💻 Project

**RaceDay – Event Management System**

**Technology Stack:** C# | ASP.NET Core | SQL Server | Entity Framework Core | Azure | Docker | GitHub Actions
