using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro
using System.Collections; // For Coroutines

public class UIManager : MonoBehaviour
{
    [Header("Clock and Dropdown Menu")]
    public Button clockButton; // The Clock Button now IS the dropdown trigger
    public RectTransform dropdownMenu; // The Panel containing Pause, Play, FF
    public Button pauseDropdownButton;
    public Button playDropdownButton;
    public Button fastForwardDropdownButton;
    private bool isDropdownActive = false;
    private float dropdownTimer = 0f;
    public float dropdownDelay = 7f;

    [Header("Color Buttons")]
    public GameObject colorButtonsContainer; // Parent with Horizontal Layout Group
    public Button[] colorButtons; // Assign all 7 color buttons in order

    [Header("Resource Buttons")]
    public Button trainButton;
    public TextMeshProUGUI trainCounterText_TMP; // Or Text if not using TMP
    public Button tunnelButton;

    private MainManager gameManager;
    private float normalButtonScale = 1f;
    public float hoverScaleFactor = 1.1f;
    private Color normalClockColor;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<MainManager>();
        if (gameManager == null)
        {
            Debug.LogError("UIManager could not find the GameManager!");
            enabled = false;
            return;
        }

        // Initially hide the dropdown menu
        if (dropdownMenu != null)
        {
            dropdownMenu.gameObject.SetActive(false);
        }

        // Clock button listener (now the only trigger)
        if (clockButton != null)
        {
            clockButton.onClick.AddListener(ToggleDropdownMenu);
            // Store the initial clock color
            Image clockImage = clockButton.GetComponent<Image>();
            if (clockImage != null)
            {
                normalClockColor = clockImage.color;
            }
        }

        // Dropdown button listeners
        if (pauseDropdownButton != null) pauseDropdownButton.onClick.AddListener(PauseGame);
        if (playDropdownButton != null) playDropdownButton.onClick.AddListener(ResumeGame);
        if (fastForwardDropdownButton != null) fastForwardDropdownButton.onClick.AddListener(FastForwardGame);

        // Color button listeners (you'll likely need to pass the color index)
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            if (colorButtons[i] != null)
            {
                colorButtons[i].onClick.AddListener(() => SelectColor(index));
            }
        }

        // Resource button listeners
        if (trainButton != null) trainButton.onClick.AddListener(SelectTrain);
        if (tunnelButton != null) tunnelButton.onClick.AddListener(SelectTunnel);

        // Set initial button scale
        normalButtonScale = clockButton != null ? clockButton.transform.localScale.x : 1f;

        // Initial train counter (now correctly getting the value)
        UpdateTrainCounter();
    }

    void Update()
    {
        // Handle dropdown timer
        if (!isDropdownActive && dropdownTimer < dropdownDelay)
        {
            dropdownTimer += Time.deltaTime;
            if (dropdownTimer >= dropdownDelay)
            {
                // Optionally, you could trigger a visual cue that the clock is ready
                // or just allow it to be clicked.
            }
        }

        // Handle spacebar pause
        if (Input.GetKeyDown(KeyCode.Space) && dropdownTimer >= dropdownDelay)
        {
            TogglePauseGameViaSpacebar();
        }

        // Button hover effect
        HandleButtonHover(clockButton);
        HandleButtonHover(pauseDropdownButton);
        HandleButtonHover(playDropdownButton);
        HandleButtonHover(fastForwardDropdownButton);
        HandleButtonHover(trainButton);
        HandleButtonHover(tunnelButton);
        foreach (var button in colorButtons)
        {
            HandleButtonHover(button);
        }
    }

    void ToggleDropdownMenu()
    {
        if (dropdownTimer >= dropdownDelay)
        {
            isDropdownActive = !isDropdownActive;
            if (dropdownMenu != null)
            {
                dropdownMenu.gameObject.SetActive(isDropdownActive);
            }
        }
    }

    void PauseGame()
    {
        if (gameManager != null)
        {
            gameManager.PauseGame();
            gameManager.SetGameSpeed(1f); // Ensure normal speed when paused
        }
    }

    void ResumeGame()
    {
        if (gameManager != null)
        {
            gameManager.ResumeGame();
            gameManager.SetGameSpeed(1f); // Ensure normal speed when resuming
        }
    }

    void FastForwardGame()
    {
        if (gameManager != null)
        {
            gameManager.SetGameSpeed(2f); // Example: Assuming a SetGameSpeed function
        }
    }

    void TogglePauseGameViaSpacebar()
    {
        if (gameManager != null)
        {
            if (gameManager.isGamePaused)
            {
                gameManager.ResumeGame();
                gameManager.SetGameSpeed(1f); // Ensure normal speed
                // Optionally change clock color back to normal
                if (clockButton != null)
                {
                    Image clockImage = clockButton.GetComponent<Image>();
                    if (clockImage != null)
                    {
                        clockImage.color = normalClockColor;
                    }
                }
            }
            else
            {
                gameManager.PauseGame();
                gameManager.SetGameSpeed(0f); // Ensure paused
                // Change clock color to red
                if (clockButton != null)
                {
                    Image clockImage = clockButton.GetComponent<Image>();
                    if (clockImage != null)
                    {
                        clockImage.color = Color.red;
                    }
                }
            }
        }
    }

    void SelectColor(int colorIndex)
    {
        if (gameManager != null)
        {
            gameManager.SelectColor(colorIndex);
        }
    }

    void SelectTunnel()
    {
        if (gameManager != null)
        {
            gameManager.SelectTunnel();
        }
    }

    void SelectTrain()
    {
        if (gameManager != null)
        {
            if (gameManager.GetAvailableTrains() > 0)
            {
                gameManager.SelectTrain();
                gameManager.UseTrain();
                UpdateTrainCounter(); // Update the UI
            }
            else
            {
                Debug.Log("No more trains available!");
                // Optionally, provide feedback to the player (e.g., a sound, a visual cue on the button)
            }
        }
    }

    public void UpdateTrainCounter()
    {
        if (trainCounterText_TMP != null && gameManager != null)
        {
            trainCounterText_TMP.text = gameManager.GetAvailableTrains().ToString();
        }
        // Update UI for train availability (e.g., disable button if count is 0)
    }

    void HandleButtonHover(Button button)
    {
        if (button != null)
        {
            bool isHovering = RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition, Camera.main);
            if (isHovering)
            {
                button.transform.localScale = Vector3.one * hoverScaleFactor * normalButtonScale;
            }
            else
            {
                button.transform.localScale = Vector3.one * normalButtonScale;
            }
        }
    }
}