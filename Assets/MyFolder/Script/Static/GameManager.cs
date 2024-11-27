using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score;

    [SerializeField] private GameObject[] initializeMembers;
    private readonly List<Initializer> initializeMemberList = new();

    private VideoPlayer timerPlayer;
    private Result result;

    public float[] odd = { -460, -230, 0, 230, 460 };
 
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            instance.score = 0;
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    public void SetTimer(VideoPlayer videoPlayer)
    {
        timerPlayer = videoPlayer;
        timerPlayer.loopPointReached += PlayerOnloopPointReached;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        score = 0;
    }
    
    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (필수: 메모리 누수 방지)
        SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
    }

    private void PlayerOnloopPointReached(VideoPlayer source)
    {
        result.ShowResult(score);
    }

    private void OnEnable()
    {
        AddInitializeMember();
        foreach (var v in initializeMemberList)
        {
            v.Initialize();
        }
    }

    private void AddInitializeMember()
    {
        if (initializeMembers is null)
        {
            return;
        }
        foreach (var v in initializeMembers)
        {
            var x = v.GetComponent<Initializer>();
            if (x != null) // 컴포넌트가 있을 때만 추가
            {
                initializeMemberList.Add(x);
            }
        }
    }
    

    public void AddScore()
    {
        ++score;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetResult(Result result)
    {
        this.result = result;
    }
}
