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

    // Start is called before the first frame update
    void Start()
    {
        Title.GetComponent<MoveNormal>().MoveDown();
        StartContainer.GetComponent<MoveNormal>().MoveUp();        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MoveRows();
        HandleRowCreation();
    }

    public void HandleInput()
    {

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
            // missed++;
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
    }

    public void SelectUp()
    {

    }
    public void SelectLeft()
    {
        
    }
    public void SelectDown()
    {
        
    }
    public void SelecRight()
    {
        
    }

    void CreateRow()
    {
        GameObject row = Instantiate(RowPrefab, new Vector3(0, -100f, 0), Quaternion.identity, RowContainer.transform);
        RectTransform rt = row.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        Globals.Orientation newOrientation = (Globals.Orientation)Random.Range(0, 4);
        row.GetComponent<Row>().SetArrow(newOrientation);
        row.GetComponent<Row>().Orientation = newOrientation;
        Rows.Add(row);
    }
}
