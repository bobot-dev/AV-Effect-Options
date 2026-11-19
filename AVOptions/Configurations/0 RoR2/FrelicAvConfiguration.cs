using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;
using System.Reflection;
using UnityEngine;

namespace AVOptions.Configurations
{
    public class FrelicAvConfiguration
    {
        //private readonly RiskOfOptions _riskOfOptions = RiskOfOptions.Instance;
        private protected ManualLogSource _logger;

        readonly ConfigEntry<bool> _soundEnabled;
        readonly ConfigEntry<bool> _fovEnabled;
        readonly ConfigEntry<bool> _particlesEnabled;

        readonly FieldInfo _icicleAuraAimRequest;

        internal FrelicAvConfiguration(ConfigFile configFile)
        {
            _logger = BepInEx.Logging.Logger.CreateLogSource($"c.t.a.c.{GetType().Name}");

            var fovDesc = "Enables the temporary FOV change that Frost Relic's on-kill proc gives. " +
                "Does not affect the particle effects (see the Frost Relic Particles option)." +
                "\n\nEffective immediately";
            _fovEnabled = configFile.Bind(Category.BASE_VFX, "Frost Relic FOV", true, fovDesc);

            var particleDesc = "Enables the chunk and ring effects of Frost Relic. " +
                "Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item." +
                "\n\nEffective immediately";
            _particlesEnabled = configFile.Bind(Category.BASE_VFX, "Frost Relic Particles", true, particleDesc);

            _soundEnabled = configFile.Bind(Category.BASE_SFX, "Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.\n\nEffective immediately");

            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            _icicleAuraAimRequest = typeof(IcicleAuraController).GetField("aimRequest", bindingFlags);

            On.RoR2.IcicleAuraController.OnIcicleGained += FrelicGainedEventHandler;
            On.RoR2.IcicleAuraController.OnIciclesActivated += FrelicActivationEventHandler;

            AVOptions.RiskOfOptions.AddOption(_fovEnabled);
            AVOptions.RiskOfOptions.AddOption(_particlesEnabled);
            AVOptions.RiskOfOptions.AddOption(_soundEnabled);

            _logger.LogDebug("Configuration completed.");
        }

        void FrelicActivationEventHandler(On.RoR2.IcicleAuraController.orig_OnIciclesActivated _, IcicleAuraController self)
        {
            if (_soundEnabled.Value)
                Util.PlaySound("Play_item_proc_icicle", self.gameObject);

            if (_fovEnabled.Value)
            {
                var ctp = self.owner.GetComponent<CameraTargetParams>();
                if (ctp) _icicleAuraAimRequest.SetValue(self, ctp.RequestAimType(CameraTargetParams.AimType.Aura));
            }

            foreach (ParticleSystem part in self.auraParticles)
                if (_particlesEnabled.Value | part.name == "Area")
                {
                    var main = part.main;
                    main.loop = true;
                    part.Play();
                }
        }

        void FrelicGainedEventHandler(On.RoR2.IcicleAuraController.orig_OnIcicleGained orig, IcicleAuraController self)
        {
            foreach (ParticleSystem part in self.procParticles)
                if (_particlesEnabled.Value | part.name == "Area")
                    part.Play();
        }

    }
}
