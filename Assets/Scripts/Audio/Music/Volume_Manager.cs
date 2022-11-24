using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_Manager : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    [SerializeField] private float volMaster = 100f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float volMusic = 80f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float volSfx = 70f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float volUI = 75f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float volVoices = 85f;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetRTPCValue("MASTER", volMaster);
        AkSoundEngine.SetRTPCValue("MUSIC", volMusic);
        AkSoundEngine.SetRTPCValue("SFX", volSfx);
        AkSoundEngine.SetRTPCValue("UI", volUI);
        AkSoundEngine.SetRTPCValue("VOICES", volVoices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
