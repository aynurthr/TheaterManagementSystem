using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Theater.Application.Services
{
    public class CryptoService : ICryptoService, IDisposable
    {
        private readonly CryptoServiceOptions options;
        private readonly TripleDES provider;
        private readonly SHA1 sha1;
        private readonly MD5 md5;

        public CryptoService(IOptions<CryptoServiceOptions> options)
        {
            this.options = options.Value;
            this.provider = TripleDES.Create();
            this.md5 = MD5.Create();
            this.sha1 = SHA1.Create();

            byte[] keyBuffer = md5.ComputeHash(Encoding.ASCII.GetBytes($"!@#c8919d0f9{this.options.Key}20@4"));
            byte[] ivBuffer = md5.ComputeHash(Encoding.ASCII.GetBytes($"20@4{this.options.Key}808b62{this.options.Salt}e59@$#"));

            Array.Copy(keyBuffer, 0, provider.Key, 0, Math.Min(keyBuffer.Length, provider.Key.Length));
            Array.Copy(ivBuffer, 0, provider.IV, 0, Math.Min(ivBuffer.Length, provider.IV.Length));
        }

        public string ToMd5(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes($"{text}{options.Salt}");
            byte[] hashedBuffer = md5.ComputeHash(buffer);

            return string.Join("", hashedBuffer.Select(b => $"{b:x2}"));
        }

        public string ToSha1(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes($"{options.Salt}@{text}");
            byte[] hashedBuffer = sha1.ComputeHash(buffer);

            return string.Join("", hashedBuffer.Select(b => $"{b:x2}"));
        }

        public string Encrypt(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var transform = provider.CreateEncryptor();

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string Decrypt(string text)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(text);
                var transform = provider.CreateDecryptor();

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Cryptographic error during decryption: {ex.Message}");
                throw new InvalidOperationException("Decryption failed. Ensure the key and IV are correct.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error during decryption: {ex.Message}");
                throw new InvalidOperationException("An error occurred during decryption.", ex);
            }
        }

        public void Dispose()
        {
            provider.Dispose();
            md5.Dispose();
            sha1.Dispose();
        }
    }
}
