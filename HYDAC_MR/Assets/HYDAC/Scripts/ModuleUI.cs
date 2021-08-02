using System;
using HYDAC.Scripts.MOD;
using UnityEngine;

using HYDAC.SOCS;
using TMPro;

public class ModuleUI : MonoBehaviour
{
    [SerializeField] private SocModuleUI socUI = null;
    [SerializeField] private SocAssemblyEvents assemblyEvents = null;
    
    [Space] [Header("Transforms")]
    [SerializeField] private Transform moduleSpawnTransform;
    [SerializeField] private Transform moduleUITransform;
    [SerializeField] private Transform backboardUITransform;

    [Space] [Header("Info UI")] 
    [SerializeField] private TextMeshPro nameText = null;

    private Vector3 moduleUIStartPosition;
    private Vector3 backboardUIStartPosition;
    
    private Quaternion moduleUIStartRotation;
    private Quaternion backboardUIStartRotation;
    
    private Vector3 moduleUIStartScale;
    private Vector3 backboardUIStartScale;

    private GameObject currentModule = null;
    
    private void Awake()
    {
        moduleUIStartPosition = moduleUITransform.position;
        moduleUIStartRotation = moduleUITransform.rotation;
        moduleUIStartScale = moduleUITransform.localScale;
        
        backboardUIStartPosition = backboardUITransform.position;
        backboardUIStartRotation = backboardUITransform.rotation;
        backboardUIStartScale = backboardUITransform.localScale;
    }

    private void OnEnable()
    {
        assemblyEvents.ECurrentModuleChange += OnCurrentModuleChanged;
        
        socUI.EUIRequestFocusOff += OnUIRequestFocusOff;
        socUI.EUIRequestToggleInfoUI += OnUIRequestToggleInfoUI;
    }

    private void OnDisable()
    {
        assemblyEvents.ECurrentModuleChange -= OnCurrentModuleChanged;
        
        socUI.EUIRequestFocusOff -= OnUIRequestFocusOff;
        socUI.EUIRequestToggleInfoUI -= OnUIRequestToggleInfoUI;
    }


    private void OnCurrentModuleChanged(SModuleInfo newModule)
    {
        if (currentModule == null)
        {
            currentModule = Instantiate(newModule.prefab, moduleSpawnTransform);
            nameText.text = newModule.iname;
        }
        else
        {
            GameObject temp = currentModule;
            currentModule = null;
            Destroy(temp);
        }
    }

    private void OnUIRequestToggleInfoUI()
    {
        throw new NotImplementedException();
    }

    private void OnUIRequestFocusOff()
    {
        throw new NotImplementedException();
    }
    
    private void Reset()
    {
        // Delete Module
        // Reset UI
    }
}
