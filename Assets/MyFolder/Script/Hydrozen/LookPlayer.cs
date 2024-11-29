using System;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.LookAt(target);
        
        // 현재 회전을 로컬 Euler 각도로 가져오기
        Vector3 currentEulerAngles = transform.localEulerAngles;

        // X축에 90도 추가
        currentEulerAngles.x += 90;

        // 수정된 회전을 다시 적용
        transform.localEulerAngles = currentEulerAngles;
    }
}
