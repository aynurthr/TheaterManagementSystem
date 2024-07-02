namespace Theater.Infrastructure.Abstracts
{
    public interface ICryptoService
    {
        string ToMd5(string text);
        string ToSha1(string text);



        string Encrypt(string text);
        string Decrypt(string text);
    }
}
