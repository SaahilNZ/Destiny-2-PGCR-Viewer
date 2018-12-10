using Destiny2PgcrTimeline.Shared.Services.Bungie.Definitions;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class DestinyWorldDb
    {
        string dbPath;

        public DestinyWorldDb(string dbPath)
        {
            this.dbPath = dbPath;
        }

        public async Task<DestinyActivityDefinition> GetDestinyActivityDefinitionAsync(uint referenceId)
        {
            List<string> queryResults = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
            {
                await db.OpenAsync();
                string queryString = $"SELECT json FROM DestinyActivityDefinition WHERE id={(int)referenceId}";
                SqliteCommand selectCommand = new SqliteCommand(queryString, db);
                var query = await selectCommand.ExecuteReaderAsync();
                while (query.Read())
                {
                    queryResults.Add(query.GetString(0));
                }
                db.Close();
            }

            var json = queryResults.FirstOrDefault();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<DestinyActivityDefinition>(json);
            }
        }

        public async Task<DestinyActivityModeDefinition> GetDestinyActivityModeDefinitionAsync(int mode)
        {
            List<string> queryResults = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
            {
                await db.OpenAsync();
                string queryString = $"SELECT json FROM DestinyActivityModeDefinition WHERE json like '%\"modeType\":{mode},%'";
                SqliteCommand selectCommand = new SqliteCommand(queryString, db);
                var query = await selectCommand.ExecuteReaderAsync();
                while (query.Read())
                {
                    queryResults.Add(query.GetString(0));
                }
                db.Close();
            }

            var json = queryResults.FirstOrDefault();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<DestinyActivityModeDefinition>(json);
            }
        }

        public async Task<DestinyClassDefinition> GetDestinyClassDefinitionAsync(uint classHash)
        {
            List<string> queryResults = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
            {
                await db.OpenAsync();
                string queryString = $"SELECT json FROM DestinyClassDefinition WHERE id={(int)classHash}";
                SqliteCommand selectCommand = new SqliteCommand(queryString, db);
                var query = await selectCommand.ExecuteReaderAsync();
                while (query.Read())
                {
                    queryResults.Add(query.GetString(0));
                }
                db.Close();
            }

            var json = queryResults.FirstOrDefault();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<DestinyClassDefinition>(json);
            }
        }

        public async Task<DestinyGenderDefinition> GetDestinyGenderDefinitionAsync(uint genderHash)
        {
            List<string> queryResults = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
            {
                await db.OpenAsync();
                string queryString = $"SELECT json FROM DestinyGenderDefinition WHERE id={(int)genderHash}";
                SqliteCommand selectCommand = new SqliteCommand(queryString, db);
                var query = await selectCommand.ExecuteReaderAsync();
                while (query.Read())
                {
                    queryResults.Add(query.GetString(0));
                }
                db.Close();
            }

            var json = queryResults.FirstOrDefault();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<DestinyGenderDefinition>(json);
            }
        }

        public async Task<DestinyRaceDefinition> GetDestinyRaceDefinitionAsync(uint raceHash)
        {
            List<string> queryResults = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
            {
                await db.OpenAsync();
                string queryString = $"SELECT json FROM DestinyRaceDefinition WHERE id={(int)raceHash}";
                SqliteCommand selectCommand = new SqliteCommand(queryString, db);
                var query = await selectCommand.ExecuteReaderAsync();
                while (query.Read())
                {
                    queryResults.Add(query.GetString(0));
                }
                db.Close();
            }

            var json = queryResults.FirstOrDefault();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<DestinyRaceDefinition>(json);
            }
        }
    }
}
