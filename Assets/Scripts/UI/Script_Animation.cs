using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Animation : MonoBehaviour
{
    private enum myEnum { ZeroToOne, OneToZero, PingPong };
    [SerializeField] private myEnum enumAnim;

    [Header("REFERENCE")]
    [SerializeField] private GameObject myObj;

    private RectTransform rectTransformObj;
    private Transform transformObj;
    private Renderer matObj;
    private Image imageObj;
    private TextMeshProUGUI textObj;
    private bool textIsUsed = false;

    [Header("ANIMATION")]
    [SerializeField] private float beginDuration = 0f;
    [SerializeField] private float duration = 0f;
    private float coef = 0f;
    private float coefEasing = 0f;
    private bool etat = false;
    [SerializeField] private int numberOfLoops = 1;
    private int currentLoop = 0;
    private int ppValue = 0;
    private float ppState = 2f;
    
    private enum myEnumEasing { Linear,
        QuadraticIn, QuadraticOut, QuadraticInOut,
        CubicIn, CubicOut, CubicInOut,
        QuarticIn, QuarticOut, QuarticInOut,
        QuinticIn, QuinticOut, QuinticInOut,
        SinusoidalIn, SinusoidalOut, SinusoidalInOut,
        ExponentialIn, ExponentialOut, ExponentialInOut,
        CircularIn, CircularOut, CircularInOut,
        ElasticIn, ElasticOut, ElasticInOut,
        BackIn, BackOut, BackInOut,
        BounceIn, BounceOut, BounceInOut,
        AnimationCurve
    };

    [Header("TYPE")]
    [SerializeField] private myEnumEasing enumEasing;
    [SerializeField] private AnimationCurve myCurve;
    private int iterationEasing = 0;
    [Header("Quadratic => Max = 0.99, Min = 0.01")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float lerpLimitMax = 0.99f;
    [Range(0.0f, 1f)]
    [SerializeField] private float lerpLimitMin = 0.01f;

    private enum myEnumType { Position, Rotation, Scale, Color };
    [Header("TYPE")]
    [SerializeField] private myEnumType enumType;

    [Header("Transform")]
    [SerializeField] private Vector3 vecEnd;
    private Vector3 initPositionObj = Vector3.zero;
    private Vector3 initRotationObj = Vector3.zero;
    private Vector3 initScaleObj = Vector3.one;

    [Header("Color")]
    [SerializeField] private Color colorStart;
    [SerializeField] private Color colorEnd;
    private bool in3D = false;

    [Header("Debug")]
    [SerializeField] private Text textDebug;

    // Start is called before the first frame update
    void Start()
    {
        InitComponentTransform();
        InitComponentRenderer();
        ChooseEasing();
    }

    // Update is called once per frame 
    void Update()
    {
        if (!etat)
        {
            coef = 0f;
            coefEasing = 0f;
            currentLoop = 0;
            //print("does not' working");
            return;
        }
        
        SwitchBelongType();
    }
    //ANIM /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Zero To One
    //ZERO TO ONE
    private void ZeroToOnePosition()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(0, 1, Timer());

        if (in3D == true)
        {
            transformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefZ);
        }
        else
        {
            rectTransformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefZ);
        }
        
        if (coefZ >= lerpLimitMax) { ValuesOnEnd(); };
    }
    private void ZeroToOneRotation()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(0, 1, Timer());
       
        if(in3D == true)
        {
            Quaternion qStart = Quaternion.Euler(initRotationObj);
            Quaternion qEnd = Quaternion.Euler(vecEnd);

            transformObj.localRotation = Quaternion.Lerp(qStart, qEnd, coefZ);
        }
        else
        {
            Quaternion qStart = Quaternion.Euler(initRotationObj);
            Quaternion qEnd = Quaternion.Euler(vecEnd);

            rectTransformObj.localRotation = Quaternion.Lerp(qStart, qEnd, coefZ);
        }

        if (coefZ >= lerpLimitMax) { ValuesOnEnd(); }
    }
    private void ZeroToOneScale()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(0, 1, Timer());

        if(in3D == true)
        {
            transformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefZ);
        }
        else
        {
            rectTransformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefZ);
        }
        
        if (coefZ >= lerpLimitMax) { ValuesOnEnd(); Debug.Log("eee"); }
    }
    private void ZeroToOneColor()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(0, 1, Timer());

        if(in3D == true)
        {
            Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefZ);
            matObj.material.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
        }
        else
        {
            if (textIsUsed)
            {
                Vector4 newColor = Vector4.Lerp(colorStart, colorEnd, coefZ);
                textObj.color = new Vector4(newColor.x, newColor.y, newColor.z, newColor.w);
            }
            else
            {
                Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefZ);
                imageObj.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
            }
            
        }
        
        if (coefZ >= lerpLimitMax) { ValuesOnEnd(); }
    }
    #endregion

    #region One To Zero

    //ONE TO ZERO
    private void OneToZeroPosition()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(1, 0, Timer());

        if (in3D == true)
        {
            transformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefZ);
        }
        else
        {
            rectTransformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefZ);
        }

        if (coefZ <= lerpLimitMin) { ValuesOnEnd(); }
    }
    private void OneToZeroRotation()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(1, 0, Timer());

        if (in3D == true)
        {
            Quaternion qStart = Quaternion.Euler(initRotationObj);
            Quaternion qEnd = Quaternion.Euler(vecEnd);

            transformObj.localRotation = Quaternion.Lerp(qStart, qEnd, coefZ);
        }
        else
        {
            Quaternion qStart = Quaternion.Euler(initRotationObj);
            Quaternion qEnd = Quaternion.Euler(vecEnd);

            rectTransformObj.localRotation = Quaternion.Lerp(qStart, qEnd, coefZ);
        }

        if (coefZ <= lerpLimitMin) { ValuesOnEnd(); }
    }
    private void OneToZeroScale()
    {
        Loop();
        if (etat == false) { return; }

        float coefZ = Mathf.Lerp(1, 0, Timer());

        if (in3D == true)
        {
            transformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefZ);
        }
        else
        {
            rectTransformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefZ);
        }

        if (coefZ <= lerpLimitMin) { ValuesOnEnd(); }

    }
    private void OneToZeroColor()
    {
        Loop();
        if(etat == false) { return; }
        
        float coefZ = Mathf.Lerp(1, 0, Timer());

        if (in3D == true)
        {
            Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefZ);
            matObj.material.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
        }
        else
        {
            if (textIsUsed)
            {
                Vector4 newColor = Vector4.Lerp(colorStart, colorEnd, coefZ);
                textObj.color = new Vector4(newColor.x, newColor.y, newColor.z, newColor.w);
            }
            else
            {
                Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefZ);
                imageObj.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
            }
            
        }

        if (coefZ <= lerpLimitMin) { ValuesOnEnd(); }
    }


    #endregion

    #region PingPong

    //PING PONG
    private void PingPongPosition(int value)
    {
        Loop();

        switch (value)
        {
            case 0:
                float coefA = Mathf.Lerp(0, 1, Timer());

                if (in3D == true)
                {
                    transformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefA);
                }
                else
                {
                    rectTransformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefA);
                }

                if (coefA >= lerpLimitMax) { coef = 0; coefEasing = 0f; ppValue = 1; };
                break;
            case 1:
                float coefB = Mathf.Lerp(1, 0, Timer());

                if (in3D == true)
                {
                    transformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefB);
                }
                else
                {
                    rectTransformObj.localPosition = Vector3.Lerp(initPositionObj, vecEnd, coefB);
                }

                if (coefB <= lerpLimitMin) { ValuesOnEnd(); ppValue = 0; };
                break;
        }
    }
    private void PingPongRotation(int value)
    {
        Loop();

        switch (value)
        {
            case 0:
                float coefA = Mathf.Lerp(0, 1, Timer());

                if (in3D == true)
                {
                    Quaternion qStartA = Quaternion.Euler(initRotationObj);
                    Quaternion qEndA = Quaternion.Euler(vecEnd);

                    transformObj.localRotation = Quaternion.Lerp(qStartA, qEndA, coefA);
                }
                else
                {
                    Quaternion qStartA = Quaternion.Euler(initRotationObj);
                    Quaternion qEndA = Quaternion.Euler(vecEnd);

                    rectTransformObj.localRotation = Quaternion.Lerp(qStartA, qEndA, coefA);
                }
                
                if (coefA >= lerpLimitMax) { coef = 0; coefEasing = 0f; ppValue = 1; };
                break;
            case 1:
                float coefB = Mathf.Lerp(1, 0, Timer());

                if(in3D == true)
                {
                    Quaternion qStartB = Quaternion.Euler(initRotationObj);
                    Quaternion qEndB = Quaternion.Euler(vecEnd);

                    transformObj.localRotation = Quaternion.Lerp(qStartB, qEndB, coefB);
                }
                else
                {
                    Quaternion qStartB = Quaternion.Euler(initRotationObj);
                    Quaternion qEndB = Quaternion.Euler(vecEnd);

                    rectTransformObj.localRotation = Quaternion.Lerp(qStartB, qEndB, coefB);
                }
                
                if (coefB <= lerpLimitMin) { ValuesOnEnd(); ppValue = 0; };
                break;
        }
    }
    private void PingPongScale(int value)
    {
        Loop();

        switch (value)
        {
            case 0:
                float coefA = Mathf.Lerp(0, 1, Timer());

                if (in3D == true)
                {
                    transformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefA);
                }
                else
                {
                    rectTransformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefA);
                }
                
                if (coefA >= lerpLimitMax) { coef = 0; coefEasing = 0f; ppValue = 1; };
                break;
            case 1:
                float coefB = Mathf.Lerp(1, 0, Timer());

                if (in3D == true)
                {
                    transformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefB);
                }
                else
                {
                    rectTransformObj.localScale = Vector3.Lerp(initScaleObj, vecEnd, coefB);
                }

                if (coefB <= lerpLimitMin) { ValuesOnEnd(); ppValue = 0; };
                break;
        }
    }
    private void PingPongColor(int value)
    {
        Loop();

        switch (value)
        {
            case 0:
                float coefA = Mathf.Lerp(0, 1, Timer());

                if (in3D == true)
                {
                    Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefA);
                    matObj.material.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
                }
                else
                {
                    if (textIsUsed)
                    {
                        Vector4 newColor = Vector4.Lerp(colorStart, colorEnd, coefA);
                        textObj.color = new Vector4(newColor.x, newColor.y, newColor.z, newColor.w);
                    }
                    else
                    {
                        Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefA);
                        imageObj.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
                    }                 
                }

                if (coefA >= lerpLimitMax) { coef = 0; coefEasing = 0f; ppValue = 1; };
                break;
            case 1:
                float coefB = Mathf.Lerp(1, 0, Timer());

                if (in3D == true)
                {
                    Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefB);
                    matObj.material.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
                }
                else
                {
                    if (textIsUsed)
                    {
                        Vector4 newColor = Vector4.Lerp(colorStart, colorEnd, coefB);
                        textObj.color = new Vector4(newColor.x, newColor.y, newColor.z, newColor.w);
                    }
                    else
                    {
                        Vector4 newVector = Vector4.Lerp(colorStart, colorEnd, coefB);
                        imageObj.color = new Vector4(newVector.x, newVector.y, newVector.z, newVector.w);
                    }
                }

                if (coefB <= lerpLimitMin) { ValuesOnEnd(); ppValue = 0; };
                break;
        }
    }
    #endregion


    // FUNCTIONS /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Others functions
    public void LaunchAnim()
    {
        StartCoroutine(WaitLaunch());
    }

    public float Timer()
    {
        coef += Time.deltaTime / (duration / ppState);

        coefEasing = coef;

       return OnEasing(coefEasing);
    }
    public void Loop()
    {
        if (numberOfLoops != -1)
        {
            if (currentLoop >= numberOfLoops)
            {
                coef = 0f; etat = false;
                currentLoop = 0;
                return;
            }
        }
        else if (numberOfLoops == 0)
        {
            coef = 0f; etat = false;
            currentLoop = 0;
            Debug.Log("NOPE ERROR");
            return;
        }
    }

    #region InitComponents
    public void InitComponentTransform()
    {
        
        if (myObj.TryGetComponent(out RectTransform recTrans) == true)
        {
            rectTransformObj = myObj.GetComponent<RectTransform>();
            initPositionObj = rectTransformObj.localPosition;
            initRotationObj = rectTransformObj.localRotation.eulerAngles;
            initScaleObj = rectTransformObj.localScale;
            in3D = false;
        }
        else
        {
            transformObj = myObj.GetComponent<Transform>();
            initPositionObj = transformObj.localPosition;
            initRotationObj = transformObj.localRotation.eulerAngles;
            initScaleObj = transformObj.localScale;
            in3D = true;
        }

    }
    public void InitComponentRenderer()
    {
        if (myObj.TryGetComponent(out Renderer mat))
        {
            matObj = myObj.GetComponent<Renderer>();
        }
        else if (myObj.TryGetComponent(out Image image))
        {
            imageObj = myObj.GetComponent<Image>();
        }
        else if (myObj.TryGetComponent(out TextMeshProUGUI text))
        {
            textObj = myObj.GetComponent<TextMeshProUGUI>();
            textIsUsed = true;
        }
    }
    #endregion
    
    public void ValuesOnEnd()
    {
        currentLoop++; coef = 0f; coefEasing = 0f;
    }
    public void SwitchBelongType()
    {
        switch (enumAnim)
        {
            case myEnum.ZeroToOne:
                ppState = 2f;

                switch (enumType)
                {
                    case myEnumType.Position:
                        ZeroToOnePosition();
                        break;

                    case myEnumType.Rotation:
                        ZeroToOneRotation();
                        break;

                    case myEnumType.Scale:
                        ZeroToOneScale();
                        break;

                    case myEnumType.Color:
                        ZeroToOneColor();
                        break;
                }
                break;

            case myEnum.OneToZero:
                ppState = 2f;

                switch (enumType)
                {
                    case myEnumType.Position:
                        OneToZeroPosition();

                        break;

                    case myEnumType.Rotation:
                        OneToZeroRotation();

                        break;

                    case myEnumType.Scale:
                        OneToZeroScale();

                        break;

                    case myEnumType.Color:
                        OneToZeroColor();

                        break;
                }
                break;

            case myEnum.PingPong:
                ppState = 4f;
                switch (enumType)
                {
                    case myEnumType.Position:
                        PingPongPosition(ppValue);

                        break;

                    case myEnumType.Rotation:
                        PingPongRotation(ppValue);

                        break;

                    case myEnumType.Scale:
                        PingPongScale(ppValue);

                        break;

                    case myEnumType.Color:
                        PingPongColor(ppValue);

                        break;
                }
                break;
        }
    }

    public void ChooseEasing()
    {
        switch (enumEasing)
        {
            case myEnumEasing.Linear:
                iterationEasing = 0;
                break;

            case myEnumEasing.QuadraticIn:
                iterationEasing = 1;
                break;
            case myEnumEasing.QuadraticOut:
                iterationEasing = 2;
                break;
            case myEnumEasing.QuadraticInOut:
                iterationEasing = 3;
                break;

            case myEnumEasing.CubicIn:
                iterationEasing = 4;
                break;
            case myEnumEasing.CubicOut:
                iterationEasing = 5;
                break;
            case myEnumEasing.CubicInOut:
                iterationEasing = 6;
                break;

            case myEnumEasing.QuarticIn:
                iterationEasing = 7;
                break;
            case myEnumEasing.QuarticOut:
                iterationEasing = 8;
                break;
            case myEnumEasing.QuarticInOut:
                iterationEasing = 9;
                break;

            case myEnumEasing.QuinticIn:
                iterationEasing = 10;
                break;
            case myEnumEasing.QuinticOut:
                iterationEasing = 11;
                break;
            case myEnumEasing.QuinticInOut:
                iterationEasing = 12;
                break;

            case myEnumEasing.SinusoidalIn:
                iterationEasing = 13;
                break;
            case myEnumEasing.SinusoidalOut:
                iterationEasing = 14;
                break;
            case myEnumEasing.SinusoidalInOut:
                iterationEasing = 15;
                break;

            case myEnumEasing.ExponentialIn:
                iterationEasing = 16;
                break;
            case myEnumEasing.ExponentialOut:
                iterationEasing = 17;
                break;
            case myEnumEasing.ExponentialInOut:
                iterationEasing = 18;
                break;

            case myEnumEasing.CircularIn:
                iterationEasing = 19;
                break;
            case myEnumEasing.CircularOut:
                iterationEasing = 20;
                break;
            case myEnumEasing.CircularInOut:
                iterationEasing = 21;
                break;

            case myEnumEasing.ElasticIn:
                iterationEasing = 22;
                break;
            case myEnumEasing.ElasticOut:
                iterationEasing = 23;
                break;
            case myEnumEasing.ElasticInOut:
                iterationEasing = 24;
                break;

            case myEnumEasing.BackIn:
                iterationEasing = 25;
                break;
            case myEnumEasing.BackOut:
                iterationEasing = 26;
                break;
            case myEnumEasing.BackInOut:
                iterationEasing = 27;
                break;

            case myEnumEasing.BounceIn:
                iterationEasing = 28;
                break;
            case myEnumEasing.BounceOut:
                iterationEasing = 29;
                break;
            case myEnumEasing.BounceInOut:
                iterationEasing = 30;
                break;

            case myEnumEasing.AnimationCurve:
                iterationEasing = 31;
                break;
        }
    }
    public float OnEasing(float myEasing)
    {
        switch (iterationEasing)
        {
            default: return GetLinear(myEasing);
            case 0: return GetLinear(myEasing);

            case 1: return GetQuadraticIn(myEasing);
            case 2: return GetQuadraticOut(myEasing);
            case 3: return GetQuadraticInOut(myEasing);

            case 4: return GetCubicIn(myEasing);
            case 5: return GetCubicOut(myEasing);
            case 6: return GetCubicInOut(myEasing);

            case 7: return GetQuarticIn(myEasing);
            case 8: return GetQuarticOut(myEasing);
            case 9: return GetQuarticInOut(myEasing);

            case 10: return GetQuinticIn(myEasing);
            case 11: return GetQuinticOut(myEasing);
            case 12: return GetQuinticInOut(myEasing);

            case 13: return GetSinusoidalIn(myEasing);
            case 14: return GetSinusoidalOut(myEasing);
            case 15: return GetSinusoidalInOut(myEasing);

            case 16: return GetExponentialIn(myEasing);
            case 17: return GetExponentialOut(myEasing);
            case 18: return GetExponentialInOut(myEasing);

            case 19: return GetCircularIn(myEasing);
            case 20: return GetCircularOut(myEasing);
            case 21: return GetCircularInOut(myEasing);

            case 22: return GetElasticIn(myEasing);
            case 23: return GetElasticOut(myEasing);
            case 24: return GetElasticInOut(myEasing);

            case 25: return GetBackIn(myEasing);
            case 26: return GetBackOut(myEasing);
            case 27: return GetBackInOut(myEasing);

            case 28: return GetBounceIn(myEasing);
            case 29: return GetBounceOut(myEasing);
            case 30: return GetBounceInOut(myEasing);

            case 31: return GetAnimationCurve(myEasing);
        }

    }
    IEnumerator WaitLaunch()
    {
        etat = false;
        yield return new WaitForSeconds(beginDuration);
        etat = true;
    }
    #endregion

    //EASINGS///////////////////////////////////////////////////////////////////////////////////////
    #region Linear
    //Linear
    public float GetLinear(float k)
    {
        return k; 
    }
    #endregion

    #region Quadratic
    //QUADRATIC
    //in
    public float GetQuadraticIn(float k)
    {
        return k * k;
    }
    //out
    public float GetQuadraticOut(float k)
    {
        return k * (2 - k);
    }
    //inout
    public float GetQuadraticInOut(float k)
    {
        if ((k *= 2) < 1)
        {
            return 0.5f * k * k;
        }
        return -0.5f * (--k * (k - 2) - 1);
    }
    #endregion

    #region Cubic
    //CUBIC
    //in
    public float GetCubicIn(float k)
    {
        return k * k * k;
    }
    //out 
    public float GetCubicOut(float k)
    {
        return --k * k * k + 1;
    }
    //inout
    public float GetCubicInOut(float k)
    {
        if ((k *= 2) < 1)
        {
            return 0.5f * k * k * k;
        }
        return 0.5f * ((k -= 2) * k * k + 2);
    }
    #endregion

    #region Quartic
    //QUARTIC
    //in
    public float GetQuarticIn(float k)
    {
        return k * k * k * k;
    }
    //out
    public float GetQuarticOut(float k)
    {
        return 1 - (--k * k * k * k);
    }
    //inout
    public float GetQuarticInOut(float k)
    {
        if ((k *= 2) < 1)
        {
            return 0.5f * k * k * k * k;
        }
        return -0.5f * ((k -= 2) * k * k * k - 2);
    }
    #endregion

    #region Quintic
    //QUINTIC
    //in
    public float GetQuinticIn(float k)
    {
        return k * k * k * k * k;
    }
    //out
    public float GetQuinticOut(float k)
    {
        return --k * k * k * k * k + 1;
    }
    //inout
    public float GetQuinticInOut(float k)
    {
        if ((k *= 2) < 1)
        {
            return 0.5f * k * k * k * k * k;
        }
        return 0.5f * ((k -= 2) * k * k * k * k + 2);
    }
    #endregion

    #region Sinusoidal
    //SINUSOIDAL
    //in
    public float GetSinusoidalIn(float k)
    {
        return 1 - Mathf.Cos(k * Mathf.PI / 2);

    }
    //out
    public float GetSinusoidalOut(float k)
    {
        return Mathf.Sin(k * Mathf.PI / 2);
    }
    //inout
    public float GetSinusoidalInOut(float k)
    {
        return 0.5f * (1 - Mathf.Cos(Mathf.PI * k));
    }
    #endregion

    #region Exponential
    //EXPONENTIAL
    //in
    public float GetExponentialIn(float k)
    {
        return k == 0 ? 0 : Mathf.Pow(1024, k - 1);
    }
    //out
    public float GetExponentialOut(float k)
    {
        return k == 1 ? 1 : 1 - Mathf.Pow(2, -10 * k);
    }
    //inout
    public float GetExponentialInOut(float k)
    {
        if (k == 0)
        {
            return 0;
        }
        if (k == 1)
        {
            return 1;
        }
        if ((k *= 2) < 1)
        {
            return 0.5f * Mathf.Pow(1024, k - 1);
        }
        return 0.5f * (-Mathf.Pow(2, -10 * (k - 1)) + 2);
    }
    #endregion

    #region Circular 
    //CIRCULAR
    //in
    public float GetCircularIn(float k)
    {
        return 1 - Mathf.Sqrt(Mathf.Abs(1 - Mathf.Pow(k, 2)));
    }
    //out
    public float GetCircularOut(float k)
    {
        return Mathf.Sqrt(Mathf.Abs(1 - (--k * k)));
    }
    //inout
    public float GetCircularInOut(float k)
    {
        if ((k *= 2) < 1)
        {
            return -0.5f * (Mathf.Sqrt(Mathf.Abs(1 - k * k)) - 1);
        }
        return 0.5f * (Mathf.Sqrt(Mathf.Abs(1 - (k -= 2) * k)) + 1);
    }
    #endregion

    #region Elastic
    //ELASTIC
    //in
    public float GetElasticIn(float k)
    {
        if (k == 0)
        {
            return 0;
        }
        if (k == 1)
        {
            return 1;
        }
        return -Mathf.Pow(2, 10 * (k - 1)) * Mathf.Sin((k - 1.1f) * 5 * Mathf.PI);
    }
    //out
    public float GetElasticOut(float k)
    {
        if (k == 0)
        {
            return 0;
        }
        if (k == 1)
        {
            return 1;
        }
        return Mathf.Pow(2, -10 * k) * Mathf.Sin((k - 0.1f) * 5 * Mathf.PI) + 1;
    }
    //inout
    public float GetElasticInOut(float k)
    {
        if (k == 0)
        {
            return 0;
        }
        if (k == 1)
        {
            return 1;
        }

        k *= 2;

        if (k < 1)
        {
            return -0.5f * Mathf.Pow(2, 10 * (k - 1)) * Mathf.Sin((k - 1.1f) * 5 * Mathf.PI);
        }
        return 0.5f * Mathf.Pow(2, -10 * (k - 1)) * Mathf.Sin((k - 1.1f) * 5 * Mathf.PI) + 1;
    }
    #endregion

    #region Back
    //BACK
    //in
    public float GetBackIn(float k)
    {
        float s = 1.70158f;
        return k * k * ((s + 1) * k - s);
    }
    //out
    public float GetBackOut(float k)
    {
        float s = 1.70158f;
        return --k * k * ((s + 1) * k + s) + 1;
    }
    //inout
    public float GetBackInOut(float k)
    {
        float s = 1.70158f * 1.525f;
        if ((k *= 2) < 1)
        {
            return 0.5f * (k * k * ((s + 1) * k - s));
        }
        return 0.5f * ((k -= 2) * k * ((s + 1) * k + s) + 2);
    }
    #endregion

    #region Bounce
    //BOUNCE
    //in
    public float GetBounceIn(float k)
    {
        return 1 - GetBounceOut(1 - k);
    }
    //out
    public float GetBounceOut(float k)
    {
        if (k < (1 / 2.75f))
        {
            return 7.5625f * k * k;
        }
        else if (k < (2 / 2.75f))
        {
            return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
        }
        else if (k < (2.5 / 2.75f))
        {
            return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
        }
        else
        {
            return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
        }
    }
    //inout
    public float GetBounceInOut(float k)
    {
        if (k < 0.5f)
        {
            return GetBounceIn(k * 2) * 0.5f;
        }
        return GetBounceOut(k * 2 - 1) * 0.5f + 0.5f;
    }
    #endregion

    #region AnimationCurve
    public float GetAnimationCurve(float k)
    {
        
        return myCurve.Evaluate(k);
    }
    #endregion
}
