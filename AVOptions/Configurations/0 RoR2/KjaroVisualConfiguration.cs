using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class KjaroVisualConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Kjaros Band", "Enables Kjaro's Band's fire tornado.\n\nEffective immediately")
    {
        static GameObject s_smoke;
        static GameObject s_tornadoMeshCore;
        static GameObject s_tornadoMeshCoreWide;
        static GameObject s_embers;
        static GameObject s_pointLight;
        static GameObject s_initialBurst;

        protected override void HandleEvent(object x, EventArgs args) => ToggleEffects(((ConfigEntry<bool>)x).Value);

        protected override void SetBehavior()
        {
            
            var FireTornadoGhost = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_ElementalRings.FireTornadoGhost_prefab).WaitForCompletion().transform;
            s_smoke = FireTornadoGhost.Find("Smoke").gameObject;
            s_tornadoMeshCore = FireTornadoGhost.Find("TornadoMeshCore").gameObject;
            s_tornadoMeshCoreWide = FireTornadoGhost.Find("TornadoMeshCore, Wide").gameObject;
            s_embers = FireTornadoGhost.Find("Embers").gameObject;
            s_pointLight = FireTornadoGhost.Find("Point Light").gameObject;
            s_initialBurst = FireTornadoGhost.Find("InitialBurst").gameObject;

            ToggleEffects(_configEntry.Value);
        }

        void ToggleEffects(bool enabled)
        {
            s_smoke.SetActive(enabled);
            s_tornadoMeshCore.SetActive(enabled);
            s_tornadoMeshCoreWide.SetActive(enabled);
            s_embers.SetActive(enabled);
            s_pointLight.SetActive(enabled);
            s_initialBurst.SetActive(enabled);
        }
    }
}
