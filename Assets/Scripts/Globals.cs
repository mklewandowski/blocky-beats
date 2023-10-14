using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
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

}
