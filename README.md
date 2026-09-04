# RaceDay – Event Management System

## Project Overview

RaceDay is a full-stack web-based event management system designed for the South African road running, walking, and cycling community.

The system allows Event Organisers to create and manage sporting events, categories, participant enrolments and race results. Participants can create accounts, browse upcoming events, enter events, view their enrolments and track their personal race results.

The project is being developed progressively across three parts:

- **Part 1:** System Planning and Database Design
- **Part 2:** RESTful API Development and Testing
- **Part 3:** MVC Web Application, Azure Blob Storage and Docker Containerisation

---

# User Roles

## Organiser

Organisers are responsible for managing sporting events on the RaceDay platform.

Organisers can:

- Create events
- Edit events
- Delete or deactivate events
- Create event categories
- Edit categories
- Delete or deactivate categories
- View participant enrolments
- Manage enrolment status
- Capture participant results
- Edit results
- Delete incorrect results

## Participant

Participants use RaceDay to register and participate in events.

Participants can:

- Create an account
- Log into the system
- Manage their profile
- Browse upcoming events
- View event details
- View event categories
- Enrol in events
- Select an event category
- View their own enrolments
- View their personal race results

---

# Part 1 – System Planning and Database

Part 1 focuses on planning the RaceDay system before API development begins.

The following documents have been produced:

1. Entity Relationship Diagram (ERD)
2. API Endpoint Plan
3. SQL Server Database Script

---

## Database Entities

The RaceDay database contains the following entities:

1. Users
2. Organisers
3. Events
4. Categories
5. Enrolments
6. Results

The database relationships allow:

- Organisers to manage events
- Events to contain categories
- Participants to enrol in events
- Enrolments to have race results

---

## Entity Relationship Diagram

The Entity Relationship Diagram shows the structure of the RaceDay database and the relationships between the entities.

![RaceDay ERD](docs/RaceDay_ERD.png)

---

# Project Documentation

The `/docs` folder contains the following project documentation:

| File | Description |
|---|---|
| `RaceDay_ERD.png` | Entity Relationship Diagram |
| `RaceDay_API_Endpoint_Plan.md` | RESTful API endpoint planning document |
| `RaceDay_Database.sql` | SQL Server database creation and sample data script |
| `ci-green-build.png` | Successful GitHub Actions CI/CD build screenshot |

---

# Database Setup

The RaceDay database was designed for Microsoft SQL Server and can be executed using SQL Server Management Studio (SSMS).

## Steps

1. Open **SQL Server Management Studio (SSMS)**.
2. Create a new query.
3. Open `docs/RaceDay_Database.sql`.
4. Copy the complete SQL script into the query window if necessary.
5. Execute the complete script.
6. The script creates the `RaceDay` database.
7. The script creates all required tables and constraints.
8. Sample users, organisers, events, categories, enrolments and results are inserted.
9. Verification queries are included at the end of the script.

---

## Expected Seed Data

| Entity | Records |
|---|---:|
| Users | 4 |
| Organisers | 2 |
| Events | 3 |
| Categories | 7 |
| Enrolments | 4 |
| Results | 2 |

---

# API Planning

The API Endpoint Plan defines the RESTful API that will be implemented in Part 2.

The planned API covers:

- Authentication
- User profiles
- Events
- Categories
- Event enrolments
- Results

Role-based access will be enforced at API level during Part 2 development.

The complete endpoint plan can be found in:

`docs/RaceDay_API_Endpoint_Plan.md`

---

# GitHub Actions / CI/CD

GitHub Actions is used to automatically validate the Part 1 repository structure.

The workflow checks that:

- The `/docs` folder exists
- The ERD exists
- The API endpoint plan exists
- The SQL database script exists
- The required database entities are present in the SQL script
- The README file exists

## Successful CI/CD Build

The successful GitHub Actions build is shown below.

![Successful CI/CD Build](docs/ci-green-build.png)

---

# Video Presentation

An unlisted YouTube video demonstrating Part 1 will be provided below.

The video demonstrates:

- RaceDay system overview
- ERD and database relationships
- API endpoint plan
- SQL database script
- Running the SQL script in SSMS
- Verification of the created database
- Verification of the sample data

## YouTube Video

**Video Link:**  
_Paste your unlisted YouTube link here after recording._

---

# Repository Structure

```text
RaceDay/
│
├── .github/
│   └── workflows/
│       └── part1-ci.yml
│
├── docs/
│   ├── RaceDay_ERD.png
│   ├── RaceDay_API_Endpoint_Plan.md
│   ├── RaceDay_Database.sql
│   └── ci-green-build.png
│
└── README.md
