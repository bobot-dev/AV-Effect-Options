
using BepInEx.Configuration;
using RoR2;
using System;

namespace AVOptions.Configurations
{
    public class PlasmaShrimpConfiguration(ConfigFile configFile) : AvConfiguration(configFile, Category.VOID_SFX, "Plasma Shrimp Sounds", "Sounds like bowling!.\n\nEffective immediately", true, true)
    {
        static EffectDef s_effectDef;

        protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            
            s_effectDef.spawnSoundEventName = enabled ? "Play_item_void_critGlasses" : string.Empty;
        }

        protected override void SetBehavior()
        {
            var effectIndex = EffectCatalog.FindEffectIndex("MissileVoidOrbEffect");
            s_effectDef = EffectCatalog.GetEffectDef(effectIndex);
            
            s_effectDef.spawnSoundEventName = _configEntry.Value ? "Play_item_void_critGlasses" : string.Empty;
        }
    }
}
