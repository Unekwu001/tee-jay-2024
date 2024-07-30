namespace Core.TokenServices.TokenValidationService
{
    public interface ITokenValidationService
    {
        bool ValidateToken(string tokenValue);
    }
}