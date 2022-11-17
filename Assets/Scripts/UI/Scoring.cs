using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myText;
    [SerializeField] private UnityEvent eventOnAdd;
    [SerializeField] private UnityEvent eventOnAddMonster;
    [SerializeField] private float currentScore;
    [SerializeField] private float durationAppearScore;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScoring(50);
        }
    }

    public void AddScoring(float value)
    {
        currentScore += value;
        eventOnAddMonster.Invoke();
        StartCoroutine(WaitScoring());
    }

    IEnumerator WaitScoring()
    {
        yield return new WaitForSeconds(durationAppearScore);
        myText.text = currentScore.ToString();
        eventOnAdd.Invoke();
    }
}
