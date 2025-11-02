namespace Banking_CapStone.DTO.Response.Query
{
    public class QueryListResponseDto
    {
        public int QueryId { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DaysPending { get; set; }
    }
}
