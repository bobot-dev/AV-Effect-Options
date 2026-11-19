using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class IdpVisualConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Interstellar Desk Plant Vfx", Description)
    {
        const String Description =
@"Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. 
Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.

Disable: Effective immediately
Enable: Effective on next level
";

        GameObject _deskPlantSpores;
        GameObject _deskPlantSymbols;
        GameObject _deskPlantMushrooms;

        protected override void HandleEvent(object x, EventArgs args = null)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            _deskPlantSpores.SetActive(enabled);
            _deskPlantSymbols.SetActive(enabled);
            _deskPlantMushrooms.SetActive(enabled);
        }

        protected override void SetBehavior()
        {
            
            
            var DeskplantIndicatorTransform = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Plant.DeskplantWard_prefab)
                .WaitForCompletion().transform
                .Find("Indicator").gameObject.transform;
            _deskPlantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
            _deskPlantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
            _deskPlantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;

        }
    }
}
