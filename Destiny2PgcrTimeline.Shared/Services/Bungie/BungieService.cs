using Destiny2PgcrTimeline.Shared.Services.Bungie.Definitions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class BungieService
    {
        private string bungieApiKey;
        private const string endpoint = "https://www.bungie.net/Platform/";
        private DestinyWorldDb worldDb;

        public BungieService(string apiKey)
        {
            bungieApiKey = apiKey;
            string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Destiny2WorldDb.content");
            worldDb = new DestinyWorldDb(dbPath);
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", bungieApiKey);
            return client;
        }

        private async Task<string> GetAsync(string url)
        {
            string json = "";
            using (HttpClient client = CreateClient())
            {
                json = await client.GetStringAsync(url);
            }
            return json;
        }

        public async Task<List<DestinyActivity>> GetActivityHistory(int platform, string accountId, string characterId, int mode)
        {
            var url = $"{endpoint}Destiny2/{platform}/Account/{accountId}/Character/{characterId}/Stats/Activities/?components=204&mode={mode}";
            var json = await GetAsync(url);
            var response = JsonConvert.DeserializeObject<BungieResponse<DestinyActivityHistoryResponse>>(json);
            return response.Response.Activities == null ? new List<DestinyActivity>() : response.Response.Activities.ToList();
        }

        public async Task<DestinyActivityDefinition> GetActivityDefinitionAsync(uint referenceId)
        {
            return await worldDb.GetDestinyActivityDefinitionAsync(referenceId);
        }

        public async Task<DestinyActivityModeDefinition> GetActivityModeDefinitionAsync(int mode)
        {
            // select json from DestinyActivityModeDefinition where json like '%"modeType":{mode},%'
            return await worldDb.GetDestinyActivityModeDefinitionAsync(mode);
        }

        public async Task<DestinyClassDefinition> GetDestinyClassDefinitionAsync(uint classHash)
        {
            return await worldDb.GetDestinyClassDefinitionAsync(classHash);
        }

        public async Task<DestinyGenderDefinition> GetDestinyGenderDefinitionAsync(uint genderHash)
        {
            return await worldDb.GetDestinyGenderDefinitionAsync(genderHash);
        }

        public async Task<DestinyRaceDefinition> GetDestinyRaceDefinitionAsync(uint raceHash)
        {
            return await worldDb.GetDestinyRaceDefinitionAsync(raceHash);
        }

        public async Task<DestinyManifestResponse> GetDestinyManifest()
        {
            var url = $"{endpoint}/Destiny2/Manifest/";
            var json = await GetAsync(url);
            var response = JsonConvert.DeserializeObject<BungieResponse<DestinyManifestResponse>>(json);
            return response.Response;
        }

        public async Task DownloadDestinyManifest()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            var manifest = await GetDestinyManifest();
            if (localSettings.Values["manifestVersion"] != null
                && localSettings.Values["manifestVersion"].ToString() == manifest.Version
                && !File.Exists(Path.Combine(storageFolder.Path, "Destiny2WorldDb.content")))
            {
                return;
            }

            byte[] databaseBuffer;

            using (HttpClient client = CreateClient())
            {
                var url = $"https://bungie.net{manifest.MobileWorldContentPaths["en"]}";
                databaseBuffer = await client.GetByteArrayAsync(url);
            }

            using (Stream stream = await storageFolder.OpenStreamForWriteAsync("Destiny2WorldDb.zip", CreationCollisionOption.ReplaceExisting))
            {
                stream.Write(databaseBuffer, 0, databaseBuffer.Length);
            }

            string zipPath = Path.Combine(storageFolder.Path, "Destiny2WorldDb.zip");
            string extractPath = Path.Combine(storageFolder.Path, "extract");
            if (!extractPath.EndsWith(Path.DirectorySeparatorChar))
            {
                extractPath += Path.DirectorySeparatorChar;
            }

            using (ZipArchive zip = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith(".content"))
                    {
                        string destinationPath = Path.Combine(storageFolder.Path, "Destiny2WorldDb.content");
                        entry.ExtractToFile(destinationPath, true);
                        break;
                    }
                }
            }

            File.Delete(zipPath);

            localSettings.Values["manifestVersion"] = manifest.Version;
        }

        public async Task<DestinyProfile> GetDestinyProfile(int platform, string accountId)
        {
            var url = $"{endpoint}Destiny2/{platform}/Profile/{accountId}/?components=100";
            var json = await GetAsync(url);
            var response = JsonConvert.DeserializeObject<BungieResponse<DestinyProfileResponse>>(json);
            return response.Response.Profile;
        }

        public async Task<List<DestinyPlayer>> GetDestinyPlayers(int platform, string accountName)
        {
            var url = $"{endpoint}Destiny2/SearchDestinyPlayer/{platform}/{Uri.EscapeDataString(accountName)}/";
            var json = await GetAsync(url);
            var response = JsonConvert.DeserializeObject<BungieResponse<List<DestinyPlayer>>>(json);
            return response.Response;
        }

        public async Task<DestinyCharacter> GetDestinyCharacter(int platform, string accountId, string characterId)
        {
            var url = $"{endpoint}Destiny2/{platform}/Profile/{accountId}/Character/{characterId}/?components=200";
            var json = await GetAsync(url);
            var response = JsonConvert.DeserializeObject<BungieResponse<DestinyCharacterResponse>>(json);
            return response.Response.Character;
        }

        public async Task<BitmapImage> GetDestinyImage(string path)
        {
            byte[] imageBuffer;
            using (HttpClient client = CreateClient())
            {
                var url = $"https://www.bungie.net{path}";
                imageBuffer = await client.GetByteArrayAsync(url);
            }

            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imageBuffer.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }
    }
}
