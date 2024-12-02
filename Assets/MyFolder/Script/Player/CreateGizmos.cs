using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CreateGizmos : MonoBehaviour
{
    public float frustumLength = 5f; // 프러스텀의 깊이
    public float frustumLength2 = 5f; // 프러스텀의 깊이
    public float widthScale = 1f; // 프러스텀 폭의 비율
    public float heightScale = 1f; // 프러스텀 높이의 비율
    [SerializeField] private bool showGizmo = true; // LineRenderer를 켜고 끌 수 있는 플래그
    [SerializeField] private popup[] popups;
    
    private Camera cam;
    private readonly Vector3[] frustumCorners = new Vector3[4];
    private readonly Vector3[] frustumCorners2 = new Vector3[4];
    
    private readonly Vector3[] adjustedFrustumCorners = new Vector3[4];
    private readonly Vector3[] lineRendererCorners = new Vector3[4];
    private Transform cameraTransform;
    private LineRenderer lineRenderer;

    private GameManager gameManager;
    private PlayerScr playerScr;    
    
    [SerializeField] private Transform[] targetObject;
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip shutterSound;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("이 스크립트는 카메라가 있는 오브젝트에 추가되어야 합니다.");
            return;
        }
        cameraTransform = cam.transform;
        
        gameManager = GameManager.instance;
        audioSource = gameManager.audioSource;
        playerScr = GetComponent<PlayerScr>();
        
        // LineRenderer 설정
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5; // 사각형의 네 꼭짓점과 첫 번째 점을 연결하여 닫기
        lineRenderer.loop = false; // 사각형을 닫음
        lineRenderer.startWidth = 0.02f; // 선의 두께 설정
        lineRenderer.endWidth = 0.02f;
        lineRenderer.useWorldSpace = true; // 월드 좌표 사용

    }


    private void Update()
    {
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), frustumLength, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), frustumLength2, Camera.MonoOrStereoscopicEye.Mono, frustumCorners2);
        Vector3 cameraPosition = cameraTransform.position;

        for (int i = 0; i < 4; i++)
        {
            adjustedFrustumCorners[i] = cameraTransform.TransformPoint(frustumCorners[i]);
            lineRendererCorners[i] = cameraTransform.TransformPoint(frustumCorners2[i]);

            Vector3 cameraToCorner = adjustedFrustumCorners[i] - cameraPosition;
            Vector3 rightComponent = cameraTransform.right * (Vector3.Dot(cameraToCorner, cameraTransform.right) * widthScale);
            Vector3 upComponent = cameraTransform.up * (Vector3.Dot(cameraToCorner, cameraTransform.up) * heightScale);
            Vector3 adjustedDirection = rightComponent + upComponent + cameraTransform.forward * Vector3.Dot(cameraToCorner, cameraTransform.forward);
            
            Vector3 cameraToCorner2 = lineRendererCorners[i] - cameraPosition;
            Vector3 rightComponent2 = cameraTransform.right * (Vector3.Dot(cameraToCorner2, cameraTransform.right) * widthScale);
            Vector3 upComponent2 = cameraTransform.up * (Vector3.Dot(cameraToCorner2, cameraTransform.up) * heightScale);
            Vector3 adjustedDirection2 = rightComponent2 + upComponent2 + cameraTransform.forward * Vector3.Dot(cameraToCorner2, cameraTransform.forward);

            adjustedFrustumCorners[i] = cameraPosition + adjustedDirection;
            lineRendererCorners[i] = cameraPosition + adjustedDirection2;
        }
            
        // LineRenderer에 사각형의 꼭짓점 설정
        if (showGizmo)
        {
            lineRenderer.enabled = true;
            for (int i = 0; i < 4; i++)
            {
                lineRenderer.SetPosition(i, lineRendererCorners[i]);
            }
            lineRenderer.SetPosition(4, lineRendererCorners[0]); // 마지막 점을 첫 번째 점과 연결하여 닫기
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
        float boxWidth = Vector3.Distance(adjustedFrustumCorners[0], adjustedFrustumCorners[3]) / 2;
        float boxHeight = Vector3.Distance(adjustedFrustumCorners[0], adjustedFrustumCorners[1]) / 2;
        
        Debug.DrawRay(cameraPosition, cameraTransform.forward * frustumLength, Color.red);
        Debug.DrawRay(cameraPosition, cameraTransform.right * boxWidth, Color.green);
        Debug.DrawRay(cameraPosition, cameraTransform.up * boxHeight, Color.blue);
    }

    public void CheckObject()
    {
        for (int i = 0; i < targetObject.Length; i++)
        {
            if ( targetObject[i] == null || !targetObject[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            Vector3 viewportPoint = cam.WorldToViewportPoint(targetObject[i].position);

            if (viewportPoint.x is >= 0.24f and <= 0.76f &&
                viewportPoint.y is >= 0.21f and <= 0.79f &&
                viewportPoint.z > 0)
            {
                Debug.Log("오브젝트가 카메라의 특정 범위 내에 있습니다.");
                Sprite spr = CaptureAndSaveSprite();
                gameManager.AddScore();
                Debug.Log("PlayOneShot");
                audioSource.PlayOneShot(shutterSound);
                // 팝업 떠야함
                BlurAndPopUp(i,spr).Forget();
                
                //roy(v.gameObject);
                //targetObject[i].gameObject.SetActive(false);
            }
            
            else
            {
                Debug.Log("오브젝트가 범위 밖에 있습니다.");
            }
        }
        foreach (var v in targetObject)
        {
            
        }
    }

    private async UniTaskVoid BlurAndPopUp(int x, Sprite sprite)
    {
        await UniTask.Delay(500);
        gameManager.BlurDepthAndBlack();
        await UniTask.Delay(333);
        popups[x].gameObject.SetActive(true);
        popups[x].SetImage(sprite);
        gameManager.TimerPause();
        playerScr.SetIsBlocked(true);
    }

    public void DisablePopup()
    {
        foreach (var v in popups)
        {
            if (v.gameObject.activeSelf)
            {
                v.DisableObject();
                gameManager.TimerResume();
                gameManager.BlackEffectReturn();
            }
        }

        gameManager.BlurReturn();

        if (gameManager.GetScore() == 5)
        {
            gameManager.ShowResult();
        }
    }
    
    private Sprite CaptureAndSaveSprite()
    {
        // RenderTexture 생성 및 설정
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;
        cam.Render();

        // 현재 화면을 Texture2D로 복사
        RenderTexture.active = rt;

        // 사각형 좌표를 픽셀 단위로 변환하여 캡처할 영역 계산
        Vector3 bottomLeft = cam.WorldToScreenPoint(adjustedFrustumCorners[0]);
        Vector3 topRight = cam.WorldToScreenPoint(adjustedFrustumCorners[2]);

        // Rect의 시작점과 크기 설정 (bottomLeft와 topRight 사이의 영역)
        Rect captureRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );

        // 캡처할 영역의 크기로 Texture2D 생성
        Texture2D screenShot = new Texture2D((int)captureRect.width, (int)captureRect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(captureRect, 0, 0);
        screenShot.Apply();

        // RenderTexture 해제
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Texture2D를 Sprite로 변환
        Sprite capturedSprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));
        GameController.SaveImage(capturedSprite);

        Debug.Log("잘라낸 영역만 캡처되었습니다.");

        return capturedSprite;
    }
    

    private void OnDrawGizmos()
    {
        // cameraTransform이나 다른 변수가 null인지 확인
        if (cameraTransform == null || adjustedFrustumCorners == null || adjustedFrustumCorners.Length == 0)
            return; // null인 경우 메서드 실행 종료

        Gizmos.color = Color.cyan;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(cameraTransform.position, adjustedFrustumCorners[i]);
        }

        Gizmos.DrawLine(adjustedFrustumCorners[0], adjustedFrustumCorners[1]);
        Gizmos.DrawLine(adjustedFrustumCorners[1], adjustedFrustumCorners[2]);
        Gizmos.DrawLine(adjustedFrustumCorners[2], adjustedFrustumCorners[3]);
        Gizmos.DrawLine(adjustedFrustumCorners[3], adjustedFrustumCorners[0]);

    }
}
