using System;
using UnityEngine;

public abstract class Initializer : MonoBehaviour
{
    // abstract : 반드시 재정의 해야함, 본문 없어야함, 클래스도 abstract class여야함
    // virtual : 재정의 안해도 됨, 본문 필요
    private GameManager gameManager;
    
    public virtual void Initialize()
    {
        gameManager.AddInitializeMember(this);
    }
    

  
}
