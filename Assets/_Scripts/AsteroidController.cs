using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;
    
    Vector3 movement;
    float speed = 50;

	// Use this for initialization
	void Start () {
        movement.Set(0, 0, -1);
        movement = movement.normalized * speed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = (transform.position + movement);
	}

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        gameManager.AsteroidRemoved(gameObject);
    }

    
}
