using UnityEngine;

public class UI_Credits : MonoBehaviour
{
    [SerializeField] private RectTransform rectTrans;
    [SerializeField] private float scrollSpeed = 200;

    private void Update()
    {
        rectTrans.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}
