using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AlphaControl : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        image.DOFade(0.8f, 1f);
    }

    private void OnDisable()
    {
        image.DOFade(0f, 0f);
    }
}
