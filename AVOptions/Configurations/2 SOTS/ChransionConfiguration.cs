using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using IncreaseDamageOnMultiKillItemDisplayUpdater = RoR2.IncreaseDamageOnMultiKillItemDisplayUpdater;

namespace AVOptions.Configurations
{
    public class ChransionConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.SOTS_SFX, "Chronic Expansion Beep", "Chronic Expansion beeping and text flashing")
    {

        static IncreaseDamageOnMultiKillItemDisplayUpdater s_displayUpdater;
        static float s_blinkBelow;
        
        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            s_displayUpdater.blinkBelow = enabled ? s_blinkBelow : 0;
        }
        
        protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/DLC2/Items/IncreaseDamageOnMultiKill/DisplayIncreaseDamageOnMultiKill.prefab").WaitForCompletion();
            s_displayUpdater = prefab.GetComponent<IncreaseDamageOnMultiKillItemDisplayUpdater>();
            
            s_blinkBelow = s_displayUpdater.blinkBelow;
        }
    }
}
