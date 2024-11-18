using UnityEngine;
using UnityEngine.Video;

public class LoadingControl : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] players;
    [SerializeField] private RenderTexture renderTexture;

    private void OnEnable()
    {
        foreach (var v in players)
        {
            v.targetTexture = null; // 초기화
            v.prepareCompleted -= OnPrepareCompleted;
            v.loopPointReached -= OnVideoEnded;
        }

        VideoPlayer currentPlayer = null;

        // currentScene에 따라 VideoPlayer 선택
        switch (GameController.currentScene)
        {
            case 2:
                currentPlayer = players[0];
                break;
            case 3:
                currentPlayer = players[1];
                break;
            case 4:
                currentPlayer = players[2];
                break;
            case 5:
                currentPlayer = players[3];
                break;
        }
        
        currentPlayer.targetTexture = renderTexture;

        if (currentPlayer != null)
        {
            // 이벤트 등록
            currentPlayer.prepareCompleted += OnPrepareCompleted;
            currentPlayer.loopPointReached += OnVideoEnded;

            currentPlayer.Prepare(); // 영상 준비
        }
    }

    private void OnDisable()
    {
        // 모든 이벤트 제거
        foreach (var v in players)
        {
            v.prepareCompleted -= OnPrepareCompleted;
            v.loopPointReached -= OnVideoEnded;
        }
    }
    
    private void OnPrepareCompleted(VideoPlayer source)
    {
        Debug.Log("Video prepared, starting map loading...");
        GameController.LoadSceneProgress().Forget();
        source.Play(); // 준비된 영상 재생
    }

    private void OnVideoEnded(VideoPlayer source)
    {
        Debug.Log("Video ended, loading next scene...");
        GameController.op.allowSceneActivation = true;
    }
}