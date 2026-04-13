using GestionAbsences.DTOs.Class;

namespace GestionAbsences.Services.Class
{
    public interface IClassService
    {
        Task<IEnumerable<ClassResponseDto>> GetAllClassesAsync();
        Task<ClassResponseDto?> GetClassByIdAsync(int id);
        Task<ClassWithStudentsDto?> GetClassWithStudentsAsync(int id);
        Task<ClassResponseDto> CreateClassAsync(CreateClassDto dto);
        Task<ClassResponseDto?> UpdateClassAsync(int id, CreateClassDto dto);
        Task<bool> DeleteClassAsync(int id);
        Task<bool> EnrollStudentAsync(int classId, int studentId);
    }
}
