namespace GestionAbsences.DTOs.Absence
{
    public class BulkMarkAbsenceDto
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<int> StudentIds { get; set; } = new List<int>();
    }
}
