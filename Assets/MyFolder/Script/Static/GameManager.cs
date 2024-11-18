using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score;

    [SerializeField] private GameObject[] initializeMembers;
    private readonly List<Initializer> initializeMemberList = new();

    [SerializeField] private VideoPlayer player;
    [SerializeField] private GameObject Renderer;
 
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            player.loopPointReached += PlayerOnloopPointReached;
        }
        else
        {
            instance.score = 0;
            Destroy(gameObject);
        }
    }

    private void PlayerOnloopPointReached(VideoPlayer source)
    {
        GameController.LoadScene();
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
}
