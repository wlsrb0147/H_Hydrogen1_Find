using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class EndPlayer : MonoBehaviour
{
    private Settings settings;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private VideoPlayer player1;
    [SerializeField] private VideoPlayer player2;

    [SerializeField] private MoveAndFlip1[] images;

    [SerializeField] private AudioClip clip;
    private AudioSource audioSource;
    

    private bool canSkip;
    private bool isSecondVideo;
    private float returnTime;
    
    private List<Sprite> sprites = new List<Sprite>();

    private T LoadJsonData<T>(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath = filePath.Replace("\\", "/");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Loaded JSON: " + json); // JSON 문자열 출력
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }

        Debug.LogWarning("File does not exist!");
        return default;
    }
    
    private void Awake()
    {
        settings = LoadJsonData<Settings>("settings.json");
        returnTime = settings.endingStandardTime;
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
        
        returnTime = settings.endingStandardTime;
        
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
            returnTime = settings.endingStandardTime;
        }

        if (context.canceled)
        {
            player2.playbackSpeed = 1;
            returnTime = settings.endingStandardTime;
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

    private void Update()
    {
        returnTime -= Time.deltaTime;

        if (returnTime < 0)
        {
            GameController.LoadScene();
        }
    }
}
