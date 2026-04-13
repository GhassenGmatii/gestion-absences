using GestionAbsences.DTOs.Absence;

namespace GestionAbsences.Services.Report
{
    public interface IReportService
    {
        Task<object> GetSummaryAsync();
        Task<byte[]> ExportCsvAsync();
        Task<byte[]> ExportPdfAsync();
        Task<IEnumerable<AbsenceResponseDto>> GetReportByClassAsync(int classId);
        Task<IEnumerable<AbsenceResponseDto>> GetReportByStudentAsync(int studentId);
    }
}
