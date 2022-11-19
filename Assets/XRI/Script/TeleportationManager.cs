using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private XRRayInteractor _rayInteractor;
    [SerializeField] private TeleportationProvider _teleportationProvider;

    [SerializeField] private InputActionReference _activate;
    [SerializeField] private InputActionReference _cancel;
    [SerializeField] private InputActionReference _move;
    
    private InputAction _thumbstick;
    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        _activate.action.Enable();
        _activate.action.performed += OnTeleportActivate;

        _cancel.action.Enable();
        _cancel.action.performed += OnTeleportCancel;

        _thumbstick = _move.action;
        _thumbstick.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) 
            return;

        if (_thumbstick.triggered)
            return;

        if (_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            TeleportRequest teleportRequest = new TeleportRequest()
            {
                destinationPosition = hit.point,
            };
        
            _teleportationProvider.QueueTeleportRequest(teleportRequest);
            _isActive = false;
        }
        else
        {
            _isActive = false;
        }
    }

    private void OnTeleportActivate(InputAction.CallbackContext obj)
    {
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext obj)
    {
        _isActive = false;
    }
}
