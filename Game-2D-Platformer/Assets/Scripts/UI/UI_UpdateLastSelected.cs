using UnityEngine;
using UnityEngine.EventSystems;

public class UI_UpdateLastSelected : MonoBehaviour, ISelectHandler
{
    private UI_MainMenu mainMenu;

    private void Awake()
    {
        mainMenu = GetComponentInParent<UI_MainMenu>();    
    }

    public void OnSelect(BaseEventData eventData)
    {
        mainMenu.UpdateLastSelected(this.gameObject);
    }
}
