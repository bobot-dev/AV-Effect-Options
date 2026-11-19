using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class WungusVisualConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.VOID_VFX, "Weeping Fungus Visuals", Description)
    {
        TemporaryVisualEffect _mushroomVoidVisual;
        const String Description =
@"Enables Weeping Fungus' visual particle effects. 
This includes the floating plus symbols, the floating spore particles, and the void star particle effects. 
Does not affect the generic green healing pulsing effect. 

Disable: Effective immediately
Enable: Effective on next level";

        protected override void HandleEvent(object x, EventArgs args) => _mushroomVoidVisual.enabled = ((ConfigEntry<bool>)x).Value;

        protected override void SetBehavior()
        {
            
            var mushroomVoidEffectPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_MushroomVoid.MushroomVoidEffect_prefab).WaitForCompletion();
            _mushroomVoidVisual = mushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
            _mushroomVoidVisual.enabled = _configEntry.Value;
        }
    }
}
