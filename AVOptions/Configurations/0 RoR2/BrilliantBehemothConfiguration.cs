using BepInEx.Configuration;
using RoR2.Projectile;
using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class BrilliantBehemothConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Brilliant Behemoth", "Behemoth explosion.\n\nEffective immediately")
    {
        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

        }

        protected override void SetBehavior()
        {
            IL.RoR2.GlobalEventManager.OnHitAllProcess += GlobalEventManagerOnOnHitAllProcess;
        }
        void GlobalEventManagerOnOnHitAllProcess(ILContext il)
        {
            
            var c = new ILCursor(il);
            var label = c.DefineLabel();
            
            if (!c.TryGotoNext(MoveType.After, 
                    x => x.MatchLdsfld(typeof(RoR2.RoR2Content.Items), nameof(RoR2.RoR2Content.Items.Behemoth)),
                    x => x.MatchCallOrCallvirt<RoR2.Inventory>(nameof(RoR2.Inventory.GetItemCountEffective))
                ))
            {
                _logger.LogWarning("BrilliantBehemothConfiguration.GlobalEventManagerOnOnHitAllProcess GetItemCountEffective(Behemoth) TryGotoNext failed");
                return;
            }
            

            if (!c.TryGotoNext(MoveType.Before, 
                    x => x.MatchLdstr("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"),
                    x => x.MatchCallOrCallvirt(typeof(RoR2.LegacyResourcesAPI), nameof(RoR2.LegacyResourcesAPI.Load))
                ))
            {
                _logger.LogWarning("BrilliantBehemothConfiguration.GlobalEventManagerOnOnHitAllProcess OmniExplosionVFXQuick TryGotoNext failed");
                return;
            }

            c.EmitDelegate(() => _configEntry.Value);
            c.Emit(OpCodes.Brfalse, label);
            
            if (!c.TryGotoNext(MoveType.After, x => x.MatchCallOrCallvirt(typeof(RoR2.EffectManager), nameof(RoR2.EffectManager.SpawnEffect))))
            {
                _logger.LogWarning("BrilliantBehemothConfiguration.GlobalEventManagerOnOnHitAllProcess EffectManager.SpawnEffect TryGotoNext failed");
                return;
            }

            c.MarkLabel(label);
        }
    }
}
