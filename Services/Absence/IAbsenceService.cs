using GestionAbsences.DTOs.Absence;

namespace GestionAbsences.Services.Absence
{
    public interface IAbsenceService
    {
        Task<AbsenceResponseDto> MarkAbsenceAsync(MarkAbsenceDto dto, int markedById);
        Task<IEnumerable<AbsenceResponseDto>> BulkMarkAbsenceAsync(BulkMarkAbsenceDto dto, int markedById);
        Task<IEnumerable<AbsenceResponseDto>> GetAllAbsencesAsync();
        Task<IEnumerable<AbsenceResponseDto>> GetAbsencesByStudentAsync(int studentId);
        Task<IEnumerable<AbsenceResponseDto>> GetAbsencesByClassAsync(int classId);
        Task<bool> DeleteAbsenceAsync(int id);
        Task<AbsenceResponseDto?> JustifyAbsenceAsync(int id, string justification);
        Task<int> GetAbsenceCountAsync(int studentId);
    }
}
