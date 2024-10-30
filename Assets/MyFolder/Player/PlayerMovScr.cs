using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovScr : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private PlayerInput playerInput;
    
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
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += input.x; // Y축 회전 (수평 회전)
        currentRotation.x -= input.y; // X축 회전 (수직 회전)
        transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, 0f);
        
    }
}
