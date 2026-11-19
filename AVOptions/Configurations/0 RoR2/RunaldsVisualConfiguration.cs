
using BepInEx.Configuration;
using RoR2;
using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class RunaldsVisualConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Runalds Band", "Enables Runald's Band's ice explosion.\n\nEffective immediately")
    {

        static EffectComponent s_effectComponent;
        static Transform s_transform;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            s_effectComponent.enabled = enabled;

            for (var i = 0; i < s_transform.childCount; i++)
            {
                var child = s_transform.GetChild(i);
                child.gameObject.SetActive(enabled);
            }
        }

        protected override void SetBehavior()
        {
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_ElementalRings.IceRingExplosion_prefab).WaitForCompletion();
            s_transform = prefab.transform;
            s_effectComponent = prefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
        }

    }
}
