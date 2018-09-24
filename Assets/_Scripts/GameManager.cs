using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> asteroidPrefabs = new List<GameObject>();
    public List<GameObject> planetPrefabs = new List<GameObject>();
    public List<GameObject> vehiclePrefabs = new List<GameObject>();
    public GameObject ringPrefab;
    public GameObject explosionPrefab;
    public GameObject energyExplosionPrefab;
    public Camera cam;
    [HideInInspector]
    public GameObject player;
    public GameObject jet;
    public GameObject shooter;

    public float asteroidSpawnDelay = 0.8f;
    public int asteroidAmount = 4;

    private List<GameObject> asteroids = new List<GameObject>();
    private List<GameObject> planets = new List<GameObject>();
    private List<GameObject> rings = new List<GameObject>();

    private bool stopSpawning = false;

    

    // Use this for initialization
    void Start() {
        LoadPlayer();
        stopSpawning = false;

        StartCoroutine(SpawnAsteroids(asteroidAmount));
        SpawnPlanet();
        SpawnRing();
	}
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int rn = Random.Range(0, asteroidPrefabs.Count);
            GameObject newAsteroid = Instantiate(asteroidPrefabs[rn]);
            newAsteroid.GetComponent<AsteroidController>().gameManager = this;

            //Random vector for new asteroid
            int randomX = Random.Range(-12, 14);
            int randomY = Random.Range(-4, 6);
            newAsteroid.transform.position = new Vector3(randomX, randomY, 100);
            asteroids.Add(newAsteroid);
            yield return new WaitForSeconds(asteroidSpawnDelay);
        }
    }


    void SpawnPlanet()
    {
        int rn = Random.Range(0, planetPrefabs.Count);
        GameObject newPlanet = Instantiate(planetPrefabs[rn]);
        newPlanet.GetComponent<PlanetController>().gameManager = this;

        //Random vector for new planet
        int pos = Random.Range(0, 2);
        int randomX = 1;
        int randomY = 1;
        if (pos == 1)
        {
            randomX = Random.Range(15, 100);
            randomY = Random.Range(-10, 30);
        }
        else if(pos == 0)
        {
            randomX = Random.Range(-15, -90);
            randomY = Random.Range(-10, 30);
        }
        newPlanet.transform.position = new Vector3(randomX, randomY, 400);
        planets.Add(newPlanet);
        
    }

    void SpawnRing()
    {
        int randomX = Random.Range(-11, 13);
        int randomY = Random.Range(-3, 6);

        GameObject newRing = Instantiate(ringPrefab);
        newRing.GetComponent<RingController>().gameManager = this;

        newRing.transform.position = new Vector3(randomX, randomY, 100);
        rings.Add(newRing);
    }
    
    
    public void PlayerCollisionDetected()
    {
        Handheld.Vibrate();
        GameObject playerExplosion = Instantiate(explosionPrefab);
        playerExplosion.transform.position = player.transform.position;
        ParticleSystem parts = playerExplosion.GetComponent<ParticleSystem>();
        float duration = parts.main.duration;
        Destroy(player);
        Destroy(playerExplosion, duration);
    }

    public void AsteroidKilled(GameObject deadAsteroid)
    {
        GameObject energyExplosion = Instantiate(energyExplosionPrefab);
        energyExplosion.transform.position = deadAsteroid.transform.position;
        ParticleSystem parts = energyExplosion.GetComponent<ParticleSystem>();
        float duration = parts.main.duration;
        Destroy(deadAsteroid);
        Destroy(energyExplosion, duration);
    }

    private void GameOver()
    {

    }
    
    public void AsteroidRemoved(GameObject asteroid)
    {
        asteroids.Remove(asteroid);
        if(!stopSpawning)
            StartCoroutine(SpawnAsteroids(1));
    }
    public void PlanetRemoved(GameObject planet)
    {
        planets.Remove(planet);
        if(!stopSpawning)
            SpawnPlanet();
    }
    public void RingRemoved(GameObject ring)
    {
        rings.Remove(ring);
        if(!stopSpawning)
            SpawnRing();
    }

    private void OnApplicationQuit()
    {
        StopSpawning();
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }

    private void LoadPlayer()
    {
        //Load selected vehicle
        player = Instantiate(vehiclePrefabs[mSettings.shipIndex]);
        player.GetComponent<PlayerController>().gameManager = this;
        player.GetComponent<PlayerController>().cam = cam;

        //Attach jet to vehicle
        GameObject newJet = Instantiate(jet);
        newJet.transform.SetParent(player.transform);

        //Attach IceLance to vehicle
        GameObject newShooter = Instantiate(shooter);
        newShooter.transform.SetParent(player.transform);

        //Load and attach animation
        Animator animator = player.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("animator_controller") as RuntimeAnimatorController;

    }



}
