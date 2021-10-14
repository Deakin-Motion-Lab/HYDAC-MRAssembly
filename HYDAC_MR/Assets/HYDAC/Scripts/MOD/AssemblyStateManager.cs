using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

using Photon.Pun;

using HYDAC.Scripts.ADD;
using HYDAC.Scripts.INFO;

namespace HYDAC.Scripts.MOD
{
    public class AssemblyStateManager : MonoBehaviour
    {
        [SerializeField] private SocAssemblyEvents assemblyEvents;
        [SerializeField] private SocAssemblyUI assemblyUI;

        [SerializeField] private SocAssemblySettings settings;

        private PhotonView _photonView;
        private bool _isMasterClient;

        IAssembly _currentAssembly;
        private IList<IResourceLocation> _assemblyAssetsLocations = new List<IResourceLocation>();

        private SModuleInfo _currentSelectedModule;
        private SSubModuleInfo _currentSelectedSubModule;

        private void Awake()
        {
            _isMasterClient = PhotonNetwork.IsMasterClient;
            _photonView = GetComponent<PhotonView>();

            // This scenario if the master client had already loaded the model in the local scene
            if (PhotonNetwork.IsMasterClient && assemblyEvents.IsInitialised)
            {
                int currentAssemblyID = assemblyEvents.CurrentCatalogue.ID;
                _photonView.RPC("OnAssemblySelectedRPC", RpcTarget.AllBuffered, new object[] { currentAssemblyID });

                // Disable catalogue UI
                assemblyUI.InvokeToggleCatalogueUI(false);
            }
            else
            {
                // Enable catalogue UI
                assemblyUI.InvokeToggleCatalogueUI(true);
            }

            Debug.Log("SOCAssemblyUI: " + assemblyUI.GetInstanceID());
            Debug.Log("SOCAssemblyEvents: " + assemblyEvents.GetInstanceID());
        }

        private void OnEnable()
        {
            assemblyUI.EUIRequestAssemblySelect += OnUIAssemblySelect;
            assemblyUI.EUIRequestModuleExplode += OnUIExplodeToggle;

            assemblyUI.EUIRequestSubModuleSelect += OnSubmoduleSelect;
        }



        private void OnDisable()
        {
            assemblyUI.EUIRequestAssemblySelect -= OnUIAssemblySelect;
            assemblyUI.EUIRequestModuleExplode -= OnUIExplodeToggle;

            assemblyUI.EUIRequestSubModuleSelect -= OnSubmoduleSelect;
        }


        #region Module Functions

        private void OnModuleSelect(SModuleInfo modInfo)
        {
            if (_photonView.IsMine)
            {
                Debug.Log("#AssemblyStateManager#---------RPC RAISING - Module Selected: " + modInfo.ID);

                _photonView.RPC("OnModuleSelectRPC", RpcTarget.All, new object[] { modInfo.ID });
            }
        }
        [PunRPC]
        void OnModuleSelectRPC(int modID)
        {
            SModuleInfo selectedModule = new SModuleInfo();

            // Look up module info
            SModuleInfo[] modInfos = assemblyEvents.Modules;
            foreach(var module in modInfos)
            {
                if (modID == module.ID)
                {
                    selectedModule = module;
                    _currentSelectedModule = selectedModule;
                }
            }

            Debug.Log("#AssemblyStateManager#---------RPC RECEIVED - Module Selected: " + _currentSelectedModule.iname);

            assemblyEvents.OnModuleSelected(_currentSelectedModule);
        }


        private void OnUIExplodeToggle(bool toggle)
        {
            Debug.Log("#AssemblyStateManager#---------RPC RAISING - Module Explode: " + toggle);

            _photonView.RPC("OnModuleExplodeRPC", RpcTarget.All, new object[] { toggle });
        }
        [PunRPC]
        void OnModuleExplodeRPC(bool toggle)
        {
            Debug.Log("#AssemblyStateManager#---------RPC RECEIVED - Module Explode: " + toggle);

            assemblyEvents.OnModuleExplode(toggle);
        }


        private void OnSubmoduleSelect(SSubModuleInfo subModInfo)
        {
            Debug.Log("#AssemblyStateManager#---------RPC RAISING - Sub Module Select: " + subModInfo.ID);

            _photonView.RPC("OnSubModuleSelectRPC", RpcTarget.All, new object[] { subModInfo.ID });
        }
        [PunRPC]
        void OnSubModuleSelectRPC(int subModID)
        {
            SSubModuleInfo selectedSubModule = new SSubModuleInfo();

            // Look up module info
            SSubModuleInfo[] subModInfos = assemblyEvents.CurrentModule.SubModules;
            foreach (var module in subModInfos)
            {
                if (subModID == module.ID)
                {
                    selectedSubModule = module;
                    _currentSelectedSubModule = selectedSubModule;
                }
            }

            Debug.Log("#AssemblyStateManager#---------RPC RECEIVED - Sub Module Select: " + _currentSelectedSubModule.ID);

            assemblyEvents.OnSubModuleSelected(_currentSelectedSubModule);
        }
        #endregion


        private void OnUIAssemblySelect(SCatalogueInfo assemblyInfo)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("#AssemblyStateManager#---------RPC RAISING - Assembly Selected: " + assemblyInfo.ID);

                _photonView.RPC("OnAssemblySelectedRPC", RpcTarget.AllBuffered, new object[] { assemblyInfo.ID });
            }
        }
        [PunRPC]
        void OnAssemblySelectedRPC(int assemblyID)
        {
            Debug.Log("#AssemblyStateManager#---------RPC RECEIVED - Assembly Selected: " + assemblyID);

            foreach (var catalogueEntry in assemblyEvents.Catalogue)
            {
                if (catalogueEntry.ID == assemblyID)
                {
                    LoadAssemblyAssets(new string[] { catalogueEntry.AssemblyFolderKey }, catalogueEntry.AssemblyPrefab);
                }

                // Disable catalogue UI
                assemblyUI.InvokeToggleCatalogueUI(false);
            }
        }

        private async Task LoadAssemblyAssets(string[] label, AssetReference assemblyPrefab)
        {
            // Load assembly dependencies
            await AddressableLoader.LoadLabels(label, _assemblyAssetsLocations);

            // Instantiate Assembly
            var result = await Addressables.InstantiateAsync(assemblyPrefab, transform.position, transform.rotation).Task;
            _currentAssembly = result.GetComponent<IAssembly>();

            // Get Modules of Assembly
            var assemblyModules = _currentAssembly.GetAssemblyModules();

            SModuleInfo[] moduleInfos = new SModuleInfo[assemblyModules.Length];

            for (int i = 0; i < assemblyModules.Length; i++)
            {
                // Register for the module OnClick event
                assemblyModules[i].EOnClicked += OnModuleSelect;

                moduleInfos[i] = (SModuleInfo)assemblyModules[i].Info;

                // Set module infos in AssemblyEvents sock
                assemblyEvents.Modules = moduleInfos;
            }

            result.GetComponent<PrefabLightmapData>().Initialize();
        }
    }
}