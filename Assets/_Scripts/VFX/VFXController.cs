using UnityEngine;
using System.Collections.Generic;
public class VFXController : MonoBehaviour
{
    public List<VFX> VFXes; // mb change to objkindcont

    void Start()
    {
        foreach(VFX vfx in VFXes)
        {
            if (vfx) Instantiate(vfx).Init(transform);
        }  
    }
}
