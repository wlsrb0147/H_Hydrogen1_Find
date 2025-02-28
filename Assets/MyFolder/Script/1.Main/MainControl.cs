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

    private bool canControl = true;

    private float time;
    private const float StandardTime = 40f;
    
    private void Awake()
    {
        Cursor.visible = false; 
        maxVideoLength = players.Length;
        objTransition.SetActive(false);
        if (!transition.isPrepared)
        {
           transition.Prepare();
        }

        gameManager = GameManager.instance;
        audioSource = gameManager.audioSource;
        transition.time = 0;

        time = StandardTime;
    }

    private void Update()
    {
        if (currentVideoIndex == 0) return;
        
        time -= Time.deltaTime;

        if (time <=0)
        {
            GameController.GoToTitle();
        }
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
        time = StandardTime;
        
        if (currentVideoIndex >= maxVideoLength || !canControl)  return;
        
        canControl = false; 
        
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

            switch (currentVideoIndex)
            {
                case 1:
                    Invoke(nameof(SetCanControlTrue),6f);
                    break;
                case 2:
                    Invoke(nameof(SetCanControlTrue),9.5f);
                    break;
                case 3:
                    Invoke(nameof(SetCanControlTrue),5f);
                    break;
            }
        }
    }

    private void SetCanControlTrue()
    {
        canControl = true;
    }

    private async UniTaskVoid Call()
    {
        await UniTask.WaitForSeconds(1.4f);
        GameController.LoadScene();
        transition.Pause();
    }
    
}
