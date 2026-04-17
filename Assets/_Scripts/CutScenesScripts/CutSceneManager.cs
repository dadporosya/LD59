using Unity.VisualScripting;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public CutSceneBase currentCutScene;
    public string currentCutScenePath;
    [SerializeField] private string pathLabel = "Cutscenes/";

    [SerializeField] private bool runOnStart=false;

    private void Start()
    {
        
        if (runOnStart) RunCutscene();
    }
    
    public void RunCutscene()
    {
        if (currentCutScene)
        {
            RunCutscene(currentCutScene);
            return;
        }

        if (currentCutScenePath != "")
        {
            RunCutscene(currentCutScenePath);
            return;
        }
        
        h.Out("Current cutscene is not assigned");
    }

    public void RunCutscene(CutSceneBase scene)
    {
        scene.Run();
    }

    public void RunCutscene(string path)
    {
        var cutScene = Resources.Load<CutSceneBase>(pathLabel + path);
        if (cutScene)
        {
            h.Out(cutScene);
            cutScene.Run();
        };
        
    }
}
