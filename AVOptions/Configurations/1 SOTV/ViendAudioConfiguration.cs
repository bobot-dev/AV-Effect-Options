using BepInEx.Configuration;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class ViendAudioConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.UNIT_EFFECTS, "Viend Sprint/Jump SFX", "Enable run/jump sound effects\nRequires new level/run to take effect", true)
    {

        SprintEffectController _sprintEffectController;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            _sprintEffectController.enabled = enabled;
        }

        protected override void SetBehavior()
        {
            var voidManPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_VoidSurvivor.VoidSurvivorBody_prefab).WaitForCompletion();
            _sprintEffectController = voidManPrefab.GetComponent<SprintEffectController>();
        }
    }
}
