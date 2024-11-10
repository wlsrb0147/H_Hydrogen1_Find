using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static int _currentScene = 1;
    public static int score = 0;
    private static readonly Sprite[] SavedImage = new Sprite[15];
    
    [SerializeField] private Slider progressBar;

    private static int _currentSavedImage;
    
    public static void SaveImage(Sprite sprite)
    {
        int endIndex;
        switch (_currentScene)
        {
            case 2:
                endIndex = 5;
                break;
            case 3:
                endIndex = 10;
                break;
            case 4:
                endIndex = 15;
                break;
            default:
                endIndex = 1;
                break;
        }

        // 현재 씬의 범위 내에서만 저장
        if ( _currentSavedImage < endIndex)
        {
            SavedImage[_currentSavedImage] = sprite;
            ++_currentSavedImage;
        }
    }

    public static void LoadImage(Image[] images)
    {
        for (int i = 0; i < 15; i++)
        {
            if (SavedImage[i] is not null)
            {
                images[i].sprite = SavedImage[i];
                images[i].type = Image.Type.Simple; // 이미지 타입을 Simple로 설정
                images[i].preserveAspect = true; // 비율 유지
            }
        }
    }

    public static async void LoadScene()
    {
        ++_currentScene;

        if (_currentScene == 6)
        {
            _currentScene = 1;
            SceneManager.LoadScene(1);
            Debug.Log(_currentScene);
            for (int i = 0; i < _currentSavedImage; i++)
            {
                SavedImage[i] = null;
            }
            _currentSavedImage = 0;
            return;
        }

        _currentSavedImage = (_currentScene - 2) * 5;
        Debug.Log("else" + _currentScene);
        
        await SceneManager.LoadSceneAsync(0);
        
        GameObject loadingObject = GameObject.FindGameObjectWithTag("LoadingBar"); // 로딩 씬에 있는 로딩바를 찾음
        Slider progressBar = loadingObject?.GetComponent<Slider>();
        Debug.Log(progressBar);
        LoadSceneProgress(progressBar).Forget();

    }
    
    private static async UniTaskVoid LoadSceneProgress(Slider progressBar)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_currentScene);
        op.allowSceneActivation = false;
        const float offset = 1 / 0.9f;
        
        float timer = 0f;
        while (!op.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            if (op.progress < 0.9f)
            {
                progressBar.value = op.progress * offset;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = (timer / op.progress) * offset;

                if (progressBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    return;
                }
            }
        }
    }
}
