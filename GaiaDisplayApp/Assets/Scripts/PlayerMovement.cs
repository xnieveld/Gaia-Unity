using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the player movement. The player moves in their own coordinate system, not using Unity's coords.
/// </summary>
public class PlayerMovement : MonoBehaviour {
    /// <summary>
    /// Player position
    /// </summary>
    DoubleVector3 position = new DoubleVector3();

    /// <summary>
    /// Look orientation
    /// </summary>
    Vector3 eulerAnglesOrientation = new Vector3(180, 90, 0);

    /// <summary>
    /// Horizontal field of view
    /// </summary>
    double fov = 90;

    /// <summary>
    /// Prefab of a star model
    /// </summary>
    GameObject starPrefab;

    /// <summary>
    /// List of currently model-loaded in stars.
    /// </summary>
    List<GameObject> currentModelStars = new List<GameObject>();


    /// <summary>
    /// Load a new star model. (Not currently used)
    /// </summary>
    /// <param name="star">The parameters of the star to be loaded.</param>
    public void LoadStar(Star star)
    {
        GameObject newStar = GameObject.Instantiate(starPrefab);
        newStar.GetComponent<StarModel>().SetParameters(star);
        currentModelStars.Add(newStar);
    }


    // Update is called once per frame
    void Update()
    {
        bool change = false; //Have any settings changed?
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
            eulerAnglesOrientation.y += (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.Rotate(new Vector3(0, -(float)fov / 90, 0));
            float a = -(float)fov / 90;
            eulerAnglesOrientation.y -= (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float a = (float)fov / 90;
            eulerAnglesOrientation.x += (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float a = -(float)fov / 90;
            eulerAnglesOrientation.x -= (float)fov / 90;
            change = true;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            float a = -1;
            eulerAnglesOrientation.z -= 1;
            change = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            float a = 1;
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
            eulerAnglesOrientation.x = WrapAngle(eulerAnglesOrientation.x);
            eulerAnglesOrientation.y = WrapAngle(eulerAnglesOrientation.y, true);
            eulerAnglesOrientation.z = WrapAngle(eulerAnglesOrientation.z);
            if (fov < 0.01)
            {
                fov = 0.01;
            }
            if (fov > 170)
            {
                fov = 170;
            }
            //print(eulerAnglesOrientation);
            StarRenderer.Instance.reRender = true;
        }
    }

    /// <summary>
    /// Wrap an angle such that 361 becomes 1, and similar. 
    /// </summary>
    /// <param name="angle">The angle to wrap</param>
    /// <param name="half">Wrap it between 0 and 180</param>
    /// <returns>The wrapped angle</returns>
    private float WrapAngle(float angle,  bool half = false)
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
        {/* //atempt at getting proper angles from quaternions, failed.
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
            orientation.y *= 180 / Mathf.PI;*/

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
