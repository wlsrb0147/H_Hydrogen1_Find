using UnityEngine;

public class CreateGizmos : MonoBehaviour
{
    public float frustumLength = 5f; // 프러스텀의 깊이

    private void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("이 스크립트는 카메라가 있는 오브젝트에 추가되어야 합니다.");
            return;
        }

        // 프러스텀의 코너 좌표를 계산합니다.
        Vector3[] frustumCorners = new Vector3[4];
        cam.CalculateFrustumCorners(
            new Rect(0, 0, 1, 1),
            frustumLength,
            Camera.MonoOrStereoscopicEye.Mono,
            frustumCorners
        );

        // 로컬 좌표를 월드 좌표로 변환합니다.
        for (int i = 0; i < 4; i++)
        {
            frustumCorners[i] = cam.transform.TransformPoint(frustumCorners[i]);
        }

        // Gizmo의 색상을 설정합니다.
        Gizmos.color = Color.cyan;

        // 카메라의 위치를 가져옵니다.
        Vector3 cameraPosition = cam.transform.position;

        // 카메라 위치에서 프러스텀 코너로의 선을 그립니다.
        Gizmos.DrawLine(cameraPosition, frustumCorners[0]);
        Gizmos.DrawLine(cameraPosition, frustumCorners[1]);
        Gizmos.DrawLine(cameraPosition, frustumCorners[2]);
        Gizmos.DrawLine(cameraPosition, frustumCorners[3]);

        // 프러스텀의 코너를 연결하여 사각형을 만듭니다.
        Gizmos.DrawLine(frustumCorners[0], frustumCorners[1]); // 좌하단 -> 좌상단
        Gizmos.DrawLine(frustumCorners[1], frustumCorners[2]); // 좌상단 -> 우상단
        Gizmos.DrawLine(frustumCorners[2], frustumCorners[3]); // 우상단 -> 우하단
        Gizmos.DrawLine(frustumCorners[3], frustumCorners[0]); // 우하단 -> 좌하단
    }
}
