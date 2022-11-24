using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.SceneManagement;

public class Scoring : MonoBehaviour
{
    [Header("// SCORING //")]
    [SerializeField] TextMeshProUGUI myText;
    [SerializeField] private UnityEvent eventOnAdd;
    [SerializeField] private float currentScore;
    [SerializeField] private float durationAppearScore;

    [Header("// JAUGE COMBO //")]
    [SerializeField] private GameObject parentJauge;
    [SerializeField] private Image jaugeStep;
    [SerializeField] private Image fullJauge;
    [SerializeField] private Image rondJauge;
    [SerializeField] private GameObject fakeJauge;
    [SerializeField] private TextMeshProUGUI comboTextValue;
    private float currentValueJauge;
    private float currentValueFullJauge;
    private int currentValueCombo = 0;

    private float coefRondJauge;
    private float currentValueRondJauge;
    private float currentValueOnDisappearJauge = 0f;
    private float currentValueOnDisappearRond = 0f;
    
    private float originSpeedRondJauge;

    private bool canDisappearJauge = false;
    private bool canDisappearRond = false;

    [Header(" ")]
    [SerializeField] private float coefJauge;
    [SerializeField] private float speedJauge;
    [SerializeField] private float speedJaugeDisappear;
    [SerializeField] private float speedRondJauge;
    [SerializeField] private float destroySpeedRond;

    [SerializeField] private int coefCombo = 5;
    [SerializeField] private float limitComboMax = 10;
    private bool onFullJauge = false;

    [Header("// AUDIO //")]
    [SerializeField] private string[] hitSound;
    [SerializeField] private string[] comboSound;
    [SerializeField] private string deathMonster;

    [Header("// EVENTS AUDIO //")]
    [SerializeField] private UnityEvent resetTimer;
    [SerializeField] private UnityEvent combo1;
    [SerializeField] private UnityEvent combo2;
    [SerializeField] private UnityEvent combo3;
    [SerializeField] private UnityEvent combo4;
    [SerializeField] private UnityEvent combo5;
    [SerializeField] private UnityEvent resetCombo;

    [Header("PARTICLES")]
    /*
    [SerializeField] private GameObject myObjPart;
    [SerializeField] private ParticleSystem myParticles;*/

    [Header(" ")]
    [Header("// EVENTS ANIMATIONS //")]
    [SerializeField] private UnityEvent bumpCombo;
    [SerializeField] private UnityEvent megaBumpCombo;
    [SerializeField] private UnityEvent newCombo;
    [SerializeField] private UnityEvent shakeJauge;


    float width;
    Vector3 tempV;

    // Start is called before the first frame update
    void Start()
    {
        coefRondJauge = (100/limitComboMax)/100;

        originSpeedRondJauge = speedRondJauge;

        width = jaugeStep.GetComponent<RectTransform>().rect.width;
        tempV = jaugeStep.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {

        /*
        tempV.x = -width / 2;
        tempV.x += width * jaugeStep.fillAmount;
        jaugeStep.GetComponent<RectTransform>().anchoredPosition = tempV;

        myObjPart.transform.position = new Vector3(tempV.x,-100,0);*/

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //AddScoring(50);
            SceneManager.LoadScene("Lose");
        }
        

        if (canDisappearRond) { RondJaugeDisappear(); }

        if (onFullJauge) { FullJaugeAppear(); }

        if (canDisappearJauge) { JaugeDisappear(); }
        else { currentValueOnDisappearJauge = jaugeStep.fillAmount; }

    }


    public void AddScoring(float value)
    {
        Debug.Log("MORT");

        currentScore += value;
        StartCoroutine(WaitScoring());

        //AkSoundEngine.PostEvent(hitSound[0], this.gameObject);
        AkSoundEngine.PostEvent(deathMonster, this.gameObject);

        parentJauge.GetComponent<RectTransform>().rotation = new Quaternion(0,0,0,0);

        if (currentValueCombo >= limitComboMax)
        {
            ResetJauge();
            CallFeedbackJauge();
            canDisappearJauge = false;
            return;
        }

        ComboStrike();
        
    }

    #region Combo
    private void AddValueCombo()
    {
        canDisappearRond = true;
        currentValueOnDisappearRond = 0;

        if (currentValueCombo < limitComboMax)
        {
            currentValueCombo += coefCombo;
            comboTextValue.text = currentValueCombo.ToString();

            AkSoundEngine.PostEvent(hitSound[1], this.gameObject);

            CallFeedbackCombo();
            AddRondJauge();

            //MUSIC
            resetTimer.Invoke();
            switch (currentValueCombo)
            {
                case 1: combo1.Invoke();
                    AkSoundEngine.PostEvent(comboSound[0], this.gameObject);
                    break;
                case 2: combo2.Invoke();
                    AkSoundEngine.PostEvent(comboSound[1], this.gameObject);
                    break;
                case 3: combo3.Invoke();
                    AkSoundEngine.PostEvent(comboSound[2], this.gameObject);
                    break;
                case 4: combo4.Invoke();
                    AkSoundEngine.PostEvent(comboSound[3], this.gameObject);
                    break;
                case 5: combo5.Invoke();
                    AkSoundEngine.PostEvent(comboSound[4], this.gameObject);
                    break;
            }
        }
    }
    private void ComboStrike()
    {
        if (currentValueJauge < 1)
        {
            if (jaugeStep.fillAmount <= 0f)
            {
                currentValueJauge = 0f;
            }
            else if (jaugeStep.fillAmount <= 0.2f)
            {
                currentValueJauge = 0.2f;
            }
            else if (jaugeStep.fillAmount <= 0.4f)
            {
                currentValueJauge = 0.4f;
            }
            else if (jaugeStep.fillAmount <= 0.6f)
            {
                currentValueJauge = 0.6f;
            }
            else if (jaugeStep.fillAmount <= 0.8f)
            {
                currentValueJauge = 0.8f;
            }

            currentValueJauge += coefJauge;

            currentValueOnDisappearJauge = 0;

            jaugeStep.fillAmount = currentValueJauge;
            canDisappearJauge = true;

            CallFeedbackJauge();
        }
        else
        {
            ResetJauge();
            ResetFullJauge();

            canDisappearJauge = false;

            onFullJauge = true;
        }
    }

    private void AddRondJauge()
    {
        currentValueRondJauge += coefRondJauge;
        rondJauge.fillAmount = currentValueRondJauge;
    }
    #endregion

    #region Jauges
    private void FullJaugeAppear()
    {
        float coef = Mathf.Lerp(0, 1, currentValueFullJauge += (Time.deltaTime * speedJauge));
        fullJauge.fillAmount = coef;

        fakeJauge.gameObject.SetActive(true);

        if (coef >= 1)
        {
            fakeJauge.gameObject.SetActive(false);

            ResetFullJauge();
            AddValueCombo();
            

            if (currentValueCombo != limitComboMax)
            {
                fakeJauge.gameObject.SetActive(false);
            }
            else
            {
                fakeJauge.gameObject.SetActive(true);
                speedRondJauge = destroySpeedRond;
            }
        }
    }

    private void JaugeDisappear()
    {
        if(currentValueCombo >= 5) { return; }
        
        float current = currentValueJauge;
       
        currentValueOnDisappearJauge += (Time.deltaTime * speedJaugeDisappear);
        float coef = current - currentValueOnDisappearJauge;

        jaugeStep.fillAmount = coef;

        if (coef <= 0)
        {
            currentValueOnDisappearJauge = 0;

            currentValueFullJauge = 0;
            fullJauge.fillAmount = 0;

            
            currentValueCombo = 0;
            comboTextValue.text = "0";

            currentValueJauge = 0;
            jaugeStep.fillAmount = 0;

            Debug.Log("Jauge");
            ResetRondJauge();

            resetTimer.Invoke();
            resetCombo.Invoke();

            canDisappearJauge = false;
        }

    }

    private void RondJaugeDisappear()
    {
        float current = currentValueRondJauge;
        currentValueOnDisappearRond += (Time.deltaTime * speedRondJauge);
        float coef = current - currentValueOnDisappearRond; 

        rondJauge.fillAmount = coef;
        
        if (coef <= 0)
        {
            Debug.Log("Rond");
            ResetCombo();
            ResetRondJauge();
        }
    }
    #endregion

    #region Feedbacks
    private void CallFeedbackJauge()
    {
        if (!onFullJauge)
        {
            bumpCombo.Invoke();
            shakeJauge.Invoke();
        }
    }

    private void CallFeedbackCombo()
    {
        megaBumpCombo.Invoke();

        newCombo.Invoke();
    }
    #endregion

    #region Reset
    private void ResetCombo()
    {
        currentValueCombo = 0;
        comboTextValue.text = "0";
        fakeJauge.gameObject.SetActive(false);

        resetTimer.Invoke();
        resetCombo.Invoke();
    }

    private void ResetFullJauge()
    {
        currentValueFullJauge = 0;
        fullJauge.fillAmount = 0;
        onFullJauge = false;
    }

    private void ResetJauge()
    {
        currentValueJauge = 0;
        jaugeStep.fillAmount = 0;
        onFullJauge = false;
    }

    private void ResetRondJauge()
    {
        currentValueRondJauge = 0;
        rondJauge.fillAmount = 0;
        canDisappearRond = false;
        speedRondJauge = originSpeedRondJauge;
    }
    #endregion

    IEnumerator WaitScoring()
    {
        yield return new WaitForSeconds(durationAppearScore);
        myText.text = currentScore.ToString();
        eventOnAdd.Invoke();
    }
}

