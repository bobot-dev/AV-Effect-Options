
using BepInEx.Configuration;
using RoR2;
using System;
using System.Linq;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions.Configurations
{
    public class LittleDiscipleConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.BASE_SFX, "Little Disciple Sounds", "Laughing sound when the Little Disciple wisps spawn and its impact sound.\n\nEffective immediately", true, true)
    {
        static AkEvent[] s_events;
        AK.Wwise.Event[] _enabledEvents;


        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            for (int i = 0; i < s_events.Length; i++)
            {
                s_events[i].data = !enabled ? null : _enabledEvents[i];
            }
        }

        protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_SprintWisp.WispOrbEffect_prefab).WaitForCompletion();

            s_events = prefab.GetComponents<AkEvent>();

            _enabledEvents = new AK.Wwise.Event[s_events.Length];
            
            for (int i = 0; i < s_events.Length; i++)
            {
                _enabledEvents[i] = s_events[i].data;
            }
        }
    }
}
