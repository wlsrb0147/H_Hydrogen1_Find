using UnityEngine;
using UnityEngine.InputSystem;

public class EndPlayer : MonoBehaviour
{
    private void OnGoToMain(InputValue value)
    {
        GameController.LoadScene();
    }
}
