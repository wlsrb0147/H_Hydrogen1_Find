using UnityEngine;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject obj;

    public void OnPointerClick(PointerEventData eventData)
    {
        obj.SetActive(true);
        gameObject.SetActive(false);
    }
}
