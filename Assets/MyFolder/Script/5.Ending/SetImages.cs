using System;
using UnityEngine;
using UnityEngine.UI;

public class SetImages : MonoBehaviour
{
    [SerializeField] private Image[] images;
    private void OnEnable()
    {
        GameController.LoadImage(images);
    }
}
