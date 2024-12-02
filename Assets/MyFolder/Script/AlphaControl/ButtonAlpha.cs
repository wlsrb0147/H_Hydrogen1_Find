using System;
using DG.Tweening;
using UnityEngine;

public class ButtonAlpha : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Vector3 vec = Vector3.zero;
        vec.z = 1;
        rectTransform.localScale = vec;
        rectTransform.DOKill();
        rectTransform.DOScale(1, 0.3f);
    }
}
