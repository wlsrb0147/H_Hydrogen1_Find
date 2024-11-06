using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static int _nextScene = 1;
    public static int score = 0;
    
    [SerializeField] private Slider progressBar;

    public static async void LoadScene()
    {
        ++_nextScene;

        if (_nextScene == 6)
        {
            _nextScene = 1;
            SceneManager.LoadScene(1);
            Debug.Log(_nextScene);
            return;
        }
        Debug.Log("else" + _nextScene);
        
        await SceneManager.LoadSceneAsync(0);
        
        GameObject loadingObject = GameObject.FindGameObjectWithTag("LoadingBar"); // 로딩 씬에 있는 로딩바를 찾음
        Slider progressBar = loadingObject?.GetComponent<Slider>();
        Debug.Log(progressBar);
        LoadSceneProgress(progressBar).Forget();

    }
    
    private static async UniTaskVoid LoadSceneProgress(Slider progressBar)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        float offest = 1 / 0.9f;
        
        float timer = 0f;
        while (!op.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            if (op.progress < 0.9f)
            {
                progressBar.value = op.progress * offest;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = (timer / op.progress) * offest;

                if (progressBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    return;
                }
            }
        }
    }
}
