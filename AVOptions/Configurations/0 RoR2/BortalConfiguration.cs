using BepInEx.Configuration;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

namespace AVOptions.Configurations
{
    public class BortalConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_VFX, "Blue Portal Glow", "Bortal is too bright.\n\nRequires next stage to take effect")
    {

        Light _light;
        PostProcessVolume _postProcessVolume;

        protected override void HandleEvent(object x, EventArgs args)
        {
            if (_light != null && _postProcessVolume != null)
            {
                var enabled = ((ConfigEntry<bool>)x).Value;
                _light.enabled = enabled;
                _postProcessVolume.enabled = enabled;
            }
        }

        protected override void SetBehavior()
        {
            // Using a callback here because for some reason, the portal shop prefab won't load on construction of this class.
            On.RoR2.UI.HUD.Awake += Callback;
            On.RoR2.Stage.Start += StageStart;
        }

        /*
          This exists solely to capture the bortal inside the lunar shop, because the bortal in there doesn't seem to respond
          to the callback function below
        */
        IEnumerator StageStart(On.RoR2.Stage.orig_Start orig, RoR2.Stage self)
        {
            yield return orig(self);
            if (NetworkServer.active)
            {
                
                var bortal = GameObject.Find("PortalShop");
                if (bortal == null)
                    yield break;
            

                var light = bortal.GetComponentInChildren<Light>();
                setLightRange(light);
            }
            yield break;
        }

        /*
          The biggest offender, especially on rallypoint delta is the PostProcessVolume.
          Simply disabling this effect is arguably enough, but lowering the light's range
          helps as well
        */
        void Callback(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/PortalShop/PortalShop.prefab");
            var gameObject = prefab.WaitForCompletion();

            _light = gameObject.GetComponentInChildren<Light>();
            setLightRange(_light);

            _postProcessVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
            _postProcessVolume.enabled = _configEntry.Value;

            orig(self);
        }

        void setLightRange(Light light) => light.range = _configEntry.Value ? 100 : 20;

    }
}
