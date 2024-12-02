using System;
using UnityEngine;

public class ZoomScr : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject[] objects;
    private void Awake()
    {
        gameManager = GameManager.instance;
        gameObject.SetActive(false);
        objects[0].SetActive(true);
        objects[1].SetActive(false);
        gameManager.SetZoomScr(this);
    }
}
