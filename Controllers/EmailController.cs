using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromQuery] string toEmail,
            [FromQuery] string subject,
            [FromBody] string body,
            [FromQuery] bool isHtml = true)
        {
            if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject))
                return BadRequest("Email and subject are required");

            var result = await _emailService.SendEmailAsync(toEmail, subject, body, isHtml);

            if (result)
                return Ok(new { success = true, message = "Email sent successfully" });

            return BadRequest(new { success = false, message = "Failed to send email" });
        }

        [HttpPost("send-welcome")]
        public async Task<IActionResult> SendWelcomeEmail(
            [FromQuery] string toEmail,
            [FromQuery] string fullName,
            [FromQuery] string username)
        {
            var result = await _emailService.SendWelcomeEmailAsync(toEmail, fullName, username);

            if (result)
                return Ok(new { success = true, message = "Welcome email sent successfully" });

            return BadRequest(new { success = false, message = "Failed to send welcome email" });
        }

        [HttpPost("send-payment-notification")]
        public async Task<IActionResult> SendPaymentNotification(
            [FromQuery] string toEmail,
            [FromQuery] string clientName,
            [FromQuery] decimal amount,
            [FromQuery] string beneficiaryName,
            [FromQuery] bool approved = true)
        {
            bool result;

            if (approved)
                result = await _emailService.SendPaymentApprovedEmailAsync(toEmail, clientName, amount, beneficiaryName);
            else
                result = await _emailService.SendPaymentRejectedEmailAsync(toEmail, clientName, amount, "Rejected by bank");

            if (result)
                return Ok(new { success = true, message = "Payment notification sent successfully" });

            return BadRequest(new { success = false, message = "Failed to send payment notification" });
        }

        [HttpPost("send-salary-credited")]
        public async Task<IActionResult> SendSalaryCreditedNotification(
            [FromQuery] string toEmail,
            [FromQuery] string employeeName,
            [FromQuery] decimal amount,
            [FromQuery] string month,
            [FromQuery] int year)
        {
            var result = await _emailService.SendSalaryCreditedEmailAsync(toEmail, employeeName, amount, month, year);

            if (result)
                return Ok(new { success = true, message = "Salary notification sent successfully" });

            return BadRequest(new { success = false, message = "Failed to send salary notification" });
        }

        [HttpPost("send-bulk")]
        public async Task<IActionResult> SendBulkEmail(
            [FromBody] List<string> toEmails,
            [FromQuery] string subject,
            [FromQuery] string body,
            [FromQuery] bool isHtml = true)
        {
            if (toEmails == null || !toEmails.Any())
                return BadRequest("Email list is required");

            var result = await _emailService.SendBulkEmailAsync(toEmails, subject, body, isHtml);

            if (result)
                return Ok(new { success = true, message = $"Bulk email sent to {toEmails.Count} recipients" });

            return BadRequest(new { success = false, message = "Failed to send bulk email" });
        }

        [HttpPost("test-email")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEmail([FromQuery] string toEmail)
        {
            if (string.IsNullOrEmpty(toEmail))
                return BadRequest("Email is required");

            var result = await _emailService.SendEmailAsync(
                toEmail,
                "Test Email from Banking System",
                "<h1>Test Email</h1><p>This is a test email from the Banking System API.</p>",
                true);

            if (result)
                return Ok(new { success = true, message = "Test email sent successfully" });

            return BadRequest(new { success = false, message = "Failed to send test email" });
        }
    }
}