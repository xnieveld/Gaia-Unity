using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Renderer of the stars.
/// </summary>
public class StarRenderer : MonoBehaviour {

    private static StarRenderer instance;
    public static StarRenderer Instance { get { return instance; } }

    StarList starList;
    public Material skyboxMaterial;
    public ComputeShader starRenderShader;
    public int textureWidth = 1920;
    public int textureHeight = 1080;
    public int xChunks = 10;
    public int yChunks = 10;
    public int starCount = 6000000; // (8 * 20)^2

    bool clear = true;

    RenderTexture renderTex;

    [NonSerialized]
    Material bloomMaterial;
    [Range(1, 16)]
    public int iterations = 1;
    [Range(0, 10)]
    public float threshold = 1;
    [Range(0, 1)]
    public float power = 1;

    [Range(0, 100)]
    public float exposure = 1;

    public Shader bloomShader;

    const int FilterPass = 0;
    const int DownScalePass = 1;
    const int UpScalePass = 2;
    const int MergePass = 3;
    const int MergePassBackDrop = 4;
    float magIncrease = 13;

    RenderTexture[] textures = new RenderTexture[16];

    PlayerMovement player;
    public PlayerMovement Player { get { return player; } }
    public bool reRender = true;

    private void Awake()
    {
        if (instance == null)
        {
           // DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            instance.Start();
            GameObject.Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        player = GetComponent<PlayerMovement>();
        // starList = JSONReader.ParseStarJSON(JSONReader.ReadJSON("gaia-parsed0"));
        textureHeight = Screen.height;
        textureWidth = Screen.width;

        starList = JSONReader.ParseStarJSONList(JSONReader.ReadJSONFiles("gaia", 1));
        //BinaryManager.StarListToBinary(starList, "aapje");
        //starList = BinaryManager.BinaryToStarList("aapje");

        renderTex = new RenderTexture(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
        renderTex.enableRandomWrite = true;
        renderTex.Create();

        RenderStars(true, true);
    }

    /// <summary>
    /// Render all visible stars onto a texture. 
    /// </summary>
    /// <param name="updateStars"></param>
    /// <param name="newTex"></param>
    private void RenderStars(bool updateStars, bool newTex)
    {
        ComputeBuffer starBuffer = null;
        ComputeBuffer returnStarBuffer = null;
        if (newTex)
        {
            renderTex = RenderTexture.GetTemporary(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
            renderTex.enableRandomWrite = true;
            renderTex.autoGenerateMips = false;
            renderTex.Create();
        }
        int kernel = starRenderShader.FindKernel("CSClear");
        if (clear)
        {
            starRenderShader.SetTexture(kernel, "Result", renderTex);
            starRenderShader.Dispatch(kernel, renderTex.width / 7, renderTex.height / 7, 1);
        }

        kernel = starRenderShader.FindKernel("CSMain");

        starRenderShader.SetInt("textureHeight", textureHeight);
        starRenderShader.SetInt("textureWidth", textureWidth);

        //transform.eulerAngles ipv EulerAnglesOrientation
        float leftBound = player.EulerAnglesOrientation.x - (float)(player.FoV / 2);
        float rightBound = player.EulerAnglesOrientation.x + (float)(player.FoV / 2);
        float downBound = player.EulerAnglesOrientation.y - (float)(player.FoV * ((float)Screen.height / Screen.width) / 2);
        float upBound = player.EulerAnglesOrientation.y + (float)(player.FoV * ((float)Screen.height / Screen.width) / 2);
        bool exceedLeft = false;
        bool exceedRight = false;
        bool exceedDown = false;
        bool exceedUp = false;

        int kernerlStarNumber = starCount / 1000;

        //if any of these are true, render a 2nd texture, and merge the two. 
        if (leftBound < 0)
        {
            exceedLeft = true;
        }
        if (downBound < 0)
        {
            exceedDown = true;
        }
        if (rightBound > 360)
        {
            exceedRight = true;
        }
        if (upBound > 180)
        {
            exceedUp = true;
        }

        leftBound /= 180f / Mathf.PI;
        rightBound /= 180f / Mathf.PI;
        downBound /= 180f / Mathf.PI;
        upBound /= 180f / Mathf.PI;
        float twisty = player.EulerAnglesOrientation.z / 180 * Mathf.PI;


        starRenderShader.SetFloat("playerAngle1", player.EulerAnglesOrientation.x / 180 * Mathf.PI);
        starRenderShader.SetFloat("playerAngle2", player.EulerAnglesOrientation.y / 180 * Mathf.PI);
        starRenderShader.SetFloat("playerAngle3", twisty);
        starRenderShader.SetMatrix("rotationMatrix", player.rotationMatrix);

        starRenderShader.SetFloat("horizontalAngleLeftBound", leftBound);
        starRenderShader.SetFloat("horizontalAngleRightBound", rightBound);
        starRenderShader.SetFloat("verticalAngleLeftBound", downBound);
        starRenderShader.SetFloat("verticalAngleRightBound", upBound);
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
            print(starList.Stars[0].x);
            starRenderShader.SetBuffer(0, "starList", starBuffer);
            starRenderShader.SetFloat("starsPerChunkX", 3000);
        }
        
        starRenderShader.SetTexture(kernel, "Result", renderTex);
        starRenderShader.Dispatch(kernel, 3000, 1, 1);
        
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (textureHeight != source.height || textureWidth != source.width)
        {
            textureHeight = source.height;
            textureWidth = source.width;
            RenderStars(false, true);
        }

     //   Graphics.Blit(renderTex, destination);
        if (false) //TODO is voor voorbeeld
        {
            Graphics.Blit(source, destination);
            return;
        }
        if (bloomMaterial == null)
        {
            bloomMaterial = new Material(bloomShader);
            bloomMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        bloomMaterial.SetFloat("_Threshold", threshold);
        //power = Mathf.Clamp((transform.position - focus.transform.position).magnitude / 10, 0.01f, 1);
        bloomMaterial.SetFloat("_Power", power);
        bloomMaterial.SetFloat("_Exposure", exposure);
        int width = source.width / 2;
        int height = source.height / 2;
        RenderTextureFormat format = source.format;


        bloomMaterial.SetTexture("_SourceTex", source);
        RenderTexture currentDestination = RenderTexture.GetTemporary(source.width, source.height, 0, format);
        Graphics.Blit(renderTex, currentDestination, bloomMaterial, MergePassBackDrop);
        RenderTexture merged = currentDestination;
       

        currentDestination = textures[0] =  RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(merged, currentDestination, bloomMaterial, FilterPass);
        RenderTexture currentSource = currentDestination;


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
        
        for (i -= 2; i >= 0; i--)
        {
            currentDestination = textures[i];
            RenderTexture.ReleaseTemporary(currentSource);
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination, bloomMaterial, UpScalePass);
            currentSource = currentDestination;
        }

        bloomMaterial.SetTexture("_SourceTex", merged);
        Graphics.Blit(currentSource, destination, bloomMaterial, MergePass);
        RenderTexture.ReleaseTemporary(currentSource);
        RenderTexture.ReleaseTemporary(merged);
    }



    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.Comma))
        {
            magIncrease -= 0.05f;
            reRender = true;
        }
        if (Input.GetKey(KeyCode.Period))
        {
            magIncrease += 0.05f;
            reRender = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
