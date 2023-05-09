using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotRightClick : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.localPlayer.trashCard(gameObject.GetComponent<Slot>().Index + GameManager.slotManager.shift);
        }
    }
}