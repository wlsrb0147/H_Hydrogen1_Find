using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score;

    [SerializeField] private GameObject[] initializeMembers;
    private readonly List<Initializer> initializeMemberList = new();
    private Volume PPvolume;
    private DepthOfField depthOfField;
    private float dof;
    
    private VideoPlayer timerPlayer;
    private Result result;

    [NonSerialized] public float[] odd = { -460, -230, 0, 230, 460 };
    
    private AlphaControl blackEffect;
    public AudioSource audioSource;
    
    private Tweener activeTweener;

    private ZoomScr zoomScr;
    
    public PlayerScr playerScr;
    

    public void SetZoomScr(ZoomScr zoomScr)
    {
        this.zoomScr = zoomScr;
    }
    
    public void SetResult(Result result)
    {
        this.result = result;
    }
    
    public void SetbackEffect(AlphaControl blackEffect)
    {
        this.blackEffect = blackEffect;
    }
    
    public void SetPPvolume(Volume volume)
    {
        PPvolume = volume;
        PPvolume.profile.TryGet(out depthOfField);
        dof = depthOfField.focusDistance.value;
    }
    public void SetTimer(VideoPlayer videoPlayer)
    {
        timerPlayer = videoPlayer;
        timerPlayer.loopPointReached += PlayerOnloopPointReached;
    }
    public int GetScore()
    {
        return score;
    }

    public void TimerPause()
    {
        timerPlayer.Pause();
    }

    
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
    public void TimerResume()
    {
        timerPlayer.Play();
    }

    public void BlurDepthAndBlack()
    {
        if (activeTweener != null && activeTweener.IsActive())
        {
            activeTweener.Kill();
        }
        
        activeTweener = DOTween.To(
            () => depthOfField.focusDistance.value,        // 현재 focusDistance 값 가져오기
            x => depthOfField.focusDistance.value = x,     // 계산된 값을 focusDistance.value에 설정
            0.1f,                                          // 목표 값 (0.1f로 설정)
            0.5f                                             
        );
        blackEffect.gameObject.SetActive(false);
        blackEffect.gameObject.SetActive(true);
    }

    public void BlackEffectReturn()
    {
        blackEffect.DisableWithFade();
    }

    public void BlurReturn()
    {
        if (activeTweener != null && activeTweener.IsActive())
        {
            activeTweener.Kill();
        }
        
        activeTweener = DOTween.To(() => depthOfField.focusDistance.value,    // 현재 focusDistance 값
            x => depthOfField.focusDistance.value = dof, // 변경된 값을 할당
            dof,                                       // 목표 값
            2f);                                 // 애니메이션 지속 시간
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
        ShowResult();
    }

    public void ShowResult()
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


}
