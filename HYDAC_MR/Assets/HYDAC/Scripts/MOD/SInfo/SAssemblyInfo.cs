using System;

namespace HYDAC.Scripts.MOD.SInfo
{
    public class SAssemblyInfo : ASInfo
    {
        //public IAssemblyModule[] AssemblyModules => assemblyModules;
        private SModuleInfo[] _modules = null;
        public SModuleInfo[] Modules => _modules;


        private void Awake()
        {
            _modules = null;
        }


        internal void SetModules(SModuleInfo[] foundModules)
        {
            _modules = foundModules;
        }
        
        
        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "AInfo_" + ID + "_" + iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}
