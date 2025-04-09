using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Mobile Controls")]
    public GameObject mobileControls;
    public Button jumpButton;
    public Button attackButton;
    public Joystick movementJoystick;

    [Header("Game UI")]
    public Text livesText;
    public Text coinsText;
    public Text levelText;
    public GameObject pauseMenu;
    public GameObject gameOverScreen;

    private PlayerController playerController;
    private CharacterSystem characterSystem;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        characterSystem = FindObjectOfType<CharacterSystem>();

        #if UNITY_ANDROID || UNITY_IOS
        mobileControls.SetActive(true);
        jumpButton.onClick.AddListener(OnJumpPressed);
        attackButton.onClick.AddListener(OnAttackPressed);
        movementJoystick.OnStickValueChanged += OnMovementChanged;
        #else
        mobileControls.SetActive(false);
        #endif

        UpdateUI();
    }

    private void OnMovementChanged(Vector2 movement)
    {
        playerController.SetMovement(movement.x);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void UpdateUI()
    {
        livesText.text = "Lives: " + GameManager.Instance.lives;
        coinsText.text = "Coins: " + GameManager.Instance.coins;
        levelText.text = "World " + GameManager.Instance.currentLevel;
    }

    private void OnJumpPressed()
    {
        playerController.Jump();
    }

    private void OnAttackPressed()
    {
        characterSystem.UseSpecialAbility();
        // Visual feedback
        StartCoroutine(ButtonPressAnimation(attackButton));
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    private void OnSwitchCharacterPressed()
    {
        characterSystem.SwitchToNextCharacter();
        // Visual feedback
        StartCoroutine(ButtonPressAnimation(switchCharacterButton));
    }
}
