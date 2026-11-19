using System.Reflection;
using System.Runtime.CompilerServices;
using AVOptions.Configurations;
using BepInEx;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using RoR2;
using FlickerLight = On.RoR2.FlickerLight;

namespace AVOptions
{
    [BepInPlugin("com.thejpaproject.AVFX_Options", "Bobot's AV Effect Options", "3.0.0")]
    [BepInDependency("___riskofthunder.RoR2BepInExPack")]
    [BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags)2)]
    public sealed class AvOptions : BaseUnityPlugin
    {
        //public static PrefabConfigurer _prefabConfigurer;

        public static FlickerConfiguration _flickerConfiguration;
        public static DamageFlashConfiguration _damageFlashConfiguration;
        
        public static BlastShowerConfiguration _blastShowerConfiguration;
        public static FireworkTailConfiguration _fireworkTailConfiguration;
        public static FrelicAvConfiguration _frelicAvConfiguration;
        public static IdpVisualConfiguration _idpVisualConfiguration;
        public static KjaroVisualConfiguration _kjaroVisualConfiguration;
        public static PlasmaShrimpConfiguration _plasmaShrimpConfiguration;
        public static RoseBucklerConfiguration _roseBucklerConfiguration;
        public static RunaldsVisualConfiguration _runaldsVisualConfiguration;
        public static ViendAudioConfiguration _viendAudioConfiguration;
        public static WungusVisualConfiguration _wungusVisualConfiguration;
        public static WungusAudioConfiguration _wungusAudioConfiguration;
        public static BortalConfiguration _bortalConfiguration;
        public static GortalConfiguration _gortalConfiguration;
        public static LittleDiscipleConfiguration _littleDiscipleConfiguration;
        public static BrilliantBehemothConfiguration brilliantBehemothConfiguration;
        
        public static FaradaySpurConfiguration _faradaySpurConfiguration;
        public static FaradaySpurSoundConfiguration _faradaySpurSoundConfiguration;
        
        public static GhostBladeConfiguration _ghostBladeConfiguration;
        public static ShurikenConfiguration _shurikenConfiguration;
        
        public static AntlerConfiguration _antlerConfiguration;
        //public static ChransionConfiguration _chransionConfiguration;

        public static AvOptions Instance;
        
        //sticky bomb, molten perf, wisp, gas, spleen

        [MethodImpl(768)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "RoR2 Mod Lifecycle Method")]
        void Awake()
        {
            Instance = this;

            try
            {
                RiskOfOptions.Configure();
                
                


                brilliantBehemothConfiguration = new BrilliantBehemothConfiguration(Config);
                    
                _blastShowerConfiguration = new BlastShowerConfiguration(Config);
                _bortalConfiguration = new BortalConfiguration(Config);
                PrefabConfigurer.BindVfx("KillEliteFrenzy/NoCooldownEffect", "Brainstalks", "Enables Brainstalks' screen effect. \n\nEffective upon next activation");

                _fireworkTailConfiguration = new FireworkTailConfiguration(Config);
                _frelicAvConfiguration = new FrelicAvConfiguration(Config);

                PrefabConfigurer.BindVfx("IgniteOnKill/IgniteExplosionVFX", "Gasoline", "Enables Gasoline's explosion\n\nEffective immediately");

                _gortalConfiguration = new GortalConfiguration(Config);

                _idpVisualConfiguration = new IdpVisualConfiguration(Config);

                _kjaroVisualConfiguration = new KjaroVisualConfiguration(Config);


                PrefabConfigurer.BindVfx("FireballsOnHit/FireMeatBallGhost", "Molten Perforator Fireballs", "Enables the Molten Perforator fireballs.\n\nEffective immediately");

                _plasmaShrimpConfiguration = new PlasmaShrimpConfiguration(Config);
                PrefabConfigurer.BindVoidVfx("MissileVoid/MissileVoidOrbEffect", "Plasma Shrimp Orbs", "It's the missiles that shoot out.\n\nEffective immediately");
                
                _shurikenConfiguration = new ShurikenConfiguration(Config);
                

                _roseBucklerConfiguration = new RoseBucklerConfiguration(Config);
                _runaldsVisualConfiguration = new RunaldsVisualConfiguration(Config);
                _littleDiscipleConfiguration = new LittleDiscipleConfiguration(Config);

                PrefabConfigurer.BindVfx("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Shatterspleen", "Enables Shatterspleen's explosion.\n\nEffective immediately");
                PrefabConfigurer.BindVfx("Tonic/TonicBuffEffect", "Spinel Tonic", "Enables Spinel Tonic's screen effect. \n\nEffective upon next activation");
                PrefabConfigurer.BindVfx("StickyBomb/StickyBombGhost", "Sticky Bomb Bombs", "Enables Sticky Bomb's bombs.\n\nEffective immediately");
                PrefabConfigurer.BindVfx("StickyBomb/BehemothVFX", "Sticky Bomb Explosion", "Enables Sticky Bomb's explosion.\n\nEffective immediately");



                _antlerConfiguration = new AntlerConfiguration(Config);
                //_chransionConfiguration = new ChransionConfiguration(Config);
                
                _faradaySpurConfiguration = new FaradaySpurConfiguration(Config);
                _faradaySpurSoundConfiguration = new FaradaySpurSoundConfiguration(Config);
                _ghostBladeConfiguration = new GhostBladeConfiguration(Config);
                

                _flickerConfiguration = new FlickerConfiguration(Config);
                _damageFlashConfiguration = new DamageFlashConfiguration(Config);


                PrefabConfigurer.BindAsset(Category.UNIT_EFFECTS, "Base/Titan/TitanDeathEffect", "Titan Death", "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.");

                _viendAudioConfiguration = new ViendAudioConfiguration(Config);

                PrefabConfigurer.BindAsset(Category.UNIT_EFFECTS, "Base/Vagrant/VagrantDeathExplosion", "Wandering Vagrant Death", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.");
                PrefabConfigurer.BindVfx("ExplodeOnDeath/WilloWispExplosion", "Will-o-the-Wisp", "Enables Will o' the Wisp's explosion.\n\nEffective immediately");
                _wungusVisualConfiguration = new WungusVisualConfiguration(Config);
                _wungusAudioConfiguration = new WungusAudioConfiguration(Config);

            }
            catch (ConfigurationException e)
            {
                Logger.LogError(e);
                Logger.LogError($"Failed to register configuration for {e.Message}\r{e.StackTrace}");
            }

            Logger.LogInfo("Finished!");

        }

    }
}