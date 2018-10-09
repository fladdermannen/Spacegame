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
    public GameObject enemyPrefab;
    public GameObject popupTextPrefab;
    public GameObject respawnPrefab;
    public Canvas canvas;
    public Camera cam;
    private GameObject player;
    public GameObject jet;
    public GameObject shooter;
    public GameObject scoreText;
    public float asteroidSpawnDelay = 0.8f;
    public int asteroidAmount = 4;
    
    private List<GameObject> asteroids = new List<GameObject>();
    private List<GameObject> planets = new List<GameObject>();
    private List<GameObject> rings = new List<GameObject>();

    private bool stopSpawning = false;
    private ScoreController scoreController;

    private int asteroidPoints = 300;
    private int enemyPoints = 5000;
    private int scoreFontSize = 16;
    

    // Use this for initialization
    void Start() {
        stopSpawning = false;
        scoreController = scoreText.GetComponent<ScoreController>();
        
        
        StartCoroutine(LoadPlayer());
        StartCoroutine(LoadGameObjects());
        //SpawnEnemy();
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
            asteroids.Add(newAsteroid);
            yield return new WaitForSeconds(asteroidSpawnDelay);
        }
    }


    void SpawnPlanet()
    {
        int rn = Random.Range(0, planetPrefabs.Count);
        GameObject newPlanet = Instantiate(planetPrefabs[rn]);
        newPlanet.GetComponent<PlanetController>().gameManager = this;
        planets.Add(newPlanet);
    }

    void SpawnRing()
    {
        GameObject newRing = Instantiate(ringPrefab);
        newRing.GetComponent<RingController>().gameManager = this;
        rings.Add(newRing);
    }
    
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.GetComponent<EnemyController>().gameManager = this;

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

        GameOver();
    }

    public void AsteroidKilled(GameObject deadAsteroid)
    {
        CreatePopupText("" + asteroidPoints, deadAsteroid.transform, scoreFontSize);
        GameObject energyExplosion = Instantiate(energyExplosionPrefab);
        energyExplosion.transform.position = deadAsteroid.transform.position;
        ParticleSystem parts = energyExplosion.GetComponent<ParticleSystem>();
        float duration = parts.main.duration;
        Destroy(deadAsteroid);
        Destroy(energyExplosion, duration);

        //Add points
        scoreController.AddPoints(asteroidPoints);
    }

    public void EnemyKilled(GameObject deadEnemy)
    {
        CreatePopupText("" + enemyPoints, deadEnemy.transform, scoreFontSize);
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = deadEnemy.transform.position;
        ParticleSystem parts = explosion.GetComponent<ParticleSystem>();
        float duration = parts.main.duration;
        Destroy(deadEnemy);
        Destroy(explosion, duration);

        //Add points
        scoreController.AddPoints(enemyPoints);
    }

    private void GameOver()
    {
        scoreController.StopScore();
    }
    
    public void AsteroidRemoved(GameObject asteroid)
    {
        asteroids.Remove(asteroid);
        if(!stopSpawning)
            StartCoroutine(SpawnAsteroids(1));
    }
    public void PlanetRemoved(GameObject planet)
    {
        //planets.Remove(planet);
        if (!stopSpawning)
        {
            SpawnPlanet();
            if (planets.Count % 3 == 0)
                SpawnEnemy();
        }
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


    private IEnumerator LoadPlayer()
    {
        GameObject respawn = Instantiate(respawnPrefab);
        respawn.transform.position = Vector3.zero;

        //Load selected vehicle
        player = Instantiate(vehiclePrefabs[mSettings.shipIndex]);
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.enabled = false;
        pc.gameManager = this;
        pc.cam = cam;

        //Load and attach animation
        Animator animator = player.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("animator_controller") as RuntimeAnimatorController;
        animator.Play("Invincibility");

        yield return new WaitForSeconds(2f);

        //Attach jet to vehicle
        GameObject newJet = Instantiate(jet);
        newJet.transform.SetParent(player.transform);

        //Attach IceLance to vehicle
        GameObject newShooter = Instantiate(shooter);
        newShooter.transform.SetParent(player.transform);

        animator.Play("ShipAnimation");
        pc.enabled = true;
    }

    private IEnumerator LoadGameObjects()
    {
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(SpawnAsteroids(asteroidAmount));
        SpawnPlanet();
        SpawnRing();
    }

    public void CreatePopupText(string text, Transform location, int fontSize)
    {
        GameObject popup = Instantiate(popupTextPrefab);
        PopupController pop = popup.GetComponent<PopupController>();

        popup.transform.SetParent(canvas.transform, false);
        pop.SetText(text);
        pop.SetFontSize(fontSize);

        Vector3 screenPosition = cam.WorldToScreenPoint(location.position);
        screenPosition += new Vector3(0, 12, 0);
        popup.transform.position = screenPosition;
    }

}
