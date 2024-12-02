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
        GameManager.instance.SetbackEffect(this);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("AlphaEnabled");
        image.DOKill();
        image.DOFade(0.8f, 1f);
    }

    public void DisableWithFade()
    {
        image.DOKill();
        image.DOFade(0.0f, 1f).SetEase(Ease.Linear).OnComplete(() =>gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        image.DOKill();
        image.DOFade(0f, 0f);
    }
}
