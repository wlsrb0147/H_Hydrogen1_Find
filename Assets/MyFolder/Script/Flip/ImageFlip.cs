using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageFlip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite front;
    [SerializeField] private Sprite back;

    private RectTransform rectTransform;
    private Image image;
    private bool isFlipped;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image.sprite = front;
    }
    
    public void FlipCard()
    {
        // DOTween Sequence 생성
        Sequence flipSequence = DOTween.Sequence();

        // 1단계: 0도에서 90도까지 회전
        flipSequence.Append(rectTransform.DORotate(new Vector3(0, 90, 0), 0.2f)
            .SetEase(Ease.InOutQuad));

        // 2단계: 90도에서 각도 -90도로 변경하고 이미지 교체
        flipSequence.AppendCallback(() =>
        {
            rectTransform.localEulerAngles = new Vector3(0, -90, 0);
            image.sprite = isFlipped ? front : back;
            isFlipped = !isFlipped; // 상태 변경
        });

        // 3단계: -90도에서 0도까지 회전
        flipSequence.Append(rectTransform.DORotate(new Vector3(0, 0, 0), 0.2f)
            .SetEase(Ease.InOutQuad));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FlipCard();
    }
}
