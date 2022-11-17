using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Animator))]
public class HandAnimationController : MonoBehaviour
{

    public InputDeviceCharacteristics controllerType;
    
    private Animator _animationController;
    private InputDevice _controller;
    private bool _isControllerFound;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animationController = GetComponent<Animator>();
        Initialize();
    }

    private void Initialize()
    {
        List<InputDevice> xrDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerType, xrDevices);

        if (xrDevices.Count == 0)
        {
            Debug.LogWarning($"No XR Devices found !");
        }
        else
        {
            _controller = xrDevices[0];
            _isControllerFound = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isControllerFound)
        {
            Initialize();
        }
        else
        {
            if (_controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                _animationController.SetFloat("Trigger", triggerValue);
            }
            
            if (_controller.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                _animationController.SetFloat("Grip", gripValue);
            }
        }
    }
}
