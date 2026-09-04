# RaceDay API Endpoint Plan

## 1. Authentication

| HTTP Method | Route                | Description                                                                   | Role Required | Request Body                                                  | Expected Response                                                                                                            |
| ----------- | -------------------- | ----------------------------------------------------------------------------- | ------------- | ------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| POST        | `/api/auth/register` | Creates a new RaceDay user account as an Organiser or Participant.            | None          | `{ FirstName, LastName, Email, Password, PhoneNumber, Role }` | **201 Created** - User account created. **400 Bad Request** - Invalid data or role. **409 Conflict** - Email already exists. |
| POST        | `/api/auth/login`    | Authenticates a user and returns an access token for authorised API requests. | None          | `{ Email, Password }`                                         | **200 OK** - Login successful and token returned. **401 Unauthorized** - Invalid email or password.                          |

## 2. User Profile

| HTTP Method | Route                | Description                                                      | Role Required | Request Body                           | Expected Response                                                                                               |
| ----------- | -------------------- | ---------------------------------------------------------------- | ------------- | -------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| GET         | `/api/users/profile` | Returns the profile information of the currently logged-in user. | Any           | None                                   | **200 OK** - User profile returned. **401 Unauthorized** - User is not logged in.                               |
| PUT         | `/api/users/profile` | Updates the profile information of the currently logged-in user. | Any           | `{ FirstName, LastName, PhoneNumber }` | **200 OK** - Profile updated. **400 Bad Request** - Invalid data. **401 Unauthorized** - User is not logged in. |

## 3. Events

| HTTP Method | Route              | Description                                                         | Role Required | Request Body                                                                                                              | Expected Response                                                                                                                                              |
| ----------- | ------------------ | ------------------------------------------------------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET         | `/api/events`      | Returns a list of active upcoming events available on RaceDay.      | None          | None                                                                                                                      | **200 OK** - List of events returned.                                                                                                                          |
| GET         | `/api/events/{id}` | Returns detailed information about a specific event.                | None          | None                                                                                                                      | **200 OK** - Event details returned. **404 Not Found** - Event does not exist.                                                                                 |
| POST        | `/api/events`      | Creates a new road running, walking, or cycling event.              | Organiser     | `{ EventName, EventDescription, EventDate, Location, StartTime, EndTime, RouteDistance, EventType, PosterUrl }`           | **201 Created** - Event created. **400 Bad Request** - Invalid event data. **401 Unauthorized** - Not logged in. **403 Forbidden** - User is not an Organiser. |
| PUT         | `/api/events/{id}` | Updates an existing event belonging to the logged-in Organiser.     | Organiser     | `{ EventName, EventDescription, EventDate, Location, StartTime, EndTime, RouteDistance, EventType, PosterUrl, IsActive }` | **200 OK** - Event updated. **404 Not Found** - Event does not exist. **403 Forbidden** - User does not own the event.                                         |
| DELETE      | `/api/events/{id}` | Deletes or deactivates an event created by the logged-in Organiser. | Organiser     | None                                                                                                                      | **204 No Content** - Event deleted/deactivated. **404 Not Found** - Event does not exist. **403 Forbidden** - User does not own the event.                     |

## 4. Categories

| HTTP Method | Route                              | Description                                           | Role Required | Request Body                                                                        | Expected Response                                                                                                                                                               |
| ----------- | ---------------------------------- | ----------------------------------------------------- | ------------- | ----------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET         | `/api/events/{eventId}/categories` | Returns all categories belonging to a specific event. | None          | None                                                                                | **200 OK** - List of categories returned. **404 Not Found** - Event does not exist.                                                                                             |
| GET         | `/api/categories/{id}`             | Returns details of a specific event category.         | None          | None                                                                                | **200 OK** - Category returned. **404 Not Found** - Category does not exist.                                                                                                    |
| POST        | `/api/events/{eventId}/categories` | Creates a new participation category for an event.    | Organiser     | `{ CategoryName, CategoryDescription, MinAge, MaxAge, Gender, EntryFee }`           | **201 Created** - Category created. **400 Bad Request** - Invalid category data. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Event does not exist. |
| PUT         | `/api/categories/{id}`             | Updates an existing event category.                   | Organiser     | `{ CategoryName, CategoryDescription, MinAge, MaxAge, Gender, EntryFee, IsActive }` | **200 OK** - Category updated. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Category does not exist.                                                |
| DELETE      | `/api/categories/{id}`             | Deletes or deactivates an event category.             | Organiser     | None                                                                                | **204 No Content** - Category deleted/deactivated. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Category does not exist.                            |

## 5. Event Enrolments

| HTTP Method | Route                              | Description                                                                   | Role Required | Request Body                | Expected Response                                                                                                                                                                      |
| ----------- | ---------------------------------- | ----------------------------------------------------------------------------- | ------------- | --------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| POST        | `/api/events/{eventId}/enrolments` | Enrols the logged-in Participant into a selected category for an event.       | Participant   | `{ CategoryID }`            | **201 Created** - Enrolment created. **400 Bad Request** - Invalid category. **404 Not Found** - Event or category does not exist. **409 Conflict** - Participant is already enrolled. |
| GET         | `/api/enrolments/my`               | Returns all event enrolments belonging to the logged-in Participant.          | Participant   | None                        | **200 OK** - Participant's enrolments returned. **401 Unauthorized** - User is not logged in.                                                                                          |
| GET         | `/api/events/{eventId}/enrolments` | Returns all participants enrolled in a specific event.                        | Organiser     | None                        | **200 OK** - Event enrolments returned. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Event does not exist.                                                 |
| GET         | `/api/enrolments/{id}`             | Returns details of a specific enrolment.                                      | Any           | None                        | **200 OK** - Enrolment details returned. **403 Forbidden** - User is not authorised to view the enrolment. **404 Not Found** - Enrolment does not exist.                               |
| PUT         | `/api/enrolments/{id}/status`      | Updates the status of an enrolment, such as Pending, Confirmed, or Cancelled. | Organiser     | `{ Status, PaymentStatus }` | **200 OK** - Enrolment updated. **400 Bad Request** - Invalid status. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Enrolment does not exist.               |

## 6. Results

| HTTP Method | Route                                   | Description                                                      | Role Required | Request Body                               | Expected Response                                                                                                                                                                                                          |
| ----------- | --------------------------------------- | ---------------------------------------------------------------- | ------------- | ------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| POST        | `/api/enrolments/{enrolmentId}/results` | Captures the race result for a participant's enrolment.          | Organiser     | `{ FinishTime, ChipTime, Position, Pace }` | **201 Created** - Result recorded. **400 Bad Request** - Invalid result data. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Enrolment does not exist. **409 Conflict** - Result already exists. |
| GET         | `/api/results/my`                       | Returns all race results belonging to the logged-in Participant. | Participant   | None                                       | **200 OK** - Participant's results returned. **401 Unauthorized** - User is not logged in.                                                                                                                                 |
| GET         | `/api/results/{id}`                     | Returns details of a specific race result.                       | Any           | None                                       | **200 OK** - Result returned. **403 Forbidden** - User is not authorised to view the result. **404 Not Found** - Result does not exist.                                                                                    |
| PUT         | `/api/results/{id}`                     | Updates a participant's recorded race result.                    | Organiser     | `{ FinishTime, ChipTime, Position, Pace }` | **200 OK** - Result updated. **400 Bad Request** - Invalid result data. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Result does not exist.                                                    |
| DELETE      | `/api/results/{id}`                     | Removes an incorrectly recorded race result.                     | Organiser     | None                                       | **204 No Content** - Result deleted. **403 Forbidden** - User is not the event Organiser. **404 Not Found** - Result does not exist.                                                                                       |

## 7. Role Summary

### Organiser

Organisers can:

* Create events.
* Edit their events.
* Delete/deactivate their events.
* Create event categories.
* Edit event categories.
* Delete/deactivate categories.
* View all enrolments for their events.
* Update enrolment status.
* Capture participant results.
* Edit participant results.
* Delete incorrect results.

### Participant

Participants can:

* Create an account.
* Log into RaceDay.
* View and update their profile.
* Browse upcoming events.
* View event details.
* View event categories.
* Enrol in an event by selecting a category.
* View their own enrolments.
* View their personal race results.

### Public Users

Users who are not logged in can:

* Browse upcoming events.
* View individual event details.
* View event categories.
* Register for an account.
* Log into the system.

## 8. API Design Notes

All routes use the `/api/` prefix as required by the RaceDay specification.

Authentication will use a token-based approach. Protected endpoints will require the authenticated user's identity and role.

Role-based authorisation will be enforced at the API level in Part 2. Organisers will only be permitted to manage events and related information that they own.

Participants will only be able to access and manage their own enrolments and personal results.

The API will use standard HTTP status codes including `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found`, and `409 Conflict`.

The endpoint design corresponds to the RaceDay database entities: Users, Organisers, Events, Categories, Enrolments, and Results.
