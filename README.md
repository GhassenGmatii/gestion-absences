# 📚 Système de Gestion des Absences

Application Web API complète pour la gestion des absences scolaires, développée avec ASP.NET Core 8.0.

## 🎯 Fonctionnalités

### Authentification & Sécurité
- 🔐 JWT Token pour l'authentification
- 🔄 Refresh tokens pour renouvellement
- 👥 3 rôles : **Admin**, **Professeur**, **Étudiant**
- 🔒 Hachage des mots de passe avec BCrypt

### Gestion des Utilisateurs (Admin)
- CRUD complet des utilisateurs
- Assignation de rôles
- Désactivation de comptes

### Gestion des Classes
- Créer/Modifier/Supprimer des classes
- Assigner un professeur à une classe
- Inscrire des étudiants
- Voir la liste des étudiants par classe

### Gestion des Absences (Professeur)
- Marquer les absences par classe
- Marquer en masse (plusieurs étudiants)
- Notifications automatiques aux étudiants

### Consultation des Absences (Étudiant)
- Voir ses propres absences
- Voir la date, le cours, le professeur
- Voir le statut (justifié/non justifié)

### Notifications
- Notifications automatiques d'absence
- Marquer comme lu
- Historique des notifications

### Rapports (Admin)
- Résumé des absences
- Export CSV
- Export PDF
- Rapports par classe ou par étudiant

## 🛠️ Technologies

| Technologie | Usage |
|-------------|-------|
| ASP.NET Core 8.0 | Framework Web API |
| Entity Framework Core | ORM / Base de données |
| JWT Bearer | Authentification |
| BCrypt.Net | Hachage des mots de passe |
| FluentValidation | Validation des données |
| CsvHelper | Export CSV |
| Swagger/OpenAPI | Documentation API |
| SQL Server | Base de données (Production) |
| InMemory Database | Base de données (Développement) |

## 📋 Prérequis

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (pour la production) - optionnel pour le développement
- Un éditeur de code (Visual Studio, VS Code, Rider)

## 🚀 Installation

### 1. Cloner le repository

```bash
git clone https://github.com/GhassenGmatii/gestion-absences.git
cd gestion-absences
```

### 2. Restaurer les packages

```bash
dotnet restore
```

### 3. Configurer la base de données

Pour le **développement**, l'application utilise une base de données **InMemory** par défaut - aucune configuration supplémentaire n'est nécessaire.

Pour la **production**, modifier la connection string dans `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=votre-serveur;Database=GestionAbsences;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 4. Lancer l'application

```bash
dotnet run
```

### 5. Accéder à Swagger

Ouvrir dans le navigateur : http://localhost:5000/swagger

## 🔌 API Endpoints

### Authentication
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/auth/login` | Authentifier un utilisateur |
| POST | `/api/auth/register` | Créer un nouveau compte |
| POST | `/api/auth/refresh-token` | Rafraîchir le JWT token |

### Utilisateurs (Admin)
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/users` | Lister tous les utilisateurs |
| GET | `/api/users/{id}` | Détails d'un utilisateur |
| POST | `/api/users` | Créer un utilisateur |
| PUT | `/api/users/{id}` | Modifier un utilisateur |
| DELETE | `/api/users/{id}` | Supprimer un utilisateur |
| PUT | `/api/users/{id}/role` | Changer le rôle |

### Classes
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/classes` | Lister les classes |
| GET | `/api/classes/{id}` | Détails d'une classe |
| GET | `/api/classes/{id}/students` | Étudiants d'une classe |
| POST | `/api/classes` | Créer une classe (Admin) |
| PUT | `/api/classes/{id}` | Modifier une classe (Admin) |
| DELETE | `/api/classes/{id}` | Supprimer une classe (Admin) |
| POST | `/api/classes/{id}/enroll` | Inscrire un étudiant (Admin) |

### Absences
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/absences` | Marquer une absence (Prof) |
| POST | `/api/absences/bulk` | Marquer en masse (Prof) |
| GET | `/api/absences` | Toutes les absences (Admin) |
| GET | `/api/absences/my` | Mes absences (Étudiant) |
| GET | `/api/absences/class/{id}` | Absences par classe (Prof) |
| GET | `/api/absences/student/{id}` | Absences d'un étudiant (Admin) |
| DELETE | `/api/absences/{id}` | Supprimer (Admin) |
| PUT | `/api/absences/{id}/justify` | Justifier (Admin) |

### Notifications
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/notifications` | Mes notifications |
| GET | `/api/notifications/unread` | Non lues |
| PUT | `/api/notifications/{id}/read` | Marquer comme lu |
| DELETE | `/api/notifications/{id}` | Supprimer |

### Rapports (Admin)
| Méthode | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/reports/summary` | Résumé des absences |
| GET | `/api/reports/export-csv` | Export CSV |
| GET | `/api/reports/export-pdf` | Export PDF |
| GET | `/api/reports/by-class/{id}` | Rapport par classe |
| GET | `/api/reports/by-student/{id}` | Rapport par étudiant |

## 📊 Données de Test

L'application inclut des données de test automatiquement chargées au démarrage :

| Rôle | Email | Mot de passe |
|------|-------|-------------|
| Admin | admin@test.com | Admin123! |
| Professeur | prof1@test.com | Password123! |
| Professeur | prof2@test.com | Password123! |
| Étudiant | etudiant1@test.com - etudiant10@test.com | Password123! |

### Classes de test
- **Informatique 1A** (Prof: Ahmed Ben Ali)
- **Mathématiques 2B** (Prof: Ahmed Ben Ali)
- **Physique 3C** (Prof: Fatma Trabelsi)

## 📁 Structure du Projet

```
GestionAbsences/
├── Models/          # Entités de base de données
├── DTOs/            # Data Transfer Objects
├── Data/            # DbContext et données initiales
├── Repositories/    # Couche d'accès aux données
├── Services/        # Logique métier
├── Controllers/     # Endpoints API
├── Middleware/       # JWT et gestion des erreurs
├── Helpers/         # Utilitaires (JWT, Password, Auth)
├── Validators/      # Validation FluentValidation
├── Program.cs       # Point d'entrée de l'application
└── appsettings.json # Configuration
```

## 🔐 Sécurité

- ✅ Hachage des mots de passe avec BCrypt
- ✅ JWT tokens avec expiration configurable
- ✅ Refresh tokens pour renouvellement sécurisé
- ✅ Autorisation basée sur les rôles (RBAC)
- ✅ Validation des données avec FluentValidation
- ✅ CORS configuré
- ✅ Gestion globale des erreurs
- ✅ Protection des endpoints sensibles

## 🤝 Contribution

1. Fork le projet
2. Créer une branche (`git checkout -b feature/nouvelle-fonctionnalite`)
3. Commit les changements (`git commit -m 'Ajout nouvelle fonctionnalité'`)
4. Push vers la branche (`git push origin feature/nouvelle-fonctionnalite`)
5. Ouvrir une Pull Request

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## 👤 Auteur

**Ghassen Gmatii**

---

⭐ Si ce projet vous a été utile, n'hésitez pas à lui donner une étoile !
