using GestionAbsences.Data;
using GestionAbsences.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAbsences.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Class?> GetWithStudentsAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.Professor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }

        public async Task<IEnumerable<Class>> GetByProfessorIdAsync(int professorId)
        {
            return await _context.Classes
                .Include(c => c.Professor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Where(c => c.ProfessorId == professorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsAsync(int classId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.ClassEntity)
                .Where(e => e.ClassId == classId)
                .ToListAsync();
        }
    }
}
