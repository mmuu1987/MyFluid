using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

public class FluidTest : MonoBehaviour
{
    public VortexStreetManager Fluid;

    public VisualEffect VisualEffect;

    public RenderTexture VideoRT;


    // Start is called before the first frame update
    void Start()
    {
       // VisualEffect.SetTexture("filedTex", Fluid.VFB.V1);

       Screen.SetResolution(3840,2160,true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VisualEffect.GetTexture("filedTex") == null)
        {
            VisualEffect.SetTexture("filedTex", Fluid.VelocityRT);
            VideoRT = Fluid.VelocityRT;
            Debug.Log("¸³Öµ");
            Texture tex = VisualEffect.GetTexture("filedTex");

            Debug.Log(tex.name);
        }
       
    }
}
