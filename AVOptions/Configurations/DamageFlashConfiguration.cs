using System;
using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;

namespace AVOptions.Configurations
{
    public class DamageFlashConfiguration
    {
        //private readonly RiskOfOptions _riskOfOptions = RiskOfOptions.Instance;
        private protected ManualLogSource _logger;

        readonly ConfigEntry<int> _damageFlashSliderPlayer;
        readonly ConfigEntry<int> _damageFlashSliderOther;
        
        readonly ConfigEntry<int> _healFlashSliderPlayer;
        readonly ConfigEntry<int> _healFlashSliderOther;
        
        internal DamageFlashConfiguration(ConfigFile configFile)
        {
            _logger = BepInEx.Logging.Logger.CreateLogSource($"c.t.a.c.{GetType().Name}");


            _damageFlashSliderPlayer = configFile.Bind(Category.FLICKERING, "Ally Damage Flash Intensity", 100, "Controls the intensity of the orange flash on allies' models when they take damage.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_damageFlashSliderPlayer);
            
            _damageFlashSliderOther = configFile.Bind(Category.FLICKERING, "Enemy Damage Flash Intensity", 100, "Controls the intensity of the orange flash on enemies models when they take damage.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_damageFlashSliderOther);
            
            _healFlashSliderPlayer = configFile.Bind(Category.FLICKERING, "Ally Heal Flash Intensity", 100, "Controls the intensity of the green flash on allies' models when they heal.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_healFlashSliderPlayer);
            
            _healFlashSliderOther = configFile.Bind(Category.FLICKERING, "Enemy Heal Flash Intensity", 100, "Controls the intensity of the green flash on enemies' models when they heal.\n\nEffective immediately");
            RiskOfOptions.AddSliderOption(_healFlashSliderOther);

            _logger.LogDebug("Configuration completed.");
            
            
            IL.RoR2.CharacterModel.UpdateMaterials += CharacterModelOnUpdateMaterials;
        }
        void CharacterModelOnUpdateMaterials(ILContext il)
        {
            var c = new ILCursor(il);
            
            int colourVar = -1;
            
            if (!c.TryGotoNext(MoveType.After, 
                    x => x.MatchLdsfld<RoR2.CharacterModel>(nameof(RoR2.CharacterModel.healFlashColor)),
                    x => x.MatchLdloc(out _),
                    x => x.MatchCallOrCallvirt<UnityEngine.Color>("op_Multiply"),
                    x => x.MatchStloc(out colourVar)
                ))
            {
                _logger.LogWarning("DamageFlashConfiguration.CharacterModelOnUpdateMaterials healFlashColor TryGotoNext failed");
                return;
            }

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldloc_S, (byte)colourVar);
            c.EmitDelegate<Func<CharacterModel, Color, Color>>((model, color) =>
            {
                float mult = model.body.teamComponent.teamIndex == TeamIndex.Player ? _healFlashSliderPlayer.Value : _healFlashSliderOther.Value;

                mult /= 100f;
                mult = Mathf.Clamp01(mult);

                return color * mult;
            });
            c.Emit(OpCodes.Stloc_S, (byte)colourVar);
            

            if (!c.TryGotoNext(MoveType.After, 
                    x => x.MatchLdsfld<RoR2.CharacterModel>(nameof(RoR2.CharacterModel.hitFlashShieldColor)),
                    x => x.MatchLdloc(out _),
                    x => x.MatchCallOrCallvirt<UnityEngine.Color>("op_Multiply"),
                    x => x.MatchStloc(colourVar)
                ))
            {
                _logger.LogWarning("DamageFlashConfiguration.CharacterModelOnUpdateMaterials hitFlashShieldColor TryGotoNext failed");
                return;
            }
            
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldloc_S, (byte)colourVar);
            c.EmitDelegate<Func<CharacterModel, Color, Color>>((model, color) =>
            {
                float mult = model.body.teamComponent.teamIndex == TeamIndex.Player ? _damageFlashSliderPlayer.Value : _damageFlashSliderOther.Value;

                mult /= 100f;
                mult = Mathf.Clamp01(mult);

                return color * mult;
            });
            c.Emit(OpCodes.Stloc_S, (byte)colourVar);
        }
    }
}
