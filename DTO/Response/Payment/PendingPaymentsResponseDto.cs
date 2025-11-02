namespace Banking_CapStone.DTO.Response.Payment
{
    public class PendingPaymentsResponseDto
    {
        public int PaymentId { get; set; }
        public string ClientName { get; set; }
        public string BeneficiaryName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentPurpose { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DaysPending { get; set; }

        public bool IsUrgent { get; set; }
    }
}
