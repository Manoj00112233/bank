using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Banking_CapStone.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["Email:Port"] ?? "587");
            _smtpUsername = _configuration["Email:Username"] ?? "";
            _smtpPassword = _configuration["Email:Password"] ?? "";
            _fromEmail = _configuration["Email:FromAddress"] ?? "noreply@banking.com";
            _fromName = _configuration["Email:FromName"] ?? "Banking System";
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true,
                    Timeout = 30000 // 30 seconds timeout
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                    Priority = MailPriority.Normal
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error (you can inject ILogger here if needed)
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string toEmail, string fullName, string username)
        {
            var subject = "Welcome to Banking System";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .footer {{ text-align: center; padding: 10px; font-size: 12px; color: #666; }}
                        .button {{ background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; display: inline-block; margin: 10px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to Banking System</h1>
                        </div>
                        <div class='content'>
                            <h2>Hello {fullName}!</h2>
                            <p>Your account has been created successfully.</p>
                            <p><strong>Username:</strong> {username}</p>
                            <p>You can now login to your account and start managing your banking operations.</p>
                            <p>If you have any questions, please don't hesitate to contact our support team.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 Banking System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string fullName, string resetToken)
        {
            var subject = "Password Reset Request";
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={resetToken}";

            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #ff9800; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .button {{ background-color: #ff9800; color: white; padding: 12px 24px; text-decoration: none; display: inline-block; margin: 15px 0; border-radius: 5px; }}
                        .warning {{ color: #d32f2f; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <h2>Hello {fullName},</h2>
                            <p>We received a request to reset your password.</p>
                            <p>Click the button below to reset your password:</p>
                            <a href='{resetLink}' class='button'>Reset Password</a>
                            <p>Or copy and paste this link into your browser:</p>
                            <p>{resetLink}</p>
                            <p class='warning'>This link will expire in 24 hours.</p>
                            <p>If you didn't request this password reset, please ignore this email or contact support if you have concerns.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendAccountCreatedEmailAsync(string toEmail, string clientName, string accountNumber)
        {
            var subject = "New Account Created Successfully";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .account-box {{ background-color: white; padding: 15px; border-left: 4px solid #2196F3; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Account Created Successfully</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Congratulations! Your new account has been created successfully.</p>
                            <div class='account-box'>
                                <p><strong>Account Number:</strong> {accountNumber}</p>
                                <p><strong>Account Status:</strong> Active</p>
                                <p><strong>Created On:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                            </div>
                            <p>You can now use this account for all your banking transactions.</p>
                            <p>Thank you for choosing our banking services!</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendPaymentApprovedEmailAsync(string toEmail, string clientName, decimal amount, string beneficiaryName)
        {
            var subject = "Payment Approved Successfully";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .payment-box {{ background-color: white; padding: 15px; border-left: 4px solid #4CAF50; margin: 15px 0; }}
                        .amount {{ font-size: 24px; color: #4CAF50; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>✓ Payment Approved</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your payment has been approved and processed successfully.</p>
                            <div class='payment-box'>
                                <p class='amount'>₹{amount:N2}</p>
                                <p><strong>Beneficiary:</strong> {beneficiaryName}</p>
                                <p><strong>Status:</strong> Approved & Processed</p>
                                <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</p>
                            </div>
                            <p>The funds have been transferred to the beneficiary's account.</p>
                            <p>You can view the transaction details in your account dashboard.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendPaymentRejectedEmailAsync(string toEmail, string clientName, decimal amount, string reason)
        {
            var subject = "Payment Rejected - Action Required";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #f44336; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .payment-box {{ background-color: white; padding: 15px; border-left: 4px solid #f44336; margin: 15px 0; }}
                        .reason {{ background-color: #ffebee; padding: 10px; border-radius: 5px; margin: 10px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>✗ Payment Rejected</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Unfortunately, your payment request has been rejected.</p>
                            <div class='payment-box'>
                                <p><strong>Amount:</strong> ₹{amount:N2}</p>
                                <p><strong>Status:</strong> Rejected</p>
                                <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</p>
                            </div>
                            <div class='reason'>
                                <p><strong>Reason for Rejection:</strong></p>
                                <p>{reason}</p>
                            </div>
                            <p>Please review the rejection reason and submit a new payment request with the necessary corrections.</p>
                            <p>If you have any questions, please contact our support team.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendSalaryDisbursementApprovedEmailAsync(string toEmail, string clientName, decimal totalAmount, int employeeCount)
        {
            var subject = "Salary Disbursement Approved";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .salary-box {{ background-color: white; padding: 15px; border-left: 4px solid #4CAF50; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>✓ Salary Disbursement Approved</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your salary disbursement request has been approved and is being processed.</p>
                            <div class='salary-box'>
                                <p><strong>Total Amount:</strong> ₹{totalAmount:N2}</p>
                                <p><strong>Number of Employees:</strong> {employeeCount}</p>
                                <p><strong>Status:</strong> Approved & Processing</p>
                                <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                            </div>
                            <p>The salaries will be credited to employees' accounts shortly.</p>
                            <p>You will receive individual notifications for each transaction.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendSalaryDisbursementRejectedEmailAsync(string toEmail, string clientName, string reason)
        {
            var subject = "Salary Disbursement Rejected - Action Required";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #f44336; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .reason {{ background-color: #ffebee; padding: 10px; border-radius: 5px; margin: 10px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>✗ Salary Disbursement Rejected</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your salary disbursement request has been rejected.</p>
                            <div class='reason'>
                                <p><strong>Reason for Rejection:</strong></p>
                                <p>{reason}</p>
                            </div>
                            <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                            <p>Please review the rejection reason and submit a new request with the necessary corrections.</p>
                            <p>For assistance, please contact our support team.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendSalaryCreditedEmailAsync(string toEmail, string employeeName, decimal amount, string month, int year)
        {
            var subject = $"Salary Credited - {month} {year}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .salary-box {{ background-color: white; padding: 15px; border-left: 4px solid #4CAF50; margin: 15px 0; }}
                        .amount {{ font-size: 28px; color: #4CAF50; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>💰 Salary Credited</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {employeeName},</h2>
                            <p>Your salary has been successfully credited to your account.</p>
                            <div class='salary-box'>
                                <p class='amount'>₹{amount:N2}</p>
                                <p><strong>Salary Period:</strong> {month} {year}</p>
                                <p><strong>Credited On:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</p>
                            </div>
                            <p>Please check your account for the updated balance.</p>
                            <p>Thank you for your continued dedication and hard work!</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendQueryResponseEmailAsync(string toEmail, string name, string querySubject, string response)
        {
            var subject = $"Response to Your Query: {querySubject}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .query-box {{ background-color: #e3f2fd; padding: 10px; border-radius: 5px; margin: 10px 0; }}
                        .response-box {{ background-color: white; padding: 15px; border-left: 4px solid #2196F3; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Query Response</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {name},</h2>
                            <p>Thank you for contacting us. Here is the response to your query:</p>
                            <div class='query-box'>
                                <p><strong>Your Query:</strong> {querySubject}</p>
                            </div>
                            <div class='response-box'>
                                <p><strong>Our Response:</strong></p>
                                <p>{response}</p>
                            </div>
                            <p>If you have any further questions, please don't hesitate to reach out.</p>
                            <p>Best regards,<br>Banking System Support Team</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendDocumentVerificationEmailAsync(string toEmail, string clientName, string documentType, bool verified)
        {
            var subject = verified ? "Document Verified Successfully" : "Document Verification Failed";
            var statusColor = verified ? "#4CAF50" : "#f44336";
            var statusIcon = verified ? "✓" : "✗";
            var statusText = verified ? "Verified" : "Rejected";

            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: {statusColor}; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .doc-box {{ background-color: white; padding: 15px; border-left: 4px solid {statusColor}; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>{statusIcon} Document {statusText}</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your document verification status has been updated.</p>
                            <div class='doc-box'>
                                <p><strong>Document Type:</strong> {documentType}</p>
                                <p><strong>Status:</strong> {statusText}</p>
                                <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</p>
                            </div>
                            {(verified ?
                                "<p>Your document has been successfully verified. You can now proceed with your account operations.</p>" :
                                "<p>Unfortunately, your document could not be verified. Please upload a valid document or contact support for assistance.</p>"
                            )}
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendLowBalanceAlertEmailAsync(string toEmail, string clientName, string accountNumber, decimal balance)
        {
            var subject = "⚠️ Low Balance Alert";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #ff9800; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .alert-box {{ background-color: #fff3e0; padding: 15px; border-left: 4px solid #ff9800; margin: 15px 0; }}
                        .balance {{ font-size: 24px; color: #ff9800; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>⚠️ Low Balance Alert</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your account balance has fallen below the minimum threshold.</p>
                            <div class='alert-box'>
                                <p><strong>Account Number:</strong> {accountNumber}</p>
                                <p><strong>Current Balance:</strong></p>
                                <p class='balance'>₹{balance:N2}</p>
                                <p><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</p>
                            </div>
                            <p>Please ensure sufficient funds are available to avoid any transaction failures or penalties.</p>
                            <p>You can add funds through any of our convenient channels.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendMonthlyStatementEmailAsync(string toEmail, string clientName, string accountNumber, DateTime month)
        {
            var subject = $"Monthly Account Statement - {month:MMMM yyyy}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .statement-box {{ background-color: white; padding: 15px; border-left: 4px solid #2196F3; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Monthly Account Statement</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {clientName},</h2>
                            <p>Your monthly account statement is ready.</p>
                            <div class='statement-box'>
                                <p><strong>Account Number:</strong> {accountNumber}</p>
                                <p><strong>Statement Period:</strong> {month:MMMM yyyy}</p>
                                <p><strong>Generated On:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                            </div>
                            <p>Please login to your account to view and download your detailed statement.</p>
                            <p>You can also contact our support team if you need any assistance.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = true)
        {
            try
            {
                var tasks = toEmails.Select(email => SendEmailAsync(email, subject, body, isHtml));
                var results = await Task.WhenAll(tasks);
                return results.All(r => r); // Returns true if all emails sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bulk email sending failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath, bool isHtml = true)
        {
            try
            {
                if (!File.Exists(attachmentPath))
                {
                    Console.WriteLine($"Attachment file not found: {attachmentPath}");
                    return false;
                }

                using var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(toEmail);

                // Add attachment
                var attachment = new Attachment(attachmentPath);
                mailMessage.Attachments.Add(attachment);

                await smtpClient.SendMailAsync(mailMessage);

                // Dispose attachment
                attachment.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email with attachment sending failed: {ex.Message}");
                return false;
            }
        }
    }
}