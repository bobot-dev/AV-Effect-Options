using BepInEx.Configuration;
using RoR2.Projectile;
using System;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class FireworkTailConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Fireworks Tails", "Fireworks normally have tails.\n\nEffective immediately")
    {

        static TrailRenderer s_trailRenderer;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            s_trailRenderer.enabled = enabled;
        }

        protected override void SetBehavior()
        {

            
            var fireworkPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Firework.FireworkGhost_prefab).WaitForCompletion();
            var controller = fireworkPrefab.GetComponent<ProjectileGhostController>();
            s_trailRenderer = controller.GetComponent<TrailRenderer>();

        }
    }
}
