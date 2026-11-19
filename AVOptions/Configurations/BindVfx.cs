using System;
using BepInEx.Configuration;
using UnityEngine;

namespace AVOptions.Configurations;

public class BindVfx : MonoBehaviour
{
    public string configKey;


    void OnEnable()
    {
        gameObject.SetActive(PrefabConfigurer.configEntries[configKey].Value);
    }
}
