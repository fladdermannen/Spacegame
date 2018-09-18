using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public GameManager gameManager;
    public Camera cam;
    public float speed = 2f;

    Vector3 movement;

    Rigidbody playerRigidbody;

    private bool DontMove = false;

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Move(h, v);
        }
        //Move(h,v);
#endif


        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);

            //Check if clicking the exit button to ignore player inputs
            if (myTouch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(myTouch.fingerId))
                {
                    DontMove = true;
                }
            }

            else if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)
            {
                if (!DontMove)
                {
                    Vector3 touchPos = cam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, 10f));
                    transform.position = Vector3.Lerp(transform.position, touchPos, speed * Time.deltaTime);
                }
            }

            if (myTouch.phase == TouchPhase.Ended)
            {
                DontMove = false;
            }
            
        }
    }

    
    //För editorn
    public void Move(float h, float v)
    {

        movement.Set(h, v, 0f);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Ring")
        {
            Debug.Log("Ring collided");
        }

        Debug.Log("Collision");
        gameManager.CollisionDetected();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ring")
        {
            Debug.Log("Ring trigger enter");
        }
    }


}
