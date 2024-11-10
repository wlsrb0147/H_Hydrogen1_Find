using System;
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
    
    [SerializeField] private Vector3 rottedAngle = Vector3.zero;
    
    
    private Vector2 currentInput = Vector2.zero;
    private float verticalInput;
    
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private float speed = 2.5f;
    private Vector3 moveDirection2; // 이동 방향

    public float x = 75;
    
    private CreateGizmos gizmos;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = playerInput.camera ?? Camera.main;
        imgZoom = objZoom.GetComponent<Image>();
        rectZoom = objZoom.GetComponent<RectTransform>();
        
        Cursor.lockState = CursorLockMode.Locked;  // 커서를 잠금 해제
        Cursor.visible = false;                   // 커서를 보이게 설정
        gizmos = GetComponent<CreateGizmos>();
    }

    private void OnClearCheat()
    {
        Debug.Log("Activated");
        GameController.LoadScene();
    }
    
    public void OnClearCheat(InputAction.CallbackContext context)
    {
        if (context.performed) // 버튼이 눌렸을 때만 실행
        {
            GameController.LoadScene();
        }
    }

    private void FixedUpdate()
    {
        Move(moveDirection);
    }

    private void Update()
    {
        Vector2 normalizedInput = currentInput.normalized; // Vector2에서 정규화
        Vector3 moveForwardBackward = transform.forward * normalizedInput.y;
        Vector3 moveLeftRight = transform.right * normalizedInput.x;
        Vector3 moveDirection = moveForwardBackward + moveLeftRight;

// 속도 적용 (로컬 좌표에서 이동)
        transform.localPosition += moveDirection * (speed * Time.deltaTime);

        
        transform.Translate(moveDirection2 * (speed * Time.deltaTime));
    }


    
    private void OnMove2(InputValue value)
    {
        currentInput = value.Get<Vector2>();
    }
    
    public void OnMove2(InputAction.CallbackContext context)
    {
        // 기존 Vector2 값 받기
        currentInput = context.ReadValue<Vector2>();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.performed) // 키가 눌러져 있을 때
        {
            moveDirection2 = Vector3.up;
        }
        else if (context.canceled) // 키가 떼어졌을 때
        {
            moveDirection2 = Vector3.zero;
        }
    }

    // 아래로 이동 (E)
    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.performed) // 키가 눌러져 있을 때
        {
            moveDirection2 = Vector3.down;
        }
        else if (context.canceled) // 키가 떼어졌을 때
        {
            moveDirection2 = Vector3.zero;
        }
    }
    
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        
        moveDirection.x = input.x;
        moveDirection.y = input.y;
        
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) // 키 입력과 키 해제 이벤트 처리
        {
            Vector2 input = context.ReadValue<Vector2>();
            moveDirection.x = input.x;
            moveDirection.y = input.y;
        }
    }

    
    public void OnShot(InputAction.CallbackContext context)
    {
        // 입력이 시작될 때
        if (context.started && !is50FOV)
        {
            gizmos.PerformBoxCast();
        }
    }
    
    private void Move(Vector2 input)
    {
        if (input != Vector2.zero && !isZooming)
        {
            input *= defaultSpeed * zoomSpeed;
            Vector3 currentRotation = cam.transform.eulerAngles;
            rottedAngle.y += input.x;
            rottedAngle.x += input.y;


            if (Mathf.Abs(rottedAngle.y) > x)
            {
                rottedAngle.y -= input.x;
                Debug.Log(x+"도 넘음");
            }
            else
            {
                currentRotation.y += input.x; // Y축 회전 (수평 회전)
            }

            if (Mathf.Abs(rottedAngle.x) > 30)
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
    
    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed) // 키가 눌렸을 때만 작동
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
