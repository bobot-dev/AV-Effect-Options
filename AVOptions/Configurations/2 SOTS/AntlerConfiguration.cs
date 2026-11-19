using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ElusiveAntlersPickup = RoR2.ElusiveAntlersPickup;

namespace AVOptions.Configurations
{
    public class AntlerConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.SOTS_SFX, "Elusive Antler Orbs", Description)
    {
        const string Description =
            """
            Enables Elusive Antler's orb spawn sound.

            Toggle: Effective immediately
            """;
        
        static ElusiveAntlersPickup s_pickup;
        AK.Wwise.Event _enabledEvent;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            s_pickup.shardSpawnEvent = !enabled ? null : _enabledEvent;
        }
        
        protected override void SetBehavior()
        {
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC2_Items_SpeedBoostPickup.ElusiveAntlersPickup_prefab).WaitForCompletion();
            s_pickup = prefab.GetComponent<ElusiveAntlersPickup>();

            _enabledEvent = s_pickup.shardSpawnEvent;
        }
    }
}
