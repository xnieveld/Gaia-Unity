using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for stars with a 3d model. 
/// </summary>
public class StarModel : MonoBehaviour {

    /// <summary>
    /// True position of the star
    /// </summary>
    DoubleVector3 truePosition = new DoubleVector3();
    
    /// <summary>
    /// Mesh of the model
    /// </summary>
    Mesh mesh;

    /// <summary>
    /// Renderer of the mesh
    /// </summary>
    private MeshRenderer renderer;

    /// <summary>
    /// Material of the mesh
    /// </summary>
    private Material material;

    // Use this for initialization
    void Start () {

        mesh = GetComponent<MeshFilter>().mesh;
        renderer = GetComponent<MeshRenderer>();
        material = GetComponent<Renderer>().material;

    }
	
	/// <summary>
    /// Update the star's position
    /// </summary>
	void Update () {
        UpdatePosition();

    }

    /// <summary>
    /// Apply the star's parameters onto the model
    /// </summary>
    /// <param name="star">The star</param>
    public void SetParameters(Star star)
    {
        truePosition = new DoubleVector3(star.x, star.y, star.z); //Stars currently do not use double precision as the GPU cannot use that. 
        UpdatePosition(); //Update the position
        material.SetColor("_ColorTemperature", new Color(star.r, star.g, star.b)); //Set the color temperature
        material.SetFloat("_Brightness", star.m);
    }

    /// <summary>
    /// Update the position of the star
    /// </summary>
    private void UpdatePosition()
    {
        DoubleVector3 deltaPosition = truePosition - StarRenderer.Instance.Player.Position;
        transform.position = (Vector3)deltaPosition;
        if (transform.position.magnitude > 10) //If the star is too far, delete the model. Number is arbitrary.
        {
            GameObject.Destroy(gameObject);
        }
    }

    
}
