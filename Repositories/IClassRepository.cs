using GestionAbsences.Models;

namespace GestionAbsences.Repositories
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<Class?> GetWithStudentsAsync(int classId);
        Task<IEnumerable<Class>> GetByProfessorIdAsync(int professorId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsAsync(int classId);
    }
}
