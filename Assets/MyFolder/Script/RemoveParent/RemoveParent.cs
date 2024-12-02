using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    public Transform target; // LookAt 대상
    private Transform parentTransform;

    private void Start()
    {
        // 부모 오브젝트 Transform 가져오기
        parentTransform = transform.parent;
    }

    private void Update()
    {
        if (target == null || parentTransform == null)
            return;

        // 부모의 월드 회전을 제거한 LookAt 계산
        Quaternion parentRotation = parentTransform.rotation;
        Quaternion worldLookRotation = Quaternion.LookRotation(target.position - transform.position);

        // 부모의 회전을 제거하고 로컬 회전으로 적용
        transform.rotation = Quaternion.Inverse(parentRotation) * worldLookRotation;
    }
}
