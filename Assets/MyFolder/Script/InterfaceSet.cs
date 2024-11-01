using System;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // abstract : 반드시 재정의 해야함, 본문 없어야함, 클래스도 abstract class여야함
    // virtual : 재정의 안해도 됨, 본문 필요

    protected virtual void Awake()
    {
  
    }

    public virtual void Initialize()
    {
    }
    

  
}
