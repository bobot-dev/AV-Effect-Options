using BepInEx.Configuration;
using RoR2;
using System;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class BlastShowerConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Blast Shower", Description)
    {
        static Transform s_cleanseTransform;
        static EffectComponent s_effectComponent;

        const string Description = """
                                   Enables Blast Shower's effects.

                                   Effective immediately
                                   """;

        protected override void HandleEvent(object x, EventArgs args)
        {

            var enabled = ((ConfigEntry<bool>)x).Value;
            _logger.LogDebug($"Cleanse effects enabled={enabled}");

            s_effectComponent.enabled = enabled;
            _logger.LogDebug($"Index set to : {s_effectComponent.effectIndex}");

            for (var i = 0; i < s_cleanseTransform.childCount; i++)
                s_cleanseTransform.GetChild(i).gameObject.SetActive(enabled);

        }

        protected override void SetBehavior()
        {
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Cleanse.CleanseEffect_prefab).WaitForCompletion();
            s_cleanseTransform = prefab.transform;
            s_effectComponent = prefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
        }
    }
}
