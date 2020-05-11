namespace WebApplication.DTO
{
    public class TransferParametersDTO
    {
        public string OwnAccount { get; set; }
        public string PayeeAccount { get; set; }
        public decimal Amount { get; set; }
    }
}