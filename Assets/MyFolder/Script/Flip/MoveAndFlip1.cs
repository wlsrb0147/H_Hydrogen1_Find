using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveAndFlip1 : MonoBehaviour
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
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    

    private void Awake()
    {
        gameManager = GameManager.instance;
        source = gameManager.audioSource;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        pos = gameManager.odd[order];
    }
    
    private void OnEnable()
    {
        image.sprite = front;
        Invoke(nameof(StartMove),2f);
    }

    // order는 01234
    // score는 012345, 5일때 4까지
    public void StartMove()
    {
        rectTransform.DOAnchorPosX(pos, 1f).SetEase(Ease.InOutQuad);
        Invoke(nameof(FlipCard), 1.5f);
    }
    
    private void FlipCard()
    {
        source.PlayOneShot(clip);
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
}
