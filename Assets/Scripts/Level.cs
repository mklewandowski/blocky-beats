using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public float TimeInterval = 1f;

    public List<int> Orientations;

    public Level(float tInterval, int[] orientations)
    {
        TimeInterval = tInterval;
        Orientations = new List<int>();
        foreach (int i in orientations)
        {
            Orientations.Add(i);
        }
    }
}
