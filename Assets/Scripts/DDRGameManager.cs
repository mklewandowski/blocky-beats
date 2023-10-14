using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRGameManager : MonoBehaviour
{
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
    float rowTimer = 0;
    float rowTimerMax = 2f;

    int correct = 0;
    int incorrect = 0;
    int missed = 0;

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
            if (r.transform.localPosition.y > 70f)
            {
                r.GetComponent<Row>().InHitZone = true;
            }
            if (r.transform.localPosition.y > 103f)
            {
                deleteFirst = true;
            }
        }
        if (deleteFirst)
        {
            // if (RateCoroutine != null) StopCoroutine(RateCoroutine);
            // RateCoroutine = StartCoroutine(ShowRate("MISSED IT!", new Color(255f/255f, 0, 110f/255f)));
            missed++;
            // UpdateScore();
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
        if (Rows.Count > 0 && Rows[0].GetComponent<Row>().InHitZone)
        {
            if (Rows[0].GetComponent<Row>().Orientation == inputOrientation)
            {
                correct++;
                // StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, Color.yellow, .15f, .3f));
                // if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                // RateCoroutine = StartCoroutine(ShowRate("GREAT!", Color.yellow));
            }
            else 
            {
                incorrect++;
                // StartCoroutine(ShowHighlight(Rows[0].GetComponent<Row>().Orientation, new Color(255f/255f, 0, 110f/255f), .15f, .3f));
                // if (RateCoroutine != null) StopCoroutine(RateCoroutine);
                // RateCoroutine = StartCoroutine(ShowRate("OOPS!", new Color(255f/255f, 0, 110f/255f)));
            }
            Destroy(Rows[0]);
            Rows.RemoveAt(0);
    }
        else
        {
            incorrect++;
            // if (RateCoroutine != null) StopCoroutine(RateCoroutine);
            // RateCoroutine = StartCoroutine(ShowRate("OOPS!", new Color(255f/255f, 0, 110f/255f)));
        }
        // UpdateScore();
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
}
