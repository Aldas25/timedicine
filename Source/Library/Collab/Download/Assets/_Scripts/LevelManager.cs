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
    private float timeLeft;
    public TextMeshProUGUI timeText;

    [Header("Main Menu")]
    public GameObject menuCanvas;
    public Animator creditsAnimator;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool timeGoing = false;

    private GameController gameController;
    private bool creditsOn = false;
    private bool menuOn = true;

    void Awake () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start () {
        TurnMenu(true);
        currentLevelId = 0;
        currentLevel = levels[0];
        // TODO: load level from save (player prefs)
        
    }  

    public void TurnMenu (bool on) {
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
    }

    void TurnLosePanel (bool on) {
        losePanel.SetActive(on);
    }

    public void ToggleCredits () {
        creditsOn = !creditsOn;
        creditsAnimator.SetBool ("CreditsOn", creditsOn);
    }
    

    void Update () {
        if (!menuOn && currentLevel.hasTLimit && timeGoing) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f) {
                timeLeft = 0f;
                ToggleLevelPanel(false);
                menuOn = true;
                TurnLosePanel(true);
            } 
            int seconds = (int)timeLeft;
            int minutes = (int)(seconds/60);
            seconds %= 60;
            timeText.text = "TIME LEFT: " + minutes.ToString() + ":" + (seconds < 10 ? "0" : "") + seconds.ToString();
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
            } else {
                ToggleLevelPanel(false);
                TurnWinPanel(true);
            }
        } else if (currentLevel.hasTLReduction && item == currentLevel.itemForTLReduction) {
            LoadAdditionalTLReduction();
        }
    }

    public void LoadLevel (Level level) {
        gameController.ClearItemSlots();

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
    }

    public void LoadAdditionalTLReduction () {
        ToggleLevelPanel(true);
        levelNameText.text = "LEVEL " + currentLevel.levelId + "\n" + currentLevel.levelName;
        levelIntroText.text = currentLevel.textForTLReduction;
        timeLeft -= currentLevel.timeLimitReduction;
    }
}
