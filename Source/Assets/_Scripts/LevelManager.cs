using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    
    public Level[] levels;
    private int currentLevelId = 1;
    private Level currentLevel;
    [Header("Level canvas")]
    public GameObject levelCanvas;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelIntroText;
    public ItemObject giftItemSlot;
    public ItemObject finalItemSlot;

    [Header("Time Limit")]
    public GameObject tlrPanel;
    public TextMeshProUGUI tlrText;
    private float timeLeft;
    public TextMeshProUGUI timeText;

    [Header("Main Menu")]
    public GameObject menuCanvas;
    public Animator creditsAnimator;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool timeGoing = false;

    private GameController gameController;
    private SoundManager soundManager;
   // private bool creditsOn = false;
    private bool menuOn = true;

    void Awake () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        soundManager = gameObject.GetComponent<SoundManager> ();
    }

    void Start () {
        TurnMenu(true);
        currentLevelId = 0;
        currentLevel = levels[0];
        // TODO: load level from save (player prefs)
        
    }  

    public void TurnMenu (bool on) {

        soundManager.StopIntenseSound();
        soundManager.StopTickingSound();
        if (on) {
            soundManager.StopBackground();
            soundManager.PlayTheme();
        } else {
            soundManager.StopTheme();
            soundManager.PlayBackground();
        }

        TurnLosePanel(false);
        TurnWinPanel(false);

        menuOn = on;

        menuCanvas.SetActive(on);
        if (!on) {
            currentLevelId = 0;
            currentLevel = levels[0];
            gameController.RenewList();
            LoadLevel(currentLevel);
        }
    }

    void TurnWinPanel (bool on) {
        winPanel.SetActive(on);
        
        soundManager.StopIntenseSound();
        soundManager.StopTickingSound();
    }

    void TurnLosePanel (bool on) {
        losePanel.SetActive(on);
    }

    public void ToggleCredits (bool on) {
        //creditsOn = !creditsOn;
        creditsAnimator.SetBool ("CreditsOn", on);
    }
    

    void Update () {
        if (!menuOn && currentLevel.hasTLimit && timeGoing) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f) {
                timeLeft = 0f;
                ToggleLevelPanel(false);
                //menuOn = true;
                timeGoing = false;
                TurnLosePanel(true);
                TurnWinPanel(false);
                soundManager.StopIntenseSound();
                soundManager.StopTickingSound();

                soundManager.PlayGameOver();
               // StartCoroutine (soundManager.LoadThemeAfter(soundManager.gameOverSound));
            } 
            int seconds = (int)timeLeft;
            int minutes = (int)(seconds/60);
            seconds %= 60;
            timeText.text = "TIME LEFT: " + minutes.ToString() + ":" + (seconds < 10 ? "0" : "") + seconds.ToString();
            if (soundManager.checkForTimeTicking(timeLeft)) soundManager.PlayClockTicking();
            else if (soundManager.checkForIntense(timeLeft)) soundManager.PlayIntense();
        } else if (!currentLevel.hasTLimit) {
            timeText.text = "";
        }


        //if (Input.GetKeyDown(KeyCode.Space))
        //    gameController.ClearItemSlots();
    }

    public void CheckLevelStatus (Item item) {
        //if (item == null) return;
        if (item == currentLevel.finalItem) {
            currentLevelId++;
            if (currentLevelId < levels.Length) {
                currentLevel = levels[currentLevelId];
                LoadLevel(currentLevel);
                
                soundManager.PlayLevelWin();
                //StartCoroutine (soundManager.LoadThemeAfter(soundManager.levelWinSound));
            } else {
                ToggleLevelPanel(false);
                TurnWinPanel(true);

                timeGoing = false;

                soundManager.PlayWholeGameWin();
                //StartCoroutine (soundManager.LoadThemeAfter(soundManager.wholeGameWinSound));
            }
        } else if (currentLevel.hasTLReduction && item == currentLevel.itemForTLReduction) {
            LoadAdditionalTLReduction();
        }
    }

    public void LoadLevel (Level level) {
        
        soundManager.StopIntenseSound();
        soundManager.StopTickingSound();
        //soundManager.PlayTheme();

        gameController.ClearItemSlots();
        gameController.SaveRestartItems();

        ToggleLevelPanel(true);
        levelNameText.text = "LEVEL " + level.levelId + "\n" + level.levelName;
        levelIntroText.text = level.introductionText;
        giftItemSlot.ChangeItem(level.giftItem);
        finalItemSlot.ChangeItem(level.finalItem);
        timeLeft = level.timeLimit;

        gameController.CreateNewItem(level.giftItem);
    }

    public void ToggleLevelPanel (bool on) {
        levelCanvas.SetActive(on);
        timeGoing = !on;
        gameController.levelGoing = !on;

        if (!on) gameController.CheckForUnusedItems();
    }

    public void ToggleTLRPanel (bool on) {
        tlrPanel.SetActive(on);
        timeGoing = !on;
        gameController.levelGoing = !on;

        if (!on) gameController.CheckForUnusedItems();
    }

    public void LoadAdditionalTLReduction () {
        ToggleTLRPanel(true);
        //levelNameText.text = "LEVEL " + currentLevel.levelId + "\n" + currentLevel.levelName;
        //levelIntroText.text = currentLevel.textForTLReduction;
        tlrText.text = currentLevel.textForTLReduction;
        timeLeft -= currentLevel.timeLimitReduction;
    }

    public void RestartLevel () {
        ToggleTLRPanel(false);
        ToggleLevelPanel(true);
        TurnLosePanel(false);
        TurnWinPanel(false);
        gameController.ReloadLevel();
        
        LoadLevel(currentLevel);

        gameController.CheckForUnusedItems();
    }

    public void QuitGame () {
        Application.Quit();
    }
}
