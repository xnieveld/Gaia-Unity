using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarRenderer : MonoBehaviour {

    StarList starList;
    public Material skyboxMaterial;
    public ComputeShader starRenderShader;
    public int textureWidth = 1920;
    public int textureHeight = 1080;
    public int xChunks = 10;
    public int yChunks = 10;
    public int starCount = 10000; // (8 * 20)^2

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
    double xs = 0;
    double xx = 360;
    double ys = 0;
    double yx = 180;
    double xc = 180;
    double yc = 90;
    double w = 360;
    double h = 180;

    RenderTexture[] textures = new RenderTexture[16];

    // Use this for initialization
    void Start () {
        starList = JSONReader.ParseStarJSON(JSONReader.ReadJSON("data-NaN-temp_Max"));
        renderTex = new RenderTexture(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
        renderTex.enableRandomWrite = true;
        renderTex.Create();
        
        RenderStars(true, true);
	}

    private void RenderStars(bool updateStars, bool newTex)
    {
        if (newTex)
        {
            renderTex = RenderTexture.GetTemporary(textureWidth, textureHeight, 24, RenderTextureFormat.ARGBFloat);
            renderTex.enableRandomWrite = true;
            renderTex.autoGenerateMips = false;
            renderTex.Create();
        }
        int kernel = starRenderShader.FindKernel("CSClear");
        starRenderShader.SetTexture(kernel, "Result", renderTex);
        starRenderShader.Dispatch(kernel, renderTex.width / 7, renderTex.height / 7, 1);

        kernel = starRenderShader.FindKernel("CSMain");

        starRenderShader.SetInt("textureHeight", textureHeight);
        starRenderShader.SetInt("textureWidth", textureWidth);
        starRenderShader.SetFloat("xMin", (float)xs);
        starRenderShader.SetFloat("xMax", (float)xx);
        starRenderShader.SetFloat("yMin", (float)ys);
        starRenderShader.SetFloat("yMax", (float)yx);

        if (updateStars)
        {
            ComputeBuffer buffer_0 = new ComputeBuffer(starList.StarCoords.Length, sizeof(float) * 4);
            buffer_0.SetData(starList.StarCoords);
            starRenderShader.SetBuffer(0, "starCoords", buffer_0);

            ComputeBuffer buffer_1 = new ComputeBuffer(starList.StarColours.Length, sizeof(float) * 4);
            buffer_1.SetData(starList.StarColours);
            starRenderShader.SetBuffer(0, "starColours", buffer_1);

            ComputeBuffer buffer_2 = new ComputeBuffer(starList.StarMags.Length, sizeof(float));
            buffer_2.SetData(starList.StarMags);
            starRenderShader.SetBuffer(0, "starMagnitudes", buffer_2);


            starRenderShader.SetFloat("starsPerChunkX", 100);
            starRenderShader.SetFloat("starsPerChunkY", 100);
        }
        
        starRenderShader.SetTexture(kernel, "Result", renderTex);
        //starRenderShader.SetVectorArray("starCoords", starList.StarCoords);
        //starRenderShader.SetVectorArray("starColours", starList.StarColours);
        //starRenderShader.SetFloats("starMagnitudes", starList.StarMags);

        //        starRenderShader.Dispatch(kernel, textureWidth / xChunks, textureHeight / yChunks, 1);
        //starRenderShader.Dispatch(kernel, (int)Mathf.Sqrt(starCount) / xChunks, (int)Mathf.Sqrt(starCount) / yChunks, 1);
        starRenderShader.Dispatch(kernel, 100, 1, 1);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (textureHeight != source.height || textureWidth != source.width)
        {
            textureHeight = source.height;
            textureWidth = source.width;
            RenderStars(false, true);
        }

        Graphics.Blit(renderTex, destination);
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

        RenderTexture currentDestination = textures[0] =
            RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(renderTex, currentDestination, bloomMaterial, FilterPass);
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
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination, bloomMaterial, UpScalePass);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }

        bloomMaterial.SetTexture("_SourceTex", renderTex);
        Graphics.Blit(currentSource, destination, bloomMaterial, MergePass);
        RenderTexture.ReleaseTemporary(currentSource);
    }



    // Update is called once per frame
    void Update ()
    {
        bool change = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            change = true;
        }
        if (Input.GetKey(KeyCode.O))
        {
            h *= 1.03f;
            w *= 1.03f;
            change = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            h /= 1.03f;
            w /= 1.03f;
            change = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yc += h * 0.01f;
            change = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            yc -= h * 0.01f;
            change = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            xc += w * 0.01f;
            change = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xc -= w * 0.01f;
            change = true;
        }
        if (change)
        {
            xs = xc - w / 2;
            xx = xc + w / 2;
            ys = yc - h / 2;
            yx = yc + h / 2;
            RenderStars(false, false);
        }
    }
}
