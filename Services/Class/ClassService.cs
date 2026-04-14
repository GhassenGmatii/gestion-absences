using GestionAbsences.DTOs.Class;
using GestionAbsences.DTOs.User;
using GestionAbsences.Models;
using GestionAbsences.Repositories;

namespace GestionAbsences.Services.Class
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;

        public ClassService(IClassRepository classRepository, IUserRepository userRepository)
        {
            _classRepository = classRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ClassResponseDto>> GetAllClassesAsync()
        {
            var classes = await _classRepository.GetAllAsync();
            var result = new List<ClassResponseDto>();

            foreach (var cls in classes)
            {
                var withStudents = await _classRepository.GetWithStudentsAsync(cls.Id);
                if (withStudents != null)
                    result.Add(MapToDto(withStudents));
            }

            return result;
        }

        public async Task<ClassResponseDto?> GetClassByIdAsync(int id)
        {
            var cls = await _classRepository.GetWithStudentsAsync(id);
            return cls == null ? null : MapToDto(cls);
        }

        public async Task<ClassWithStudentsDto?> GetClassWithStudentsAsync(int id)
        {
            var cls = await _classRepository.GetWithStudentsAsync(id);
            if (cls == null)
                return null;

            return new ClassWithStudentsDto
            {
                Id = cls.Id,
                Name = cls.Name,
                Description = cls.Description,
                ProfessorName = cls.Professor != null
                    ? $"{cls.Professor.FirstName} {cls.Professor.LastName}"
                    : null,
                StudentCount = cls.Enrollments.Count,
                CreatedAt = cls.CreatedAt,
                Students = cls.Enrollments.Select(e => new UserResponseDto
                {
                    Id = e.Student.Id,
                    FirstName = e.Student.FirstName,
                    LastName = e.Student.LastName,
                    Email = e.Student.Email,
                    Role = e.Student.Role.ToString(),
                    IsActive = e.Student.IsActive,
                    CreatedAt = e.Student.CreatedAt
                }).ToList()
            };
        }

        public async Task<ClassResponseDto> CreateClassAsync(CreateClassDto dto)
        {
            var cls = new Models.Class
            {
                Name = dto.Name,
                Description = dto.Description,
                ProfessorId = dto.ProfessorId,
                CreatedAt = DateTime.UtcNow
            };

            await _classRepository.AddAsync(cls);

            // Reload with navigation properties
            var created = await _classRepository.GetWithStudentsAsync(cls.Id);
            return MapToDto(created ?? cls);
        }

        public async Task<ClassResponseDto?> UpdateClassAsync(int id, CreateClassDto dto)
        {
            var cls = await _classRepository.GetByIdAsync(id);
            if (cls == null)
                return null;

            cls.Name = dto.Name;
            cls.Description = dto.Description;
            cls.ProfessorId = dto.ProfessorId;

            await _classRepository.UpdateAsync(cls);

            var updated = await _classRepository.GetWithStudentsAsync(id);
            return MapToDto(updated ?? cls);
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var cls = await _classRepository.GetByIdAsync(id);
            if (cls == null)
                return false;

            await _classRepository.DeleteAsync(cls);
            return true;
        }

        public async Task<bool> EnrollStudentAsync(int classId, int studentId)
        {
            var cls = await _classRepository.GetByIdAsync(classId);
            if (cls == null)
                return false;

            var student = await _userRepository.GetByIdAsync(studentId);
            if (student == null)
                return false;

            // Check if already enrolled
            var enrollments = await _classRepository.GetEnrollmentsAsync(classId);
            if (enrollments.Any(e => e.StudentId == studentId))
                return false;

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                ClassId = classId,
                EnrolledAt = DateTime.UtcNow
            };

            cls.Enrollments.Add(enrollment);
            await _classRepository.UpdateAsync(cls);
            return true;
        }

        private static ClassResponseDto MapToDto(Models.Class cls)
        {
            return new ClassResponseDto
            {
                Id = cls.Id,
                Name = cls.Name,
                Description = cls.Description,
                ProfessorName = cls.Professor != null
                    ? $"{cls.Professor.FirstName} {cls.Professor.LastName}"
                    : null,
                StudentCount = cls.Enrollments.Count,
                CreatedAt = cls.CreatedAt
            };
        }
    }
}
