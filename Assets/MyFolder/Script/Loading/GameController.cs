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

    public static void LoadScene()
    {
        ++_nextScene;

        if (_nextScene == 6)
        {
            _nextScene = 1;
            SceneManager.LoadScene(1);
        }
        else
        {
         SceneManager.LoadScene(0);
        }
        //_nextScene = sceneName;
        
        Debug.Log(_nextScene);
    }

    private void Start()
    {
        LoadSceneProgress().Forget();
    }

    private async UniTaskVoid LoadSceneProgress()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        
        float timer = 0f;
        while (!op.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            if (op.progress < 0.9f)
            {
                progressBar.value = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = timer / op.progress;

                if (progressBar.value >= 0.9f)
                {
                    op.allowSceneActivation = true;
                    return;
                }
            }
        }
    }
}
