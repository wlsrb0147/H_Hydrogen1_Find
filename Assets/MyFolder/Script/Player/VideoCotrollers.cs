using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoCotrollers : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] videoPlayers;

    private void Awake()
    {
        foreach (var v in videoPlayers)
        {
           v.Prepare();
        }
    }
}
