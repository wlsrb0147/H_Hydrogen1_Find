using System;
using UnityEngine;
using UnityEngine.Video;

public class LoadingControl : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] players;
    [SerializeField] private VideoPlayer[] players2;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    private bool isFinal;
    private bool isLoading;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        foreach (var v in players)
        {
            v.targetTexture = null; // 초기화
            v.prepareCompleted -= OnPrepareCompleted;
            v.loopPointReached -= OnVideoEnded;
        }

        VideoPlayer currentPlayer = null;

        isFinal = false;
        isLoading = false;   
        // currentScene에 따라 VideoPlayer 선택
        switch (GameController.currentScene)
        {
            case 2:
                currentPlayer = players[0];
                players2[0].Prepare();
                break;
            case 3:
                currentPlayer = players[1];
                players2[1].Prepare();
                break;
            case 4:
                currentPlayer = players[2];
                players2[2].Prepare();
                break;
            case 5:
                currentPlayer = players[3];
                audioSource.clip = clips[1];
                audioSource.Play();
                isFinal = true;
                break;
        }
        
        currentPlayer.targetTexture = renderTexture;

        if (currentPlayer != null)
        {
            // 이벤트 등록
            currentPlayer.prepareCompleted += OnPrepareCompleted;
            Debug.Log("PrePareAdded");
            currentPlayer.loopPointReached += OnVideoEnded;

            currentPlayer.Prepare(); // 영상 준비
        }
    }

    private void OnDisable()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.black); // 검정 화면으로 지우기
        RenderTexture.active = null;
        if (isFinal)
        {

        }
        
        // 모든 이벤트 제거
        foreach (var v in players)
        {
            v.prepareCompleted -= OnPrepareCompleted;
            v.loopPointReached -= OnVideoEnded;
        }
        
        foreach (var v in players2)
        {
            v.loopPointReached -= LoadNext;
        }
    }
    
    private void OnPrepareCompleted(VideoPlayer source)
    {
        if (!isLoading)
        {
            Debug.Log("Video prepared, starting map lading...");
            GameController.LoadSceneProgress();
            source.Play(); // 준비된 영상 재생
            isLoading = true;
        }   

    }
    
    private void OnVideoEnded(VideoPlayer source)
    {
        int x = -1;
        switch (GameController.currentScene)
        {
            case 2:
                x = 0;
                break;
            case 3:
                x = 1;
                break;
            case 4:
                x = 2;
                break;
            case 5:
                GameController.op.allowSceneActivation = true;
                break;
        }

        if (x >= 0 )
        {
            players[x].Stop();
            players[x].targetTexture = null;
            players2[x].targetTexture = renderTexture;
            players2[x].Play();
            players2[x].loopPointReached += LoadNext;
            audioSource.clip = clips[0];
            audioSource.Play();
        }
    }

    private void LoadNext(VideoPlayer source)
    {
        GameController.op.allowSceneActivation = true;
    }
}