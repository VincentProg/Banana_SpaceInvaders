using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    [Header ("Music")]
    [SerializeField] private string music01Play;
    [SerializeField] private string bass01Play;
    [SerializeField] private string bass02Play;
    [SerializeField] private float duration = 1f;

    [Header ("Voices")]
    [SerializeField] private string voice01Keep;
    [SerializeField] private string voice02Nice;
    [SerializeField] private string voice03More;
    [SerializeField] private string voice04Destroy;
    [SerializeField] private string voice05Perfect;
    [SerializeField] private string voiceClose;

    /*
    [SerializeField] private string music01Stop;
    [SerializeField] private string bass01Stop;
    [SerializeField] private string bass02Stop;*/

    private float timer = 0;
    private int currentCombo = -1;
    

    //VOLUME
    private float currentVolStep01 = 0f;
    private float currentVolStep02 = 0f;
    private float currentVolBass01 = 100f;
    private float currentVolBass02 = 0f;

    //LOWPASS
    private float currentLPStep02 = 100f;
    private float currentLPBass01 = 100f;
    private float currentLPBass02 = 100f;

   
    private void OnDisable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent(music01Play, this.gameObject);
        AkSoundEngine.PostEvent(bass01Play, this.gameObject);
        AkSoundEngine.PostEvent(bass02Play, this.gameObject);

        //VOLUME
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step01", 0f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step02", 0f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass_Step03", 100f);
        
        AkSoundEngine.SetRTPCValue("Fade_VolBass02_Step04", 0f);

        //LOWPASS
        AkSoundEngine.SetRTPCValue("Fade_LowPass_Step02", 100f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass_Step03", 100f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass02_Step04", 100f);

    }

    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        SwitchMusicOnCombo();
    }

    #region CURRENT
    private void CurrentVolume()
    {
        //VOLUME
        currentVolStep01 = LerpRTPC(currentVolStep01, 0f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step01", currentVolStep01);

        currentVolStep02 = LerpRTPC(currentVolStep02, 0f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step02", currentVolStep02);

        currentVolBass01 = LerpRTPC(currentVolBass01, 100f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass_Step03", currentVolBass01);

        currentVolBass02 = LerpRTPC(currentVolBass02, 0f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass02_Step04", currentVolBass02);
    }

    private void CurrentLowPass()
    {
        //LOWPASS
        currentLPStep02 = LerpRTPC(currentLPStep02, 100f);
        AkSoundEngine.SetRTPCValue("Fade_LowPass_Step02", currentLPStep02);

        currentLPBass01 = LerpRTPC(currentLPBass01, 100f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass_Step03", currentLPBass01);

        currentLPBass02 = LerpRTPC(currentLPBass02, 100f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass02_Step04", currentLPBass02);
    }
    #endregion

    #region COMBO MUSIC
    public void Combo1() 
    {
        
        currentVolStep01 = LerpRTPC(currentVolStep01, 0f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step01", currentVolStep01);
        
        currentVolStep02 = LerpRTPC(currentVolStep02,100f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step02", currentVolStep02);

    }

    public void Combo2()
    {

        currentVolBass01 = LerpRTPC(currentVolBass01, 100f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass_Step03", currentVolBass01);

        currentLPStep02 = LerpRTPC(currentLPStep02, 0f);
        AkSoundEngine.SetRTPCValue("Fade_LowPass_Step02", currentLPStep02);
    }

    public void Combo3()
    {
        currentLPBass01 = LerpRTPC(currentLPBass01, 0f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass_Step03", currentLPBass01);
    }

    public void Combo4()
    {
        currentVolStep02 = LerpRTPC(currentVolStep02, 100f);
        AkSoundEngine.SetRTPCValue("Fade_Vol_Step02", currentVolStep02);

        currentVolBass02 = LerpRTPC(currentVolBass02, 100f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass02_Step04", currentVolBass02);
    }
     
    public void Combo5() 
    {
        currentVolBass01 = LerpRTPC(currentVolBass01, 0f);
        AkSoundEngine.SetRTPCValue("Fade_VolBass_Step03", currentVolBass01);

        currentLPBass02 = LerpRTPC(currentLPBass02, 0f);
        AkSoundEngine.SetRTPCValue("Fade_LowPassBass02_Step04", currentLPBass02);
    }

    private void ResetCombo()
    {

        CurrentVolume();
        
        CurrentLowPass();
    }
    #endregion

    private void SwitchMusicOnCombo()
    {
        switch (currentCombo)
        {
            case 0: ResetCombo();
                break;
            case 1: Combo1();
                break;
            case 2: Combo2();
                break;
            case 3: Combo3();
                break;
            case 4: Combo4();
                break;
            case 5: Combo5();
                break;
            default:
                return;
        }
    }

    public void SwitchCurrentCombo(int value)
    {
        switch (value)
        {
            case 0:
                currentCombo = 0;
                //AkSoundEngine.PostEvent(voiceClose, this.gameObject);
                break;
            case 1:
                currentCombo = 1;
                AkSoundEngine.PostEvent(voice01Keep, this.gameObject);
                break;
            case 2:
                currentCombo = 2;
                AkSoundEngine.PostEvent(voice02Nice, this.gameObject);
                break;
            case 3:
                currentCombo = 3;
                AkSoundEngine.PostEvent(voice03More, this.gameObject);
                break;
            case 4:
                currentCombo = 4;
                AkSoundEngine.PostEvent(voice04Destroy, this.gameObject);
                break;
            case 5:
                currentCombo = 5;
                AkSoundEngine.PostEvent(voice05Perfect, this.gameObject);
                break;
        }
    }
    public void ResetTimer()
    {
        timer = 0;
    }

    private float LerpRTPC(float a, float b)
    {
        return Mathf.Lerp(a, b, timer / duration);
    }
}
