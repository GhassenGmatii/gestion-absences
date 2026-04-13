using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAbsences.Services.Absence;
using GestionAbsences.DTOs.Absence;
using GestionAbsences.Helpers;

namespace GestionAbsences.Controllers
{
    [ApiController]
    [Route("api/absences")]
    [Authorize]
    public class AbsencesController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsencesController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        [HttpPost]
        public async Task<IActionResult> MarkAbsence([FromBody] MarkAbsenceDto dto)
        {
            if (!AuthorizationHelper.IsProfesseur(User) && !AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var markedById = AuthorizationHelper.GetCurrentUserId(User);
                var absence = await _absenceService.MarkAbsenceAsync(dto, markedById);
                return CreatedAtAction(nameof(MarkAbsence), new { id = absence.Id }, absence);
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

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkMarkAbsence([FromBody] BulkMarkAbsenceDto dto)
        {
            if (!AuthorizationHelper.IsProfesseur(User) && !AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var markedById = AuthorizationHelper.GetCurrentUserId(User);
                var absences = await _absenceService.BulkMarkAbsenceAsync(dto, markedById);
                return CreatedAtAction(nameof(BulkMarkAbsence), absences);
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

        [HttpGet]
        public async Task<IActionResult> GetAllAbsences()
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var absences = await _absenceService.GetAllAbsencesAsync();
                return Ok(absences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyAbsences()
        {
            if (!AuthorizationHelper.IsEtudiant(User))
                return Forbid();

            try
            {
                var userId = AuthorizationHelper.GetCurrentUserId(User);
                var absences = await _absenceService.GetAbsencesByStudentAsync(userId);
                return Ok(absences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetAbsencesByClass(int classId)
        {
            if (!AuthorizationHelper.IsProfesseur(User) && !AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var absences = await _absenceService.GetAbsencesByClassAsync(classId);
                return Ok(absences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAbsencesByStudent(int studentId)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var absences = await _absenceService.GetAbsencesByStudentAsync(studentId);
                return Ok(absences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var result = await _absenceService.DeleteAbsenceAsync(id);
                if (!result)
                    return NotFound(new { message = $"Absence with ID {id} not found." });

                return Ok(new { message = "Absence deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("{id}/justify")]
        public async Task<IActionResult> JustifyAbsence(int id, [FromBody] JustifyAbsenceRequest request)
        {
            if (!AuthorizationHelper.IsAdmin(User))
                return Forbid();

            try
            {
                var absence = await _absenceService.JustifyAbsenceAsync(id, request.Justification);
                if (absence == null)
                    return NotFound(new { message = $"Absence with ID {id} not found." });

                return Ok(absence);
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

    public class JustifyAbsenceRequest
    {
        public string Justification { get; set; } = string.Empty;
    }
}
