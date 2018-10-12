using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;

    private float speed = 1;

    Vector3 targetPosition;
    private bool firstPosition = true;
    private float wait = 3f;
    private int hitpoints = 10;
    
    private void Start()
    {
        StartCoroutine(NewTargetPosition());
    }

    void Update () {

        transform.LookAt(Camera.main.transform);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

    }

    IEnumerator NewTargetPosition()
    {
        while (gameObject != null)
        {
            if (firstPosition)
                targetPosition = new Vector3(0, 0, 20);
            firstPosition = false;

            int rx = Random.Range(-7, 7);
            int ry = Random.Range(-4, 4);
            targetPosition = new Vector3(rx, ry, 20);

            yield return new WaitForSeconds(wait);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        hitpoints--;
        if (hitpoints == 0)
        {
            gameManager.EnemyKilled(gameObject);
        }
    }
}
