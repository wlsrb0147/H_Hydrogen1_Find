using System;
using UnityEngine;

public class ZoomScr : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private ZoomAlpha[] objects;


    private bool isZoomed;
    
    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.zoomScr = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        objects[0].gameObject.SetActive(true);
        objects[1].gameObject.SetActive(false);
    }

    public void ToggleZoom()
    {
        isZoomed = !isZoomed;

        if (isZoomed)
        {
            objects[0].Close();
            objects[1].gameObject.SetActive(true);
        }
        else
        {
            objects[1].Close();
            objects[0].gameObject.SetActive(true);
        }
        
    }
}
