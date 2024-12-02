using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ZoomAlpha : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;
    private Vector3 startScale = new Vector3(0.9f, 0.9f, 1f);
    
    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        rectTransform.localScale = startScale;
        rectTransform.DOScale(Vector3.one,0.5f);
        image.DOFade(1,0.5f);
    }

    public void Close()
    {
        rectTransform.DOScale(startScale, 0.5f).OnComplete(()=>gameObject.SetActive(false));
        image.DOFade(0,0.5f);
    }
}
