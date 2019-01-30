using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the player movement. The player moves in their own coordinate system, not using Unity's coords.
/// </summary>
public class PlayerMovement : MonoBehaviour {
    
    DoubleVector3 position = new DoubleVector3();
    Vector3 eulerAnglesOrientation = new Vector3(180, 90, 0);
    double fov = 90;
    GameObject starPrefab;
    List<GameObject> currentModelStars = new List<GameObject>();
    public Matrix4x4 rotationMatrix = new Matrix4x4();

    /// <summary>
    /// Load a new star model. 
    /// </summary>
    /// <param name="star">The parameters of the star to be loaded.</param>
    public void LoadStar(Star star)
    {
        GameObject newStar = GameObject.Instantiate(starPrefab);
        newStar.GetComponent<StarModel>().SetParameters(star);
        currentModelStars.Add(newStar);

        ///Hoe vinden we welke sterren dichtbij genoeg zijn? .. hm.. Misschien een deel van de shader dat ie ook een list terugstuurt, een uitleesbare buffer, waarin gekeken wordt of de ster dichtbij genoeg is.
    }


    // Update is called once per frame
    void Update()
    {
        bool change = false;
        float speed = 1;
        transform.rotation = Quaternion.Euler(eulerAnglesOrientation);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            change = true;
        }
        if (Input.GetKey(KeyCode.O))
        {
            fov *= 1.03f;
            change = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            fov /= 1.03f;
            change = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.Rotate(new Vector3(0, (float)fov / 90, 0));
            float a = (float)fov / 90;
            rotationMatrix *= new Matrix4x4(new Vector4(Mathf.Cos(a), Mathf.Sin(a), 0, 0), new Vector4(-Mathf.Sin(a), Mathf.Cos(a), 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.y += (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.Rotate(new Vector3(0, -(float)fov / 90, 0));
            float a = -(float)fov / 90;
            rotationMatrix *= new Matrix4x4(new Vector4(Mathf.Cos(a), Mathf.Sin(a), 0, 0), new Vector4(-Mathf.Sin(a), Mathf.Cos(a), 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.y -= (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float a = (float)fov / 90;
            rotationMatrix *= new Matrix4x4(new Vector4(Mathf.Cos(a),0,  -Mathf.Sin(a), 0), new Vector4(0, 1, 0, 0), new Vector4(Mathf.Sin(a), 0, Mathf.Cos(a), 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.x += (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float a = -(float)fov / 90;
            rotationMatrix *= new Matrix4x4(new Vector4(Mathf.Cos(a), 0, -Mathf.Sin(a), 0), new Vector4(0, 1, 0, 0), new Vector4(Mathf.Sin(a), 0, Mathf.Cos(a), 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.x -= (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            float a = -1;
            rotationMatrix *= new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, Mathf.Cos(a), Mathf.Sin(a), 0), new Vector4(0, -Mathf.Sin(a), Mathf.Cos(a), 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.z -= 1;
            change = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            float a = 1;
            rotationMatrix *= new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, Mathf.Cos(a), Mathf.Sin(a), 0), new Vector4(0, -Mathf.Sin(a), Mathf.Cos(a), 0), new Vector4(0, 0, 0, 1));
            eulerAnglesOrientation.z += 1;
            change = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            position += Vector3.forward * speed;
            change = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position -= Vector3.right * speed;
            change = true;
        }
        if (Input.GetKey(KeyCode.S))
        {

            position -= Vector3.forward * speed;
            change = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position += Vector3.right * speed;
            change = true;
        }
        if (change)
        {
            eulerAnglesOrientation.x = ClampAngle(eulerAnglesOrientation.x);
            eulerAnglesOrientation.y = ClampAngle(eulerAnglesOrientation.y, true);
            eulerAnglesOrientation.z = ClampAngle(eulerAnglesOrientation.z);
            if (fov < 0.01)
            {
                fov = 0.01;
            }
            if (fov > 170)
            {
                //fov = 170;
            }
            //print(eulerAnglesOrientation);
            StarRenderer.Instance.reRender = true;
        }
    }

    private float ClampAngle(float angle,  bool half = false)
    {
        float maxVal = 360;
        if (half)
            maxVal /= 2;
        while (angle > maxVal)
        {
            angle -= maxVal;
        }
        while(angle < 0)
        {
            angle += maxVal;
        }
        return angle;
    }


    /// <summary>
    /// Position of the player.
    /// </summary>
    public DoubleVector3 Position
    {
        get
        {
            return position;
        }
    }

    /// <summary>
    /// Position of the player casted to floats.
    /// </summary>
    public Vector3 PositionFloat
    {
        get
        {
            return (Vector3)position;
        }
    }

    /// <summary>
    /// Orientation of the player.
    /// </summary>
    public Vector3 EulerAnglesOrientation
    {
        get
        {
            Vector3 forwardPoint = transform.forward;
            Vector3 orientation = new Vector3();


            orientation.y = Mathf.Atan(forwardPoint.z / forwardPoint.x);
            if (forwardPoint.x >= 0)
            {
                orientation.x += Mathf.PI;
            }
            if (float.IsNaN(orientation.x))
            {
                if (forwardPoint.x >= 0)
                {
                    orientation.x = 0;
                }
                else
                {
                    orientation.x = Mathf.PI;
                }
            }
            orientation.x = Mathf.Acos(forwardPoint.y);
            orientation.z = eulerAnglesOrientation.z;
            orientation.x *= 180 / Mathf.PI;
            orientation.y *= 180 / Mathf.PI;

            return eulerAnglesOrientation;
        }
    }

    /// <summary>
    /// The field of vision
    /// </summary>
    public double FoV
    {
        get
        {
            return fov;
        }
    }
}
