# 🏗️ Architecture Détaillée

## Structure des Dossiers

```
GestionAbsences/
├── Controllers/                 # Endpoints API
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── ClassController.cs
│   ├── AbsenceController.cs
│   ├── NotificationController.cs
│   └── ReportController.cs
│
├── Services/                    # Logique métier
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── ClassService.cs
│   ├── AbsenceService.cs
│   ├── NotificationService.cs
│   └── ReportService.cs
│
├── Repositories/                # Accès aux données
│   ├── IRepository.cs
│   ├── UserRepository.cs
│   ├── ClassRepository.cs
│   ├── AbsenceRepository.cs
│   └── NotificationRepository.cs
│
├── Models/                      # Entités de base de données
│   ├── User.cs
│   ├── Class.cs
│   ├── Student.cs
│   ├── Absence.cs
│   ├── Notification.cs
│   └── RefreshToken.cs
│
├── DTOs/                        # Data Transfer Objects
│   ├── Auth/
│   │   ├── LoginDto.cs
│   │   ├── RegisterDto.cs
│   │   └── TokenDto.cs
│   ├── User/
│   │   ├── UserDto.cs
│   │   └── CreateUserDto.cs
│   ├── Class/
│   │   ├── ClassDto.cs
│   │   └── CreateClassDto.cs
│   └── Absence/
│       ├── AbsenceDto.cs
│       └── CreateAbsenceDto.cs
│
├── Validators/                  # FluentValidation
│   ├── LoginValidator.cs
│   ├── RegisterValidator.cs
│   ├── CreateUserValidator.cs
│   ├── CreateClassValidator.cs
│   └── CreateAbsenceValidator.cs
│
├── Middleware/                  # Custom Middleware
│   ├── JwtMiddleware.cs
│   ├── ExceptionHandlingMiddleware.cs
│   └── RateLimitingMiddleware.cs
│
├── Helpers/                     # Utilitaires
│   ├── JwtHelper.cs
│   ├── PasswordHelper.cs
│   ├── DateHelper.cs
│   └── EnumHelper.cs
│
├── Data/                        # Configuration DB
│   ├── AppDbContext.cs
│   └── DataSeeder.cs
│
├── Program.cs                   # Configuration & Startup
├── appsettings.json             # Configuration
├── appsettings.Development.json
└── GestionAbsences.csproj       # Projet
```

## Dépendances NuGet

```xml
<ItemGroup>
  <!-- ASP.NET Core -->
  <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
  <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
  
  <!-- Entity Framework -->
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
  
  <!-- Validation -->
  <PackageReference Include="FluentValidation" Version="11.8.0" />
  <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.8.0" />
  
  <!-- Hachage des mots de passe -->
  <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
  
  <!-- Export CSV -->
  <PackageReference Include="CsvHelper" Version="30.0.0" />
  
  <!-- Export PDF -->
  <PackageReference Include="iTextSharp.LGPLv2.Core" Version="1.8.4" />
  
  <!-- Swagger -->
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  
  <!-- Logging -->
  <PackageReference Include="Serilog" Version="3.1.1" />
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
  
  <!-- Testing -->
  <PackageReference Include="xunit" Version="2.6.6" />
  <PackageReference Include="Moq" Version="4.20.70" />
</ItemGroup>
```
