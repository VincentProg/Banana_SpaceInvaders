using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HapticsGameController(Vector2 amplitude, float duration)
    {
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
}
