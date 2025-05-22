using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
    private int levelIndex;
    public string sceneName;

    public void SetupButton(int newLevelIndex)
    {
        levelIndex = newLevelIndex;

        levelNumberText.text = "Level " + levelIndex;
        sceneName = "Level_" + levelIndex;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
