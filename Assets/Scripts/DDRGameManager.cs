using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

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
    List<GameObject> Rows = new List<GameObject>();
    float rowTimer = 2f;
    float rowTimerMax = 1.0f;
    [SerializeField]
    TextMeshProUGUI Score;
    [SerializeField]
    GameObject Rate;
    [SerializeField]
    TextMeshProUGUI RateText;
    [SerializeField]
    TextMeshProUGUI RateRearText;

    Coroutine RateCoroutine;

    int good = 0;
    int great = 0;
    int perfect = 0;
    int incorrect = 0;
    int missed = 0;
    float inGoodThreshold = 70f;
    float inGreatThreshold = 83f;
    float inPerfectThreshold = 96f;
    float destroyThreshold = 103f;

    Color goodColor = new Color(239f/255f, 210f/255f, 153f/255f);
    Color badColor = new Color(195f/255f, 93f/255f, 93f/255f);

    void Awake()
    {
        audioManager = this.GetComponent<AudioManager>();
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
    }

    void PlayGame()
    {
        if (Globals.CurrentGameState != Globals.GameStates.Playing)
            return;
        HandleInput();
        MoveRows();
        HandleRowCreation();
    }

    public void MoveRows()
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
            RateCoroutine = StartCoroutine(ShowRate("MISSED IT!", badColor));
            missed++;
            UpdateScore();
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
        Globals.CurrentGameState = Globals.GameStates.Playing;
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
                    RateCoroutine = StartCoroutine(ShowRate("GOOD!", goodColor));
                }
                else if (Rows[0].GetComponent<Row>().CurrentScoreQuality == Globals.ScoreQualities.Great)
                {
                    great++;
                    if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                    RateCoroutine = StartCoroutine(ShowRate("GREAT!", goodColor));
                }
                else if (Rows[0].GetComponent<Row>().CurrentScoreQuality == Globals.ScoreQualities.Perfect)
                {
                    perfect++;
                    if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                    RateCoroutine = StartCoroutine(ShowRate("PERFECT!!", goodColor));
                }
                StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, Color.yellow, .15f, .3f));
            }
            else 
            {
                incorrect++;
                StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, new Color(255f/255f, 0, 110f/255f), .15f, .3f));
                if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                RateCoroutine = StartCoroutine(ShowRate("OOPS!", badColor));
            }
            Destroy(Rows[0]);
            Rows.RemoveAt(0);
        }
        else
        {
            incorrect++;
            if (RateCoroutine != null) StopCoroutine(RateCoroutine);
            RateCoroutine = StartCoroutine(ShowRate("OOPS!", badColor));
        }
        UpdateScore();
    }

    void CreateRow()
    {
        GameObject row = Instantiate(RowPrefab, new Vector3(0, -100f, 0), Quaternion.identity, RowContainer.transform);
        RectTransform rt = row.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        Globals.Orientations newOrientation = (Globals.Orientations)Random.Range(0, 4);
        row.GetComponent<Row>().SetArrow(newOrientation);
        row.GetComponent<Row>().Orientation = newOrientation;
        Rows.Add(row);
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
