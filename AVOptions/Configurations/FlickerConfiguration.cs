using System;
using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;
using System.Reflection;
using MonoMod.Cil;
using UnityEngine;

namespace AVOptions.Configurations
{
    public class FlickerConfiguration
    {
        //private readonly RiskOfOptions _riskOfOptions = RiskOfOptions.Instance;
        private protected ManualLogSource _logger;
        
        readonly ConfigEntry<int> _flickerIntensitySlider;
        readonly ConfigEntry<int> _flickerSpeedSlider;
        
        internal FlickerConfiguration(ConfigFile configFile)
        {
            _logger = BepInEx.Logging.Logger.CreateLogSource($"c.t.a.c.{GetType().Name}");
            
            _flickerIntensitySlider = configFile.Bind(Category.FLICKERING, "Flickering Light Intensity", 100, "Controls the intensity of (most) flickering lights.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_flickerIntensitySlider);
            
            _flickerSpeedSlider = configFile.Bind(Category.FLICKERING, "Flickering Light Speed", 100, "Controls the speed of (most) flickering lights.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_flickerSpeedSlider);
            
            IL.RoR2.FlickerLight.Update += FlickerLightOnUpdate;
        }
        
        
        void FlickerLightOnUpdate(ILContext il)
        {
            var c = new ILCursor(il);
            

            if (!c.TryGotoNext(MoveType.After, 

                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld<FlickerLight>(nameof(FlickerLight.stopwatch)),
                    x => x.MatchCallOrCallvirt<Time>("get_deltaTime")
                ))
            {
                _logger.LogWarning("FlickerLight.FlickerLightOnUpdate get_deltaTime TryGotoNext failed");
                return;
            }
            
            c.EmitDelegate<Func<float, float>>((time) =>
            {
                var mult = _flickerSpeedSlider.Value / 100f;
                return time * Mathf.Clamp01(mult);
            });
            
            if (!c.TryGotoNext(MoveType.After, 

                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld<FlickerLight>(nameof(FlickerLight.stopwatch)),
                    x => x.MatchCallOrCallvirt<Wave>(nameof(Wave.Evaluate))
                ))
            {
                _logger.LogWarning("FlickerLight.FlickerLightOnUpdate Wave.Evaluate TryGotoNext failed");
                return;
            }
            
            c.EmitDelegate<Func<float, float>>((wave) =>
            {
                var mult = _flickerIntensitySlider.Value / 100f;
                return wave * Mathf.Clamp01(mult);
            });
        }
    }
}
