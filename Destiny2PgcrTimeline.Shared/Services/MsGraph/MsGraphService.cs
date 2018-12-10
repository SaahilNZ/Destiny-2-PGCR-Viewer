using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.MsGraph
{
    public class MsGraphService
    {
        private static readonly string[] scopes = new string[]
        {
            "UserActivity.ReadWrite.CreatedByApp"
        };

        private static PublicClientApplication app;

        public MsGraphService(string clientId)
        {
            if (app == null)
            {
                app = new PublicClientApplication(clientId);
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            AuthenticationResult authResult = null;
            var accounts = await app.GetAccountsAsync();

            try
            {
                authResult = await app.AcquireTokenSilentAsync(scopes, accounts.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                authResult = await app.AcquireTokenAsync(scopes);
            }

            return authResult.AccessToken;
        }

        private async Task<HttpClient> CreateClientAsync()
        {
            var client = new HttpClient();
            var token = await GetAccessTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task<HttpResponseMessage> PutAsync(string url, string json)
        {
            using (HttpClient client = await CreateClientAsync())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await client.PutAsync(url, content);
            }
        }

        public async Task<HttpResponseMessage> CreateOrReplaceActivityAsync(MsGraphUserActivity activity)
        {
            string url = $"https://graph.microsoft.com/v1.0/me/activities/{activity.AppActivityId}";
            string json = JsonConvert.SerializeObject(activity);
            return await PutAsync(url, json);
        }
    }
}
