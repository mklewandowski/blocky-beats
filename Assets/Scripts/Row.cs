using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Globals.Orientations Orientation;
    public bool InHitZone = false;

    [SerializeField]
    GameObject[] Arrows;

    public void SetArrow(Globals.Orientations newOrientation)
    {
        Orientation = newOrientation;
        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].SetActive(i == (int)Orientation);
        }
    }
}
