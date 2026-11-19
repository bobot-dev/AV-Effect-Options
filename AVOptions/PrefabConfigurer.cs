using System;
using System.Collections.Generic;
using AVOptions.Configurations;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AVOptions
{
    public static class PrefabConfigurer
    {
        public static void BindVfx(string assetPath, string title, string description) => BindAsset(Category.BASE_VFX, "Base/" + assetPath, title, description);
        public static void BindSfx(string assetPath, string title, string description) => BindAsset(Category.BASE_SFX, "Base/" + assetPath, title, description);
        public static void BindVoidVfx(string assetPath, string title, string description) => BindAsset(Category.VOID_VFX, "DLC1/" + assetPath, title, description);
        public static void BindVoidSfx(string assetPath, string title, string description) => BindAsset(Category.VOID_SFX, "DLC1/" + assetPath, title, description);
        
        public static void BindACVfx(string assetPath, string title, string description) => BindAsset(Category.AC_VFX, "DLC3/" + assetPath, title, description);
        public static void BindACSfx(string assetPath, string title, string description) => BindAsset(Category.AC_SFX, "DLC3/" + assetPath, title, description);
        
        public static void BindHCVfx(string assetPath, string title, string description) => BindAsset(Category.HC_VFX, "DLC4/" + assetPath, title, description);
        public static void BindHCSfx(string assetPath, string title, string description) => BindAsset(Category.HC_SFX, "DLC4/" + assetPath, title, description);

        public static Dictionary<string, ConfigEntry<bool>> configEntries = new Dictionary<string, ConfigEntry<bool>>();
        
        
        public static void BindAsset(string category, string assetPath, string title, string description)
        {
            try
            {
                var prefab = Addressables.LoadAsset<GameObject>("RoR2/" + assetPath + ".prefab").WaitForCompletion();
                var config = AvOptions.Instance.Config.Bind(category, title, true, description);
                //config.SettingChanged += (x, _) => prefab.SetActive(((ConfigEntry<bool>)x).Value);
                //prefab.SetActive(config.Value);

                configEntries.Add(title, config);
                
                prefab.AddComponent<BindVfx>().configKey = title;
                
                RiskOfOptions.AddOption(config);
            }
            catch (Exception e)
            {
                throw new ConfigurationException(assetPath, e);
            }
        }

    }
}