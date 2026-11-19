using System;
using BepInEx.Configuration;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{

    public class ShurikenConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.VOID_SFX, "Shuriken Sounds", "for wife.\n\nEffective immediately")
    {
        static ProjectileController s_projectileController;
        static ProjectileSingleTargetImpact s_projectileSingleTargetImpact;


        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            
            s_projectileController.startSound = enabled ? "Play_bandit2_m2_alt_throw" : string.Empty;
            s_projectileSingleTargetImpact.hitSoundString = enabled ? "Play_bandit2_m2_impact" : string.Empty;
            s_projectileSingleTargetImpact.enemyHitSoundString = enabled ? "Play_bandit2_m2_impact" : string.Empty;
        }

        protected override void SetBehavior()
        {
            
            var shurikenProjectilePrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_PrimarySkillShuriken.ShurikenProjectile_prefab).WaitForCompletion();
            
            s_projectileController = shurikenProjectilePrefab.GetComponent<ProjectileController>();
            s_projectileSingleTargetImpact = shurikenProjectilePrefab.GetComponent<ProjectileSingleTargetImpact>();
        }
    }
}
