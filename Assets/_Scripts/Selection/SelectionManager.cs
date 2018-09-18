using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public Camera cam;
    CameraController camController;
    public AudioSource audioSwipe;
    
    Vector2 startPos;
    Vector2 direction;
    private bool directionChosen;


    private int selection;

    
    // Use this for initialization
    void Start () {
        camController = cam.GetComponent<CameraController>();
        selection = 0;
	}

    
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown("d"))
        {
            audioSwipe.Play();
            camController.Swiped(1);
            selection++;
            if (selection > 7)
                selection = 0;
        } else if (Input.GetKeyDown("a"))
        {
            audioSwipe.Play();
            camController.Swiped(-1);
            selection--;
            if (selection < 0)
                selection = 7;
        }
#endif

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            switch (myTouch.phase)
            {
                case TouchPhase.Began:
                    startPos = myTouch.position;
                    directionChosen = false;
                    break;

                case TouchPhase.Moved:
                    direction = myTouch.position - startPos;
                    break;

                case TouchPhase.Ended:
                    directionChosen = true;
                    break;
            }

        }
        if (directionChosen)
        {
            audioSwipe.Play();

            if (direction.x < 0)
            {
                camController.Swiped(1);
                selection++;
                if (selection > 7)
                    selection = 0;
                directionChosen = false;
            }else if(direction.x > 0)
            {
                camController.Swiped(-1);
                selection--;
                if (selection < 0)
                    selection = 7;
                directionChosen = false;
            }
            direction.Set(0, 0);
            startPos.Set(0, 0);
        }


    }

    public void OnVehicleSelected()
    {
        mSettings.shipIndex = selection;
        directionChosen = false;
    }


    
    
}
