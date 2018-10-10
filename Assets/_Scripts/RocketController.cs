using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class RocketController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;
    ParticleSystem mSystem;
   
    private GameObject target;

    float rotationSpeed = 10000f;
   
    private void Start()
    {
        mSystem = GetComponent<ParticleSystem>();

        Vector3 targetSpeed = new Vector3(0, 0, -1) * 50.001163315f;

        bool acquireTargetLockSuccess;
        Vector3 direction = CalculateInterceptCourse(target.transform.position, targetSpeed, transform.position, 50, out acquireTargetLockSuccess);

        if(acquireTargetLockSuccess)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.zero);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (Mathf.Abs(transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y) < 3 && target.transform.position.z < 90)
            {
                mSystem.Play();
            }
        }

    }

    public Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed, out bool aSuccess)
    {
        aSuccess = true;
        Vector3 targetDir = aTargetPos - aInterceptorPos;
        float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
        float tSpeed2 = aTargetSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)
            aSuccess = false;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + aTargetSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + aTargetSpeed;
        else if (S1 < S2)
            return (S2) * targetDir + aTargetSpeed;
        else
            return (S1) * targetDir + aTargetSpeed;
    }

    public void PassTarget(GameObject asteroid)
    {
        target = asteroid;
    }


}
