using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;
    [SerializeField] private RectTransform rectTrans;
    [SerializeField] private float scrollSpeed = 200;
    [SerializeField] private float offScreenPosition = 1500;

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private bool creditsSkipped;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
        fadeEffect.ScreenFade(0, 1);
    }

    private void Update()
    {
        rectTrans.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectTrans.anchoredPosition.y > offScreenPosition)
            GoToMainMenu();
    }

    public void SkipCredits()
    {
        if(creditsSkipped == false)
        {
            scrollSpeed *= 10;
            creditsSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu() => fadeEffect.ScreenFade(1, 1, SwitchToMenuScene);

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
