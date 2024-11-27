using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveAndFlip : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    
    // move
    [SerializeField] private int order;
    private GameManager gameManager;
    private float pos;
 
    //flip
    [SerializeField] private Sprite front;
    public Sprite back;
    
    private Image image;
    private bool isFlipped;

    private void Awake()
    {
        gameManager = GameManager.instance;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        pos = gameManager.odd[order];
    }
    
    private void OnEnable()
    {
        image.sprite = front;
    }

    // order는 01234
    // scroe는 012345, 5일때 4까지
    public void StartMove(int score)
    {
        rectTransform.DOAnchorPosX(pos, 1f).SetEase(Ease.InOutQuad);
        
        if (order < score)
        {
            GameController.LoadAnSprite(out back,(GameController.currentScene-2) * 5 + order);
            Invoke(nameof(FlipCard), 1.5f);
        }
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
