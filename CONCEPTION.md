# 📐 Conception Complète - Système de Gestion des Absences

## 1. ARCHITECTURE GÉNÉRALE

```
┌─────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                         │
│  (Web Browser / Mobile App / Desktop Client / Swagger UI)   │
└──────────────────────┬──────────────────────────────────────┘
                       │ HTTPS/REST
                       ▼
┌─────────────────────────────────────────────────────────────┐
│                    API GATEWAY LAYER                        │
│            (Authentication & Rate Limiting)                 │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                   CONTROLLER LAYER                          │
│  ┌─────────────┬─────────────┬──────────────┬────────────┐ │
│  │AuthController│UserController│ClassController│Absence │ │
│  │             │             │              │Controller│ │
│  └─────────────┴─────────────┴──────────────┴────────────┘ │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────��───────────────▼──────────────────────────────────────┐
│                    SERVICE LAYER                            │
│  ┌─────────────┬─────────────┬──────────────┬────────────┐ │
│  │AuthService  │UserService  │ClassService  │Absence    │ │
│  │             │             │              │Service    │ │
│  └─────────────┴─────────────┴──────────────┴────────────┘ │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  NotificationService │ ReportService │ ValidationSvc│  │
│  └──────────────────────────────────────────────────────┘  │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                 REPOSITORY LAYER                            │
│  ┌─────────────┬─────────────┬──────────────┬────────────┐ │
│  │UserRepository│ClassRepository│AbsenceRepository│Notif  │ │
│  │             │             │              │Repository│ │
│  └─────────────┴─────────────┴──────────────┴────────────┘ │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                    DATA LAYER                               │
│         (Entity Framework Core - DbContext)                 │
└──────────────────────┬──────────────────────────────────────┘
                       │
        ┌──────────────┴──────────────┐
        ▼                             ▼
   ┌─────────────┐          ┌──────────────────┐
   │ InMemory DB │          │  SQL Server DB   │
   │(Dev/Test)   │          │ (Production)     │
   └─────────────┘          └──────────────────┘
```

---

## 2. DIAGRAMME DE CLASSES (UML)

```
┌─────────────────────────────────────────────────────────────────┐
│                        User                                     │
├─────────────────────────────────────────────────────────────────┤
│ - Id: int (PK)                                                  │
│ - FirstName: string                                             │
│ - LastName: string                                              │
│ - Email: string (Unique)                                        │
│ - PasswordHash: string                                          │
│ - Role: string (Admin | Teacher | Student)                      │
│ - IsActive: boolean                                             │
│ - CreatedAt: DateTime                                           │
│ - UpdatedAt: DateTime                                           │
├─────────────────────────────────────────────────────────────────┤
│ + Login(): JWT                                                  │
│ + RefreshToken(): JWT                                           │
│ + ChangePassword(): void                                        │
└─────────────────────────────────────────────────────────────────┘
           ▲                    ▲                    ▲
           │                    │                    │
      (1..*) │              (1..*) │              (1..*) │
           │                    │                    │
      ┌─────┴──────┐       ┌─────┴──────┐      ┌─────┴──────┐
      │   Teacher  │       │   Student  │      │   Admin    │
      └────────────┘       └────────────┘      └────────────┘
           │                    │
        (1) │              (*)   │
           │                    │
      ┌────┴─────────────────────┴────┐
      │         Class                 │
      ├─────────────────────────────────┤
      │ - Id: int (PK)                 │
      │ - Name: string                 │
      │ - Description: string          │
      │ - TeacherId: int (FK)          │
      │ - CreatedAt: DateTime          │
      └─────────────────────────────────┘
                  │
                  │ (1)
              (*) │
                  │
      ┌───────────┴─────────────┐
      │     Absence             │
      ├─────────────────────────┤
      │ - Id: int (PK)          │
      │ - StudentId: int (FK)   │
      │ - ClassId: int (FK)     │
      │ - AbsenceDate: DateTime │
      │ - IsJustified: boolean  │
      │ - Reason: string        │
      │ - CreatedAt: DateTime   │
      └─────────────────────────┘

      ┌─────────────────────────────────┐
      │     Notification                │
      ├─────────────────────────────────┤
      │ - Id: int (PK)                  │
      │ - UserId: int (FK)              │
      │ - Message: string               │
      │ - IsRead: boolean               │
      │ - CreatedAt: DateTime           │
      └─────────────────────────────────┘
```

---

## 3. DIAGRAMME DE FLUX (Use Cases)

```
                    ┌──────────────┐
                    │ Administrateur│
                    └────────┬──────┘
                             │
           ┌─────────────────┼─────────────────┐
           │                 │                 │
      ┌────▼──────┐  ┌──────▼───┐  ┌────────▼──┐
      │ Gérer Les │  │ Gérer Les │  │ Consulter │
      │ Utilisateurs│ Classes   │  │ Rapports  │
      └────┬──────┘  └──────┬───┘  └────────┬──┘
           │                │               │
      ┌────▼──────┐  ┌──────▼───────┐  ┌───▼─────┐
      │ CRUD      │  │ Assigner     │  │ Générer │
      │ (Create,  │  │ Professeur   │  │ Rapports│
      │ Read,     │  │ Inscrire     │  │ (CSV,  │
      │ Update,   │  │ Étudiants    │  │ PDF)   │
      │ Delete)   │  └──────────────┘  └────────┘
      └───────────┘

                    ┌──────────────┐
                    │  Professeur  │
                    └────────┬──────┘
                             │
           ┌─────────────────┼─────────────────┐
           │                 │                 │
      ┌────▼──────────┐  ┌───▼────────┐  ┌────▼──┐
      │ Marquer Les   │  │ Consulter  │  │Valider│
      │ Absences      │  │ Absences de│  │Absences│
      │ Individuelles │  │ Sa Classe  │  │Groupe │
      └────┬──────────┘  └────────────┘  └───────┘
           │
      ┌────▼──────────┐
      │ Marquer En    │
      │ Masse         │
      │ (Plusieurs    │
      │ Étudiants)    │
      └───────────────┘

                    ┌──────────────┐
                    │   Étudiant   │
                    └────────┬──────┘
                             │
           ┌─────────────────┼──────────────────┐
           │                 │                  │
      ┌────▼──────────┐  ┌───▼──────────┐  ┌──▼────────┐
      │ Consulter Ses │  │ Recevoir Les │  │Consulter  │
      │ Absences      │  │ Notifications│  │ Ses Rapports│
      └───────────────┘  └──────────────┘  └───────────┘
```

---

## 4. FLUX D'AUTHENTIFICATION

```
┌─────────┐                                    ┌────────────────┐
│ Client  │                                    │ API Server     │
└────┬────┘                                    └────────┬───────┘
     │                                                  │
     │ 1. POST /api/auth/login                          │
     │ { email: "user@example.com",                    │
     │   password: "password" }                        │
     ├─────────────────────────────────────────────────>│
     │                                                  │
     │                          2. Valider credentials  │
     │                             Hacher password      │
     │                             Vérifier dans DB     │
     │                                                  │
     │ 3. Générer JWT Token                            │
     │    + Refresh Token                              │
     │<─────────────────────────────────────────────────┤
     │ { access_token: "eyJhbGc...",                   │
     │   refresh_token: "eyJhbGc...",                  │
     │   expires_in: 3600 }                            │
     │                                                  │
     │ 4. Stocker le JWT localement                    │
     │                                                  │
     │ 5. GET /api/users/profile                        │
     │    Header: Authorization: Bearer {access_token} │
     ├─────────────────────────────────────────────────>│
     │                                                  │
     │    6. Valider le JWT                            │
     │       Vérifier la signature                      │
     │       Vérifier l'expiration                      │
     │                                                  │
     │ 7. Retourner les données utilisateur             │
     │<─────────────────────────────────────────────────┤
     │
     │ [JWT EXPIRÉ]
     │
     │ 8. POST /api/auth/refresh-token                  │
     │    { refresh_token: "eyJhbGc..." }              │
     ├─────────────────────────────────────────────────>│
     │                                                  │
     │    9. Valider le refresh token                   │
     │       Générer un nouveau JWT                     │
     │                                                  │
     │ 10. Retourner le nouveau JWT                     │
     │<─────────────────────────────────────────────────┤
```

---

## 5. FLUX DE GESTION DES ABSENCES

```
┌──────────────┐
│ Professeur   │
│ Se connecte  │
└──────┬───────┘
       │
       ▼
┌──────────────────────────┐
│ Consultation des classes │
│ GET /api/classes         │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Sélectionner une classe  │
│ avec ses étudiants       │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Marquer absences         │
│ (Individuelle ou En Masse)│
│ POST /api/absences       │
│ POST /api/absences/bulk  │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Créer Notification       │
│ Envoyer à l'Étudiant     │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Étudiant Reçoit          │
│ Notification d'Absence   │
│ GET /api/notifications   │
└──────┬──────────────���────┘
       │
       ▼
┌──────────────────────────┐
│ Étudiant Consulte        │
│ Ses Absences             │
│ GET /api/absences/my     │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Admin Consulte Rapports  │
│ GET /api/reports/summary │
│ GET /api/reports/export  │
└──────────────────────────┘
```

---

## 6. MODÈLE DE BASE DE DONNÉES

```sql
-- Table Users
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- Table Classes
CREATE TABLE Classes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    TeacherId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (TeacherId) REFERENCES Users(Id)
);

-- Table Students (Pivot entre Users et Classes)
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ClassId INT NOT NULL,
    EnrolledAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ClassId) REFERENCES Classes(Id),
    UNIQUE (UserId, ClassId)
);

-- Table Absences
CREATE TABLE Absences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassId INT NOT NULL,
    AbsenceDate DATE NOT NULL,
    IsJustified BIT NOT NULL DEFAULT 0,
    Reason NVARCHAR(500),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (ClassId) REFERENCES Classes(Id)
);

-- Table Notifications
CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Message NVARCHAR(1000) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Table RefreshTokens
CREATE TABLE RefreshTokens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Token NVARCHAR(MAX) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

---

## 7. API ENDPOINTS

### Authentication
```
POST   /api/auth/login              - Login utilisateur
POST   /api/auth/register           - Créer un compte
POST   /api/auth/refresh-token      - Rafraîchir JWT
POST   /api/auth/logout             - Logout
```

### Users (Admin only)
```
GET    /api/users                   - Lister tous les utilisateurs
GET    /api/users/{id}              - Détails d'un utilisateur
POST   /api/users                   - Créer un utilisateur
PUT    /api/users/{id}              - Modifier un utilisateur
DELETE /api/users/{id}              - Supprimer un utilisateur
PUT    /api/users/{id}/role         - Changer le rôle
PUT    /api/users/{id}/password     - Changer le mot de passe
```

### Classes (Admin)
```
GET    /api/classes                 - Lister les classes
GET    /api/classes/{id}            - Détails d'une classe
GET    /api/classes/{id}/students   - Étudiants de la classe
POST   /api/classes                 - Créer une classe
PUT    /api/classes/{id}            - Modifier une classe
DELETE /api/classes/{id}            - Supprimer une classe
POST   /api/classes/{id}/enroll     - Inscrire un étudiant
DELETE /api/classes/{classId}/students/{studentId} - Radier
```

### Absences (Professeur/Admin)
```
POST   /api/absences                - Marquer une absence
POST   /api/absences/bulk           - Marquer en masse
GET    /api/absences                - Toutes les absences (Admin)
GET    /api/absences/my             - Mes absences (Étudiant)
GET    /api/absences/class/{id}     - Absences par classe
GET    /api/absences/student/{id}   - Absences d'un étudiant
GET    /api/absences/{id}           - Détails d'une absence
PUT    /api/absences/{id}           - Modifier une absence
PUT    /api/absences/{id}/justify   - Justifier une absence
DELETE /api/absences/{id}           - Supprimer une absence
```

### Notifications
```
GET    /api/notifications           - Mes notifications
GET    /api/notifications/unread    - Non lues
PUT    /api/notifications/{id}/read - Marquer comme lu
DELETE /api/notifications/{id}      - Supprimer
```

### Reports (Admin)
```
GET    /api/reports/summary         - Résumé des absences
GET    /api/reports/export-csv      - Export CSV
GET    /api/reports/export-pdf      - Export PDF
GET    /api/reports/by-class/{id}   - Rapport par classe
GET    /api/reports/by-student/{id} - Rapport par étudiant
```

---

## 8. SÉCURITÉ

```
┌───────���──────────────────────────────────────────────────┐
│             COUCHES DE SÉCURITÉ                          │
├──────────────────────────────────────────────────────────┤
│ 1. HTTPS/TLS                                             │
│    └─ Chiffrement du transport (SSL/TLS)               │
│                                                          │
│ 2. AUTHENTIFICATION JWT                                  │
│    └─ Tokens signés cryptographiquement                 │
│    └─ Expiration et refresh tokens                      │
│    └─ Claims (UserId, Role, etc.)                       │
│                                                          │
│ 3. HACHAGE DES MOTS DE PASSE                             │
│    └─ BCrypt avec salt                                  │
│    └─ Jamais stocké en clair                            │
│                                                          │
│ 4. AUTORISATION (RBAC)                                   │
│    └─ Vérification des rôles (Admin, Teacher, Student) │
│    └─ Contrôle d'accès par endpoint                     │
│                                                          │
│ 5. VALIDATION DES DONNÉES                                │
│    └─ FluentValidation                                  │
│    └─ DTO Validation                                    │
│    └─ Prévention SQL Injection (EF Core paramétrisé)   │
│                                                          │
│ 6. CORS (Cross-Origin Resource Sharing)                  │
│    └─ Configuration des origines autorisées            │
│                                                          │
│ 7. RATE LIMITING                                         │
│    └─ Protection contre les brute force                │
│    └─ Limite de requêtes par IP                        │
│                                                          │
│ 8. LOGGING & AUDIT                                       │
│    └─ Enregistrement des actions sensibles             │
│    └─ Suivi des modifications                          │
└──────────────────────────────────────────────────────────┘
```

---

## 9. PATTERNS DE CONCEPTION

### Repository Pattern
```csharp
public interface IAbsenceRepository
{
    Task<Absence> GetByIdAsync(int id);
    Task<IEnumerable<Absence>> GetByStudentAsync(int studentId);
    Task<IEnumerable<Absence>> GetByClassAsync(int classId);
    Task AddAsync(Absence absence);
    Task UpdateAsync(Absence absence);
    Task DeleteAsync(int id);
}

public class AbsenceRepository : IAbsenceRepository
{
    private readonly DbContext _context;
    
    public async Task<Absence> GetByIdAsync(int id)
    {
        return await _context.Absences.FindAsync(id);
    }
    // Implémentation des autres méthodes...
}
```

### Dependency Injection
```csharp
// Dans Program.cs
builder.Services.AddScoped<IAbsenceRepository, AbsenceRepository>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
```

### DTO (Data Transfer Object)
```csharp
public class CreateAbsenceDto
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime AbsenceDate { get; set; }
    public string? Reason { get; set; }
}

public class AbsenceResponseDto
{
    public int Id { get; set; }
    public string StudentName { get; set; }
    public string ClassName { get; set; }
    public DateTime AbsenceDate { get; set; }
    public bool IsJustified { get; set; }
}
```

---

## 10. TECHNOLOGIES STACK

```
┌─────────────────────────────────────────────────────────┐
│                   FRONTEND                              │
├─────────────────────────────────────────────────────────┤
│ • React / Angular / Vue.js (Web)                        │
│ • Flutter / React Native (Mobile)                       │
│ • Swagger UI (API Documentation)                        │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                   BACKEND                               │
├─────────────────────────────────────────────────────────┤
│ Framework:     ASP.NET Core 8.0                         │
│ Language:      C#                                       │
│ ORM:           Entity Framework Core 8.0                │
│ Auth:          JWT Bearer Tokens                        │
│ Validation:    FluentValidation                         │
│ API Docs:      Swagger/OpenAPI                          │
│ Export:        CsvHelper, iTextSharp (PDF)              │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                   DATABASE                              │
├─────────────────────────────────────────────────────────┤
│ Development:   InMemory Database                        │
│ Production:    SQL Server 2019+                         │
│ ORM:           Entity Framework Core                    │
│ Migrations:    Code-First Approach                      │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                   DevOps & Tools                         │
├─────────────────────────────────────────────────────────┤
│ Version Control:  Git / GitHub                          │
│ IDE:              Visual Studio Code                    │
│ Package Manager:  NuGet                                 │
│ CI/CD:            GitHub Actions                        │
│ Deployment:       Docker / Azure / AWS                  │
│ Testing:          xUnit / Moq                           │
└─────────────────────────────────────────────────────────┘
```

---

## 11. CALENDRIER DE DÉVELOPPEMENT

```
┌──────────────────────────────────────────────────────────┐
│ SPRINT 1 (Semaine 1-2): Authentification & Utilisateurs │
├──────────────────────────────────────────────────────────┤
│ ✓ Setup du projet ASP.NET Core                         │
│ ✓ Configuration JWT                                    │
│ ✓ Entity Framework & DbContext                         │
│ ✓ Model User & RefreshToken                            │
│ ✓ Auth Controller (Login, Register, Refresh)           │
│ ✓ User Repository & Service                            │
│ ✓ Unit Tests pour Auth                                 │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│ SPRINT 2 (Semaine 3-4): Gestion des Classes             │
├──────────────────────────────────────────────────────────┤
│ ✓ Model Class & Student                                │
│ ✓ Class Controller                                     │
│ ✓ Class Repository & Service                           │
│ ✓ Enrollment logic                                     │
│ ✓ Integration tests                                    │
│ ✓ Swagger documentation                                │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│ SPRINT 3 (Semaine 5-6): Gestion des Absences            │
├──────────────────────────────────────────────────────────┤
│ ✓ Model Absence                                        │
│ ✓ Absence Controller                                   │
│ ✓ Mark Individual & Bulk Absences                      │
│ ✓ Absence Repository & Service                         │
│ ✓ FluentValidation                                     │
│ ✓ Tests & Integration                                  │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│ SPRINT 4 (Semaine 7-8): Notifications & Rapports        │
├──────────────────────────────────────────────────────────┤
│ ✓ Model Notification                                   │
│ ✓ Notification Service & Controller                    │
│ ✓ Report Service                                       │
│ ✓ CSV Export                                           │
│ ✓ PDF Export                                           │
│ ✓ Report Endpoints                                     │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│ SPRINT 5 (Semaine 9-10): Tests, Docs & Déploiement      │
├──────────────────────────────────────────────────────────┤
│ ✓ Tests complets (Unit, Integration, E2E)             │
│ ✓ Documentation Swagger complète                       │
│ ✓ Performance testing                                  │
│ ✓ Security audit                                       │
│ ✓ Docker configuration                                 │
│ ✓ Déploiement Azure/AWS                                │
└──────────────────────────────────────────────────────────┘
```

---

## 12. DÉPLOIEMENT

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
CMD ["dotnet", "GestionAbsences.dll"]
```

```bash
# Docker Compose
docker-compose up -d
```

---

## 13. FONCTIONNALITÉS FUTURES

```
┌──────────────────────────────────────────────────────────┐
│ Phase 2 (Améliorations)                                 │
├──────────────────────────────────────────────────────────┤
│ □ Interface Web frontend (React/Angular)               │
│ □ Application Mobile (Flutter)                         │
│ □ Géolocalisation pour tracking de présence            │
│ □ Notifications par SMS/Email                          │
│ □ Dashboard analytique avec graphiques                 │
│ □ Intégration Single Sign-On (SSO)                     │
│ □ Support multi-langage                                │
│ □ Cache Redis                                          │
│ □ Message Queue (RabbitMQ)                             │
│ □ Microservices architecture                           │
└──────────────────────────────────────────────────────────┘
```

---

## Fin de la Conception Complète ✅
