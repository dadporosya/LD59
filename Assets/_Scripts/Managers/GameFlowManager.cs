using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public enum States
    {
        Intro,
        Game, 
    }

    public States state = States.Intro;
}
