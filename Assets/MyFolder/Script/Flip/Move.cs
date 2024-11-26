using System;
using DG.Tweening;
using UnityEngine;

public class Move : MonoBehaviour
{
    private ImageFlip imageFlip;

    private void Awake()
    {
        imageFlip = GetComponent<ImageFlip>();
    }

    public void SetPosition(Vector2 pos)
    {
        transform.DOMoveX(pos.x, 2f);
        
        Invoke(nameof(imageFlip.FlipCard), 2f);
    }
}
