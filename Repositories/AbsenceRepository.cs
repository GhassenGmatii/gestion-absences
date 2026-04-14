using GestionAbsences.Data;
using GestionAbsences.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAbsences.Repositories
{
    public class AbsenceRepository : Repository<Absence>, IAbsenceRepository
    {
        public AbsenceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Absence>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.ClassEntity)
                .Include(a => a.MarkedBy)
                .Where(a => a.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Absence>> GetByClassIdAsync(int classId)
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.ClassEntity)
                .Include(a => a.MarkedBy)
                .Where(a => a.ClassId == classId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Absence>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.ClassEntity)
                .Include(a => a.MarkedBy)
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Absence>> GetByClassAndDateAsync(int classId, DateTime date)
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.ClassEntity)
                .Include(a => a.MarkedBy)
                .Where(a => a.ClassId == classId && a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<int> GetAbsenceCountByStudentAsync(int studentId)
        {
            return await _context.Absences
                .CountAsync(a => a.StudentId == studentId);
        }
    }
}
