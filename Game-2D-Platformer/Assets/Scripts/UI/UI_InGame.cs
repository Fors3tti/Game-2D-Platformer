using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    private PlayerInput playerInput;
    private Player player;
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect { get; private set; }

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI fruitText;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused;
 
    private void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.UI.Pause.performed += ctx => PauseButton();
    }

    private void OnDisable()
    {
        playerInput.Disable();

        playerInput.UI.Pause.performed -= ctx => PauseButton();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PauseButton();
    }

    public void PauseButton()
    {
        player = PlayerManager.instance.player;

        if (isPaused)
            UnpauseTheGame();
        else
            PauseTheGame();
    }

    private void UnpauseTheGame()
    {
        player.playerInput.Enable();
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    private void PauseTheGame()
    {
        player.playerInput.Disable();
        isPaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        fruitText.text = collectedFruits + "/" + totalFruits;
    }

    public void UpdateTimerUI(float timer)
    {
        timerText.text = timer.ToString("00") + " s";
    }
}
