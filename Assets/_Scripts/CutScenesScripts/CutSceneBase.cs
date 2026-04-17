using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CutSceneBase : MonoBehaviour
{
    public List<IEnumerator> cutsceneSteps = new List<IEnumerator>();
    [SerializeField] private bool runOnStart = false;

    private bool initialized=false;

    void Start()
    {
        if (runOnStart) Run();
    }

    public virtual void Init()
    {
        initialized = true;
    }

    private void Awake() // ?
    {
        Init();
    }

    public void Run()
    {
        // if (!initialized) Init();
        CutSceneBase instance = Instantiate(this);
        instance.StartCoroutine(instance.ExecuteSequence(instance.gameObject));
    }

    public IEnumerator ExecuteSequence(GameObject instanceToDestroy = null)
    {
        h.Out("Execute Sequence");
        if (!initialized) Init();
        h.Out(cutsceneSteps);
        
        foreach (IEnumerator step in cutsceneSteps)
        {
            h.Out(step);
            yield return StartCoroutine(step);
        }

        h.Out("Cutscene complete.");
    
        if (instanceToDestroy) Destroy(instanceToDestroy);
    }
    
    public IEnumerator FadeIn(float duration)
    {
        h.Out("FadeIN");
        yield return ScreenManager.Instance.FadeRoutine(0, 1, duration);
    }
    
    public IEnumerator FadeOut(float duration)
    {
        h.Out("Fadeout");
        yield return ScreenManager.Instance.FadeRoutine(1, 0, duration);
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}