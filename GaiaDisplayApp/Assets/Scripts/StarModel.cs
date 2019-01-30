using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for stars with a 3d model. 
/// </summary>
public class StarModel : MonoBehaviour {

    DoubleVector3 truePosition = new DoubleVector3(); //The true position of the star in double coordinates. 
    Mesh mesh;
    private MeshRenderer renderer;
    private Material material;

    // Use this for initialization
    void Start () {

        mesh = GetComponent<MeshFilter>().mesh;
        renderer = GetComponent<MeshRenderer>();
        material = GetComponent<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();

    }

    public void SetParameters(Star star)
    {
        truePosition = new DoubleVector3(star.x, star.y, star.z);
        UpdatePosition();
        material.SetColor("_ColorTemperature", Color.white);
        material.SetFloat("_Brightness", star.m);
    }

    private void UpdatePosition()
    {
        DoubleVector3 deltaPosition = truePosition - StarRenderer.Instance.Player.Position;
        transform.position = (Vector3)deltaPosition / 1000000;
        if (transform.position.magnitude > 100000)
        {
            GameObject.Destroy(gameObject);
        }
    }

    
}
