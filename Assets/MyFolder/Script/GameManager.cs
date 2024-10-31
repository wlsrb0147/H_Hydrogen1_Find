using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Initializer> initializeMembers;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void AddInitializeMember(Initializer initializer)
    {
        initializeMembers.Add(initializer);
    }

    public void Initialize()
    {
        foreach (var v in initializeMembers)
        {
            v.Initialize();
        }
    }
}
