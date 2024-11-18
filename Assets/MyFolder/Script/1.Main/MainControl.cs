using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class MainControl : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] players;
    [SerializeField] private RenderTexture renderTexture;
    
    private int maxVideoLength;
    private int currentVideoIndex;

    private void Awake()
    {
        maxVideoLength = players.Length;


    }

    private void OnEnable()
    {
        currentVideoIndex = 0;
        
        foreach (var v in players)
        {
            v.targetTexture = null;
        }
        
        players[0].targetTexture = renderTexture;
    }

    private void OnRight(InputValue value)
    {
        players[currentVideoIndex].Stop();
        players[currentVideoIndex].targetTexture = null;
        ++currentVideoIndex;
        if (currentVideoIndex == maxVideoLength )
        {
            GameController.LoadScene();
        }
        else
        {
            players[currentVideoIndex].targetTexture = renderTexture;
            players[currentVideoIndex].Play();
        }
    }
    
}
