using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class FaradaySpurConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.AC_VFX, "Faraday Spur Explosion", Description)
    {
        static Transform s_strikeTransform;
        static EffectComponent s_effectComponent;

        const string Description =
            """
            Enables Faraday Spur's explosion.

            Toggle: Effective immediately
            """;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            
            s_effectComponent.enabled = enabled;

            for (var i = 0; i < s_strikeTransform.childCount; i++)
                s_strikeTransform.GetChild(i).gameObject.SetActive(enabled);
        }

        protected override void SetBehavior()
        {
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_JumpDamageStrike.JumpDamageStrikeVFX_prefab).WaitForCompletion();
            s_strikeTransform = prefab.transform;
            s_effectComponent = prefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
        }
    }
    
    public class FaradaySpurSoundConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.AC_SFX, "Faraday Spur", Description)
    {
        static Transform s_soundEmitterTransform;

        const string Description =
            """
            Enables Faraday Spur's zapping sound it makes when charged.

            Toggle: Effective immediately
            """;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            _logger.LogDebug($"FaradaySpurSound effects enabled={enabled}");
            

            s_soundEmitterTransform.GetChild(0).GetChild(2).gameObject.SetActive(enabled);
        }

        protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_JumpDamageStrike.JumpDamageStrikeLightningAura_prefab).WaitForCompletion();
            s_soundEmitterTransform = prefab.transform;
        }
    }
}
