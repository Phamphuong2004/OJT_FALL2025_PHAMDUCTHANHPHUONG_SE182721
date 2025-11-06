namespace GameStoreMini.Dtos
{
    public class CreatePaymentDto
    {
        public string? OrderNumber { get; set; }
        public string? Method { get; set; }
    }

    public class ConfirmPaymentDto
    {
        public string? OrderNumber { get; set; }
        public string? TransactionId { get; set; }
    }
}
