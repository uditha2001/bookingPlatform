namespace CoreGateway.API.Service.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(string username);
        Task<String> ValidateUserCredentials(string username, string password);
    }
}
