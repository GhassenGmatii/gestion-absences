namespace GestionAbsences.DTOs.Class
{
    public class CreateClassDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ProfessorId { get; set; }
    }
}
