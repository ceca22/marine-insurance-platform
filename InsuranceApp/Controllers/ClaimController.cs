using InsuranceApp.Domain.Models;
using InsuranceApp.Mapper;
using InsuranceApp.Services.Interfaces;
using InsuranceApp.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimController : ControllerBase
    {
        private IClaimService _claimService;
        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var claims = await _claimService.GetAllAsync();

                return Ok(claims);
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
                var claim = await _claimService.GetByIdAsync(id);

                if (claim == null) {
                    return NotFound();
                }

                return Ok(claim);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Claim request)
        {
            try
            {
                var item = request.ToClaimModel();
                var created = await _claimService.CreateAsync(item);
                
                return Ok(created);
            }
            catch (ClaimException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var deleted = await _claimService.DeleteEntityAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
