using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        // POST api/contact
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubmitMessage([FromBody] ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                // 400 — validation failed, return field errors
                return BadRequest(ModelState);
            }

            try
            {
                await Task.Delay(500); // Simulate processing

                // In production you would:
                // - Save to database: await _db.ContactMessages.AddAsync(message);
                // - Send email:       await _emailService.SendAsync(message);
                _logger.LogInformation("Contact message received from {Email} at {Time}",
                    message.Email, message.SubmittedAt);

                // 200 — success
                return Ok(new { success = true, message = "Message received successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process contact message from {Email}", message.Email);

                // 500 — unexpected server error
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred processing your message. Please try again later."
                });
            }
        }

        // GET api/contact/health  — useful for testing the endpoint is reachable
        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Health() => Ok(new { status = "Contact API is running" });
    }
}