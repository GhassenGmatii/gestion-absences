# 📋 Spécification API Complète

## 1. Authentication Endpoints

### POST /api/auth/login
**Description:** Authentifier un utilisateur

**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (200):**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "user": {
      "id": 1,
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "Admin"
    }
  },
  "message": "Login successful"
}
```

**Response (401):**
```json
{
  "success": false,
  "error": "Invalid credentials"
}
```

---

### POST /api/auth/register
**Description:** Créer un nouveau compte utilisateur

**Request:**
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "password": "securePassword123!",
  "role": "Student"
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "id": 2,
    "email": "jane@example.com",
    "firstName": "Jane",
    "lastName": "Smith",
    "role": "Student"
  },
  "message": "Registration successful"
}
```

---

### POST /api/auth/refresh-token
**Description:** Rafraîchir le JWT Token

**Request:**
```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (200):**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600
  },
  "message": "Token refreshed successfully"
}
```

---

## 2. Users Endpoints (Admin Only)

### GET /api/users
**Description:** Lister tous les utilisateurs

**Query Parameters:**
- `page` (int): Numéro de page (défaut: 1)
- `pageSize` (int): Éléments par page (défaut: 10)
- `role` (string): Filtrer par rôle (Admin, Teacher, Student)

**Response (200):**
```json
{
  "success": true,
  "data": {
    "users": [
      {
        "id": 1,
        "firstName": "John",
        "lastName": "Doe",
        "email": "john@example.com",
        "role": "Admin",
        "isActive": true,
        "createdAt": "2024-01-01T10:00:00Z"
      }
    ],
    "totalCount": 50,
    "pageNumber": 1,
    "pageSize": 10
  }
}
```

---

### POST /api/users
**Description:** Créer un nouvel utilisateur (Admin only)

**Request:**
```json
{
  "firstName": "Ahmed",
  "lastName": "Ben Ali",
  "email": "ahmed@example.com",
  "password": "TempPassword123!",
  "role": "Teacher"
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "id": 3,
    "firstName": "Ahmed",
    "lastName": "Ben Ali",
    "email": "ahmed@example.com",
    "role": "Teacher"
  }
}
```

---

### PUT /api/users/{id}
**Description:** Modifier un utilisateur

**Request:**
```json
{
  "firstName": "Ahmed",
  "lastName": "Ben Ali",
  "email": "ahmed.benaali@example.com"
}
```

**Response (200):**
```json
{
  "success": true,
  "data": {
    "id": 3,
    "firstName": "Ahmed",
    "lastName": "Ben Ali",
    "email": "ahmed.benaali@example.com",
    "role": "Teacher",
    "updatedAt": "2024-04-20T15:30:00Z"
  }
}
```

---

### DELETE /api/users/{id}
**Description:** Supprimer un utilisateur

**Response (200):**
```json
{
  "success": true,
  "message": "User deleted successfully"
}
```

---

## 3. Classes Endpoints

### GET /api/classes
**Description:** Lister les classes

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Informatique 1A",
      "description": "Classe de première année",
      "teacher": {
        "id": 2,
        "firstName": "Ahmed",
        "lastName": "Ben Ali"
      },
      "studentCount": 30,
      "createdAt": "2024-01-01T09:00:00Z"
    }
  ]
}
```

---

### POST /api/classes
**Description:** Créer une classe (Admin only)

**Request:**
```json
{
  "name": "Mathématiques 2B",
  "description": "Classe de mathématiques avancées",
  "teacherId": 2
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "id": 2,
    "name": "Mathématiques 2B",
    "description": "Classe de mathématiques avancées",
    "teacherId": 2
  }
}
```

---

### GET /api/classes/{id}/students
**Description:** Lister les étudiants d'une classe

**Response (200):**
```json
{
  "success": true,
  "data": {
    "classId": 1,
    "className": "Informatique 1A",
    "students": [
      {
        "id": 10,
        "firstName": "Mohamed",
        "lastName": "Ali",
        "email": "mohamedali@example.com"
      }
    ],
    "totalStudents": 30
  }
}
```

---

### POST /api/classes/{id}/enroll
**Description:** Inscrire un étudiant dans une classe

**Request:**
```json
{
  "studentId": 10
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Student enrolled successfully"
}
```

---

## 4. Absences Endpoints

### POST /api/absences
**Description:** Marquer une absence (Professeur)

**Request:**
```json
{
  "studentId": 10,
  "classId": 1,
  "absenceDate": "2024-04-20",
  "reason": "Maladie"
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "studentId": 10,
    "classId": 1,
    "absenceDate": "2024-04-20",
    "isJustified": false,
    "reason": "Maladie",
    "createdAt": "2024-04-20T10:00:00Z"
  }
}
```

---

### POST /api/absences/bulk
**Description:** Marquer les absences en masse

**Request:**
```json
{
  "classId": 1,
  "absenceDate": "2024-04-20",
  "studentIds": [10, 11, 12, 13],
  "reason": "Absence collective"
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "markedCount": 4,
    "absences": [
      { "studentId": 10, "status": "marked" },
      { "studentId": 11, "status": "marked" },
      { "studentId": 12, "status": "marked" },
      { "studentId": 13, "status": "marked" }
    ]
  }
}
```

---

### GET /api/absences/my
**Description:** Consulter ses absences (Étudiant)

**Query Parameters:**
- `startDate` (date): Date de début
- `endDate` (date): Date de fin

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "className": "Informatique 1A",
      "absenceDate": "2024-04-20",
      "isJustified": false,
      "reason": "Maladie",
      "teacherName": "Ahmed Ben Ali"
    }
  ],
  "summary": {
    "totalAbsences": 5,
    "justifiedAbsences": 2,
    "unjustifiedAbsences": 3
  }
}
```

---

### GET /api/absences/class/{id}
**Description:** Consulter les absences d'une classe (Professeur)

**Response (200):**
```json
{
  "success": true,
  "data": {
    "classId": 1,
    "className": "Informatique 1A",
    "absences": [
      {
        "id": 1,
        "studentName": "Mohamed Ali",
        "absenceDate": "2024-04-20",
        "isJustified": false,
        "reason": "Maladie"
      }
    ]
  }
}
```

---

### PUT /api/absences/{id}/justify
**Description:** Justifier une absence (Admin)

**Request:**
```json
{
  "reason": "Raison médicale justifiée",
  "isJustified": true
}
```

**Response (200):**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "isJustified": true,
    "reason": "Raison médicale justifiée",
    "updatedAt": "2024-04-20T15:00:00Z"
  }
}
```

---

## 5. Notifications Endpoints

### GET /api/notifications
**Description:** Récupérer mes notifications

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "message": "Vous avez une absence le 20/04/2024",
      "isRead": false,
      "createdAt": "2024-04-20T10:00:00Z"
    }
  ]
}
```

---

### GET /api/notifications/unread
**Description:** Récupérer les notifications non lues

**Response (200):**
```json
{
  "success": true,
  "data": [
    { "id": 1, "message": "Vous avez une absence le 20/04/2024" }
  ],
  "unreadCount": 1
}
```

---

### PUT /api/notifications/{id}/read
**Description:** Marquer une notification comme lue

**Response (200):**
```json
{
  "success": true,
  "message": "Notification marked as read"
}
```

---

## 6. Reports Endpoints (Admin)

### GET /api/reports/summary
**Description:** Résumé des absences

**Query Parameters:**
- `startDate` (date): Date de début
- `endDate` (date): Date de fin
- `classId` (int): Filtrer par classe (optionnel)

**Response (200):**
```json
{
  "success": true,
  "data": {
    "totalAbsences": 150,
    "justifiedAbsences": 50,
    "unjustifiedAbsences": 100,
    "averagePerStudent": 5,
    "byClass": [
      {
        "className": "Informatique 1A",
        "totalAbsences": 30,
        "percentage": 20
      }
    ]
  }
}
```

---

### GET /api/reports/export-csv
**Description:** Exporter en CSV

**Response:** Fichier CSV téléchargé

---

### GET /api/reports/export-pdf
**Description:** Exporter en PDF

**Response:** Fichier PDF téléchargé

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "error": "Validation failed",
  "details": [
    {
      "field": "email",
      "message": "Invalid email format"
    }
  ]
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "error": "Unauthorized - Invalid or expired token"
}
```

### 403 Forbidden
```json
{
  "success": false,
  "error": "Forbidden - You don't have permission to perform this action"
}
```

### 404 Not Found
```json
{
  "success": false,
  "error": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "error": "Internal server error",
  "message": "An unexpected error occurred"
}
```
