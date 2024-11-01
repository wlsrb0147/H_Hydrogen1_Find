using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Explain : Initializer
{
    public TextMeshProUGUI textComponent; // 타이핑 효과를 줄 TextMeshProUGUI 컴포넌트
    public float typingSpeed = 0.1f; // 타이핑 속도 조절 변수

    private string fullText; // 전체 텍스트 저장
    private int textToDisplayIndex = 0;
    private int maxText;
    private CancellationTokenSource cts;

    private string[] textToDisplay =
    {
        "조작. 조이스틱. 확인. 버튼. ㅇㅋ?",
        "스테이지, 3개, 올클리어, 성공"
    };
    
    
    public override void Initialize()
    {
        textToDisplayIndex = 0;
        textComponent.text = "";
        maxText = textToDisplay.Length;
        
        gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        ShowText().Forget(); 
    }

    private void OnDisable()
    {
        textComponent.text = "";
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }


    private async UniTaskVoid ShowText()
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        
        textComponent.text = "";
        await UniTask.Delay(500,cancellationToken: cts.Token);
        foreach (char c in textToDisplay[textToDisplayIndex])
        {
            if (c == '\n')
            {
                await UniTask.Delay(500,cancellationToken: cts.Token);
            }
            
            textComponent.text += c;
            
            if ( c == ' ')
            {
                await UniTask.Delay(10,cancellationToken: cts.Token);
            }
            
            await UniTask.WaitForSeconds(typingSpeed,cancellationToken: cts.Token);
        }
    }


    public void Before()
    {
        
        Debug.Log("Before");
    }

    public void Next()
    {
        ++textToDisplayIndex;

        if (textToDisplayIndex < maxText)
        {
            ShowText().Forget();
            Debug.Log("Next");
        }
        else
        {
            GameController.LoadScene();
        }
        
    }
}
