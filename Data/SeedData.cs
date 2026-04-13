using GestionAbsences.Models;
using GestionAbsences.Models.Enums;

namespace GestionAbsences.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (context.Users.Any())
                return;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");
            var adminHashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin123!");

            // Seed Admin
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "System",
                Email = "admin@test.com",
                PasswordHash = adminHashedPassword,
                Role = UserRole.Admin
            };

            // Seed Professors
            var prof1 = new User
            {
                FirstName = "Ahmed",
                LastName = "Ben Ali",
                Email = "prof1@test.com",
                PasswordHash = hashedPassword,
                Role = UserRole.Professeur
            };
            var prof2 = new User
            {
                FirstName = "Fatma",
                LastName = "Trabelsi",
                Email = "prof2@test.com",
                PasswordHash = hashedPassword,
                Role = UserRole.Professeur
            };

            // Seed Students
            var students = new List<User>
            {
                new User { FirstName = "Mohamed", LastName = "Salah", Email = "etudiant1@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Amine", LastName = "Khelifi", Email = "etudiant2@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Sara", LastName = "Bouazizi", Email = "etudiant3@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Yassine", LastName = "Hammami", Email = "etudiant4@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Nour", LastName = "Ben Amor", Email = "etudiant5@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Karim", LastName = "Mejri", Email = "etudiant6@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Ines", LastName = "Gharbi", Email = "etudiant7@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Omar", LastName = "Jebali", Email = "etudiant8@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Rania", LastName = "Mansouri", Email = "etudiant9@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant },
                new User { FirstName = "Bilel", LastName = "Chaabane", Email = "etudiant10@test.com", PasswordHash = hashedPassword, Role = UserRole.Etudiant }
            };

            context.Users.Add(admin);
            context.Users.Add(prof1);
            context.Users.Add(prof2);
            context.Users.AddRange(students);
            await context.SaveChangesAsync();

            // Seed Classes
            var class1 = new Class { Name = "Informatique 1A", Description = "Introduction à l'informatique", ProfessorId = prof1.Id };
            var class2 = new Class { Name = "Mathématiques 2B", Description = "Mathématiques avancées", ProfessorId = prof1.Id };
            var class3 = new Class { Name = "Physique 3C", Description = "Physique générale", ProfessorId = prof2.Id };

            context.Classes.AddRange(class1, class2, class3);
            await context.SaveChangesAsync();

            // Seed Enrollments: first 4 students in class1, next 3 in class2, last 3 in class3
            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentId = students[0].Id, ClassId = class1.Id },
                new Enrollment { StudentId = students[1].Id, ClassId = class1.Id },
                new Enrollment { StudentId = students[2].Id, ClassId = class1.Id },
                new Enrollment { StudentId = students[3].Id, ClassId = class1.Id },
                new Enrollment { StudentId = students[4].Id, ClassId = class2.Id },
                new Enrollment { StudentId = students[5].Id, ClassId = class2.Id },
                new Enrollment { StudentId = students[6].Id, ClassId = class2.Id },
                new Enrollment { StudentId = students[7].Id, ClassId = class3.Id },
                new Enrollment { StudentId = students[8].Id, ClassId = class3.Id },
                new Enrollment { StudentId = students[9].Id, ClassId = class3.Id }
            };

            context.Enrollments.AddRange(enrollments);
            await context.SaveChangesAsync();

            // Seed sample Absences
            var absences = new List<Absence>
            {
                new Absence { StudentId = students[0].Id, ClassId = class1.Id, MarkedById = prof1.Id, Date = DateTime.UtcNow.AddDays(-5) },
                new Absence { StudentId = students[1].Id, ClassId = class1.Id, MarkedById = prof1.Id, Date = DateTime.UtcNow.AddDays(-3), IsJustified = true, Justification = "Certificat médical" },
                new Absence { StudentId = students[2].Id, ClassId = class1.Id, MarkedById = prof1.Id, Date = DateTime.UtcNow.AddDays(-2) },
                new Absence { StudentId = students[4].Id, ClassId = class2.Id, MarkedById = prof1.Id, Date = DateTime.UtcNow.AddDays(-4) },
                new Absence { StudentId = students[7].Id, ClassId = class3.Id, MarkedById = prof2.Id, Date = DateTime.UtcNow.AddDays(-1) },
                new Absence { StudentId = students[8].Id, ClassId = class3.Id, MarkedById = prof2.Id, Date = DateTime.UtcNow.AddDays(-1), IsJustified = true, Justification = "Raison familiale" }
            };

            context.Absences.AddRange(absences);
            await context.SaveChangesAsync();
        }
    }
}
