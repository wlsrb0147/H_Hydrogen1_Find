using System;
using UnityEngine;
using UnityEngine.Video;

public class planeTest : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        plane.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += VideoPlayerOnprepareCompleted;
        videoPlayer.Prepare();
    }

    private void VideoPlayerOnprepareCompleted(VideoPlayer source)
    {
        plane.SetActive(true);
        source.Play();
    }
}
