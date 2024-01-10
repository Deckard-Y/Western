using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float CountTime;
    [SerializeField] private float timer;
    private float startTime;
    private bool hasFinished = false;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!hasFinished)
        {

            timer = CountTime - (Time.time - startTime);

            if (timer > 0)
            {
                uiFill.fillAmount = Mathf.InverseLerp(0, CountTime, timer);
            }
            else
            {
                uiFill.fillAmount = 0;
                uiText.text = "TIME OVER!".ToString();
                hasFinished = true;
                //ResetTimer();
            }
        }
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timer = CountTime;
        uiFill.fillAmount = 1;
        hasFinished = false;
        uiText.text = "".ToString();
    }
}
