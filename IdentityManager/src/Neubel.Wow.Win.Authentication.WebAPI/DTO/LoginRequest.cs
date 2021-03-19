namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    public class LoginRequest
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
    }
}