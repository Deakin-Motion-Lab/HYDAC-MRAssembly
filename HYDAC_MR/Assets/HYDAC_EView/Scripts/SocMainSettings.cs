﻿using System.Collections.Generic;
using HYDAC_EView.Scripts.MPart;
using UnityEngine;

namespace HYDAC_EView.Scripts
{
    [CreateAssetMenu(fileName = "MainSettings", menuName = "SOCKS/MainSettings", order = 0)]
    public class SocMainSettings : ScriptableObject
    {
        [Range(0.1f, 2.0f)]
        public float positionTimeChange = 5.0f;

        public Material previousAssemblyMaterial;
        public Material currentAssemblyMaterial;
        public Material nextAssemblyMaterial;

        public List<SocMachinePartInfo> machineParts = new List<SocMachinePartInfo>();
    }
}