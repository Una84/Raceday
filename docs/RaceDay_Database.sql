IF DB_ID('RaceDay') IS NOT NULL
BEGIN
    EXEC('ALTER DATABASE RaceDay SET SINGLE_USER WITH ROLLBACK IMMEDIATE');
    EXEC('DROP DATABASE RaceDay');
END;

EXEC('CREATE DATABASE RaceDay');

USE RaceDay;


CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME(),
    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT 1,

    CONSTRAINT PK_Users
        PRIMARY KEY (UserID),

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);


CREATE TABLE Organisers
(
    OrganiserID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    OrganisationName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Website NVARCHAR(150) NULL,
    LogoUrl NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Organisers_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Organisers
        PRIMARY KEY (OrganiserID),

    CONSTRAINT UQ_Organisers_UserID
        UNIQUE (UserID),

    CONSTRAINT FK_Organisers_Users
        FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);


CREATE TABLE Events
(
    EventID INT IDENTITY(1,1) NOT NULL,
    OrganiserID INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    EventDescription NVARCHAR(1000) NULL,
    EventDate DATE NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NULL,
    RouteDistance DECIMAL(6,2) NULL,
    EventType NVARCHAR(20) NOT NULL,
    PosterUrl NVARCHAR(255) NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Events_IsActive DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Events_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Events
        PRIMARY KEY (EventID),

    CONSTRAINT FK_Events_Organisers
        FOREIGN KEY (OrganiserID)
        REFERENCES Organisers(OrganiserID),

    CONSTRAINT CK_Events_EventType
        CHECK (EventType IN ('Run', 'Walk', 'Cycle')),

    CONSTRAINT CK_Events_RouteDistance
        CHECK (RouteDistance IS NULL OR RouteDistance > 0),

    CONSTRAINT CK_Events_EventTimes
        CHECK (EndTime IS NULL OR EndTime > StartTime)
);


CREATE TABLE Categories
(
    CategoryID INT IDENTITY(1,1) NOT NULL,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryDescription NVARCHAR(255) NULL,
    MinAge INT NULL,
    MaxAge INT NULL,
    Gender NVARCHAR(20) NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Categories_IsActive DEFAULT 1,

    CONSTRAINT PK_Categories
        PRIMARY KEY (CategoryID),

    CONSTRAINT FK_Categories_Events
        FOREIGN KEY (EventID)
        REFERENCES Events(EventID),

    CONSTRAINT CK_Categories_Age
        CHECK
        (
            (MinAge IS NULL OR MinAge >= 0)
            AND
            (MaxAge IS NULL OR MaxAge >= 0)
            AND
            (MinAge IS NULL OR MaxAge IS NULL OR MaxAge >= MinAge)
        ),

    CONSTRAINT CK_Categories_Gender
        CHECK
        (
            Gender IS NULL
            OR Gender IN ('Open', 'Male', 'Female')
        ),

    CONSTRAINT CK_Categories_EntryFee
        CHECK (EntryFee >= 0),

    CONSTRAINT UQ_Categories_Event_Category
        UNIQUE (EventID, CategoryName)
);


CREATE TABLE Enrolments
(
    EnrolmentID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL
        CONSTRAINT DF_Enrolments_EnrolmentDate DEFAULT SYSDATETIME(),
    Status NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Enrolments_Status DEFAULT 'Pending',
    PaymentStatus NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Enrolments_PaymentStatus DEFAULT 'Unpaid',
    AmountPaid DECIMAL(10,2) NOT NULL
        CONSTRAINT DF_Enrolments_AmountPaid DEFAULT 0,
    ReferenceNumber NVARCHAR(100) NULL,

    CONSTRAINT PK_Enrolments
        PRIMARY KEY (EnrolmentID),

    CONSTRAINT FK_Enrolments_Users
        FOREIGN KEY (UserID)
        REFERENCES Users(UserID),

    CONSTRAINT FK_Enrolments_Events
        FOREIGN KEY (EventID)
        REFERENCES Events(EventID),

    CONSTRAINT FK_Enrolments_Categories
        FOREIGN KEY (CategoryID)
        REFERENCES Categories(CategoryID),

    CONSTRAINT CK_Enrolments_Status
        CHECK
        (
            Status IN ('Pending', 'Confirmed', 'Cancelled')
        ),

    CONSTRAINT CK_Enrolments_PaymentStatus
        CHECK
        (
            PaymentStatus IN ('Unpaid', 'Paid', 'Refunded')
        ),

    CONSTRAINT CK_Enrolments_AmountPaid
        CHECK (AmountPaid >= 0),

    CONSTRAINT UQ_Enrolments_User_Event
        UNIQUE (UserID, EventID)
);


CREATE TABLE Results
(
    ResultID INT IDENTITY(1,1) NOT NULL,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    ChipTime TIME NULL,
    Position INT NOT NULL,
    Pace DECIMAL(6,2) NULL,
    RecordedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Results_RecordedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Results
        PRIMARY KEY (ResultID),

    CONSTRAINT FK_Results_Enrolments
        FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolments(EnrolmentID),

    CONSTRAINT UQ_Results_EnrolmentID
        UNIQUE (EnrolmentID),

    CONSTRAINT CK_Results_Position
        CHECK (Position > 0),

    CONSTRAINT CK_Results_Pace
        CHECK (Pace IS NULL OR Pace > 0)
);


INSERT INTO Userss
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    PhoneNumber,
    Role
)
VALUES
(
    'John',
    'Mokoena',
    'john.mokoena@raceday.co.za',
    'PART1_SAMPLE_HASH_JOHN',
    '0712345678',
    'Organiser'
),
(
    'Thandi',
    'Ndlovu',
    'thandi.ndlovu@raceday.co.za',
    'PART1_SAMPLE_HASH_THANDI',
    '0723456789',
    'Organiser'
),
(
    'Sipho',
    'Dlamini',
    'sipho.dlamini@example.com',
    'PART1_SAMPLE_HASH_SIPHO',
    '0734567890',
    'Participant'
),
(
    'Lerato',
    'Maseko',
    'lerato.maseko@example.com',
    'PART1_SAMPLE_HASH_LERATO',
    '0745678901',
    'Participant'
);


INSERT INTO Organiser
(
    UserID,
    OrganisationName,
    Description,
    Website,
    LogoUrl
)
VALUES
(
    1,
    'Limpopo Road Runners',
    'A South African road running organisation that hosts running and walking events in Limpopo.',
    'https://www.limpoporoadrunners.co.za',
    NULL
),
(
    2,
    'Mzansi Active Events',
    'An event organisation specialising in road running, cycling and community fitness events.',
    'https://www.mzansiactive.co.za',
    NULL
);


INSERT INTO Event
(
    OrganiserID,
    EventName,
    EventDescription,
    EventDate,
    Location,
    StartTime,
    EndTime,
    RouteDistance,
    EventType,
    PosterUrl
)
VALUES
(
    1,
    'Polokwane City Run',
    'A community road running event through Polokwane featuring multiple race categories.',
    '2026-10-18',
    'Polokwane, Limpopo',
    '06:30:00',
    '11:00:00',
    21.10,
    'Run',
    NULL
),
(
    1,
    'Limpopo Community Walk',
    'A family-friendly community walking event promoting healthy and active lifestyles.',
    '2026-11-08',
    'Tzaneen, Limpopo',
    '07:00:00',
    '10:30:00',
    10.00,
    'Walk',
    NULL
),
(
    2,
    'Cape Town Cycle Challenge',
    'A recreational cycling event for participants of different experience levels.',
    '2026-11-22',
    'Cape Town, Western Cape',
    '06:00:00',
    '12:00:00',
    40.00,
    'Cycle',
    NULL
);


INSERT INTO Category
(
    EventID,
    CategoryName,
    CategoryDescription,
    MinAge,
    MaxAge,
    Gender,
    EntryFee
)
VALUES
(
    1,
    '21.1 km Open',
    'Half-marathon category open to eligible participants.',
    18,
    NULL,
    'Open',
    180.00
),
(
    1,
    '10 km Open',
    'Ten kilometre road race category.',
    16,
    NULL,
    'Open',
    120.00
),
(
    1,
    '5 km Fun Run',
    'Short community fun run suitable for recreational participants.',
    13,
    NULL,
    'Open',
    80.00
),
(
    2,
    '10 km Walk',
    'Ten kilometre community walking category.',
    16,
    NULL,
    'Open',
    100.00
),
(
    2,
    '5 km Family Walk',
    'Five kilometre family and community walking category.',
    10,
    NULL,
    'Open',
    60.00
),
(
    3,
    '40 km Cycle',
    'Forty kilometre recreational cycling category.',
    16,
    NULL,
    'Open',
    200.00
),
(
    3,
    '20 km Cycle',
    'Twenty kilometre cycling category for recreational riders.',
    13,
    NULL,
    'Open',
    130.00
);


INSERT INTO Enrolment
(
    UserID,
    EventID,
    CategoryID,
    EnrolmentDate,
    Status,
    PaymentStatus,
    AmountPaid,
    ReferenceNumber
)
VALUES
(
    3,
    1,
    1,
    '2026-09-01 09:15:00',
    'Confirmed',
    'Paid',
    180.00,
    'RD-2026-0001'
),
(
    4,
    1,
    2,
    '2026-09-01 10:20:00',
    'Confirmed',
    'Paid',
    120.00,
    'RD-2026-0002'
),
(
    3,
    2,
    4,
    '2026-09-02 08:30:00',
    'Confirmed',
    'Paid',
    100.00,
    'RD-2026-0003'
),
(
    4,
    3,
    7,
    '2026-09-02 11:45:00',
    'Pending',
    'Unpaid',
    0.00,
    NULL
);


INSERT INTO Result
(
    EnrolmentID,
    FinishTime,
    ChipTime,
    Position,
    Pace
)
VALUES
(
    1,
    '01:52:35',
    '01:51:58',
    24,
    5.31
),
(
    2,
    '00:54:20',
    '00:53:48',
    41,
    5.38
);


SELECT
    UserID,
    FirstName,
    LastName,
    Email,
    PhoneNumber,
    Role,
    CreatedAt,
    IsActive
FROM Users;


SELECT
    o.OrganiserID,
    u.FirstName,
    u.LastName,
    u.Email,
    o.OrganisationName,
    o.Description
FROM Organisers o
INNER JOIN Users u
    ON o.UserID = u.UserID;


SELECT
    e.EventID,
    e.EventName,
    e.EventDate,
    e.Location,
    e.EventType,
    e.RouteDistance,
    o.OrganisationName
FROM Events e
INNER JOIN Organisers o
    ON e.OrganiserID = o.OrganiserID;


SELECT
    c.CategoryID,
    e.EventName,
    c.CategoryName,
    c.MinAge,
    c.MaxAge,
    c.Gender,
    c.EntryFee
FROM Categories c
INNER JOIN Events e
    ON c.EventID = e.EventID
ORDER BY e.EventID, c.CategoryID;


SELECT
    en.EnrolmentID,
    u.FirstName + ' ' + u.LastName AS ParticipantName,
    e.EventName,
    c.CategoryName,
    en.EnrolmentDate,
    en.Status,
    en.PaymentStatus,
    en.AmountPaid,
    en.ReferenceNumber
FROM Enrolments en
INNER JOIN Users u
    ON en.UserID = u.UserID
INNER JOIN Events e
    ON en.EventID = e.EventID
INNER JOIN Categories c
    ON en.CategoryID = c.CategoryID
ORDER BY en.EnrolmentID;


SELECT
    r.ResultID,
    u.FirstName + ' ' + u.LastName AS ParticipantName,
    e.EventName,
    c.CategoryName,
    r.FinishTime,
    r.ChipTime,
    r.Position,
    r.Pace,
    r.RecordedAt
FROM Results r
INNER JOIN Enrolments en
    ON r.EnrolmentID = en.EnrolmentID
INNER JOIN Users u
    ON en.UserID = u.UserID
INNER JOIN Events e
    ON en.EventID = e.EventID
INNER JOIN Categories c
    ON en.CategoryID = c.CategoryID
ORDER BY r.Position;


SELECT 'Users' AS EntityName, COUNT(*) AS RecordCount
FROM Users

UNION ALL

SELECT 'Organisers', COUNT(*)
FROM Organisers

UNION ALL

SELECT 'Events', COUNT(*)
FROM Events

UNION ALL

SELECT 'Categories', COUNT(*)
FROM Categories

UNION ALL

SELECT 'Enrolments', COUNT(*)
FROM Enrolments

UNION ALL

SELECT 'Results', COUNT(*)
FROM Results;

