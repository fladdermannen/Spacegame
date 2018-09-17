using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotator : MonoBehaviour {

    Vector3 rotation = new Vector3();

	// Use this for initialization
	void Start () {
        rotation.Set(0, 20, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotation * Time.deltaTime);
	}
}
