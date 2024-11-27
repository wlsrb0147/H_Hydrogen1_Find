using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    [SerializeField] private VideoPlayer winMovie;
    [SerializeField] private VideoPlayer loseMovie;
    
    [SerializeField] private RenderTexture renderTexture;
    
    private GameManager gameManager;
    
    [SerializeField] private GameObject[] results;
    
    [SerializeField] private MoveAndFlip[] moveObjects;

    [SerializeField] private GameObject timer;
    
    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.SetResult(this);
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
        }
        else
        {
            LoseGame();
        }

        FlipImages(gameManager.GetScore()).Forget();
    }
    
    private void WinGame()
    {
        winMovie.targetTexture = renderTexture;
        winMovie.Play();
    }

    private void LoseGame()
    {
        loseMovie.targetTexture = renderTexture;
        loseMovie.Play();
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
