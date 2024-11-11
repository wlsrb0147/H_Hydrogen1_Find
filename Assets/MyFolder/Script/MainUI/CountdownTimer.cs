using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    private const float CountDownStartValue = 90f;
    private float countdown;
    private Slider slider;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        countdown = CountDownStartValue;
        slider.value = countdown / CountDownStartValue;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        slider.value = countdown / CountDownStartValue;
    }
}
