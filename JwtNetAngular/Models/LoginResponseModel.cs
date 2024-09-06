namespace JwtNetAngular.Models
{
    public class LoginResponseModel
    {
        // public required string Expiration {  get; set; }
        public required string AccessToken { get; set; }
        public required LoginModel User {  get; set; } 
    }
}
