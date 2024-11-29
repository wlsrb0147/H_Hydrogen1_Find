using UnityEngine;
using UnityEngine.Rendering;

public class SetVolume : MonoBehaviour
{
    private GameManager gameManager;
    private Volume volume;
    private void Awake()
    {
        gameManager = GameManager.instance;
        volume = GetComponent<Volume>();
        gameManager.SetPPvolume(volume);
    }
}
