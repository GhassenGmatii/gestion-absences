using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GestionAbsences.DTOs.Absence;
using GestionAbsences.Repositories;

namespace GestionAbsences.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;

        public ReportService(
            IAbsenceRepository absenceRepository,
            IClassRepository classRepository,
            IUserRepository userRepository)
        {
            _absenceRepository = absenceRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
        }

        public async Task<object> GetSummaryAsync()
        {
            var allAbsences = await _absenceRepository.GetAllAsync();
            var absenceList = allAbsences.ToList();

            var allUsers = await _userRepository.GetAllAsync();
            var students = allUsers.Where(u => u.Role == Models.Enums.UserRole.Etudiant);

            var allClasses = await _classRepository.GetAllAsync();

            return new
            {
                TotalAbsences = absenceList.Count,
                JustifiedCount = absenceList.Count(a => a.IsJustified),
                UnjustifiedCount = absenceList.Count(a => !a.IsJustified),
                TotalStudents = students.Count(),
                TotalClasses = allClasses.Count()
            };
        }

        public async Task<byte[]> ExportCsvAsync()
        {
            var absences = await GetAllAbsencesWithDetailsAsync();

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            csv.WriteField("Étudiant");
            csv.WriteField("Classe");
            csv.WriteField("Date");
            csv.WriteField("Justifié");
            csv.WriteField("Justification");
            csv.WriteField("Marqué par");
            await csv.NextRecordAsync();

            foreach (var absence in absences)
            {
                csv.WriteField(absence.StudentName);
                csv.WriteField(absence.ClassName);
                csv.WriteField(absence.Date.ToString("dd/MM/yyyy"));
                csv.WriteField(absence.IsJustified ? "Oui" : "Non");
                csv.WriteField(absence.Justification ?? string.Empty);
                csv.WriteField(absence.MarkedByName);
                await csv.NextRecordAsync();
            }

            await writer.FlushAsync();
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportPdfAsync()
        {
            // Simple text-based report (a full PDF would require iTextSharp or similar)
            var absences = await GetAllAbsencesWithDetailsAsync();
            var sb = new StringBuilder();

            sb.AppendLine("==========================================================");
            sb.AppendLine("           RAPPORT DES ABSENCES");
            sb.AppendLine($"           Généré le : {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
            sb.AppendLine("==========================================================");
            sb.AppendLine();
            sb.AppendLine($"{"Étudiant",-25} {"Classe",-20} {"Date",-12} {"Justifié",-10} {"Justification",-25} {"Marqué par",-25}");
            sb.AppendLine(new string('-', 117));

            foreach (var absence in absences)
            {
                sb.AppendLine(
                    $"{absence.StudentName,-25} " +
                    $"{absence.ClassName,-20} " +
                    $"{absence.Date:dd/MM/yyyy,-12} " +
                    $"{(absence.IsJustified ? "Oui" : "Non"),-10} " +
                    $"{absence.Justification ?? string.Empty,-25} " +
                    $"{absence.MarkedByName,-25}");
            }

            sb.AppendLine();
            sb.AppendLine($"Total absences : {absences.Count()}");
            sb.AppendLine($"Justifiées : {absences.Count(a => a.IsJustified)}");
            sb.AppendLine($"Non justifiées : {absences.Count(a => !a.IsJustified)}");
            sb.AppendLine();
            sb.AppendLine("Note : Pour un export PDF complet, intégrer une bibliothèque telle que iTextSharp.");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<IEnumerable<AbsenceResponseDto>> GetReportByClassAsync(int classId)
        {
            var absences = await _absenceRepository.GetByClassIdAsync(classId);
            return absences.Select(MapToDto);
        }

        public async Task<IEnumerable<AbsenceResponseDto>> GetReportByStudentAsync(int studentId)
        {
            var absences = await _absenceRepository.GetByStudentIdAsync(studentId);
            return absences.Select(MapToDto);
        }

        private async Task<IEnumerable<AbsenceResponseDto>> GetAllAbsencesWithDetailsAsync()
        {
            var allAbsences = await _absenceRepository.GetAllAsync();
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
    }
}
