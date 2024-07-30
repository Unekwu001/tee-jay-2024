namespace Core.TokenServices.TokenValidationService
{
    public interface ITokenValidation
    {
        bool ValidateToken(string tokenValue);
    }
}