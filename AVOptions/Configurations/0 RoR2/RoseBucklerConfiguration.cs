

using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class RoseBucklerConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_SFX, "Rose Buckler SFX", "Buckler noise when you start sprinting.\n\nEffective immediately")
    {

        static AkEvent s_event;
        AK.Wwise.Event _enabledEvent;


        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            s_event.data = !enabled ? null : _enabledEvent;
        }

        protected override void SetBehavior()
        {

            
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_SprintArmor.BucklerDefense_prefab).WaitForCompletion();
            s_event = prefab.GetComponent<AkEvent>();

            _enabledEvent = s_event.data;
        }
    }
}
