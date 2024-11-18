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
    private int maxVideoLength;
    private int currentVideoIndex;

    private void Awake()
    {
        maxVideoLength = players.Length;
        objTransition.SetActive(false);
        if (!transition.isPrepared)
        {
           transition.Prepare();
        }

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
            Call().Forget();
        }
        else if (currentVideoIndex < maxVideoLength)
        {
            players[currentVideoIndex].targetTexture = renderTexture;
            players[currentVideoIndex].Play();
        }
    }

    private async UniTaskVoid Call()
    {
        await UniTask.WaitForSeconds(1.4f);
        GameController.LoadScene();
        transition.Pause();
    }
    
}
