using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Explain : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // 타이핑 효과를 줄 TextMeshProUGUI 컴포넌트
    public float typingSpeed = 0.1f; // 타이핑 속도 조절 변수

    private string fullText; // 전체 텍스트 저장

    private void Start()
    {
        fullText = textComponent.text.Replace("\\n", "\n"); // \n을 실제 줄바꿈으로 변환
        textComponent.text = ""; // 초기화
        ShowText().Forget(); 
    }

    private async UniTaskVoid ShowText()
    {
        foreach (char c in fullText)
        {
            textComponent.text += c;
            if (c == '\n')
            {
                await UniTask.DelayFrame(500);
            }
            await UniTask.WaitForSeconds(typingSpeed);
        }
    }
}
