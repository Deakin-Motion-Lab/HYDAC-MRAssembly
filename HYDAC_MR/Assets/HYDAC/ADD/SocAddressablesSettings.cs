﻿using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HYDAC.Scripts.ADD
{
    [CreateAssetMenu(menuName = "Socks/Settings/Addressables", fileName = "SOC_AddSettings")]
    class SocAddressablesSettings : ScriptableObject
    {
        [Tooltip("List of all the labels of main assets to be loaded at the start of the application")]
        [SerializeField] private string[] List_assets;
        public string[] LoadAssets_OnStart => List_assets;

        [SerializeField] private AssetReference mainScene;
        public AssetReference MainScene => mainScene;

        [Tooltip("List of all the scenes in order they are required to be loaded")]
        [SerializeField] private AssetReference netManager;
        public AssetReference NetManager => netManager;

        [SerializeField] private AssetReference env_Default;
        public AssetReference Env_Default => env_Default;
    }
}