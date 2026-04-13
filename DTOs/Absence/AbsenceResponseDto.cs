namespace GestionAbsences.DTOs.Absence
{
    public class AbsenceResponseDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string MarkedByName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsJustified { get; set; }
        public string? Justification { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
