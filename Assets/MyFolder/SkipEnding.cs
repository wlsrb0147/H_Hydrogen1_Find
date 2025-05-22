using TMPro;
using UnityEngine;

public class SkipEnding : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private TMP_Text text;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.instance.skipEnding)
            {
                GameManager.instance.skipEnding = false;
                text.text = "엔딩을 재생합니다";
            }
            else
            {
                GameManager.instance.skipEnding = true;
                text.text = "엔딩을 스킵합니다";
            }
            
            text.gameObject.SetActive(true);
            CancelInvoke(nameof(SetTextFalse));
            Invoke(nameof(SetTextFalse),5f);
        }  
    }

    private void SetTextFalse()
    {
        text.gameObject.SetActive(false);
    }

}
