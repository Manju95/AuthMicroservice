namespace AuthMicroService.Interfaces
{
    public interface ICryptoService
    {
        string GenerateSalt();
        string ComputeHash(string password, string salt);
    }
}