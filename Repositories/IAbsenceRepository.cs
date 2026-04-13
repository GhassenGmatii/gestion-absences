using GestionAbsences.Models;

namespace GestionAbsences.Repositories
{
    public interface IAbsenceRepository : IRepository<Absence>
    {
        Task<IEnumerable<Absence>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Absence>> GetByClassIdAsync(int classId);
        Task<IEnumerable<Absence>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Absence>> GetByClassAndDateAsync(int classId, DateTime date);
        Task<int> GetAbsenceCountByStudentAsync(int studentId);
    }
}
