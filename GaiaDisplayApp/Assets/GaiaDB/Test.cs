using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Starting db");
        DB_CTX context = GaiaDB.SetupContext();
        Star star = GaiaDB.GetStar(context, 2);
        Debug.Log(star.id);
        Debug.Log(star.x);
        Debug.Log(star.y);
        Debug.Log(star.z);
        Debug.Log(star.colour);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
