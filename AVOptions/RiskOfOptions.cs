using System.Reflection;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace AVOptions
{
    public static class RiskOfOptions
    {
        static readonly bool s_enabled = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public static void AddOption(ConfigEntry<bool> value)
        {
            if (s_enabled) 
                Add(value);
        }
        
        public static void AddSliderOption(ConfigEntry<int> value)
        {
            if (s_enabled) 
                ModSettingsManager.AddOption(new IntSliderOption(value, new IntSliderConfig() { min = 0, max = 100, formatString = "{0}%"}));
        }

        static void Add(ConfigEntry<bool> value)
        {
            ModSettingsManager.AddOption(new CheckBoxOption(value));
        }

        public static void Configure()
        {
            if (!s_enabled)
                return;

            ModSettingsManager.SetModDescription("Enable or disable various item audio/visual effects.");
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AVOptions.resources.icon.png");
            var texture = new Texture2D(0, 0);
            var imgdata = new byte[stream.Length];
            stream.Read(imgdata, 0, imgdata.Length);
            
            if (texture.LoadImage(imgdata))
                ModSettingsManager.SetModIcon(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
            
            
            //Log.Debug($"RiskOfOptions integration enabled = {s_enabled}");
        }

        // RiskOfOptions()
        // {
        //     if (s_enabled) 
        //         Configure();
        //     
        //     Log.Debug($"RiskOfOptions integration enabled = {s_enabled}");
        // }
    }
}
