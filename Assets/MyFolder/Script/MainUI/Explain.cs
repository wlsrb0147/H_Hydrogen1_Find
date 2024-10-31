using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Explain : Initializer
{
    public TextMeshProUGUI textComponent; // 타이핑 효과를 줄 TextMeshProUGUI 컴포넌트
    public float typingSpeed = 0.1f; // 타이핑 속도 조절 변수

    private string fullText; // 전체 텍스트 저장

    private string[] textToDisplay =
    {
        "조작. 조이스틱. 확인. 버튼. ㅇㅋ?",
        "스테이지, 3개, 올클리어, 성공"
    };
    
    private int textToDisplayIndex = 0;
    
    public override void Initialize()
    {
        base.Initialize();
        textToDisplayIndex = 0;
        textComponent.text = "";
        gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        ShowText().Forget(); 
    }

    private void OnDisable()
    {
        textComponent.text = "";
    }


    private async UniTaskVoid ShowText()
    {
        await UniTask.Delay(500);
        foreach (char c in fullText)
        {
            if (c == '\n')
            {
                await UniTask.Delay(500);
            }
            
            textComponent.text += c;
            
            if ( c == ' ')
            {
                await UniTask.Delay(10);
            }
            
            await UniTask.WaitForSeconds(typingSpeed);
        }
    }


    public void Before()
    {
        Debug.Log("Before");
    }

    public void Next()
    {
        Debug.Log("Next");
    }
}
