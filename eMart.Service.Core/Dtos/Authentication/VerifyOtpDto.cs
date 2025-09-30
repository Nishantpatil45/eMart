namespace eMart.Service.Core.Dtos.Authentication
{
    public class VerifyOtpDto
    {
        public string UserId { get; set; }
        public string OtpCode { get; set; }
    }
}
