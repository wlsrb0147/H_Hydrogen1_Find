using System;
using UnityEngine;

public class EndingButton : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;  // 커서를 잠금 해제
        Cursor.visible = true;                   // 커서를 보이게 설정
    }

    public void OnClick()
    {
        GameController.LoadScene();
    }
}
