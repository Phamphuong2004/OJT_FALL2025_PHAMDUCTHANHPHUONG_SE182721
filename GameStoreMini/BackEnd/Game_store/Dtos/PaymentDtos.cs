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

    public class CreatePaymentRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
    }

    public class PaymentCallbackResponse
    {
        public bool Success { get; set; }
        public string? OrderNumber { get; set; }
        public string? Message { get; set; }
        public string? TransactionId { get; set; }
    }
}
