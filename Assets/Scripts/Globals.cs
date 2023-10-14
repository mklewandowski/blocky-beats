using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool AudioOn = true;

    public enum GameStates {
        Title,
        Playing
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
}
