using HYDAC.Scripts.MOD;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

using HYDAC.Scripts.MOD.SInfo;
using HYDAC.SOCS;
using HYDAC.SOCS.NET;

namespace HYDAC.Scripts
{
    public class AddressableManager : MonoBehaviour
    {
        [SerializeField] private SocNetEvents netEvents = null;
        [SerializeField] private SocAssemblyEvents assemblyEvents = null;
        
        [SerializeField] private AssetReference modelPrefabRefToLoad;
        [SerializeField] private Transform machineWorldTransform;
        [SerializeField] private Transform focusedModuleHolderTransform = null;

        private bool _isNetworkScene = false;
        private bool _isInitialised = false;
        
        private GameObject _loadedModel = null;
        
        private AssetReference _currentModuleReference = null;
        private Transform _currentModuleTransform = null;

        private void Awake()
        {
            Addressables.InitializeAsync().Completed += OnAddressablesInitialised;

            _isNetworkScene = SceneManager.GetActiveScene().buildIndex == 1;
            
            if (!_isNetworkScene) return;
            
            netEvents.ELocalUserReady += OnLocalUserReady;
        }
        
        
        private void OnAddressablesInitialised(AsyncOperationHandle<IResourceLocator> obj)
        {
            Debug.Log("#AddressableManager#-------------Initialised");

            _isInitialised = true;
            
            if (_isNetworkScene)
            { 
                LoadModel();  
            }
        }


        private void OnLocalUserReady(Transform playerTransform)
        {
            LoadModel();
            
            netEvents.ELocalUserReady -= OnLocalUserReady;
        }
        
        
        
        private void OnEnable()
        {
            assemblyEvents.ECurrentModuleChange += OnCurrentModuleChange;
        }

        private void OnDisable()
        {
            assemblyEvents.ECurrentModuleChange -= OnCurrentModuleChange;
        }

        private void OnCurrentModuleChange(SModuleInfo moduleInfo)
        {
            if (!_isInitialised) return;

            Debug.Log("#AddressableManager#-------------Module Changed");
            
            if (_currentModuleTransform != null)
            {
                _currentModuleReference.ReleaseInstance(_currentModuleTransform.gameObject);
            }
            
            _currentModuleReference = moduleInfo.HighPolyReference;
            
            _currentModuleReference.InstantiateAsync(focusedModuleHolderTransform).Completed += (handle) =>
            {
                Debug.Log("#AddressableManager#-------------Module Instantiated");
                
                _currentModuleTransform = handle.Result.transform;
                _currentModuleTransform.position = focusedModuleHolderTransform.position;
                _currentModuleTransform.rotation = focusedModuleHolderTransform.rotation;
            };
        }

        private void LoadModel()
        {
            modelPrefabRefToLoad.InstantiateAsync().Completed += (loadedAsset) =>
            {
                Debug.Log("#AddressableManager#-------------Model Instantiated");
                
                _loadedModel = loadedAsset.Result;
                _loadedModel.transform.position = machineWorldTransform.position;
                _loadedModel.transform.rotation = machineWorldTransform.rotation;

                assemblyEvents.OnModelLoaded(_loadedModel.GetComponent<BaseAssembly>().Info as SAssemblyInfo);
            };
        }

        private void OnDestroy()
        {
            modelPrefabRefToLoad.ReleaseInstance(_loadedModel);
            
            Addressables.InitializeAsync().Completed -= OnAddressablesInitialised;
        }
    }
}
