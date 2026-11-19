
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using RoR2;

namespace AVOptions.Configurations
{

    static class Category
    {
        public static readonly String FLICKERING = "FLICKERING/FLASHES";
        
        public static readonly String BASE_VFX = "VFX";
        public static readonly String BASE_SFX = "SFX";
        public static readonly String VOID_VFX = "SOTV VFX";
        public static readonly String VOID_SFX = "SOTV SFX";
        public static readonly String UNIT_EFFECTS = "Unit FX";
        
        public static readonly String SOTS_VFX = "SOTS VFX";
        public static readonly String SOTS_SFX = "SOTS SFX";
        
        public static readonly String AC_VFX = "AC VFX";
        public static readonly String AC_SFX = "AC SFX";
        
        public static readonly String HC_VFX = "HC VFX";
        public static readonly String HC_SFX = "HC SFX";
    }

    public abstract class AvConfiguration
    { 
        protected ConfigEntry<bool> _configEntry;
        protected ManualLogSource _logger;
        
        //static readonly RiskOfOptions s_riskOfOptions = RiskOfOptions.Instance;
        string _name;

        protected AvConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true, bool needsEffectCatalog = false)
        {
            _name = $"c.t.a.c.{GetType().Name}";
            _logger = Logger.CreateLogSource(_name);
            try
            {
                _configEntry = configFile.Bind(category, key, defaultSetting, description);


                if (needsEffectCatalog)
                {
                    if (EffectCatalog.availability.available)
                    {
                        
                    }
                    
                    _configEntry.SettingChanged += (sender, args) =>
                    {
                        if (EffectCatalog.availability.available)
                            HandleEvent(sender, args);
                    };
                    
                    EffectCatalog.availability.CallWhenAvailable((() =>
                    {
                        SetBehavior();
                        HandleEvent(_configEntry, null);
                    }));
                }
                else
                {
                    SetBehavior();
                    HandleEvent(_configEntry, null);
                    
                    _configEntry.SettingChanged += HandleEvent;
                }
 
                AVOptions.RiskOfOptions.AddOption(_configEntry);
                
                _logger.LogDebug("Configuration completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new ConfigurationException(_name, ex);
            }
        }

        protected abstract void HandleEvent(object x, EventArgs args);

        protected abstract void SetBehavior();
    }
}
