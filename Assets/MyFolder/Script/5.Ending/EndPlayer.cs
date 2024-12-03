using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class EndPlayer : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private VideoPlayer player1;
    [SerializeField] private VideoPlayer player2;

    [SerializeField] private MoveAndFlip1[] images;

    [SerializeField] private AudioClip clip;
    private AudioSource audioSource;
    

    private bool canSkip;
    private bool isSecondVideo;
    
    private List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        player1.loopPointReached += Player1OnloopPointReached;
        player2.loopPointReached += Player2OnloopPointReached;
        
        player2.Prepare();
        
        Invoke(nameof(SetCanSkipTrue),3f);

        for (int i = 0; i < 15; i++)
        {
            sprites.Add(GameController.GetSprite(i));
        }
        
        sprites.RemoveAt(14);
        sprites.RemoveAt(9);
        sprites.RemoveAt(4);
        
        ShuffleList(sprites);
        
        for (int i = 0; i < 10; i++)
        {
            images[i].back = sprites[i];    
        }
    }
    
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void SetCanSkipTrue()
    {
        canSkip = true;
    }
    
    private void Player2OnloopPointReached(VideoPlayer source)
    {
        GameController.LoadScene();
    }

    private void Player1OnloopPointReached(VideoPlayer source)
    {
        PlayNextVideo();
    }

    public void PressButton(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (canSkip && !isSecondVideo)
        {
            PlayNextVideo();
        }
    }

    public void playOnTwo(InputAction.CallbackContext context)
    {
        if (!isSecondVideo) return;
        
        if (context.performed)
        {
            player2.playbackSpeed = 2;
        }

        if (context.canceled)
        {
            player2.playbackSpeed = 1;
        }

    }

    private void PlayNextVideo()
    {
        player1.Stop();
        player1.targetTexture = null;
        player2.targetTexture = renderTexture;
        player2.Play();
        isSecondVideo = true;
        audioSource.clip = clip;
        audioSource.Play();

        foreach (var v in images)
        {
            v.gameObject.SetActive(false);
        }
    }
}
