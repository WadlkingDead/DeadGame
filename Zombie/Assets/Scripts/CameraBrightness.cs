////////////////////////////////////////////////////////////////////////////////////
//  CameraFilterPack v2.0 - by VETASOFT 2015 //////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
[AddComponentMenu ("Camera/Brightness")]
public class CameraBrightness : MonoBehaviour {
	#region Variables
	public Shader SCShader;
	[Range(0, 2)]
	public float _Brightness = 1.5f;
	private Material SCMaterial;

	//public static float ChangeBrightness;

	#endregion
	
	#region Properties
	Material material
	{
		get
		{
			if(SCMaterial == null)
			{
				SCMaterial = new Material(SCShader);
				SCMaterial.hideFlags = HideFlags.HideAndDontSave;	
			}
			return SCMaterial;
		}
	}
	#endregion
	void Start () 
	{
		//ChangeBrightness = _Brightness;

		if(!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if(SCShader != null)
		{
				material.SetFloat("_Val", _Brightness);

			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);	
		}
		
		
	}
//void OnValidate()
//{
//    ChangeBrightness=_Brightness;

	
//}
	// Update is called once per frame
    //void Update () 
    //{
    //    if (Application.isPlaying)
    //    {
    //        _Brightness = ChangeBrightness;
    //    }
    //    #if UNITY_EDITOR
    //    if (Application.isPlaying!=true)
    //    {
    //        SCShader = Shader.Find("CameraFilterPack/Colors_Brightness");
    //        material.SetFloat("_Val", _Brightness);

    //    }
    //    #endif

    //}
	
	void OnDisable ()
	{
		if(SCMaterial)
		{
			DestroyImmediate(SCMaterial);	
		}
		
	}

    public System.Action onDestroy;
    void OnDestroy()
    {
        if (onDestroy != null) onDestroy.Invoke();

        
    }
	
	
}