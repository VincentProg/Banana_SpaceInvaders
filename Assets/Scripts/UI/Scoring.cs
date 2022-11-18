using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using Unity.PlasticSCM.Editor.WebApi;

public class Scoring : MonoBehaviour
{
    [Header("// SCORING //")]
    [SerializeField] TextMeshProUGUI myText;
    [SerializeField] private UnityEvent eventOnAdd;
    [SerializeField] private UnityEvent eventOnAddMonster;
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
    private float currentValueOnDisappear = 0f;
    private bool canDisappear = false;
    private float originSpeedRondJauge;

    [Header(" ")]
    [SerializeField] private float coefJauge;
    [SerializeField] private float speedJauge;
    [SerializeField] private float speedRondJauge;
    [SerializeField] private float destroySpeedRond;

    [SerializeField] private int coefCombo = 5;
    [SerializeField] private float limitComboMax = 10;
    private bool onFullJauge = false;

    [Header(" ")]
    [SerializeField] private UnityEvent bumpCombo;
    [SerializeField] private UnityEvent megaBumpCombo;
    [SerializeField] private UnityEvent newCombo;
    [SerializeField] private UnityEvent shakeJauge;

    // Start is called before the first frame update
    void Start()
    {
        coefRondJauge = (100/limitComboMax)/100;
        Debug.Log(coefRondJauge);

        originSpeedRondJauge = speedRondJauge;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScoring(50);
        }

        if (canDisappear) { RondJaugeDisappear(); }
        

        if (onFullJauge) { FullJaugeAppear(); }
    }


    public void AddScoring(float value)
    {
        currentScore += value;
        eventOnAddMonster.Invoke();
        StartCoroutine(WaitScoring());

        if (currentValueCombo >= limitComboMax)
        {
            ResetJauge();
            CallFeedbackJauge();
            return;
        }

        ComboStrike();

    }

    #region Combo
    private void AddValueCombo()
    {
        canDisappear = true;
        currentValueOnDisappear = 0;

        if (currentValueCombo < limitComboMax)
        {
            currentValueCombo += coefCombo;
            comboTextValue.text = currentValueCombo.ToString();

            CallFeedbackCombo();

            AddRondJauge();
        }
    }
    private void ComboStrike()
    {

        currentValueJauge = jaugeStep.fillAmount;

        if (currentValueJauge < 1)
        {
            currentValueJauge += coefJauge;
            jaugeStep.fillAmount = currentValueJauge;

            CallFeedbackJauge();
        }
        else
        {
            ResetJauge();
            ResetFullJauge();

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

    private void RondJaugeDisappear()
    {
        float current = currentValueRondJauge;
        currentValueOnDisappear += (Time.deltaTime * speedRondJauge);
        float coef = current - currentValueOnDisappear; 

        rondJauge.fillAmount = coef;
        

        if (coef <= 0)
        {
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
        parentJauge.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 1);
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
        canDisappear = false;
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

