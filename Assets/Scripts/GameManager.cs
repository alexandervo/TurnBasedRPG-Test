using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //class random monster
    [System.Serializable]
    public class RegionData
    {
        public string regionName;
        public int maxAmountEnemys = 8;
        public string battleScene;
        public List<GameObject> possibleEnemys = new List<GameObject>();
    }

    public int curRegions;
    [Range(1, 10)] public int encounterRate = 5;

    public List<RegionData> Regions = new List<RegionData>();


    //Hero
    public GameObject heroCharacter;

    //Battlers
    public List<GameObject> battleHeroes = new List<GameObject>();

    //autobattle basic attacks
    public bool autoBattle = false;
    public int autoBattleTurns = 50;
    public int remainingAutobattleTurns = 50;
    //battle phases etc things
    [Range(0f, 10f)] public float preFightCooldown = 1f;
    [Range(0f, 10f)] public float postFightCooldown = 1f;

    //Positions
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; //battle

    //scenes
    public string sceneToLoad;
    public string lastScene; //battle

    //bools
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    //timescaling for testing purpouses
    private float fixedDeltaTime;


    //enum

    public enum GameStates
    {
        WORLD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }

    //Battle
    public List<GameObject> enemysToBattle = new List<GameObject>();
    public int enemyAmount;
    public int heroAmount;
    public GameStates gameState;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        if (!GameObject.Find("Player"))
        {
            GameObject Hero = Instantiate(heroCharacter, Vector3.zero, Quaternion.identity) as GameObject;
            Hero.name = "Player";
        }
        heroCharacter.SetActive(true);
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        switch (gameState)
        {
            case (GameStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }

                if (gotAttacked)
                {
                    gameState = GameStates.BATTLE_STATE;
                }
                break;

            case (GameStates.TOWN_STATE):

                break;

            case (GameStates.BATTLE_STATE):
                // load battle scene
                StartBattle();
                //go to idle
                gameState = GameStates.IDLE;
                break;

            case (GameStates.IDLE):

                break;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 2.0f;
                Debug.Log("Time scale is changed to " + Time.timeScale);
            }
            else if (Time.timeScale == 2.0f)
            {
                Time.timeScale = 3.0f;
                Debug.Log("Time scale is changed to " + Time.timeScale);
            }
            else
            {
                Time.timeScale = 1.0f;
                Debug.Log("Time scale is changed to " + Time.timeScale);
            }

            // Adjust fixed delta time according to timescale
            // The fixed delta time will now be 0.02 real-time seconds per frame
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
        heroCharacter.SetActive(true);
    }

    public IEnumerator WaitSeconds(float sec)
    {
        Debug.Log("Starting delay");
        yield return new WaitForSeconds(sec * Time.deltaTime);
        Debug.Log(sec + " seconds expired");
    }

    void RandomEncounter() //will be that switch to get into battle
    {
        if (isWalking && canGetEncounter)
        {
            int eRate = Mathf.Abs(encounterRate - 11);
            if(Random.Range(0, (eRate*1000)) < 10)
            {
                //Debug.Log("We got encounter!");
                gotAttacked = true;
            }
        }
        
    }

    void StartBattle()
    {
        //set amount of heroes in party
        heroAmount = battleHeroes.Count;

        //set the amount of enemys we can encounter
        enemyAmount = Random.Range(1, Regions[curRegions].maxAmountEnemys + 1);

        //which enemys we can encounter
        for (int i = 0; i < enemyAmount; i++)
        {
            enemysToBattle.Add(Regions[curRegions].possibleEnemys[Random.Range(0, Regions[curRegions].possibleEnemys.Count)]);
        }

        lastHeroPosition = GameObject.Find("Player").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;

        //Load level
        SceneManager.LoadScene(Regions[curRegions].battleScene);

        //reset player character
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
        heroCharacter.SetActive(false);
    }
}
