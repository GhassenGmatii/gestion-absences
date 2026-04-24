# 🗄️ Schéma de Base de Données

## Diagramme ER (Entity Relationship)

```
┌──────────────────────┐
│       Users          │
├──────────────────────┤
│ PK │ Id (int)        │
│    │ FirstName       │
│    │ LastName        │
│    │ Email (unique)  │
│    │ PasswordHash    │
│    │ Role            │
│    │ IsActive        │
│    │ CreatedAt       │
│    │ UpdatedAt       │
└──────┬───────────────┘
       │
       │ (1)
    ┌──┴───┐
    │ (1)  │ (*)
    │      │
    │      ▼
    │  ┌──────────────────────┐
    │  │      Classes         │
    │  ├──────────────────────┤
    │  │ PK │ Id (int)        │
    │  │ FK │ TeacherId       │◄───┐
    │  │    │ Name            │    │
    │  │    │ Description     │    │
    │  │    │ CreatedAt       │    │
    │  └──────┬───────────────┘    │
    │         │                    │
    │         │ (1)                │
    │      (*) │                    │
    │         │                    │
    │         ▼                    │
    │  ┌──────────────────────┐    │
    │  │     Students         │    │
    │  ├──────────────────────┤    │
    │  │ PK │ Id (int)        │    │
    │  │ FK │ UserId      ◄───┼────┼─ Users(*)
    │  │ FK │ ClassId     ◄───┤    │
    │  │    │ EnrolledAt  │    │
    │  │    │ UNIQUE(UserId,│  │
    │  │    │        ClassId)│  │
    │  └──────┬────────────────┘  │
    │         │                   │
    │         │ (1)               │
    │      (*) │                   │
    │         │                   │
    │         ▼                   │
    │  ┌──────────────────────┐   │
    │  │     Absences         │   │
    │  ├──────────────────────┤   │
    │  │ PK │ Id (int)        │   │
    │  │ FK │ StudentId   ◄───┘   │
    │  │ FK │ ClassId         │   │
    │  │    │ AbsenceDate     │   │
    │  │    │ IsJustified     │   │
    │  │    │ Reason          │   │
    │  │    │ CreatedAt       │   │
    │  └────────────────────────┘   │
    │                                │
    │  ┌──────────────────────┐     │
    └──│   Notifications      │     │
       ├──────────────────────┤     │
       │ PK │ Id (int)        │     │
       │ FK │ UserId      ◄───┴─────┘
       │    │ Message         │
       │    │ IsRead          │
       │    │ CreatedAt       │
       └────────────────────────┘

    ┌──────────────────────┐
    │  RefreshTokens       │
    ├──────────────────────┤
    │ PK │ Id (int)        │
    │ FK │ UserId          │
    │    │ Token           │
    │    │ ExpiresAt       │
    │    │ CreatedAt       │
    └────────────────────────┘
```

## Scripts de Création SQL

```sql
-- Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    INDEX IX_Email (Email),
    INDEX IX_Role (Role)
);

-- Classes Table
CREATE TABLE Classes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    TeacherId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (TeacherId) REFERENCES Users(Id) ON DELETE NO ACTION,
    INDEX IX_TeacherId (TeacherId),
    INDEX IX_Name (Name)
);

-- Students Table (Association entre Users et Classes)
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ClassId INT NOT NULL,
    EnrolledAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE,
    UNIQUE (UserId, ClassId),
    INDEX IX_UserId (UserId),
    INDEX IX_ClassId (ClassId)
);

-- Absences Table
CREATE TABLE Absences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassId INT NOT NULL,
    AbsenceDate DATE NOT NULL,
    IsJustified BIT NOT NULL DEFAULT 0,
    Reason NVARCHAR(500),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
    FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE NO ACTION,
    INDEX IX_StudentId (StudentId),
    INDEX IX_ClassId (ClassId),
    INDEX IX_AbsenceDate (AbsenceDate),
    INDEX IX_IsJustified (IsJustified)
);

-- Notifications Table
CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Message NVARCHAR(1000) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX IX_UserId (UserId),
    INDEX IX_IsRead (IsRead),
    INDEX IX_CreatedAt (CreatedAt)
);

-- RefreshTokens Table
CREATE TABLE RefreshTokens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Token NVARCHAR(MAX) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX IX_UserId (UserId),
    INDEX IX_ExpiresAt (ExpiresAt)
);

-- Seed Data
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Role, IsActive)
VALUES 
    ('Admin', 'User', 'admin@test.com', '$2a$11$...', 'Admin', 1),
    ('Ahmed', 'Ben Ali', 'prof1@test.com', '$2a$11$...', 'Teacher', 1),
    ('Fatma', 'Trabelsi', 'prof2@test.com', '$2a$11$...', 'Teacher', 1),
    ('Mohamed', 'Ali', 'etudiant1@test.com', '$2a$11$...', 'Student', 1);

INSERT INTO Classes (Name, Description, TeacherId)
VALUES 
    ('Informatique 1A', 'Classe de première année', 2),
    ('Mathématiques 2B', 'Classe de mathématiques avancées', 2),
    ('Physique 3C', 'Classe de physique', 3);
```
