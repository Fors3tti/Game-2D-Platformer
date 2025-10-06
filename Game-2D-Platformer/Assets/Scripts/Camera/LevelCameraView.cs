using UnityEngine;

public class LevelCameraView : MonoBehaviour
{
    private LevelCamera levelCamera;
    private int playersInView;

    private void Awake()
    {
        levelCamera = GetComponentInParent<LevelCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            playersInView++;

            if (playersInView >= PlayerManager.instance.GetPlayerList().Count)
            {
                // decrease size of lens
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            playersInView--;

            if (playersInView < PlayerManager.instance.GetPlayerList().Count)
            {
                // increase size of lens
            }
        }
    }
}
