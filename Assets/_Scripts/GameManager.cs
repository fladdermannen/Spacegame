using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject sparksPrefab;
    public GameObject smokePrefab;
    public GameObject rocketPrefab;
    public Canvas canvas;
    public Camera cam;
    public GameObject gameOverPanel;
    public GameObject exitButton;
    public GameObject jet;
    public GameObject shooter;
    public GameObject scoreText;
    public GameObject healedSfx;
    public GameObject invincibleSfx;
    public GameObject spawnSfx;
    public float asteroidSpawnDelay = 0.8f;
    public int asteroidAmount = 4;

    private GameObject player;
    private GameObject smoke;

    private List<GameObject> asteroids = new List<GameObject>();
    private List<GameObject> planets = new List<GameObject>();
    private List<GameObject> rings = new List<GameObject>();

    private bool stopSpawning = false;
    private ScoreController scoreController;

    private int asteroidPoints = 300;
    private int enemyPoints = 5000;
    private int scoreFontSize = 16;
    private int enemyWave = 0;

    private List<GameObject> rockets = new List<GameObject>();

    // Use this for initialization
    void Start() {
        stopSpawning = false;
        scoreController = scoreText.GetComponent<ScoreController>();
        healedSfx.GetComponent<AudioSource>().volume = mSettings.sfxVolume;
        invincibleSfx.GetComponent<AudioSource>().volume = mSettings.sfxVolume;
        spawnSfx.GetComponent<AudioSource>().volume = mSettings.sfxVolume;
        
        
        StartCoroutine(LoadPlayer());
        StartCoroutine(LoadGameObjects());
        //SpawnEnemy(enemiesKilled);

    }
	
	// Update is called once per frame
	void Update () {
       if (Input.GetKeyDown("space"))
        {
            FireRockets();
        }
    }

    IEnumerator SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int rn = Random.Range(0, asteroidPrefabs.Count);
            GameObject newAsteroid = Instantiate(asteroidPrefabs[rn]);

            // Fade in alpha color of material
            Material mat = newAsteroid.GetComponent<MeshRenderer>().material;
            Color c = mat.color;
            newAsteroid.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
            LeanTween.value(newAsteroid, 0, 1, 1f).setOnUpdate( (float val) =>
            {
                mat.color = new Color(c.r, c.g, c.b, val);
            });
            
            newAsteroid.GetComponent<AsteroidController>().gameManager = this;
            asteroids.Add(newAsteroid);
            yield return new WaitForSeconds(asteroidSpawnDelay);
        }
    }



    void SpawnPlanet()
    {
        int rn = Random.Range(0, planetPrefabs.Count);
        GameObject newPlanet = Instantiate(planetPrefabs[rn]);

        //Fade in alpha color
        Material mat = newPlanet.GetComponent<MeshRenderer>().material;
        Color c = mat.color;
        newPlanet.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
        LeanTween.value(newPlanet, 0, 1, 2f).setOnUpdate((float val) =>
        {
            mat.color = new Color(c.r, c.g, c.b, val);
        });

        newPlanet.GetComponent<PlanetController>().gameManager = this;
        planets.Add(newPlanet);
    }

    void SpawnRing()
    {
        GameObject newRing = Instantiate(ringPrefab);

        //Fade in alpha color
        Material mat = newRing.GetComponent<MeshRenderer>().material;
        Color c = mat.color;
        newRing.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
        LeanTween.value(newRing, 0, 1, 1f).setOnUpdate((float val) =>
        {
            mat.color = new Color(c.r, c.g, c.b, val);
        });

        newRing.GetComponent<RingController>().gameManager = this;
        rings.Add(newRing);
    }
    
    public void SpawnEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<EnemyController>().gameManager = this;

        }
    }

    public void PlayerHit()
    {
        GameObject sparks = Instantiate(sparksPrefab);
        sparks.transform.SetParent(player.transform);
        sparks.transform.position = player.transform.position;
        ParticleSystem parts = sparks.GetComponent<ParticleSystem>();
        Destroy(sparks, parts.main.duration);
    }

    public void PlayerLowHealth()
    {
        smoke = Instantiate(smokePrefab);
        smoke.transform.position = player.transform.position;
        smoke.transform.SetParent(player.transform);
    }

    public void PlayerHealed()
    {
        Destroy(smoke);
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
        exitButton.SetActive(false);
        gameOverPanel.SetActive(true);
        LeanTween.size(gameOverPanel.GetComponent<RectTransform>(), Vector3.zero, 1.5f);
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
            if (planets.Count % 2 == 0)
            {
                enemyWave++;
                SpawnEnemy(enemyWave);
                StartCoroutine(SpawnAsteroids(1));
            }
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
        scoreController.StopScore();
    }


    private IEnumerator LoadPlayer()
    {
        spawnSfx.GetComponent<AudioSource>().Play();
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
        spawnSfx.GetComponent<AudioSource>().Stop();
    }

    private IEnumerator LoadGameObjects()
    {
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(SpawnAsteroids(asteroidAmount));
        SpawnPlanet();
        SpawnRing();
        GameObject ps = GameObject.FindGameObjectWithTag("ParticleSystem");
        ps.GetComponent<ParticleSystem>().Play();
        scoreController.EnableScore();
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

    public void FireRockets()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab);
            rockets.Add(rocket);
            Vector3 spawnPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z+5);
            rocket.transform.position = spawnPos;

            RocketController rc = rocket.GetComponent<RocketController>();
            rc.gameManager = this;
            rc.PassTarget(asteroids[i]);

            ParticleSystem parts = rocket.GetComponent<ParticleSystem>();
            Destroy(rocket, parts.main.duration);

        }
    }

    public void PlayHealedSfx()
    {
        healedSfx.GetComponent<AudioSource>().Play();
    }

    public IEnumerator PlayInvincibleSfx(WaitForSeconds wfs)
    {
        invincibleSfx.GetComponent<AudioSource>().Play();
        yield return wfs;
        invincibleSfx.GetComponent<AudioSource>().Stop();
    }

    

}
