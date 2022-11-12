namespace WebAPI.Models
{
    public class ClientCallResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public object Content { get; set; }
    }
}