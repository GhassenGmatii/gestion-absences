using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAbsences.Services.Report;

namespace GestionAbsences.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var summary = await _reportService.GetSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv()
        {
            try
            {
                var csvBytes = await _reportService.ExportCsvAsync();
                return File(csvBytes, "text/csv", "absences.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            try
            {
                var pdfBytes = await _reportService.ExportPdfAsync();
                return File(pdfBytes, "application/pdf", "rapport-absences.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("by-class/{classId}")]
        public async Task<IActionResult> GetReportByClass(int classId)
        {
            try
            {
                var report = await _reportService.GetReportByClassAsync(classId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<IActionResult> GetReportByStudent(int studentId)
        {
            try
            {
                var report = await _reportService.GetReportByStudentAsync(studentId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
    }
}
