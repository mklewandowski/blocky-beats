using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject Play;
    [SerializeField]
    GameObject PlayButtons;
    // Start is called before the first frame update
    void Start()
    {
        Title.GetComponent<MoveNormal>().MoveDown();
        Play.GetComponent<MoveNormal>().MoveUp();        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {

    }

    public void StartGame()
    {      
        Title.GetComponent<MoveNormal>().MoveUp();
        Play.GetComponent<MoveNormal>().MoveDown();   
        PlayButtons.GetComponent<MoveNormal>().MoveUp();   
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
}
