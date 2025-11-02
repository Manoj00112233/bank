namespace Banking_CapStone.Service
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
        Task<bool> SendWelcomeEmailAsync(string toEmail, string fullName, string username);
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string fullName, string resetToken);
        Task<bool> SendAccountCreatedEmailAsync(string toEmail, string clientName, string accountNumber);
        Task<bool> SendPaymentApprovedEmailAsync(string toEmail, string clientName, decimal amount, string beneficiaryName);
        Task<bool> SendPaymentRejectedEmailAsync(string toEmail, string clientName, decimal amount, string reason);
        Task<bool> SendSalaryDisbursementApprovedEmailAsync(string toEmail, string clientName, decimal totalAmount, int employeeCount);
        Task<bool> SendSalaryDisbursementRejectedEmailAsync(string toEmail, string clientName, string reason);
        Task<bool> SendSalaryCreditedEmailAsync(string toEmail, string employeeName, decimal amount, string month, int year);
        Task<bool> SendQueryResponseEmailAsync(string toEmail, string name, string querySubject, string response);
        Task<bool> SendDocumentVerificationEmailAsync(string toEmail, string clientName, string documentType, bool verified);
        Task<bool> SendLowBalanceAlertEmailAsync(string toEmail, string clientName, string accountNumber, decimal balance);
        Task<bool> SendMonthlyStatementEmailAsync(string toEmail, string clientName, string accountNumber, DateTime month);
        Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = true);
        Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath, bool isHtml = true);
    }
}
