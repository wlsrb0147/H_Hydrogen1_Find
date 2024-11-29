using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    [SerializeField] private VideoPlayer winMovie;
    [SerializeField] private VideoPlayer loseMovie;
    
    [SerializeField] private RenderTexture renderTexture;
    
    private GameManager gameManager;
    
    
    [SerializeField] private MoveAndFlip[] moveObjects;

    [SerializeField] private GameObject timer;
    [SerializeField] private RawImage objResult;
    
    [SerializeField] private GameObject[] results;
    [SerializeField] private Selected[] selected;
    
    [SerializeField] private PlayerScr playerScr;
    
    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.SetResult(this);
        winMovie.loopPointReached += WinMovieOnloopPointReached;
    }

    private void WinMovieOnloopPointReached(VideoPlayer source)
    {
        GameController.LoadScene();
    }

    private void OnEnable()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }

    public void ShowResult(int score)
    {
        // 타이머 꺼야함
        timer.SetActive(false);

        foreach (GameObject obj in results)
        {
            obj.SetActive(true);
        }
        
        if (score > 3)
        {
            WinGame();
            FlipImages(gameManager.GetScore()).Forget();
            gameManager.BlurDepth();
        }
        else
        {
            LoseGame();
            EnableButtons().Forget();
            
            selected[0].gameObject.SetActive(true);
            selected[0].ToggleImage(true);
            
            selected[1].gameObject.SetActive(true);
            selected[1].ToggleImage(false);
            playerScr.SetIsFailedTrue();
        }
    }
    
    private void WinGame()
    {
        objResult.color = new Color(1, 1, 1, 0);
        winMovie.targetTexture = renderTexture;
        winMovie.Play();
        objResult.DOFade(1, 1f);
    }

    private void LoseGame()
    {
        loseMovie.targetTexture = renderTexture;
        loseMovie.Play();
    }

    private async UniTaskVoid EnableButtons()
    {
        await UniTask.Delay(100);
    }

    private async UniTaskVoid FlipImages(int score)
    {
        await UniTask.Delay(2000);
        for (int i = 0; i < moveObjects.Length; i++)
        {
            moveObjects[i].StartMove(score);

            await UniTask.Delay(100);
        }
    }
}
