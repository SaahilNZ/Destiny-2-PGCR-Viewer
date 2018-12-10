using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Destiny2PgcrTimeline
{
    internal class AppService
    {
        public async Task<DestinyPlayerData> GetPlayerDataAsync(DestinyPlayer player)
        {
            var bungie = new BungieService(Shared.SharedData.BungieApiKey);
            int platform = player.MembershipType;
            var accountId = player.MembershipId;
            var destinyProfile = await bungie.GetDestinyProfile(platform, accountId);
            int mode = 5;

            await bungie.DownloadDestinyManifest();

            var nameplates = new List<CharacterNameplateViewModel>();
            var activityHistoryLists = new List<List<PgcrCardViewModel>>();
            var characterIds = destinyProfile.Data.CharacterIds;

            for (int i = 0; i < characterIds.Count; i++)
            {
                var characterId = destinyProfile.Data.CharacterIds[i];
                var history = await bungie.GetActivityHistory(platform, accountId, characterId, mode);

                var character = await bungie.GetDestinyCharacter(platform, accountId, characterId);

                var nameplate = new CharacterNameplateViewModel
                {

                    ElementVisibility = Visibility.Visible,
                    ClassName = (await bungie.GetDestinyClassDefinitionAsync(character.Data.ClassHash)).DisplayProperties.Name,
                    Race = (await bungie.GetDestinyRaceDefinitionAsync(character.Data.RaceHash)).DisplayProperties.Name,
                    Gender = (await bungie.GetDestinyGenderDefinitionAsync(character.Data.GenderHash)).DisplayProperties.Name,
                    Level = character.Data.BaseCharacterLevel,
                    Power = character.Data.Light
                };
                
                var emblemBrush = new ImageBrush();
                emblemBrush.ImageSource = await bungie.GetDestinyImage(character.Data.EmblemBackgroundPath);
                nameplate.Emblem = emblemBrush;

                var historyList = new List<PgcrCardViewModel>();
                foreach (var pgcr in history)
                {
                    historyList.Add(new PgcrCardViewModel
                    {
                        Pgcr = pgcr,
                        CharacterId = characterId
                    });
                }

                nameplates.Add(nameplate);
                activityHistoryLists.Add(historyList);
            }

            return new DestinyPlayerData
            {
                CharacterNameplates = nameplates,
                ActivityHistoryLists = activityHistoryLists,
                CharacterIDs = characterIds,
                MembershipID = player.MembershipId,
                MembershipType = player.MembershipType
            };
        }
    }

    internal class DestinyPlayerData
    {
        public List<CharacterNameplateViewModel> CharacterNameplates;
        public List<List<PgcrCardViewModel>> ActivityHistoryLists;
        public List<string> CharacterIDs;
        public string MembershipID;
        public int MembershipType;
    }
}
