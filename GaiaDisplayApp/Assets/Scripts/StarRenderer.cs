using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Renderer of the stars.
/// </summary>
public class StarRenderer : MonoBehaviour {

    /// <summary>
    /// The instance of the singleton
    /// </summary>
    private static StarRenderer instance;
    public static StarRenderer Instance { get { return instance; } }

    /// <summary>
    /// List of stars
    /// </summary>
    StarList starList;

    /// <summary>
    /// The compute shader that renders the stars
    /// </summary>
    public ComputeShader starRenderShader;

    /// <summary>
    /// Texture width
    /// </summary>
    public int textureWidth = 1920;

    /// <summary>
    /// Texture height
    /// </summary>
    public int textureHeight = 1080;

    /// <summary>
    /// Number of stars
    /// </summary>
    public int starCount = 3000000; // (8 * 20)^2

    /// <summary>
    /// Clear the background with black each frame.
    /// </summary>
    bool clear = true;

    /// <summary>
    /// Rendertexture
    /// </summary>
    RenderTexture renderTex;

    /// <summary>
    /// Bloom material
    /// </summary>
    [NonSerialized]
    Material bloomMaterial;

    /// <summary>
    /// Itterations of up and down scaling, more = better and slower
    /// </summary>
    [Range(1, 16)]
    public int iterations = 1;

    /// <summary>
    /// Threshold for how light something has to be to start blooming, 1 = (1,1,1) (aka) white.
    /// </summary>
    [Range(0, 10)]
    public float threshold = 1;

    /// <summary>
    /// The power of the blooming, higher = more blooming
    /// </summary>
    [Range(0, 1)]
    public float power = 1;

    /// <summary>
    /// The camera exposure, 1 = standard, higher = everything brighter
    /// </summary>
    [Range(0, 100)]
    public float exposure = 1;


    /// <summary>
    /// Bloom shader
    /// </summary>
    public Shader bloomShader;

    /// <summary>
    /// Filter passes
    /// </summary>
    const int FilterPass = 0;
    const int DownScalePass = 1;
    const int UpScalePass = 2;
    const int MergePass = 3;
    const int MergePassBackDrop = 4;

    /// <summary>
    /// Default magnitude increase
    /// </summary>
    float magIncrease = 6;

    /// <summary>
    /// Texture array used for blooming
    /// </summary>
    RenderTexture[] textures = new RenderTexture[16];

    /// <summary>
    /// The player
    /// </summary>
    PlayerMovement player;

    /// <summary>
    /// The player
    /// </summary>
    public PlayerMovement Player { get { return player; } }

    /// <summary>
    /// A bool to indicate whether the frame needs to be re-rendered
    /// </summary>
    public bool reRender = true;

    /// <summary>
    /// Set the singleton
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            instance.Start();
            GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// Init stuff
    /// </summary>
    void Start () {
        player = GetComponent<PlayerMovement>();
        textureHeight = Screen.height;
        textureWidth = Screen.width;

        starList = JSONReader.ParseStarJSONList(JSONReader.ReadJSONFiles("gaia", 1));
        renderTex = new RenderTexture(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
        renderTex.enableRandomWrite = true;
        renderTex.Create();

        RenderStars(true, true);
    }

    /// <summary>
    /// Render all visible stars onto a texture. 
    /// </summary>
    /// <param name="updateStars">Read in a new list of stars</param>
    /// <param name="newTex">Remake the texture (in case of different format)</param>
    private void RenderStars(bool updateStars, bool newTex)
    {
        ComputeBuffer starBuffer = null;
        if (newTex) //If the texure format changed, remake the whole texture.
        {
            renderTex = RenderTexture.GetTemporary(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
            renderTex.enableRandomWrite = true;
            renderTex.autoGenerateMips = false;
            renderTex.Create();
        }
        int kernel = starRenderShader.FindKernel("CSClear");
        if (clear)
        {   //Fill the texture with black before drawing in stars
            starRenderShader.SetTexture(kernel, "Result", renderTex);
            starRenderShader.Dispatch(kernel, renderTex.width / 7, renderTex.height / 7, 1);
        }

        kernel = starRenderShader.FindKernel("CSMain");
        starRenderShader.SetInt("textureHeight", textureHeight);
        starRenderShader.SetInt("textureWidth", textureWidth);



        int kernerlStarNumber = starCount / 1000;
        float twisty = player.EulerAnglesOrientation.z / 180 * Mathf.PI;


        starRenderShader.SetFloat("playerAngle1", player.EulerAnglesOrientation.x / 180 * Mathf.PI);
        starRenderShader.SetFloat("playerAngle2", player.EulerAnglesOrientation.y / 180 * Mathf.PI);
        starRenderShader.SetFloat("twistyAngle", twisty);

        starRenderShader.SetFloat("fovh", (float)player.FoV / 180f * Mathf.PI);
        starRenderShader.SetFloat("fovv", ((float)player.FoV * ((float)Screen.height / Screen.width)) / 180f * Mathf.PI);
        starRenderShader.SetFloat("magIncrease", magIncrease);
        starRenderShader.SetVector("playerPos", player.PositionFloat);

        if (updateStars)
        {
            if (starBuffer != null)
            {
                starBuffer.Release();
            }
            starBuffer = new ComputeBuffer(starList.Count, sizeof(float) * 7);
            starBuffer.SetData(starList.Stars);
            starRenderShader.SetBuffer(0, "starList", starBuffer);
        }
        
        starRenderShader.SetTexture(kernel, "Result", renderTex);
        starRenderShader.Dispatch(kernel, 3000, 1, 1);
        
    }


    /// <summary>
    /// Render the image
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="destination">The destination</param>
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (textureHeight != source.height || textureWidth != source.width)
        {
            textureHeight = source.height;
            textureWidth = source.width;
            RenderStars(false, true); //Set the right size if it isn't right.
        }

        if (bloomMaterial == null) //Make a material if needed
        {
            bloomMaterial = new Material(bloomShader);
            bloomMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        bloomMaterial.SetFloat("_Threshold", threshold);
        bloomMaterial.SetFloat("_Power", power);
        bloomMaterial.SetFloat("_Exposure", exposure);

        RenderTextureFormat format = source.format;

        //Merge the background of rendered stars with the foreground of 3d models
        bloomMaterial.SetTexture("_SourceTex", source);
        RenderTexture currentDestination = RenderTexture.GetTemporary(source.width, source.height, 0, format);
        Graphics.Blit(renderTex, currentDestination, bloomMaterial, MergePassBackDrop);
        RenderTexture merged = currentDestination;


        int width = source.width;
        int height = source.height;

        //Filter for bright objects only
        currentDestination = textures[0] =  RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(merged, currentDestination, bloomMaterial, FilterPass);
        RenderTexture currentSource = currentDestination;

        //Begin the downscaling
        int i = 1;
        for (; i < iterations; i++)
        {
            width /= 2;
            height /= 2;
            if (height < 2)
            {
                break;
            }
            currentDestination = textures[i] =
                RenderTexture.GetTemporary(width, height, 0, format);
            Graphics.Blit(currentSource, currentDestination, bloomMaterial, DownScalePass);
            currentSource = currentDestination;
        }
        
        //Begin the upscaling
        for (i -= 2; i >= 0; i--)
        {
            currentDestination = textures[i];
            RenderTexture.ReleaseTemporary(currentSource);
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination, bloomMaterial, UpScalePass);
            currentSource = currentDestination;
        }
        
        //Merge the passes
        bloomMaterial.SetTexture("_SourceTex", merged);
        Graphics.Blit(currentSource, destination, bloomMaterial, MergePass);
        RenderTexture.ReleaseTemporary(currentSource);
        RenderTexture.ReleaseTemporary(merged);
    }


    /// <summary>
    /// Some keyInput
    /// </summary>
    void Update ()
    {
        if (Input.GetKey(KeyCode.Comma)) //Decrease magnitude, brightness of stars
        {
            magIncrease -= 0.05f;
            reRender = true;
        }
        if (Input.GetKey(KeyCode.Period))//Increase magnitude, brightness of stars
        {
            magIncrease += 0.05f;
            reRender = true;
        }
        if (Input.GetKeyDown(KeyCode.Space)) //Toggle black backdrop
        {
            clear = !clear;
        }
        if (reRender)
        {
            RenderStars(false, false);
            reRender = false;
        }
    }
}
