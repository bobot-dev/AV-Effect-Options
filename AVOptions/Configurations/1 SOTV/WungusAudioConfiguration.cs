
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class WungusAudioConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.VOID_SFX, "Weeping Fungus Sound", Description)
    {
        LoopSoundPlayer MushroomVoidAudio;

        const String Description =
@"Enables Weeping Fungus sound effects. 

Disable: Effective immediately
Enable: Effective on next level";

        protected override void HandleEvent(object x, EventArgs args) => MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;

        protected override void SetBehavior()
        {
            
            var mushroomVoidEffectPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_MushroomVoid.MushroomVoidEffect_prefab).WaitForCompletion();
            MushroomVoidAudio = mushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
            MushroomVoidAudio.enabled = _configEntry.Value;
        }

    }
}
