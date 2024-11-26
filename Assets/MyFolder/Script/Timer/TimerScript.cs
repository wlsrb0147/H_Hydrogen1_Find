using System;
using UnityEngine;
using UnityEngine.Video;

public class TimerScript : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.SetTimer(GetComponent<VideoPlayer>());
    }
}
