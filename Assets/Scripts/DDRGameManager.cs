using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class DDRGameManager : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject StartContainer;
    [SerializeField]
    GameObject PlayButtons;
    [SerializeField]
    GameObject PlayField;
    [SerializeField]
    GameObject[] Highlights;
    [SerializeField]
    GameObject[] Glows;
    [SerializeField]
    GameObject RowPrefab;
    [SerializeField]
    GameObject RowContainer;
    [SerializeField]
    TextMeshProUGUI Score;
    [SerializeField]
    GameObject Rate;
    [SerializeField]
    TextMeshProUGUI RateText;
    [SerializeField]
    TextMeshProUGUI RateRearText;
    [SerializeField]
    GameObject Combo;
    [SerializeField]
    TextMeshProUGUI ComboText;
    [SerializeField]
    TextMeshProUGUI ComboRearText;
    [SerializeField]
    GameObject LevelComplete;
    [SerializeField]
    GameObject LevelStats;
    [SerializeField]
    TextMeshProUGUI LevelStatsText;
    [SerializeField]
    GameObject LevelScore;
    [SerializeField]
    GameObject LevelScorePercent;

    Coroutine RateCoroutine;

    int good = 0;
    int great = 0;
    int perfect = 0;
    int incorrect = 0;
    int missed = 0;
    int combo = 0;
    float inGoodThreshold = 70f;
    float inGreatThreshold = 83f;
    float inPerfectThreshold = 96f;
    float destroyThreshold = 103f;

    Color goodColor = new Color(255f/255f, 216f/255f, 0/255f);
    Color badColor = new Color(255f/255f, 0/255f, 0/255f);

    List<GameObject> Rows = new List<GameObject>();
    float rowTimer = 2f;
    float rowTimerMax = 1.0f;
    int levelNum = 0;
    float levelDelay = 2f;
    int rowIndex = 0;
    float endLevelDelay = 3f;

    float gameTime = 0;
    string levelStatsString = "";

    void Awake()
    {
        audioManager = this.GetComponent<AudioManager>();

        Globals.CreateLevels();
    }

    // Start is called before the first frame update
    void Start()
    {
        Title.GetComponent<MoveNormal>().MoveDown();
        StartContainer.GetComponent<MoveNormal>().MoveUp();        
    }

    // Update is called once per frame
    void Update()
    {
        PlayGame();
        EndLevel();
    }

    void PlayGame()
    {
        if (Globals.CurrentGameState != Globals.GameStates.Playing)
            return;
        MoveRows();
        HandleMusic();
        HandleInput();
        HandleRowCreation();
        gameTime += Time.deltaTime;
    }

    void EndLevel()
    {
        if (Globals.CurrentGameState != Globals.GameStates.LevelComplete)
            return;
        MoveRows();
        HideLevelComplete();
    }

    void HideLevelComplete()
    {
        if (endLevelDelay > 0)
        {
            endLevelDelay -= Time.deltaTime;
            if (endLevelDelay <= 0)
            {
                PlayButtons.GetComponent<MoveNormal>().MoveDown();   
                PlayField.GetComponent<MoveNormal>().MoveUp(); 
                LevelStats.GetComponent<MoveNormal>().MoveDown();   
            }
        }
    }

    void HandleMusic()
    {
        if (levelDelay > 0)
        {
            levelDelay -= Time.deltaTime;
            if (levelDelay <= 0)
            {
                audioManager.StartMusic(levelNum);
            }
        }
    }

    void MoveRows()
    {
        bool deleteFirst = false;
        foreach (GameObject r in Rows)
        {
            float speed = 100f;
            r.transform.localPosition = new Vector3(r.transform.localPosition.x, r.transform.localPosition.y + speed * Time.deltaTime, r.transform.localPosition.z);
            Row row = r.GetComponent<Row>();
            if (r.transform.localPosition.y >= inGoodThreshold && r.transform.localPosition.y < inGreatThreshold && row.CurrentScoreQuality != Globals.ScoreQualities.Good)
            {
                r.GetComponent<Row>().SetGood();
            }
            else if (r.transform.localPosition.y >= inGreatThreshold && r.transform.localPosition.y < inPerfectThreshold && row.CurrentScoreQuality != Globals.ScoreQualities.Great)
            {
                r.GetComponent<Row>().SetGreat();
            }
            else if (r.transform.localPosition.y >= inPerfectThreshold && r.transform.localPosition.y < destroyThreshold && row.CurrentScoreQuality != Globals.ScoreQualities.Perfect)
            {
                r.GetComponent<Row>().SetPerfect();
            }
            else if (r.transform.localPosition.y >= destroyThreshold)
            {
                deleteFirst = true;
            }
        }
        if (deleteFirst)
        {
            if (RateCoroutine != null) StopCoroutine(RateCoroutine);
            RateCoroutine = StartCoroutine(ShowRate("MISS", badColor));
            missed++;
            combo = 0;
            HideCombo();
            UpdateScore();
            if (Rows[0].GetComponent<Row>().IsLast)
                CompleteLevel();
            Destroy(Rows[0]);
            Rows.RemoveAt(0);
        }
    }

    public void HandleRowCreation()
    {
        rowTimer -= Time.deltaTime;
        if (rowTimer <= 0)
        {
            CreateRow();
            rowTimer = rowTimerMax;
        }
    }

    public void StartGame()
    {      
        audioManager.StopMusic();
        audioManager.PlayButtonSound();
        Title.GetComponent<MoveNormal>().MoveUp();
        StartContainer.GetComponent<MoveNormal>().MoveDown();   
        PlayButtons.GetComponent<MoveNormal>().MoveUp();   
        PlayField.GetComponent<MoveNormal>().MoveDown(); 
        StartLevel();
    }

    public void StartLevel()
    {
        rowTimerMax = Globals.Levels[levelNum].TimeInterval;
        Globals.CurrentGameState = Globals.GameStates.Playing;
        rowTimer = 2f;
        levelDelay = 2f;
        rowIndex = 0;
   }

    public void HandleInput()
    {
        Globals.Orientations inputOrientation = Globals.Orientations.None;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputOrientation = Globals.Orientations.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputOrientation = Globals.Orientations.Right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputOrientation = Globals.Orientations.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputOrientation = Globals.Orientations.Down;
        }
        if (inputOrientation != Globals.Orientations.None)
            VetInput(inputOrientation);
    }

    public void SelectUp()
    {
        VetInput(Globals.Orientations.Up);
    }
    public void SelectDown()
    {
        VetInput(Globals.Orientations.Down);
    }
    public void SelectLeft()
    {
        VetInput(Globals.Orientations.Left);
    }
    public void SelectRight()
    {
        VetInput(Globals.Orientations.Right);
    }

    void VetInput(Globals.Orientations inputOrientation)
    {
        if (Rows.Count > 0 && Rows[0].GetComponent<Row>().CurrentScoreQuality != Globals.ScoreQualities.Invalid)
        {
            if (Rows[0].GetComponent<Row>().Orientation == inputOrientation)
            {
                if (Rows[0].GetComponent<Row>().CurrentScoreQuality == Globals.ScoreQualities.Good)
                {
                    good++;
                    if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                    RateCoroutine = StartCoroutine(ShowRate("GOOD", goodColor));
                    combo++;
                    if (combo > 1)
                        ShowCombo();
                }
                else if (Rows[0].GetComponent<Row>().CurrentScoreQuality == Globals.ScoreQualities.Great)
                {
                    great++;
                    if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                    RateCoroutine = StartCoroutine(ShowRate("GREAT", goodColor));
                    combo++;
                    if (combo > 1)
                        ShowCombo();
                }
                else if (Rows[0].GetComponent<Row>().CurrentScoreQuality == Globals.ScoreQualities.Perfect)
                {
                    perfect++;
                    if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                    RateCoroutine = StartCoroutine(ShowRate("PERFECT", goodColor));
                    combo++;
                    if (combo > 1)
                        ShowCombo();
                }
                StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, Color.yellow, .15f, .3f));
            }
            else 
            {
                incorrect++;
                StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, new Color(255f/255f, 0, 110f/255f), .15f, .3f));
                if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                RateCoroutine = StartCoroutine(ShowRate("OOPS", badColor));
                combo = 0;
                HideCombo();
            }
            if (Rows[0].GetComponent<Row>().IsLast)
                CompleteLevel();
            Destroy(Rows[0]);
            Rows.RemoveAt(0);
        }
        else
        {
            incorrect++;
            if (RateCoroutine != null) StopCoroutine(RateCoroutine);
            RateCoroutine = StartCoroutine(ShowRate("OOPS", badColor));
            combo = 0;
            HideCombo();
        }
        UpdateScore();
    }

    void ShowCombo()
    {
        ComboText.text = combo.ToString();
        ComboRearText.text = combo.ToString();
        Combo.transform.localScale = new Vector3(.1f, .1f, .1f);
        Combo.SetActive(true);
        Combo.GetComponent<GrowAndShrink>().StartEffect();
    }

    void HideCombo()
    {
        Combo.SetActive(false);
    }

    void CreateRow()
    {
        if (rowIndex >= Globals.Levels[levelNum].Orientations.Count)
            return;
        Globals.Orientations newOrientation = (Globals.Orientations)Globals.Levels[levelNum].Orientations[rowIndex];
        
        if (newOrientation != Globals.Orientations.None)
        {
            GameObject row = Instantiate(RowPrefab, new Vector3(0, -100f, 0), Quaternion.identity, RowContainer.transform);
            RectTransform rt = row.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
            rt.transform.localPosition = new Vector3(rt.transform.localPosition.x, -200f, rt.transform.localPosition.z);
            row.GetComponent<Row>().SetArrow(newOrientation);
            row.GetComponent<Row>().Orientation = newOrientation;
            if (rowIndex >= Globals.Levels[levelNum].Orientations.Count - 1)
            {
                row.GetComponent<Row>().IsLast = true;
            }
            Rows.Add(row);
        }
        rowIndex++;
    }

    void CompleteLevel()
    {
        Globals.CurrentGameState = Globals.GameStates.LevelComplete;
        LevelComplete.transform.localScale = new Vector3(.1f, .1f, .1f);
        LevelComplete.SetActive(true);
        LevelComplete.GetComponent<GrowAndShrink>().StartEffect();
        audioManager.PlayCompleteSound();

        // prepare stats
        LevelScore.SetActive(false);
        LevelScorePercent.SetActive(false);
        LevelStatsText.text = "";
        levelStatsString = "Good: " + good + "\n";
        levelStatsString += "Great: " + great + "\n";
        levelStatsString += "Perfect: " + perfect + "\n";
        levelStatsString += "Incorrect: " + incorrect + "\n";
        levelStatsString += "Missed: " + missed + "\n";
    }

    void UpdateScore()
    {
        Score.text = "Good: " + good + "\n";
        Score.text += "Great: " + great + "\n";
        Score.text += "Perfect: " + perfect + "\n";
        Score.text += "Incorrect: " + incorrect + "\n";
        Score.text += "Missed: " + missed + "\n";
    }

    IEnumerator ShowHighlight(Globals.Orientations o, Color c, float inTime, float outTime)
    {
        int index = (int)o;
        float maxInTime = inTime;
        float maxOutTime = outTime;
        Highlights[index].GetComponent<Image>().color = c;
        Glows[index].GetComponent<Image>().color = c;
        Highlights[index].SetActive(true);
        Glows[index].SetActive(true);
        while (inTime >= 0.0f) 
        {
            Glows[index].GetComponent<Image>().color = new Color(c.r, c.g, c.b, 1f - inTime / maxInTime);
            inTime -= Time.deltaTime;
            yield return null; 
        }
        while (outTime >= 0.0f) 
        {
            Glows[index].GetComponent<Image>().color = new Color(c.r, c.g, c.b, outTime / maxOutTime);
            Highlights[index].GetComponent<Image>().color = new Color(c.r, c.g, c.b, outTime / maxOutTime);
            outTime -= Time.deltaTime;
            yield return null; 
        }
        Highlights[index].SetActive(false);
        Glows[index].SetActive(false);
    }

    IEnumerator ShowRate (string text, Color c)
    {
        float maxTime = .7f;
        RateText.color = c;
        RateText.text = text;
        RateRearText.text = text;
        Rate.transform.localScale = new Vector3(.1f, .1f, .1f);
        Rate.SetActive(true);
        Rate.GetComponent<GrowAndShrink>().StartEffect();
        while (maxTime >= 0.0f) 
        {
            maxTime -= Time.deltaTime;
            yield return null; 
        }
        Rate.SetActive(false);
    }
}
