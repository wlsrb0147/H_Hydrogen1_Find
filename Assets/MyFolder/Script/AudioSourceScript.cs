using System;
using UnityEngine;

public class AudioSourceScript : MonoBehaviour
{
    private AudioSource audioSource;
    private GameManager gameManager;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        gameManager.audioSource = audioSource;
    }
}
