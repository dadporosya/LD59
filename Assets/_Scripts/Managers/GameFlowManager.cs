using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public enum States
    {
        Intro,
        Game,
        Pause
    }

    public States state = States.Intro;

    public void SetOnPause()
    {
        state = States.Pause;
        h.Out("oause");
    }

    public void SetOnGame()
    {
        state = States.Game;
        h.Out("game");
    }

    public bool IsPaused()
    {
        return state == States.Pause;
    }
}
