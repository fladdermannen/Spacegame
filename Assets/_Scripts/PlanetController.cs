using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;

    private Vector3 movement;
    float speed = 20;
    float rotationSpeed = 20;

    private void Awake()
    {
        //Random vector for new planet
        int pos = Random.Range(0, 2);
        int randomX = 100;
        int randomY = 100;
        if (pos == 1)
        {
            randomX = Random.Range(15, 100);
            randomY = Random.Range(-10, 30);
        }
        else if (pos == 0)
        {
            randomX = Random.Range(-15, -90);
            randomY = Random.Range(-10, 30);
        }
        transform.position = new Vector3(randomX, randomY, 400);
    }
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
