using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Globals.Orientations Orientation;
    public bool IsLast = false;
    public Globals.ScoreQualities CurrentScoreQuality = Globals.ScoreQualities.Invalid;

    [SerializeField]
    GameObject[] Arrows;

    [SerializeField]
    GameObject[] Good;
    [SerializeField]
    GameObject[] Great;
    [SerializeField]
    GameObject[] Perfect;

    public void SetArrow(Globals.Orientations newOrientation)
    {
        Orientation = newOrientation;
        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].SetActive(i == (int)Orientation);
        }
    }

    public void SetGood()
    {
        CurrentScoreQuality = Globals.ScoreQualities.Good;
        foreach (GameObject g in Good)
        {
            g.SetActive(true);
        }
    }
    public void SetGreat()
    {
        CurrentScoreQuality = Globals.ScoreQualities.Great;
        foreach (GameObject g in Great)
        {
            g.SetActive(true);
        }
    }
    public void SetPerfect()
    {
        CurrentScoreQuality = Globals.ScoreQualities.Perfect;
        foreach (GameObject g in Perfect)
        {
            g.SetActive(true);
        }
    }
}
