using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Preferences
{
    public static float shiftingAnimationPeriod = 0.3f;
    public static bool defaultCameraShake=true;
    public static float floatingSpeed = 0.1f;
    
    
    [Header("ACTIONS")]
    public static Dictionary<string, InputActionReference> actionLabels = new Dictionary<string, InputActionReference>()
    {
        { "0", new InputActionReference() },
        { "1", new InputActionReference() },
    };
}
