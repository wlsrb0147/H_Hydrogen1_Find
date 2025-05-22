using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popup : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    [SerializeField] private Image image;
    [SerializeField] private AudioClip opened;
    [SerializeField] private AudioClip closed;
    [SerializeField] private GameObject hydrogen;
    [SerializeField] private GameObject textParent;
    [SerializeField] private TMP_Text text;
    
    private AudioSource source;
    private GameManager gameManager;
    private PlayerScr playerScr;

    private float invokeTime;

    [SerializeField] private Button buttonImage;
    private ColorBlock colorBlock;
    private ColorBlock originalColorBlock;
    
    private Color32 alphaColor = new Color32(255,255,255,87);

    public void SetInvoke(float x)
    {
        invokeTime = x;
    }

    private void SetColorReturn()
    {
        buttonImage.colors = originalColorBlock;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        gameObject.SetActive(false);
        gameManager = GameManager.instance;
        source = gameManager.audioSource;
        
        originalColorBlock = buttonImage.colors;
        colorBlock = buttonImage.colors;
        colorBlock.normalColor = alphaColor;
    }
    
    private void OnEnable()
    {
        playerScr = gameManager.playerScr;
        buttonImage.colors = colorBlock;
        playerScr.SetIsPopup(true);
        rectTransform.DOAnchorPos(Vector2.zero, 0.5f)
            .SetEase(Ease.OutSine).
            OnComplete(() => Delay3S().Forget());
        source.PlayOneShot(opened);
    }

    private async UniTaskVoid Delay3S()
    {
        text.text = "3";
        await UniTask.Delay(1000);
        text.text = "2";
        await UniTask.Delay(1000);
        text.text = "1";
        await UniTask.Delay(1000);
        text.text = "0";
        textParent.SetActive(false);
        EnablePopup();
    }

    private void EnablePopup()
    {
        playerScr.SetCanClosePopup(true);
        SetColorReturn();
    }

    public void DisableObject()
    {
        hydrogen.SetActive(false);
        playerScr.SetCanClosePopup(false);
        playerScr.SetIsPopup(false);
        playerScr.SetSuccessPicture(false);
        source.PlayOneShot(closed);
        rectTransform.DOAnchorPos(originalPosition, 0.5f) // 0.5초 동안 애니메이션 실행
            .SetEase(Ease.InSine)                         // Ease 설정
            .OnComplete(() =>                            // 애니메이션 완료 후 실행
            {
                gameObject.SetActive(false);             // GameObject 비활성화
            });
    }

    private void OnDisable()
    {
        rectTransform.anchoredPosition = originalPosition;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
