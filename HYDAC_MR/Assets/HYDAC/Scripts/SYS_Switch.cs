using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

public class SYS_Switch : MonoBehaviour
{
    public string LockTag;

    [SerializeField] private ObjectManipulator manipulator;


    private bool _isLocked;

    private bool _isInOnPosition;
    public bool IsInOnPosition => _isInOnPosition;

    public Action<bool> OnSwitchToggle;


    private void OnEnable()
    {
        manipulator.OnManipulationStarted.AddListener(OnManipulationStarted);
        manipulator.OnManipulationEnded.AddListener(OnManipulationEnded);

        manipulator.enabled = false;
    }


    private void OnDisable()
    {
        manipulator.OnManipulationStarted.RemoveListener(OnManipulationStarted);
        manipulator.OnManipulationEnded.RemoveListener(OnManipulationEnded);
    }


    private void OnManipulationStarted(ManipulationEventData arg0)
    {
        if (_isLocked) return;

    }


    private void OnManipulationEnded(ManipulationEventData arg0)
    {
        if (_isLocked) return;

        if(transform.rotation.z - (-165) <= Mathf.Epsilon)
        {
            Debug.Log("Switch ON");
        }
        else if(transform.rotation.z <= Mathf.Epsilon)
        {
            Debug.Log("Switch OFF");

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(LockTag))
        {
            _isLocked = true;
            manipulator.enabled = false;
            _isInOnPosition = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(LockTag))
        {
            _isLocked = false;
            manipulator.enabled = true;
            _isInOnPosition = true;
        }
    }
}