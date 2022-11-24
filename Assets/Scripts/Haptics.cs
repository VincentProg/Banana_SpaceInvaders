using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class Haptics : MonoBehaviour
{
    public static Haptics instance;

    private Gamepad gamepad ;
    static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        if(Gamepad.current != null)
            gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HapticsGameController(Vector2 amplitude, float duration)
    {
        if(gamepad!=null)
            StartCoroutine(ReturnHaptics(amplitude, duration));
    }
    private void StopRumble()
    {
        if (gamepad != null)
        {
            gamepad.ResetHaptics();
        }
    }
    IEnumerator ReturnHaptics(Vector2 amplitude,float duration)
    {
        count++;
        gamepad.SetMotorSpeeds(amplitude.x, amplitude.y);
        yield return new WaitForSeconds(duration);
        count--;
        if(count == 0)
            StopRumble();
    }

    public void HapticVRRight(uint channel, float amplitude, float duration)
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, devices);

        foreach (var device in devices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {

                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }

    public void HapticVRLeft(uint channel, float amplitude, float duration)
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, devices);

        foreach (var device in devices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {

                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }

    public void HapticVR(uint channel, float amplitude, float duration)
    {
        HapticVRLeft(channel, amplitude, duration);
        HapticVRRight(channel, amplitude, duration);
    }
}
