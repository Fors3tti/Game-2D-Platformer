using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;

    public void PreviousSkin()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = maxIndex;

        UpdatedSkinDisplay();
    }

    public void NextSkin()
    {
        currentIndex++;

        if (currentIndex > maxIndex)
            currentIndex = 0;

        UpdatedSkinDisplay();
    }

    private void UpdatedSkinDisplay()
    {
        for(int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);
    }
}
