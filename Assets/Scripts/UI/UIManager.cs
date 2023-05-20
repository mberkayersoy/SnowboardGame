using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("MenuPanel")]
    public GameObject MenuPanel;
    public Button pregameButton;
    public Button menuSettingsButton;
    public Button exitButton;
    public GameObject[] playerList;
    public GameObject[] boardList;
    public Transform spawnpointPlayer;
    public Transform spawnpointSnowboard;
    private GameObject currentPlayer;
    private GameObject currentBoard;
    private int currentPlayerIndex = 0;
    private int currentBoardIndex = 0;
    
    [Space (5)]

    [Header("PreGamePanel")]
    public GameObject PreGamePanel;
    public Button playButton;
    public Button backButtonPreGame;
    public Button[] gameModes;
    private Button selectedMode;

    [Space(5)]

    [Header("Settings Panel")]
    public GameObject SettingsPanel;
    public Slider sliderMusic;
    public Slider sliderSound;
    public Button backButtonSettings;

    [Space(5)]

    [Header("Game Panel")]
    public GameObject GamePanel;
    public Button pauseButton;
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;

    [Space(5)]

    [Header("Pause Panel")]
    public GameObject PausePanel;
    public Button continueButton;
    public Button menuButton;
    public Button pauseSettingsButton;


    string lastPanel;
    string currentPanel;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        totalScore = 0;
        SetActivePanel(MenuPanel.name);
        playButton.interactable = false;
        InstantiateCharacter();
        InstantiateSnowboard();
        pregameButton.onClick.AddListener(OnClickPregameButton);
        menuSettingsButton.onClick.AddListener(OnClickSettingsButton);
        pauseSettingsButton.onClick.AddListener(OnClickSettingsButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        playButton.onClick.AddListener(OnClickPlayButton);
        menuButton.onClick.AddListener(OnClickMenuButton);
        pauseButton.onClick.AddListener(OnClickPauseButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        backButtonPreGame.onClick.AddListener(OnClickBackButton);
        backButtonSettings.onClick.AddListener(OnClickBackButton);
    }

    
    public void SetActivePanel(string activePanel)
    {
        lastPanel = currentPanel;
        MenuPanel.SetActive(activePanel.Equals(MenuPanel.name));
        PreGamePanel.SetActive(activePanel.Equals(PreGamePanel.name));
        SettingsPanel.SetActive(activePanel.Equals(SettingsPanel.name));
        GamePanel.SetActive(activePanel.Equals(GamePanel.name));
        PausePanel.SetActive(activePanel.Equals(PausePanel.name));
        currentPanel = activePanel;
    }
    public void OnClickContinueButton()
    {
        Time.timeScale = 1;
        SetActivePanel(GamePanel.name);
    }
    
    public void OnClickBackButton()
    {
        if (lastPanel == "MenuUI")
        {
            currentPlayer.SetActive(true);
        }
        SetActivePanel(lastPanel);
    }
    public void OnClickPauseButton()
    {
        SetActivePanel(PausePanel.name);
        Time.timeScale = 0;
    }

    public void OnClickPlayButton()
    {
        SetActivePanel(GamePanel.name);
    }
    public void OnClickPregameButton()
    {
        currentPlayer.SetActive(false);
        currentBoard.SetActive(false);
        SetActivePanel(PreGamePanel.name);
    }

    public void OnClickSettingsButton()
    {
        currentPlayer.SetActive(false);
        currentBoard.SetActive(false);
        SetActivePanel(SettingsPanel.name);
    }
    public void OnClickMenuButton()
    {
        Time.timeScale = 1;
        currentPlayer.SetActive(true);
        currentBoard.SetActive(true);
        SetActivePanel(MenuPanel.name);
    }

    public void UpdateScore(int score)
    {
        totalScore += score;
        scoreText.text = "Score: " + totalScore;
    }

    public void SetAllButtonsInteractable()
    {

        foreach (Button button in gameModes)
        {
            button.interactable = true;
        }
    }

    public void OnClickGameModeChanged(Button clickedButton)
    {
        if (selectedMode == clickedButton)
        {
            SetAllButtonsInteractable();
            selectedMode = null;
            playButton.interactable = false;
            return;
        }
        selectedMode = clickedButton;
        playButton.interactable = true;

        foreach (Button button in gameModes)
        {
            if (clickedButton != button) 
                button.interactable = false;
        }
    }

    public void InstantiateCharacter(int whichWay = 0)
    {
        Quaternion lastrotation = transform.rotation;
        if (currentPlayer != null)
        {
            lastrotation = currentPlayer.transform.rotation;
            DOTween.Kill(currentPlayer.transform);
            Destroy(currentPlayer);
        }

        currentPlayerIndex += whichWay;
        if (currentPlayerIndex >= playerList.Length)
        {
            currentPlayerIndex = 0;
        }
        else if (currentPlayerIndex < 0)
        {
            currentPlayerIndex = playerList.Length - 1;
        }

        currentPlayer = Instantiate(playerList[currentPlayerIndex], spawnpointPlayer.position, lastrotation);

        RotateObjects();

    }
    public void InstantiateSnowboard(int whichWay = 0)
    {
        Quaternion lastrotation = transform.rotation;
        if (currentBoard != null)
        {
            lastrotation = currentBoard.transform.rotation;
            DOTween.Kill(currentBoard.transform);
            Destroy(currentBoard);
        }

        currentBoardIndex += whichWay;
        if (currentBoardIndex >= playerList.Length)
        {
            currentBoardIndex = 0;
        }
        else if (currentBoardIndex < 0)
        {
            currentBoardIndex = boardList.Length - 1;
        }

        currentBoard = Instantiate(boardList[currentBoardIndex], spawnpointSnowboard.position, lastrotation);

        RotateObjects();

    }

    void RotateObjects()
    {
        if (currentPlayer != null)
        {
            currentPlayer.transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        }

        if (currentBoard != null)
        {
            currentBoard.transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        }
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
