using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public float TimeInterval = 1f;

    public float Delay = 2f;

    public List<int> Orientations;

    public Level(float tInterval, float tDelay, int[] orientations)
    {
        TimeInterval = tInterval;
        Delay = tDelay;
        Orientations = new List<int>();
        foreach (int i in orientations)
        {
            Orientations.Add(i);
        }
    }
}
