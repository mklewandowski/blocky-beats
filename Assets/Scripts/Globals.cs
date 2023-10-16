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
            2, 2, 1, 0, 1, 2, 3, 0, 1, 2,
            0, 1, 2, 3, 0, 1, 2, 0, 1, 2,
            0, 1, 2, 3, 0, 1, 2, 0, 1, 2,
            0, 1, 2, 3, 0, 1, 2, 0, 1, 2,
            0, 1, 2, 3, 0, 1, 2, 2,
        };
        Level level1 = new Level(1f, level1Orientations);
        Levels.Add(level1);

    }
    
}
