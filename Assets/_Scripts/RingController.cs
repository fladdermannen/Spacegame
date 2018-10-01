using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;

    Vector3 movement;
    float speed = 10;

    private void Awake()
    {
        int randomX = Random.Range(-11, 13);
        int randomY = Random.Range(-3, 6);
        transform.position = new Vector3(randomX, randomY, 100);
    }

    // Use this for initialization
    void Start () {
        movement.Set(0, 0, -1);
        movement = movement.normalized * speed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + movement;
	}

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        gameManager.RingRemoved(gameObject);
    }
}
