using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CreateGizmos : MonoBehaviour
{
    public float frustumLength = 5f; // 프러스텀의 깊이
    public float widthScale = 1f; // 프러스텀 폭의 비율
    public float heightScale = 1f; // 프러스텀 높이의 비율
    public LayerMask detectionLayer; // 탐지할 레이어 설정
    public string targetTag = "Target"; // 탐지할 태그
    [SerializeField] private bool showGizmo = true; // LineRenderer를 켜고 끌 수 있는 플래그

    private Camera cam;
    private Vector3[] frustumCorners = new Vector3[4];
    private Vector3[] adjustedFrustumCorners = new Vector3[4];
    private Transform cameraTransform;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("이 스크립트는 카메라가 있는 오브젝트에 추가되어야 합니다.");
            return;
        }
        cameraTransform = cam.transform;
        
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
        Vector3 cameraPosition = cameraTransform.position;

        for (int i = 0; i < 4; i++)
        {
            adjustedFrustumCorners[i] = cameraTransform.TransformPoint(frustumCorners[i]);

            Vector3 cameraToCorner = adjustedFrustumCorners[i] - cameraPosition;
            Vector3 rightComponent = cameraTransform.right * (Vector3.Dot(cameraToCorner, cameraTransform.right) * widthScale);
            Vector3 upComponent = cameraTransform.up * (Vector3.Dot(cameraToCorner, cameraTransform.up) * heightScale);
            Vector3 adjustedDirection = rightComponent + upComponent + cameraTransform.forward * Vector3.Dot(cameraToCorner, cameraTransform.forward);

            adjustedFrustumCorners[i] = cameraPosition + adjustedDirection;
        }
            
        // LineRenderer에 사각형의 꼭짓점 설정
        if (showGizmo)
        {
            lineRenderer.enabled = true;
            for (int i = 0; i < 4; i++)
            {
                lineRenderer.SetPosition(i, adjustedFrustumCorners[i]);
            }
            lineRenderer.SetPosition(4, adjustedFrustumCorners[0]); // 마지막 점을 첫 번째 점과 연결하여 닫기
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void PerformBoxCast()
    {
        // BoxCast 중심은 카메라 위치에서 시작
        Vector3 cameraPosition = cameraTransform.position;

        // BoxCast의 반지름 (폭과 높이의 반)
        float boxWidth = Vector3.Distance(adjustedFrustumCorners[0], adjustedFrustumCorners[3]) / 2;
        float boxHeight = Vector3.Distance(adjustedFrustumCorners[0], adjustedFrustumCorners[1]) / 2;
        Vector3 boxHalfExtents = new Vector3(boxWidth, boxHeight, frustumLength / 2);

        // BoxCast 범위를 시각적으로 확인하기 위해 Debug.DrawRay 추가
        Debug.DrawRay(cameraPosition, cameraTransform.forward * frustumLength, Color.red);

        // BoxCast 수행
        if (Physics.BoxCast(
                cameraPosition, // 카메라의 위치에서 시작
                boxHalfExtents,
                cameraTransform.forward,
                out RaycastHit hitInfo,
                cameraTransform.rotation, // 카메라 회전으로 설정
                frustumLength,
                detectionLayer
            ))
        {
            if (hitInfo.collider.CompareTag(targetTag))
            {
                Debug.Log("태그가 " + targetTag + "인 충돌 탐지됨: " + hitInfo.collider.name);
                CaptureAndSaveSprite();
            }
        }
    }
    
    private void CaptureAndSaveSprite()
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
