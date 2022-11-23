using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestHaptics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBothInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Haptics.instance.HapticVR(0u, 0.4f, 1f);
    }

    public void OnRightHandInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Haptics.instance.HapticVRRight(0u, 1f, 0.1f);
    }

    public void OnLeftHandInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Haptics.instance.HapticVRLeft(0u, 0.4f, 0.1f);
    }
}
