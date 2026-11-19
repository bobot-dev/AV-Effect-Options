using System;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using RoR2.Audio;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{

    
    
    public class GhostBladeConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.HC_SFX, "Stained Scimitar", Description)
    {

        static ProjectileController s_projectileController;
        static LoopSoundDef s_flightSoundLoop;
        
        const string Description =
            """
            Enables Stained Scimitar's sounds.

            Toggle: Effective immediately
            """;
        
        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            s_projectileController.flightSoundLoop = enabled ? s_flightSoundLoop : null;
            s_projectileController.startSound = enabled ? "Play_Item_Cursed_GhostBlades_Spawn" : null;
        }

        protected override void SetBehavior()
        {
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4_GhostBlades.GhostBladesProjectile_prefab).WaitForCompletion();

            s_projectileController = prefab.GetComponent<ProjectileController>();
            s_flightSoundLoop = s_projectileController.flightSoundLoop;
        }
    }
}
