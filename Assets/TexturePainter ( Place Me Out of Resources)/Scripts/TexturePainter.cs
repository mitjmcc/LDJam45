﻿/// <summary>
/// CodeArtist.mx 2015
/// This is the main class of the project, its in charge of raycasting to a model and place brush prefabs infront of the canvas camera.
/// If you are interested in saving the painted texture you can use the method at the end and should save it to a file.
/// </summary>


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Luminosity.IO;

public enum Painter_BrushMode{PAINT,DECAL};
public class TexturePainter : MonoBehaviour {
    public GameObject brushCursor,brushContainer; //The cursor that overlaps the model and our container for the brushes painted
    public Camera canvasCam;  //The camera that looks at the canvas.
    public Sprite cursorPaint,cursorDecal; // Cursor for the differen functions 
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)
    public GameObject objectPainter;

    Painter_BrushMode mode; //Our painter mode (Paint brushes or decals)
    float brushSize=.05f; //The size of our brush
    Color brushColor = Color.black; //The selected color
    int brushCounter=0,MAX_BRUSH_COUNT=1000; //To avoid having millions of brushes
    bool saving=false; //Flag to check if we are saving the texture

    
    void Update () {
        if (Input.GetKey("space")) {
            DoAction();
        }
        if (InputManager.GetButton("Reset"))
            Reset();
        UpdateBrushCursor ();
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DoAction(){	
        if (saving)
            return;
        Vector3 uvWorldPosition=Vector3.zero;
        if(HitTestUVPosition(ref uvWorldPosition)){
            GameObject brushObj;
            brushObj=(GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity")); //Paint a brush
            brushObj.GetComponent<SpriteRenderer>().color=brushColor; //Set the brush color
            brushColor.a=brushSize*2.0f; // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent=brushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition=uvWorldPosition; //The position of the brush (in the UVMap)
            brushObj.transform.localScale=Vector3.one*brushSize;//The size of the brush
        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT) { //If we reach the max brushes available, flatten the texture and clear the brushes
            brushCursor.SetActive (false);
            saving=true;
            Invoke("SaveTexture",0.1f);
        }
    }
    //To update at realtime the painting cursor on the mesh
    void UpdateBrushCursor(){
        Vector3 uvWorldPosition=Vector3.zero;
        if (HitTestUVPosition (ref uvWorldPosition) && !saving) {
            brushCursor.SetActive(true);
            brushCursor.transform.position =uvWorldPosition+brushContainer.transform.position;
        } else {
            brushCursor.SetActive(false);
        }
    }
    //Returns the position on the texturemap according to a hit in the mesh collider
    bool HitTestUVPosition(ref Vector3 uvWorldPosition){
        RaycastHit hit;
        Vector3 cursorPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray objectRay = new Ray(objectPainter.transform.position, Vector3.down);
        if (Physics.Raycast(objectRay,out hit, 200)){
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV  = new Vector2(hit.textureCoord.x,hit.textureCoord.y);
            uvWorldPosition.x=pixelUV.x-canvasCam.orthographicSize;//To center the UV on X
            uvWorldPosition.y=pixelUV.y-canvasCam.orthographicSize;//To center the UV on Y
            uvWorldPosition.z=0.0f;
            return true;
        }
        else{
            return false;
        }
    }
    //Sets the base material with a our canvas texture, then removes all our brushes
    void SaveTexture(){
        brushCounter=0;
        System.DateTime date = System.DateTime.Now;
        RenderTexture.active = canvasTexture;
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels (new Rect (0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply ();
        RenderTexture.active = null;
        baseMaterial.mainTexture =tex;	//Put the painted texture as the base
        foreach (Transform child in brushContainer.transform) {//Clear brushes
            Destroy(child.gameObject);
        }
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke ("ShowCursor", 0.1f);
    }
    //Show again the user cursor (To avoid saving it to the texture)
    void ShowCursor(){	
        saving = false;
    }

    // Delete the brushes to reset the image
    void Reset()
    {
        brushCounter = 0;
        foreach (Transform child in brushContainer.transform) {//Clear brushes
            Destroy(child.gameObject);
        }
    }
}
