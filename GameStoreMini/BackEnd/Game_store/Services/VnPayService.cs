using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Game_store.Services
{
    public class VnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(string orderNumber, decimal amount, string orderInfo, string ipAddress)
        {
            var vnp_TmnCode = _configuration["VnPay:TmnCode"] ?? throw new InvalidOperationException("VnPay:TmnCode is not configured");
            var vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? throw new InvalidOperationException("VnPay:HashSecret is not configured");
            var vnp_Url = _configuration["VnPay:Url"] ?? throw new InvalidOperationException("VnPay:Url is not configured");
            var vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"] ?? throw new InvalidOperationException("VnPay:ReturnUrl is not configured");

            Console.WriteLine($"⭐ TmnCode: {vnp_TmnCode}");
            Console.WriteLine($"⭐ HashSecret: {vnp_HashSecret}");

            var vnp_Params = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", ((long)(amount * 100)).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", ipAddress },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_TxnRef", orderNumber }
            };

            // Tạo hashdata - URL encode CẢ KEY và VALUE như code demo VNPay
            var hashDataParts = new List<string>();
            foreach (var param in vnp_Params)
            {
                hashDataParts.Add($"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value)}");
            }
            var hashData = string.Join("&", hashDataParts);
            Console.WriteLine($"⭐ HashData: {hashData}");
            
            // Tính chữ ký trên hashData đã encode
            var vnp_SecureHash = HmacSHA512(vnp_HashSecret, hashData);
            Console.WriteLine($"⭐ SecureHash: {vnp_SecureHash}");
            
            // URL final
            var finalUrl = $"{vnp_Url}?{hashData}&vnp_SecureHash={vnp_SecureHash}";
            Console.WriteLine($"⭐ Final URL: {finalUrl}");
            
            return finalUrl;
        }

        public bool ValidateSignature(Dictionary<string, string> vnpParams, string vnp_SecureHash)
        {
            var vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? throw new InvalidOperationException("VnPay:HashSecret is not configured");
            
            vnpParams.Remove("vnp_SecureHash");
            vnpParams.Remove("vnp_SecureHashType");

            var sortedParams = new SortedDictionary<string, string>(vnpParams);
            
            // URL encode giống như lúc tạo payment URL
            var hashDataParts = new List<string>();
            foreach (var param in sortedParams)
            {
                hashDataParts.Add($"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value)}");
            }
            var hashData = string.Join("&", hashDataParts);
            
            Console.WriteLine($"🔔 HashData for validation: {hashData}");
            var checkSum = HmacSHA512(vnp_HashSecret, hashData);
            Console.WriteLine($"🔔 Calculated checkSum: {checkSum}");
            Console.WriteLine($"🔔 VNPay sent hash: {vnp_SecureHash}");
            Console.WriteLine($"🔔 Match: {checkSum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase)}");

            return checkSum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSHA512(string key, string data)
        {
            var hash = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }
    }
}