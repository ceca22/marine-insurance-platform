using InsuranceApp.Auditing;
using InsuranceApp.DataAccess;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.Models;
using InsuranceApp.Mapper;
using InsuranceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CoverController : ControllerBase
{
    private readonly InsuranceAppDbContext _insuranceAppDbContext;
    private readonly ILogger<CoverController> _logger;
    private readonly Auditer _auditer;
    private ICoverService _coverService;

    public CoverController(ICoverService coverService, InsuranceAppDbContext insuranceAppDbContext, AuditContext auditContext, ILogger<CoverController> logger)
    {
        _coverService = coverService;
        _insuranceAppDbContext = insuranceAppDbContext;
        _logger = logger;
        _auditer = new Auditer(auditContext);
    }

    [HttpPost("compute")]
    public async Task<IActionResult> GetPremiumValueAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        try
        {
           var premium = _coverService.GetPremiumValue(startDate, endDate, coverType);
            
            return Ok(premium);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var covers = await _coverService.GetAllAsync();

            return Ok(covers);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)

    {
        try
        {
            var cover = await _coverService.GetByIdAsync(id);

            if (cover == null)
            {
                return NotFound();
            }

            return Ok(cover);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync(Cover request)
    {
        try
        {
            var item = request.ToCoverModel();
            var created = await _coverService.CreateAsync(item);

            return Ok(created);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            var deleted = await _coverService.DeleteEntityAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
