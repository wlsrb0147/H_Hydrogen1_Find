using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class MainControl : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] players;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private VideoPlayer transition;
    [SerializeField] private GameObject objTransition;
    [SerializeField] private AudioClip nextPage;
    [SerializeField] private AudioClip nextScene;
    
    private int maxVideoLength;
    private int currentVideoIndex;
    private AudioSource audioSource;    
    private GameManager gameManager;
    
    private void Awake()
    {
        maxVideoLength = players.Length;
        objTransition.SetActive(false);
        if (!transition.isPrepared)
        {
           transition.Prepare();
        }

        gameManager = GameManager.instance;
        audioSource = gameManager.audioSource;
        transition.time = 0;
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
        if (currentVideoIndex >= maxVideoLength)  return;
        
        players[currentVideoIndex].Stop();
        players[currentVideoIndex].targetTexture = null;
        ++currentVideoIndex;
        if (currentVideoIndex == maxVideoLength )
        {
            objTransition.SetActive(true);
            transition.Play();
            audioSource.PlayOneShot(nextScene);
            Call().Forget();
        }
        else if (currentVideoIndex < maxVideoLength)
        {
            players[currentVideoIndex].targetTexture = renderTexture;
            players[currentVideoIndex].Play();
            audioSource.PlayOneShot(nextPage);
        }
    }

    private async UniTaskVoid Call()
    {
        await UniTask.WaitForSeconds(1.4f);
        GameController.LoadScene();
        transition.Pause();
    }
    
}
