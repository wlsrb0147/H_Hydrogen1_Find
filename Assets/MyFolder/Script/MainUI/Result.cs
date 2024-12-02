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
    
    
    
    [SerializeField] private MoveAndFlip[] moveObjects;

    [SerializeField] private GameObject timer;
    [SerializeField] private RawImage objResult;
    
    [SerializeField] private Selected[] selected;
    
    [SerializeField] private PlayerScr playerScr;
    private GameManager gameManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    
    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.SetResult(this);
        audioSource = gameManager.audioSource;
        winMovie.loopPointReached += WinMovieOnloopPointReached;
        loseMovie.loopPointReached += LoseMovieOnloopPointReached;
    }

    private void LoseMovieOnloopPointReached(VideoPlayer source)
    {
        GameController.GoToTitle();
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
 

        if (score > 3)
        {
            WinProcess(gameManager.GetScore()).Forget();
        }
        else
        {
            audioSource.Stop();
            LoseGame();
            audioSource.PlayOneShot(loseClip);
            OpenLoseButton().Forget();
            gameManager.BlurDepthAndBlack();
        }
    }

    private async UniTaskVoid OpenLoseButton()
    {
        await UniTask.Delay(1500);
        selected[0].gameObject.SetActive(true);
        selected[0].ToggleImage(true);
        
        await UniTask.Delay(150);
        selected[1].gameObject.SetActive(true);
        selected[1].ToggleImage(false);
        playerScr.SetIsFailedTrue();
    }
    
    private void WinGame()
    {
        audioSource.Stop();
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

    private async UniTaskVoid WinProcess(int score)
    {
        await UniTask.Delay(500);
        WinGame();
        audioSource.PlayOneShot(winClip);
        gameManager.BlurDepthAndBlack();
        await UniTask.Delay(2000);
        for (int i = 0; i < moveObjects.Length; i++)
        {
            moveObjects[i].StartMove(score);
            await UniTask.Delay(100);
        }
    }
}
