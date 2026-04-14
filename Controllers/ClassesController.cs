using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAbsences.Services.Class;
using GestionAbsences.DTOs.Class;
using GestionAbsences.Helpers;

namespace GestionAbsences.Controllers
{
    [ApiController]
    [Route("api/classes")]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var classDto = await _classService.GetClassByIdAsync(id);
                if (classDto == null)
                    return NotFound(new { message = $"Class with ID {id} not found." });

                return Ok(classDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetClassWithStudents(int id)
        {
            try
            {
                var classWithStudents = await _classService.GetClassWithStudentsAsync(id);
                if (classWithStudents == null)
                    return NotFound(new { message = $"Class with ID {id} not found." });

                return Ok(classWithStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassDto dto)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var createdClass = await _classService.CreateClassAsync(dto);
                return CreatedAtAction(nameof(GetClassById), new { id = createdClass.Id }, createdClass);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] CreateClassDto dto)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var updatedClass = await _classService.UpdateClassAsync(id, dto);
                if (updatedClass == null)
                    return NotFound(new { message = $"Class with ID {id} not found." });

                return Ok(updatedClass);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var result = await _classService.DeleteClassAsync(id);
                if (!result)
                    return NotFound(new { message = $"Class with ID {id} not found." });

                return Ok(new { message = "Class deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("{id}/enroll")]
        public async Task<IActionResult> EnrollStudent(int id, [FromBody] EnrollStudentRequest request)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var result = await _classService.EnrollStudentAsync(id, request.StudentId);
                if (!result)
                    return BadRequest(new { message = "Failed to enroll student." });

                return Ok(new { message = "Student enrolled successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
    }

    public class EnrollStudentRequest
    {
        public int StudentId { get; set; }
    }
}
