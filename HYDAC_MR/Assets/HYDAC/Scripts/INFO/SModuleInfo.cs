using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HYDAC.Scripts.INFO
{
    [CreateAssetMenu(fileName = "ModuleInfo", menuName = "AssemblyInfos/Module")]
    public class SModuleInfo : ASInfo
    {
        public bool isViewable;
        
        [SerializeField] private AssetReference lowPolyReference;
        [SerializeField] private AssetReference highPolyReference;
        [SerializeField] private AssetReference imageReference = null;
        [SerializeField] private AssetReference videoReference = null;

        public AssetReference LowPolyReference => lowPolyReference;
        public AssetReference HighPolyReference => highPolyReference;
        public AssetReference ImageReference => imageReference;
        public AssetReference VideoReference => videoReference;

        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "MInfo_" + ID + "_" + iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}