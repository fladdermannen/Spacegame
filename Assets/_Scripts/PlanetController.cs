using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;

    private Vector3 movement;
    float speed = 20;
    float rotationSpeed = 20;


	// Use this for initialization
	void Start () {
        movement.Set(0, 0, -1);
        movement = movement.normalized * speed * Time.deltaTime;


    }
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = (transform.position + movement);
        gameObject.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        gameManager.PlanetRemoved(gameObject);
    }



}
