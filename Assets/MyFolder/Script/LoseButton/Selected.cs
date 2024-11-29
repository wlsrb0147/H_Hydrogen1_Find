using System;
using UnityEngine;
using UnityEngine.UI;

public class Selected : MonoBehaviour
{
    [SerializeField] private GameObject finger;
    [SerializeField] private GameObject[] sprites;

    public void ToggleImage(bool selected)
    {
        if (selected)
        {
            sprites[0].SetActive(true);
            sprites[1].SetActive(false);
        }
        else
        {
            sprites[0].SetActive(false);
            sprites[1].SetActive(true);
        }
        
        finger.SetActive(selected);
    }
}
