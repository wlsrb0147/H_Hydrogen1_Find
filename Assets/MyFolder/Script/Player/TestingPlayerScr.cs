using System;
using UnityEngine;

public class TestingPlayerScr : MonoBehaviour
{
    private Camera cam;

    public Transform targetObject;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("이 스크립트는 카메라가 있는 오브젝트에 추가되어야 합니다.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(targetObject.transform.position);

        if (viewportPoint.x is >= 0.24f and <= 0.76f &&
            viewportPoint.y is >= 0.21f and <= 0.79f &&
            viewportPoint.z > 0)
        {
            Debug.Log("오브젝트가 카메라의 특정 범위 내에 있습니다.");
        }
        else
        {
            Debug.Log("오브젝트가 범위 밖에 있습니다.");
        }
    }
    
    //0.24,0.21
    //0.76,0.79

    public Vector2 leftBottom;
    public Vector2 rightTop;
    
    private void OnDrawGizmos()
    {
        // cameraTransform이나 다른 변수가 null인지 확인
        if (cam == null )
            return; // null인 경우 메서드 실행 종료
        
        // 뷰포트 좌표를 월드 좌표로 변환
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(leftBottom.x, leftBottom.y, cam.nearClipPlane));
        Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(rightTop.x, leftBottom.y, cam.nearClipPlane));
        Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(leftBottom.x, rightTop.y, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(rightTop.x, rightTop.y, cam.nearClipPlane));

        // 선 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
