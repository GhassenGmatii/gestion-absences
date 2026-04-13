using GestionAbsences.DTOs.Absence;
using GestionAbsences.Repositories;
using GestionAbsences.Services.Notification;

namespace GestionAbsences.Services.Absence
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly INotificationService _notificationService;

        public AbsenceService(IAbsenceRepository absenceRepository, INotificationService notificationService)
        {
            _absenceRepository = absenceRepository;
            _notificationService = notificationService;
        }

        public async Task<AbsenceResponseDto> MarkAbsenceAsync(MarkAbsenceDto dto, int markedById)
        {
            var absence = new Models.Absence
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                MarkedById = markedById,
                Date = dto.Date,
                IsJustified = false,
                CreatedAt = DateTime.UtcNow
            };

            await _absenceRepository.AddAsync(absence);

            // Reload with includes to build the response DTO
            var absences = await _absenceRepository.GetByStudentIdAsync(dto.StudentId);
            var created = absences.FirstOrDefault(a => a.Id == absence.Id);

            await _notificationService.CreateNotificationAsync(
                dto.StudentId,
                "Nouvelle absence",
                $"Une absence a été enregistrée pour le {dto.Date:dd/MM/yyyy}.");

            return created != null ? MapToDto(created) : MapToBasicDto(absence);
        }

        public async Task<IEnumerable<AbsenceResponseDto>> BulkMarkAbsenceAsync(BulkMarkAbsenceDto dto, int markedById)
        {
            var results = new List<AbsenceResponseDto>();

            foreach (var studentId in dto.StudentIds)
            {
                var markDto = new MarkAbsenceDto
                {
                    StudentId = studentId,
                    ClassId = dto.ClassId,
                    Date = dto.Date
                };

                var result = await MarkAbsenceAsync(markDto, markedById);
                results.Add(result);
            }

            return results;
        }

        public async Task<IEnumerable<AbsenceResponseDto>> GetAllAbsencesAsync()
        {
            var absences = await _absenceRepository.GetByStudentIdAsync(0);
            // GetByStudentId filters by student; we need all, so use GetAllAsync and reload
            var allAbsences = await _absenceRepository.GetAllAsync();

            // GetAllAsync from base Repository doesn't include navigation properties,
            // so we gather all by fetching per-class or use a workaround.
            // For a proper implementation, we fetch all and try to map.
            // Since base GetAllAsync doesn't include nav props, we'll collect from known methods.
            var result = new List<AbsenceResponseDto>();
            foreach (var absence in allAbsences)
            {
                var studentAbsences = await _absenceRepository.GetByStudentIdAsync(absence.StudentId);
                var loaded = studentAbsences.FirstOrDefault(a => a.Id == absence.Id);
                if (loaded != null)
                    result.Add(MapToDto(loaded));
            }

            return result.DistinctBy(a => a.Id);
        }

        public async Task<IEnumerable<AbsenceResponseDto>> GetAbsencesByStudentAsync(int studentId)
        {
            var absences = await _absenceRepository.GetByStudentIdAsync(studentId);
            return absences.Select(MapToDto);
        }

        public async Task<IEnumerable<AbsenceResponseDto>> GetAbsencesByClassAsync(int classId)
        {
            var absences = await _absenceRepository.GetByClassIdAsync(classId);
            return absences.Select(MapToDto);
        }

        public async Task<bool> DeleteAbsenceAsync(int id)
        {
            var absence = await _absenceRepository.GetByIdAsync(id);
            if (absence == null)
                return false;

            await _absenceRepository.DeleteAsync(absence);
            return true;
        }

        public async Task<AbsenceResponseDto?> JustifyAbsenceAsync(int id, string justification)
        {
            var absence = await _absenceRepository.GetByIdAsync(id);
            if (absence == null)
                return null;

            absence.IsJustified = true;
            absence.Justification = justification;
            await _absenceRepository.UpdateAsync(absence);

            // Reload with navigation properties
            var absences = await _absenceRepository.GetByStudentIdAsync(absence.StudentId);
            var updated = absences.FirstOrDefault(a => a.Id == id);
            return updated != null ? MapToDto(updated) : MapToBasicDto(absence);
        }

        public async Task<int> GetAbsenceCountAsync(int studentId)
        {
            return await _absenceRepository.GetAbsenceCountByStudentAsync(studentId);
        }

        private static AbsenceResponseDto MapToDto(Models.Absence absence)
        {
            return new AbsenceResponseDto
            {
                Id = absence.Id,
                StudentName = absence.Student != null
                    ? $"{absence.Student.FirstName} {absence.Student.LastName}"
                    : string.Empty,
                ClassName = absence.ClassEntity?.Name ?? string.Empty,
                MarkedByName = absence.MarkedBy != null
                    ? $"{absence.MarkedBy.FirstName} {absence.MarkedBy.LastName}"
                    : string.Empty,
                Date = absence.Date,
                IsJustified = absence.IsJustified,
                Justification = absence.Justification,
                CreatedAt = absence.CreatedAt
            };
        }

        private static AbsenceResponseDto MapToBasicDto(Models.Absence absence)
        {
            return new AbsenceResponseDto
            {
                Id = absence.Id,
                StudentName = string.Empty,
                ClassName = string.Empty,
                MarkedByName = string.Empty,
                Date = absence.Date,
                IsJustified = absence.IsJustified,
                Justification = absence.Justification,
                CreatedAt = absence.CreatedAt
            };
        }
    }
}
