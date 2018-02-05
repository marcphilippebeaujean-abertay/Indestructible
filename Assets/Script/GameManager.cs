using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject[] spawnerPositions;
    public GameObject[] nodes;
    public GameObject comboParticle;
    public GameObject[] comboParticles;
    public GameObject[] buttons;
    public Material[] buttonMaterials;
    public GameObject nodePrefab;
    public GameObject[] healthBar;
    public GameObject[] multiplierBar;
    public AudioClip[] music;
    public AudioClip[] sounds;
    public AudioClip[] nodeSounds;
    public AudioClip[] vocalSounds;
    public AudioSource audioSource1;
    public AudioSource audioSourceNodes;
    public AudioSource musicSource;
    public AudioSource vocalsSource;
    public Anim animationPlayer;
    public GameObject particle;
    public GameObject[] deathParticles;
    public float dropRate = 1.0f;
    public Text scoreText;
    public float initSpawningRate;
    public float difficultyIncrement;
    public float minRate = 2.0f;
    float spawningRate;
    public int maxSpawnedNodes;
    public int maxSpawnedParticles;
    int multiplier = 1;
    int score = 0;
    int health = 5;
    float movTimer = 0.0f;
    float timer = 0.0f;

    public enum GameState
    {
        PREAMBLE,
        INTRO,
        MENU,
        LOADING,
        RUNNING,
        GAMEOVER,
        GIVEUP,
        FINALMENU
    }

    GameState gameState;

	// Use this for initialization
	void Start ()
    {
        // locate object spawners
        spawnerPositions = GameObject.FindGameObjectsWithTag("spawner");
        spawningRate = initSpawningRate;
        //updateUI();
        gameState = GameState.PREAMBLE;
        audioSource1 = GetComponent<AudioSource>();
        musicSource.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.PREAMBLE:
                // check if the transition is complete
                if (animationPlayer.transitionStatus() == false)
                {
                    // switch to intro animation
                    animationPlayer.switchAnim(1);
                    gameState = GameState.INTRO;
                    musicSource.clip = music[0];
                    musicSource.Play();
                }
                break;
            case GameState.INTRO:
                // check if the transition is complete
                if(animationPlayer.transitionStatus() == false)
                {
                    animationPlayer.switchAnim(7);
                    gameState = GameState.MENU;
                }
                break;
            case GameState.MENU:
                //do something
                if (Input.GetKeyDown("[5]") || Input.GetKeyDown("5"))
                {
                    //do something
                    animationPlayer.switchAnim(5);
                    gameState = GameState.LOADING;
                }
                break;
            case GameState.LOADING:
                if(animationPlayer.transitionStatus() == false)
                {
                    animationPlayer.clearDisplay();
                    animationPlayer.switchAnim(2);
                    gameState = GameState.RUNNING;
                    InitNewGame();
                }
                break;
            case GameState.RUNNING:
                // increment timer
                timer += Time.deltaTime;
                movTimer += Time.deltaTime;
                if (timer > spawningRate)
                {
                    // reset timer
                    timer = 0.0f;
                    // spawn a new object
                    SpawnNode();
                }
                if (movTimer > dropRate)
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (nodes[i].GetComponent<Node>().getActiveStatus())
                        {
                            nodes[i].GetComponent<Node>().advanceNode();
                        }
                    }
                    movTimer = 0.0f;
                }
                // iterate through numpad keycodes - keep in mind that the keys on nokia are inversed to the numpad keys
                if (Input.GetKeyDown("[7]") || Input.GetKeyDown("1"))
                {
                    InitButtonPress(0);
                    audioSource1.clip = sounds[4];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[8]") || Input.GetKeyDown("2"))
                {
                    InitButtonPress(1);
                    audioSource1.clip = sounds[5];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[9]") || Input.GetKeyDown("3"))
                {
                    InitButtonPress(2);
                    audioSource1.clip = sounds[6];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[4]") || Input.GetKeyDown("4"))
                {
                    InitButtonPress(3);
                    audioSource1.clip = sounds[7];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[5]") || Input.GetKeyDown("5"))
                {
                    InitButtonPress(4);
                }
                if (Input.GetKeyDown("[6]") || Input.GetKeyDown("6"))
                {
                    InitButtonPress(5);
                    audioSource1.clip = sounds[8];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[1]") || Input.GetKeyDown("7"))
                {
                    InitButtonPress(6);
                    audioSource1.clip = sounds[9];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[2]") || Input.GetKeyDown("8"))
                {
                    InitButtonPress(7);
                    audioSource1.clip = sounds[10];
                    audioSource1.Play();
                }
                if (Input.GetKeyDown("[3]") || Input.GetKeyDown("9"))
                {
                    InitButtonPress(8);
                    audioSource1.clip = sounds[11];
                    audioSource1.Play();
                }
                break;
            case GameState.GAMEOVER:
                if (animationPlayer.transitionStatus() == false)
                {
                    gameState = GameState.GIVEUP;
                    animationPlayer.clearDisplay();
                    animationPlayer.switchAnim(4);
                }
                break;
            case GameState.GIVEUP:
                if (animationPlayer.transitionStatus() == false)
                {
                    animationPlayer.clearDisplay();
                    animationPlayer.switchAnim(8);
                    gameState = GameState.FINALMENU;
                }
                break;
            case GameState.FINALMENU:
                if (Input.GetKeyDown("[4]") || Input.GetKeyDown("4"))
                {
                    animationPlayer.toggleGiveUpOption(false);
                }
                if (Input.GetKeyDown("[6]") || Input.GetKeyDown("6"))
                {
                    animationPlayer.toggleGiveUpOption(true);
                }
                if (Input.GetKeyDown("[5]") || Input.GetKeyDown("5"))
                {
                    if(animationPlayer.wantsToGiveUp())
                    {
                        animationPlayer.switchAnim(6);
                        gameState = GameState.PREAMBLE;
                    }
                    else
                    {
                        animationPlayer.clearDisplay();
                        animationPlayer.switchAnim(5);
                        Debug.Log("running loading animation");
                        gameState = GameState.LOADING;
                    }
                }
                break;
        }
    }

    void InitNewGame()
    {
        score = 0;
        health = 5;
        multiplier = 1;
        updateUI();
        musicSource.clip = music[1];
        musicSource.Play();
    }

    void SpawnNode()
    {
        if (spawningRate > minRate)
        {
            spawningRate -= difficultyIncrement;
        }
        // create temporary variable for rng
        int randomSelect = Random.Range(0, spawnerPositions.Length);
        // check if node prefabs still need to be spawned into the scene
        if (nodes.Length < maxSpawnedNodes)
        {
            // if so, spawn an object at a randomly allocated position
            Instantiate(nodePrefab, spawnerPositions[randomSelect].transform.position, new Quaternion(0, 180, 0, 0));
            // find nodes in scene and add them to the list
            nodes = GameObject.FindGameObjectsWithTag("node");
        }
        else
        {
            // use pooling to move spanwed objects to the position of one of the spawners
            for(int i = 0; i < nodes.Length; i++)
            {
                // check for in-active ndoes in the list
                if(nodes[i].GetComponent<Node>().getActiveStatus() == false)
                {
                    // this node is not active, send it to the position of a spawner
                    nodes[i].transform.position = spawnerPositions[randomSelect].transform.position;
                    // reset node
                    nodes[i].GetComponent<Node>().toggleActiveStatus();
                    // stop another node from respawning
                    break;
                }
            }
        }
    }

    void InitButtonPress(int button)
    {
        // create bool to determine if our button press returned a correct button press
        bool correctButtonPressed = false;
        // create list for combo counter
        List<Vector3> comboPos = new List<Vector3>();
        // iterate through each node
        for(int i = 0; i < nodes.Length; i++)
        {
            // check if the right button was pressed
            if(nodes[i].GetComponent<Node>().buttonPressed(button))
            {
                // we have successfully pressed a button that correlates to one of the active nodes - toggle the bool
                correctButtonPressed = true;
                // if our node is inactive after we pressed the right button, we know that we scored!
                if(!nodes[i].GetComponent<Node>().getActiveStatus())
                {
                    // one of the nodes has been disabled after we called the button press - add to the score
                    score += multiplier;
                    // play sound
                    audioSourceNodes.clip = nodeSounds[Random.Range(0, 2)];
                    audioSourceNodes.Play();
                    // create particle
                    if (deathParticles.Length < maxSpawnedParticles)
                    {
                        Debug.Log("creating particle!");
                        // create a new particle
                        Instantiate(particle, nodes[i].transform.position, Quaternion.identity);
                        deathParticles = GameObject.FindGameObjectsWithTag("particle");
                    }
                    else
                    {
                        for (int p = 0; p < deathParticles.Length; p++)
                        {
                            Particle particleScript = deathParticles[p].GetComponent<Particle>();
                            if (particleScript.isActive() == false)
                            {
                                // we found a particle - move it to the right position and activate it
                                deathParticles[p].transform.position = nodes[i].transform.position;
                                particleScript.activateParticle();
                                break;
                            }
                        }
                    }
                    // add to the combo deathParticles list
                    // comboPos.Add(nodes[i].GetComponent<Transform>().position);
                    if (multiplier < 5)
                    {
                        // increment the multiplier
                        multiplier++;
                    }
                    Debug.Log("Score = " + score);
                    updateUI();
                }
            }
        }
        // check if combo list is bigger than one
        if (comboPos.Count > 1)
        {
            for (int i = 0; i < comboPos.Count; i++)
            {
                if (comboParticles.Length < maxSpawnedParticles)
                {
                    // create a new particle
                    Instantiate(comboParticle, comboPos[i], Quaternion.identity);
                    deathParticles = GameObject.FindGameObjectsWithTag("particle");
                }
                else
                {
                    for (int p = 0; p < comboParticles.Length; p++)
                    {
                        Particle particleScript = deathParticles[p].GetComponent<Particle>();
                        if (particleScript.isActive() == false)
                        {
                            // we found a particle - move it to the right position and activate it
                            deathParticles[p].transform.position = comboPos[i];
                            particleScript.activateParticle();
                            break;
                        }
                    }
                }
            }
            // play audio
            vocalsSource.clip = vocalSounds[0];
            vocalsSource.Play();
        }
        if(!correctButtonPressed)
        {
            missedNode();
        }
    }

    public void missedNode()
    {
        // reset multiplier
        multiplier = 1;
        if (health > 0)
        {
            // decrement health
            health--;
            updateUI();
        }
        else
        {
            // game over
            animationPlayer.clearDisplay();
            animationPlayer.switchAnim(3);
            for (int i = 0; i < multiplierBar.Length; i++)
            {
                multiplierBar[i].SetActive(false);
            }
            for (int i = 0; i < healthBar.Length; i++)
            {
                healthBar[i].SetActive(false);
            }
            for(int i = 0; i < nodes.Length; i++)
            {
                nodes[i].GetComponent<Node>().deactivateNode();
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Renderer>().material = buttonMaterials[0];
            }
            spawningRate = initSpawningRate;
            gameState = GameState.GAMEOVER;
            score = 0;
            health = 5;
            musicSource.Pause();
            vocalsSource.clip = vocalSounds[1];
            vocalsSource.Play();
        }
    }

    void updateUI()
    {
        // display multiplier ui elements
        for(int i = 0; i < multiplierBar.Length; i++)
        {
            if(multiplier == i)
            {
                multiplierBar[i].SetActive(true);
            }
            else
            {
                multiplierBar[i].SetActive(false);
            }
        }
        // display health ui elements
        for (int i = 0; i < healthBar.Length; i++)
        {
            if (health == i)
            {
                healthBar[i].SetActive(true);
                Debug.Log("activating ui thing");
            }
            else
            {
                healthBar[i].SetActive(false);
            }
        }
        scoreText.text = "SCORE: " + score;
    }

    public void updateButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Renderer>().material = buttonMaterials[0];
        }
        for (int x = 0; x < nodes.Length; x++)
        {
            Node nodeScript = nodes[x].GetComponent<Node>();
            // check if node is active
            if (nodeScript.getActiveStatus() == true)
            {
                // check if key requirement correlates to button
                buttons[nodeScript.getKeyReq()].GetComponent<MeshRenderer>().material = buttonMaterials[1];
            }
        }
    }
}
