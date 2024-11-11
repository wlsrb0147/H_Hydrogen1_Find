using UnityEngine;

public class ReloadLevel : MonoBehaviour
{
    public void ReloadThisLevel()
    {
        GameController.ReloadScene();
    }
}
