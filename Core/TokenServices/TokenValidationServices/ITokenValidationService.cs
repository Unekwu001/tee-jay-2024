namespace Core.TokenServices.TokenValidationService
{
    public interface ITokenValidationService
    {
        Task<bool> ValidateToken(string tokenValue);
    }
}