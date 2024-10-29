using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScr : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    private Vector3 moveDirection = Vector3.zero;
    private bool is50FOV = true;
    private bool isZooming;
    [SerializeField] private GameObject objZoom;
    private Image imgZoom;
    private RectTransform rectZoom;

    private readonly Vector3 targetScale = new(1.884f, 1.635f, 1f);
    [SerializeField] private float defaultSpeed = 1f;
    [SerializeField] private float zoomSpeed = 0.5f;
    
    // boxCast
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float maxSize = 10f;
    [SerializeField] private float expansionSpeed = 0.5f;
    [SerializeField] private float currentSize = 1f;
    
    
    //
    public Vector3 maxRot ;
    private Vector3 rottedAngle = Vector3.zero;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = playerInput.camera ?? Camera.main;
        imgZoom = objZoom.GetComponent<Image>();
        rectZoom = objZoom.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Move(moveDirection);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        
        moveDirection.x = input.x;
        moveDirection.y = input.y;
        Debug.Log("Send Message : " + moveDirection);
        
    }

    private void Move(Vector2 input)
    {
        if (input != Vector2.zero && !isZooming)
        {
            input *= defaultSpeed * zoomSpeed;
            Vector3 currentRotation = cam.transform.eulerAngles;
            rottedAngle.y += input.x;
            rottedAngle.x += input.y;


            if (Mathf.Abs(rottedAngle.y) > 75)
            {
                rottedAngle.y -= input.x;
                Debug.Log("75도 넘음");
            }
            else
            {
                currentRotation.y += input.x; // Y축 회전 (수평 회전)
            }

            if (Mathf.Abs(rottedAngle.x) > 35)
            {
                rottedAngle.x -= input.y;
            }
            else
            {
                currentRotation.x -= input.y; // X축 회전 (수직 회전)
            }
            
            
            cam.transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, 0f);
        }
    }

    private void OnZoom(InputValue value)
    {
        if (value.isPressed)
        {
            StartZoom(is50FOV).Forget();
            is50FOV = !is50FOV;
        }
    }

    private async UniTaskVoid StartZoom(bool is50FOV2)
    {
        float t = 0;
        float duration = 0.5f;
        float delta = 1/duration;
        
        float target = is50FOV2 ? 20f : 50f;
        
        float current = cam.fieldOfView;
        Color col = imgZoom.color;

        
        //만약 줌 하는 중이라면 초기값 사이즈 1, 알파 0
        if (is50FOV2)
        {
            col.a = 0;
            imgZoom.color = col;
            rectZoom.localScale = Vector3.one;
        }
        
        isZooming = true;
        
        while (t < 1)
        {
            cam.fieldOfView = Mathf.Lerp(current, target, t);
            
            // 줌 도중이라면
            if (is50FOV2)
            {
                rectZoom.localScale = Vector3.Lerp(targetScale, Vector3.one , t);
                col.a = Mathf.Lerp(0, 1, t);
                imgZoom.color = col;
            }
            // 줌아웃이면
            else
            {
                rectZoom.localScale = Vector3.Lerp(Vector3.one, targetScale , t);
                col.a = Mathf.Lerp(1, 0, t);
                imgZoom.color = col;
            }
            
            t +=  Time.deltaTime * delta;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        is50FOV2 = !is50FOV2;

        if (is50FOV2)
        {
            col.a = 0;
            imgZoom.color = col;
        }
        
        defaultSpeed = is50FOV2 ? 1f : 0.5f;

        rectZoom.localScale = Vector3.one;
        cam.fieldOfView = is50FOV2 ? 50f : 20f;
        isZooming = false;
    }
}
