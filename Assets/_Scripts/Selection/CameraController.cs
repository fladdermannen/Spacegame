using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public List<GameObject> vehicles;
    public GameObject dummy;
    int currentParent;

    Vector3 offset = new Vector3(0, 1, -5);
    Quaternion offsetRotation = new Quaternion(0,0,0,0);

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    // Use this for initialization
    void Start () {
        currentParent = 0;
        for (int i = 0; i < vehicles.Count; i++) {
            dummy.transform.SetParent(vehicles[i].transform);
            dummy.transform.localPosition = offset;
            dummy.transform.localRotation = offsetRotation;
            positions.Add(dummy.transform.position);
            rotations.Add(dummy.transform.rotation);
            
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {

        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        
        transform.rotation = Quaternion.Lerp(currentRotation, rotations[currentParent], 3 * Time.deltaTime);
        transform.position = Vector3.Lerp(currentPosition, positions[currentParent], 3 * Time.deltaTime);
    }

    public void Swiped(int direction)
    {
        if (direction == 1)
        {
            currentParent++;
            if (currentParent > 7)
                currentParent = 0;
            
        }
        else if (direction == -1)
        {
            currentParent--;
            if (currentParent < 0)
                currentParent = 7;
        }
    }
}
