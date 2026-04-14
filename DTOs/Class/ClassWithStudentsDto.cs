using GestionAbsences.DTOs.User;

namespace GestionAbsences.DTOs.Class
{
    public class ClassWithStudentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ProfessorName { get; set; }
        public int StudentCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserResponseDto> Students { get; set; } = new List<UserResponseDto>();
    }
}
