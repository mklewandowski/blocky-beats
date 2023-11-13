using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool AudioOn = true;

    public enum GameStates {
        Title,
        Playing,
        LevelComplete,
        Stats,
    }
    public static GameStates CurrentGameState = GameStates.Title;

    public enum Orientations {
        Left,
        Down,
        Up,
        Right,
        None,
    }

    public enum ScoreQualities {
        Invalid,
        Good,
        Great,
        Perfect
    }

    public static List<Level> Levels = new List<Level>();
    public static void CreateLevels()
    {
        int[] level1Orientations = { // 3 seconds for first to hit
            1, 2, 0, 3, 2, 2, 1, 3, 1, 2, 
            1, 0, 3, 1, 1, 3, 0, 3, 2, 0,
            2, 1, 3, 2, 1, 0, 2, 3, 2, 3, 
            1, 1, 2, 3, 2, 1, 0, 1, 3, 2,
            3, 1, 0, 2, 2, 1, 0, 1
        };
        Level level1 = new Level(1f, 2.2f, level1Orientations);
        Levels.Add(level1);

        int[] level2Orientations = { // 3 seconds for first to hit
            0, 2, 3, 0, 3, 1, 0, 2, 2, 1, 3, 1, 3, 3, 2, 0, 1, 2, 1, 1,
            3, 2, 1, 0, 2, 0, 1, 1, 1, 0, 1, 3, 2, 0, 3, 3, 3, 2, 2, 0,
            0, 3, 0, 1, 0, 2, 1, 3, 1, 3, 0, 0, 2, 0, 3, 3, 0, 3, 2, 1,
            2, 3
        };
        Level level2 = new Level(.9f, 2f, level2Orientations);
        Levels.Add(level2);

    }
    
}
