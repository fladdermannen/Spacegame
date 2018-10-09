using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public GameManager gameManager;
    public Camera cam;

    private int maxHitpoints = 5;
    private int currentHitpoints;
    private float speed = 7f;
    private bool dontMove = false;
    private bool recentlyHit = false;
    private bool invincible = false;
    private WaitForSeconds immortalTime = new WaitForSeconds(7f);
    private CapsuleCollider col;
    private Animator ani;

    Vector3 movement;
    Rigidbody playerRigidbody;


	// Use this for initialization
	void Start () {
        currentHitpoints = maxHitpoints;
        playerRigidbody = GetComponent<Rigidbody>();
        col = gameObject.GetComponent<CapsuleCollider>();
        ani = gameObject.GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (!EventSystem.current.IsPointerOverGameObject() && !dontMove)
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
                    dontMove = true;
                }
            }

            else if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)
            {
                if (!dontMove)
                {
                    Vector3 touchPos = cam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, 10f));
                    transform.position = Vector3.Lerp(transform.position, touchPos, speed * Time.deltaTime);
                }
            }

            if (myTouch.phase == TouchPhase.Ended)
            {
                dontMove = false;
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

        Debug.Log("Collision with " + collision.gameObject.name);
        gameManager.PlayerCollisionDetected();
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ring")
        {
            Debug.Log("Ring trigger enter");
            if(currentHitpoints==5)
                StartCoroutine(Invincibility());
            else
            {
                RestoreHitpoints();
            }

            gameManager.FireRockets();
        }
    }

    private IEnumerator Invincibility()
    {
        invincible = true;
        gameManager.CreatePopupText("Invincibility!", gameObject.transform, 20);
        ani.Play("Invincibility");
        col.enabled = false;

        yield return immortalTime;

        invincible = false;
        ani.Play("ShipAnimation");
        col.enabled = true;
    }

    private void RestoreHitpoints()
    {
        currentHitpoints = maxHitpoints;
        gameManager.CreatePopupText("Ship repaired!", gameObject.transform, 20);
        gameManager.PlayerHealed();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "EnemyIceLance" && !recentlyHit && !invincible)
        {
            currentHitpoints--;
            Debug.Log(currentHitpoints);

            gameManager.PlayerHit();
            StartCoroutine(RecentlyHit());

            if (currentHitpoints == 2)
            {
                gameManager.PlayerLowHealth();
            }
            else if (currentHitpoints == 0)
            {
                gameManager.PlayerCollisionDetected();
                gameManager.PlayerHealed();
            }
        }
            
    }

    private IEnumerator RecentlyHit()
    {
        recentlyHit = true;
        yield return new WaitForSeconds(1f);
        recentlyHit = false;
    }


}
