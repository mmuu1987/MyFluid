using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertVelocity : MonoBehaviour
{
    public VortexStreetManager VortexStreetManager;
  

    public Texture2D Tex;

    public RawImage ShowImage;

   
    // Start is called before the first frame update
    void Start()
    {
        Tex = new Texture2D(1280,720);
        
        ShowImage.texture = Tex;

       
    }

    // Update is called once per frame
    void Update()
    {
          Graphics.ConvertTexture(VortexStreetManager.VelocityRT, Tex);
       


    }

   

   
   
}
