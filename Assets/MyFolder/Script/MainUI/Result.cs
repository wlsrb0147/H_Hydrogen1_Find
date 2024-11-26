using System;
using UnityEngine;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    [SerializeField] private VideoPlayer winMovie;
    [SerializeField] private VideoPlayer loseMovie;
    
    [SerializeField] private RenderTexture renderTexture;
    
    private GameManager gameManager;
    
    [SerializeField] private GameObject[] results;
    
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
}
