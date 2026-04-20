using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using M = System.Math;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using EZCameraShake;
using Unity.VisualScripting;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class h
{
    // Math
    public static int Sign(int n)
    {
        if (n > 0) return 1;
        if (n < 0) return -1;
        return 0;
    }
    public static float Sign(float n)
    {
        if (n > 0) return 1;
        if (n < 0) return -1;
        return 0;
    }

    public static float Max(params float[] args)
    {
        float result=args[0];
        for (int i = 1; i<args.Length; i++)
        {
            result = M.Max(result, args[i]);
        }
        return result;
    }

    public static int Max(params int[] args)
    {
        int result=args[0];
        for (int i = 1; i<args.Length; i++)
        {
            result = M.Max(result, args[i]);
        }
        return result;
    }

    public static float Max(List<float> args)
    {
        float result=args[0];
        for (int i = 1; i<args.Count; i++)
        {
            result = M.Max(result, args[i]);
        }
        return result;
    }

    public static int Max(List<int> args)
    {
        int result=args[0];
        for (int i = 1; i<args.Count; i++)
        {
            result = M.Max(result, args[i]);
        }
        return result;
    }
    
    public static int Sum(List<int> a)
    {
        int s = 0;
        for(int i = 0; i<a.Count; i++)
        {
            s += a[i];
        }
        return s;
    }
    public static float Sum(List<float> a)
    {
        float s = 0;
        for(int i = 0; i<a.Count; i++)
        {
            s += a[i];
        }
        return s;
    }

    public static float Min(params float[] args)
    {
        float result=args[0];
        for (int i = 1; i<args.Length; i++)
        {
            result = M.Min(result, args[i]);
        }
        return result;
    }

    public static int Min(params int[] args)
    {
        int result=args[0];
        for (int i = 1; i<args.Length; i++)
        {
            result = M.Min(result, args[i]);
        }
        return result;
    }

    public static float Min(List<float> args)
    {
        float result=args[0];
        for (int i = 1; i<args.Count; i++)
        {
            result = M.Min(result, args[i]);
        }
        return result;
    }

    public static int Min(List<int> args)
    {
        int result=args[0];
        for (int i = 1; i<args.Count; i++)
        {
            result = M.Min(result, args[i]);
        }
        return result;
    }
    
    // OUTPUT & DEBUG
    public static void Out(params object[] args)
    {
        string result = "";

        foreach (var arg in args)
        {
            result += Str(arg) + "; ";
        }

        Debug.Log(result);
    }
    public static string Str<T>(T value)
    {
        if (value == null) return "null";
        string result = "";
        List<System.Type> defaultTypes = new List<System.Type>()
        {
            typeof(int), typeof(string), typeof(float), typeof(bool)
        };
        System.Type type = typeof(T);
        if (defaultTypes.Contains(type))
        {
            result = value.ToString();
        } else if (value is Vector2 v2)
        {
            result = $"x: {v2.x}, y: {v2.y}";
        } else if (value is Vector3 v3)
        {
            result = $"x: {v3.x}, y: {v3.y}, z: {v3.z}";
        } else if (value is IDictionary dict)
        {
            foreach (DictionaryEntry entry in dict)
            {
                result += $"{Str(entry.Key)}: {Str(entry.Value)}\n";
            }
        } else if (value is IList list)
        {
            foreach (var item in list)
            {
                result += $"{Str(item)}, ";
            }
        } else if (value is ObjectsCountContainer container)
        {
            result = Str(container.values);
        }
        else result = value.ToString();

        return result;
    }

    public static void Out<T> (T value){
        Debug.Log(Str(value));
    }

    public static void Out<T>(List<T> data)
    {
        Debug.Log(string.Join("\n", data));
    }

    public static void Out<K, V>(Dictionary<K, V> data)
    {
        if (data == null || data.Count == 0)
        {
            Debug.Log("(empty dictionary)");
            return;
        }
        Debug.Log(string.Join("\n", data.Select(p => $"{p.Key}: {p.Value}")));
    }

    public static void Out(Vector2 v)
    {
        Debug.Log($"x: {v.x}, y: {v.y}");
    }

    public static void Out(Vector3 v)
    {
        Debug.Log($"x: {v.x}, y: {v.y}, z: {v.z}");
    }


    // RANDOM
    public static int Range(int a, int b)
    {
        return Random.Range(a, b);
    }

    public static float Range(float a, float b)
    {
        return Random.Range(a, b);
    }

    public static float RangeWithCoof(float a, float c)
    {
        return h.Range(a * (1 - c), a * (1 + c));
    }

    public static float Range(float a)
    {
        return Range(-a, a);
    }

    public static float Range(int a)
    {
        return Range(-a, a);
    }

    public static float RandMult(float range)
    {
        /// returns random multiplicator exmp: from 0.9 to 1.1
        return 1 + Range(range);
    }

    public static T RandChoice<T>(List<T> list)
    {
        if (list == null || list.Count == 0) return default;
        return list[Random.Range(0, list.Count)];
    }
    
    /// <summary>
    /// Returns a random element from the provided arguments.
    /// </summary>
    /// <param name="args">Variable number of items to choose from.</param>
    /// <returns>A random item from the arguments, or default if no arguments provided.</returns>
    public static T RandChoice<T>(params T[] args)
    {
        if (args == null || args.Length == 0) return default;
        return args[Random.Range(0, args.Length)];
    }
    
    public static T GetRandomWithDistribution<T>(List<T> objects, List<float> weights)
    {
        if (objects.Count == 0) return default;
        
        for (int i = 0; i<objects.Count-weights.Count; i++)
        {
            weights.Add(1);
        }

        float totalWeight = Sum(weights);
        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        for (int i = 0; i < objects.Count; i++)
        {
            cumulative += weights[i];

            if (randomValue < cumulative)
                return objects[i];
        }

        return objects[objects.Count - 1];
    }

    // GEOMETRY
    public static Vector2 RandomDirection()
    {
        float x = Random.value;
        float y = (float)M.Sqrt(1-x*x);
        int[] choice = {-1, 1};
        x *= RandChoice(choice);
        y *= RandChoice(choice);

        return new Vector2(x, y);
    }

    
    
    // FIND IN SCENE, PROCESS IN PARENT, INSTANCES 
    public static void AssignFirstObjectByType<T>(ref T target)
        where T : UnityEngine.Object
    {
        target = GameObject.FindFirstObjectByType<T>();
    }

    public static void CreateStaticInstance<T>(T obj, ref T instance)
        where T : UnityEngine.Object
    {
            if (GameObject.FindObjectsOfType<T>().Length > 1) // > 1, because called then obj is already created
            {
                GameObject.Destroy(obj);
                return;
            }
            GameObject.DontDestroyOnLoad(obj.GameObject());
            instance = obj;
    }
    
    public static List<Transform> GetAllChildren(Transform parent)
    {
        List<Transform> result = new List<Transform>();
        foreach(Transform child in parent)
        {
            result.Add(child);
        }
        return result;
    }

    public static List<GameObject> FindClosestByTag(string tag, Transform target, int n = 1, float minRange=-1)
    {
        if (!target) return default;
        return OrderByDistance(GameObject.FindGameObjectsWithTag(tag), target, n, minRange);
    }

    public static List<GameObject> FindClosestByParent(
        Transform parent,
        Transform target,
        int n = 1,
        float minRange=-1,
        List<string> targetTags=null
        )
    {
        if (!target || !parent) return default;

        GameObject[] children = new GameObject[parent.childCount];
        int i = 0;
        foreach (Transform child in parent)
        {
            if (targetTags != null && targetTags.Contains(child.tag)) continue;
            children[i] = child.gameObject;
            i++;
        }
        return OrderByDistance(children, target, n, minRange);
    }

    public static List<GameObject> OrderByDistance(GameObject[] a, Transform transform, int n = 1, float minRange=-1)
    {
        if(a.Length == 0) return default;
        
        List<GameObject> result = new List<GameObject>();
        List<GameObject> ordered = a.Where(obj => obj != null).OrderBy(obj => (obj.transform.position - transform.position).sqrMagnitude).Take(n).ToList(); //.Take(n).ToList()
        
        if (minRange <= 0) return ordered;
        minRange*=minRange;
        foreach(GameObject obj in ordered)
        {
            if ((obj.transform.position - transform.position).sqrMagnitude <= minRange)
            {
                result.Add(obj);
            }
        }
        return result;
    }

    public static GameObject FindChildrenWithTag(Transform parent, string tag)
    {
        try
        {
            GameObject result = null;
            foreach (Transform child in parent)
            {

                if (child.CompareTag(tag)) return child.gameObject;
                result = FindChildrenWithTag(child, tag);
                if (result != null) return result;
            }

            return result;
        }
        catch (Exception e)
        {
            h.Out(e.Message);
            return null;
        }
        
    }
    
    

    public static Transform GetRndChildFromParents(List<Transform> parentsOfPossibleTargets)
    {
        return GetRndChild(RandChoice(parentsOfPossibleTargets));
    }

    public static Transform GetRndChild(Transform parent)
    {
        return RandChoice(GetAllChildren(parent));
    }

    public static Transform GetFirstChildByTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return default;
    }

    public static Transform GetRndByTag(string tag)
    {
        return RandChoice(GameObject.FindGameObjectsWithTag(tag)).transform;
    }

    
    // ANIMATION
    public static void SmoothScaling(
        MonoBehaviour runner,
        Transform tObj,
        Vector3 tScale,
        float duration) // t - target
    {
        runner.StartCoroutine(SmoothScalingCoroutine(tObj, tScale, duration));
    }

    public static IEnumerator SmoothScalingCoroutine(Transform tObj, Vector3 tScale, float duration)
    {
        if (duration <= 0f)
        {
            tObj.localScale = tScale;
            yield break;
        }

        Vector3 startScale = tObj.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            tObj.localScale = Vector3.Lerp(startScale, tScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (tObj) tObj.localScale = tScale;
    }

    /// <summary>
    /// Smoothly rotates a transform over a specified duration.
    /// </summary>
    /// <param name="runner">The MonoBehaviour to run the coroutine on.</param>
    /// <param name="tObj">The transform to rotate.</param>
    /// <param name="rotatingSpeed">The rotation speed in degrees per second.</param>
    /// <param name="duration">The duration of the rotation in seconds.</param>
    /// <param name="endAngle">The target end angle. If >= 0, calculates start angle based on this.</param>
    /// <param name="reverse">If true, reverses the rotation direction.</param>
    /// <param name="acceleration">Optional acceleration factor. Positive for acceleration, negative for deceleration. 0 for constant speed.</param>
    public static void SmoothRotating(
        MonoBehaviour runner,
        Transform tObj,
        float rotatingSpeed,
        float duration,
        float endAngle = -1f,
        bool reverse = false,
        float acceleration = 0f
        )
    {
        if (reverse) rotatingSpeed *= -1;

        if (endAngle >= 0f)
        {
            float startAngle = endAngle - rotatingSpeed * duration;
            startAngle = Mathf.Repeat(startAngle, 360f); // % 360

            tObj.eulerAngles = new Vector3(
                tObj.eulerAngles.x,
                tObj.eulerAngles.y,
                startAngle
            );
        }

        runner.StartCoroutine(
            SmoothRotatingCoroutine(tObj, rotatingSpeed, duration, acceleration)
        );
    }

    static IEnumerator SmoothRotatingCoroutine(
        Transform tObj,
        float rotatingSpeed,
        float duration,
        float acceleration = 0f)
    {
        float elapsed = 0f;
        float currentSpeed = rotatingSpeed;

        while (elapsed < duration)
        {
            float dt = Time.deltaTime;
            
            // Apply acceleration/deceleration
            if (acceleration != 0f)
            {
                currentSpeed += acceleration * dt;
            }

            tObj.Rotate(0f, 0f, currentSpeed * dt);

            elapsed += dt;
            yield return null;
        }
    }
    
    public static void SmoothTranslating(MonoBehaviour runner, Transform tObj, Vector3 tPos, float duration) // t - target
    {
        runner.StartCoroutine(SmoothTranslatingCoroutine(tObj,  tObj.position + tPos, duration));
    }

    static IEnumerator SmoothTranslatingCoroutine(Transform tObj, Vector3 tPos, float duration)
    {
        if (duration <= 0f)
        {
            tObj.position = tPos;
            yield break;
        }

        Vector3 startPos = tObj.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            tObj.position = Vector3.Lerp(startPos, tPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tObj.position = tPos;
    }
    
    // OTHER HELPERS AND COROUTINES
    public static void InvokeAfterTime(MonoBehaviour runner, float duration, UnityAction func)
    {
        runner.StartCoroutine(InvokeAfterTimeCoroutine(func, duration));
    }
    static IEnumerator InvokeAfterTimeCoroutine(UnityAction func, float duration)
    {
        yield return new WaitForSeconds(duration);
        func.Invoke();
    }
    
    
    
    // DATA PROCESS ASSIGNMENT
    public static void AssignComponent<T>(Component owner, ref T field) where T : Component
    {
        if (field == null) field = owner.GetComponent<T>();
    }

    public static void CopySO<T>(ref T so) where T : ScriptableObject
    {
        so = Object.Instantiate(so);
    }

    public static T CopySO<T>(T so) where T : ScriptableObject
    {
        return Object.Instantiate(so);
    }
    
    // LISTS, ARRAYS, DATA STRUCTURES
    public static List<T> CreateList<T>(int len, T value)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < len; i++)
        {
            result.Add(value);
        }

        return result;
    }

    /// <summary>
    /// Checks, if all arguments are not null.
    /// !!!WARNING: may not function properly with ints and floats as their default value equals to 0!!! 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool CheckIfAllExist(params object[] args)
    {
        foreach (object arg in args)
        {
            if (arg == default) return false;
        }

        return true;
    }
    
    public static bool CheckIfAllExist<T>(List<T> list)
    {
        if (list == null) return false;
        return CheckIfAllExist(list.Cast<object>().ToArray());
    }
    
    public static void ForEach<T>(List<T> list, UnityAction<T> action)
    {
        foreach (T obj in list)
        {
            action.Invoke(obj);
        }
    }
    
    public static void AssignValuesToDict<TK, TV>(ref Dictionary<TK, TV> original, List<TK> keysIn, List<TV> valuesIn)
    {
        if (keysIn == null || valuesIn == null || keysIn.Count != valuesIn.Count) return;
        for (int i = 0; i < keysIn.Count; i++)
        {
            original[keysIn[i]] = valuesIn[i];
        }
    }

    public static void AssignValuesToDict<TK, TV>(ref Dictionary<TK, TV> original, Dictionary<TK, TV> data)
    {
        AssignValuesToDict(ref original, data.Keys.ToList(), data.Values.ToList());
    }
    
    // TEXT TMP
    public static bool TMPSpriteExists(string name, TMP_SpriteAsset spriteAsset=null)
    {
        if (!spriteAsset) spriteAsset = TMP_Settings.defaultSpriteAsset;
        return spriteAsset.spriteCharacterTable
            .Any(c => c.name == name);
    }

    

    

    public static float GetSpriteHypotenuse(GameObject go)
    {
        Sprite sr = go.GetComponent<SpriteRenderer>().sprite;
        if (!sr) return default;

        return (float)M.Sqrt(M.Pow(sr.bounds.size.x, 2) + M.Pow(sr.bounds.size.y, 2));
    }
    public static float GetSpriteHypotenuse(SpriteRenderer go)
    {
        Sprite sr = go.sprite;
        if (!sr) return default;

        return (float)M.Sqrt(M.Pow(sr.bounds.size.x, 2) + M.Pow(sr.bounds.size.y, 2));
    }
    public static float GetSpriteHypotenuse(Sprite sr)
    {
        return (float)M.Sqrt(M.Pow(sr.bounds.size.x, 2) + M.Pow(sr.bounds.size.y, 2));
    }

    public static bool CheckInsideCamera(Vector3 pos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);

        bool isVisible =
            viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1;
        
        return isVisible;
    }

    public static Vector2 GetCameraTopLeftCorner()
    {
        Camera cam = Camera.main;
        Vector3 topLeft = cam.ViewportToWorldPoint(
            new Vector3(0f, 1f, cam.nearClipPlane)
        );

        return new Vector2(topLeft.x, topLeft.y);
    }

    public static Vector2 GetCameraBottomRightCorner()
    {
        Camera cam = Camera.main;
        Vector3 bottomRight = cam.ViewportToWorldPoint(
            new Vector3(1f, 0f, cam.nearClipPlane)
        );

        return new Vector2(bottomRight.x, bottomRight.y);
    }

    public static float GetCameraWidth()
    {
        Vector2 topLeft = GetCameraTopLeftCorner();
        Vector2 bottomRight = GetCameraBottomRightCorner();
        return bottomRight.x - topLeft.x;
    }

    public static float GetCameraHeight()
    {
        Vector2 topLeft = GetCameraTopLeftCorner();
        Vector2 bottomRight = GetCameraBottomRightCorner();
        return topLeft.y - bottomRight.y;
    }
    
    
    // PHYSICS
    public static void BrakeWithFriction(
        Rigidbody2D target,
        float friction,
        float linearVelocityMultOnStart=1)
    {
        target.GetComponent<MonoBehaviour>().StartCoroutine(BrakeWithFrictionCoroutine(
            target,
            friction,
            linearVelocityMultOnStart
        ));
    }
    
    

    static IEnumerator BrakeWithFrictionCoroutine(Rigidbody2D rb, float frictionCoefficient, float linearVelocityMultOnStart=1)
    {
        rb.linearVelocity *= linearVelocityMultOnStart;

        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        float deceleration = frictionCoefficient * g;

        rb.gravityScale = 0f;

        while (rb.linearVelocity.sqrMagnitude > 0.0001f)
        {
            Vector2 v = rb.linearVelocity;
            float speed = v.magnitude;

            float dv = deceleration * Time.fixedDeltaTime;

            if (dv >= speed)
            {
                rb.linearVelocity = Vector2.zero;
                break;
            }

            rb.linearVelocity -= v.normalized * dv;

            yield return new WaitForFixedUpdate();
        }

        DisableRB(rb);
    }

    public static void RotateByGravity(Transform t, float duration, float offset=90)
    {
        Rigidbody2D rb = t.GetComponent<Rigidbody2D>();
        t.GetComponent<MonoBehaviour>().StartCoroutine(
            RotateByGravityCoroutine(t, rb, duration, 90)
        );
    }

    static IEnumerator RotateByGravityCoroutine(Transform t, Rigidbody2D rb, float duration, float offset)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            t.rotation = Quaternion.Euler(0, 0, angle+offset);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public static Vector3 MultiplyVectors(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x*v2.x, v1.y*v2.y, v1.z*v2.z);
    }

    public static Vector2 MultiplyVectors(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x*v2.x, v1.y*v2.y);
    }

    public static float DistanceFraction(Vector3 t1, Vector3 t2, float maxDistance, bool reversed=false)
    {
        float fraction = (t1 - t2).magnitude / maxDistance;
        // h.Out(t1, t2);
        // h.Out((t1 - t2).magnitude, maxDistance, fraction, 1-fraction);
        return reversed ? 1-fraction : fraction;
    }

    public static void DisableRB(Rigidbody2D rb)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public static void DetatchParticles(List<ParticleSystem> ps)
    {
        h.ForEach(ps, (p) => DetatchParticles(p));
    }
    public static void DetatchParticles(ParticleSystem ps)
    {
        if (!ps)
        {
            return;
        }
        if(ps.gameObject.tag == "InstantlyDestroyableParticle")
        {
            UnityEngine.Object.Destroy(ps.gameObject);
            return;
        }
        ps.transform.SetParent(null);
        var main = ps.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
        ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        
    }

    /// <summary>
    /// Fades out the target GameObject by gradually reducing its alpha over the specified duration.
    /// Supports SpriteRenderer and TextMeshProUGUI components.
    /// </summary>
    /// <param name="target">The GameObject to fade away.</param>
    /// <param name="duration">The time in seconds over which to fade.</param>
    /// <param name="runner">The MonoBehaviour to run the coroutine on. If null, uses one from the target.</param>
    public static void FadeOut(GameObject target, float duration, MonoBehaviour runner=null, bool destroyOnFinish=false)
    {
        if (!runner) runner = target.GetComponent<MonoBehaviour>();
        runner.StartCoroutine(FadeOutCoroutine(target, duration,destroyOnFinish));
    }

    static IEnumerator FadeOutCoroutine(GameObject target, float duration, bool destroyOnFinish)
    {
        System.Exception _;
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        TextMeshProUGUI tmp = target.GetComponent<TextMeshProUGUI>();

        Color getColor;
        System.Action<Color> setColor;

        if (sr)           { getColor = sr.color;  setColor = c => {
                                                                    try {sr.color  = c;}
                                                                    catch (System.Exception e) {_=e; /*just placeholder to ignore alert*/}
                                                                }; }
        else if (tmp)     { getColor = tmp.color; setColor = c => {
                                                                    try {tmp.color  = c;}
                                                                    catch (System.Exception e) {_=e; /*just placeholder to ignore alert*/}
                                                                }; }
        else yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            getColor.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            setColor(getColor);
            yield return null;
        }

        getColor.a = 0f;
        setColor(getColor);

        if (destroyOnFinish)
        {
            GameObject.Destroy(target);
        }
    }

    /// <summary>
    /// Fades in the target GameObject by gradually increasing its alpha over the specified duration.
    /// Supports SpriteRenderer and TextMeshProUGUI components.
    /// </summary>
    /// <param name="target">The GameObject to fade in.</param>
    /// <param name="duration">The time in seconds over which to fade.</param>
    /// <param name="runner">The MonoBehaviour to run the coroutine on. If null, uses one from the target.</param>
    public static void FadeIn(
        GameObject target,
        float duration,
        MonoBehaviour runner=null)
    {
        if (!runner) runner = target.GetComponent<MonoBehaviour>();
        runner.StartCoroutine(FadeInCoroutine(target, duration));
    }

    static IEnumerator FadeInCoroutine(GameObject target, float duration)
    {
        System.Exception _;
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        TextMeshProUGUI tmp = target.GetComponent<TextMeshProUGUI>();

        Color getColor;
        System.Action<Color> setColor;

        if (sr)           { getColor = sr.color;  setColor = c => {
                                                                    try {sr.color  = c;}
                                                                    catch (System.Exception e) {_=e; /*just placeholder to ignore alert*/}
                                                                }; }
        else if (tmp)     { getColor = tmp.color; setColor = c => {
                                                                    try {tmp.color  = c;}
                                                                    catch (System.Exception e) {_=e; /*just placeholder to ignore alert*/}
                                                                }; }
        else yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            getColor.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            setColor(getColor);
            yield return null;
        }

        getColor.a = 1f;
        setColor(getColor);
    }

    // CAMERA
    public static CameraShakeInstance ShakeOnce(float magnitude, float sharpness, float fadeInDuration, float fadeOutDuration)
    {
        return CameraShaker.Instance.ShakeOnce(magnitude, sharpness, fadeInDuration, fadeOutDuration);
    }

    
    public static CameraShakeInstance StartShake(float magnitude, float sharpness, float fadeInDuration)
    {
        return CameraShaker.Instance.StartShake(magnitude, sharpness, fadeInDuration);
    }

    public static void EndShake(float fadeOutTime=0f, CameraShakeInstance instance=null)
    {
        CameraShaker.Instance.EndShake(fadeOutTime, instance);
    }

    public static void EndAllShakes(float fadeOutTime=0f)
    {
        ForEach(CameraShaker.Instance.cameraShakeInstances, (inst) =>
        {
            EndShake(fadeOutTime:fadeOutTime, instance:inst);
        });
    }

    // RESOURCES
    public static T ResourcesLoad<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public static void UpdateLayersRecursively(Transform t, int deltaLayer)
    {
        // Check if current transform has SpriteRenderer
        if (t.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.gameObject.layer += deltaLayer;
        }
        
        // Recursively process all children
        foreach (Transform child in t)
        {
            UpdateLayersRecursively(child, deltaLayer);
        }
    }

    public static void SetSpriteMaskInteractionRecursively(Transform t, SpriteMaskInteraction maskInteraction)
    {
        // Check if current transform has SpriteRenderer
        if (t.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.maskInteraction = maskInteraction;
        }
        
        // Recursively process all children
        foreach (Transform child in t)
        {
            SetSpriteMaskInteractionRecursively(child, maskInteraction);
        }
    }


    public static Transform[] FindAllTransformsWithTag(string tag)
    {
        return Array.ConvertAll(
            GameObject.FindGameObjectsWithTag(tag),
            go => go.transform
        );
    }
    
}