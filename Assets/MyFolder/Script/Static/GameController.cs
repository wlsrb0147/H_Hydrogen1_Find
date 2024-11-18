using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int currentScene = 0;
    private static readonly Sprite[] SavedImage = new Sprite[15];
    private static int _currentSavedImage;

    public static AsyncOperation op;
    
    public static void SaveImage(Sprite sprite)
    {
        int endIndex;
        switch (currentScene)
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
            ++_currentSavedImage; // 얘를 스코어로 쓰면 될듯
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

    public static async void ReloadScene()
    {
        _currentSavedImage = (currentScene - 2) * 5;
        Debug.Log("else" + currentScene);
        
        await SceneManager.LoadSceneAsync(1);
    }

    public static async void LoadScene()
    {
        if (currentScene != 0)
        {
            ++currentScene;
        }
        else
        {
            currentScene = 2;
        }

        if (currentScene == 6)
        {
            currentScene = 0;
            SceneManager.LoadScene(0);
            Debug.Log(currentScene);
            for (int i = 0; i < _currentSavedImage; i++)
            {
                SavedImage[i] = null;
            }
            _currentSavedImage = 0;
            return;
        }

        _currentSavedImage = (currentScene - 2) * 5;
        Debug.Log("else" + currentScene);
        
        await SceneManager.LoadSceneAsync(1);
    }
    
    public static async UniTaskVoid LoadSceneProgress()
    {
        op = SceneManager.LoadSceneAsync(currentScene);
        op.allowSceneActivation = false;
    }
}
