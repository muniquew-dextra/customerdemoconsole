using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace ConsoleDI
{
    public class KeyVaultHandler
    {
        public static async Task<string> GetSecretValue(string secretName, string keyVaultUrl, string tenantId, 
            string clientId, string clientSecret)
        {
            try
            {
                SecretClient client = new SecretClient(
                                                    new Uri(keyVaultUrl),
                                                    new ClientSecretCredential(tenantId, clientId, clientSecret));

                KeyVaultSecret secret = await client.GetSecretAsync(secretName);

                return secret.Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
