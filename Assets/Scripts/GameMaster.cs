using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;
    public GameObject selectedButton;
    public GameObject deathCanvas;


    private Player player;
    private PlayerMovement playerMovement;

    private bool isLoading;
    private SaveData saveData;

    //camera
    
    public Cinemachine.CinemachineVirtualCamera vcam1;

    public Cinemachine.CinemachineTargetGroup group;

    // public CinemachineCameraShaker cameraShake;
    public float cameraShakeDuration;
    public float cameraShakeBlinkDuration;

    //Damage

    public Transform enemyDeathParticles;
    public float invulnerabilityTimerSet;



    //AudioManger
    private AudioManager audioManager = AudioManager.instance;

    public GameObject dialogTest;

    //SceneManager
    private Scene currentScene;


    private void Awake()
    {

    }

    private void Start()
    {

        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.Log("No audioManager found in the scene");
        }
        PlaySound("music");
        player = FindObjectOfType<Player>();
        playerMovement = player.GetComponent<PlayerMovement>();




        //DONT DESTROY ON LOAD
        // DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(vcam1.transform.parent);


        Scene currentScene = SceneManager.GetActiveScene();

    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (dialogTest.activeSelf == true)
            {
                dialogTest.SetActive(false);
                _EnablePlayerMovement(true);

            }
            else
            {
                dialogTest.SetActive(true);
                _EnablePlayerMovement(false);

            }


        }
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
            }
        }

    }

    private void FixedUpdate()
    {

    }


    /*
     public static void AddJumpPlayer(PlayerMovement player)
     {
         gm._AddJumpPlayer(player);
     }

     private void _AddJumpPlayer(PlayerMovement player)
     {
         player.totalJumpsValue++;
     }
     */

    public static void KillPlayer(Player player)
    {
        gm._KillPlayer(player);

    }
    public void _KillPlayer(Player player)
    {
        Instantiate(enemyDeathParticles, player.transform.position, Quaternion.identity);
        Destroy(player.gameObject);
        deathCanvas.SetActive(true);
        Debug.Log(Time.timeScale);
        Time.timeScale = 0f;
        Debug.Log(Time.timeScale);
    }


    public static void DamageEnemy(Enemy enemy, int dmg)
    {
        gm._DamageEnemy(enemy, dmg);
    }

    private void _DamageEnemy(Enemy enemy, int dmg)
    {
        enemy.takeDamage(dmg);
    }

    public static void DamagePlayer(int dmg)
    {
        gm._DamagePlayer(dmg);
    }

    private void _DamagePlayer(int dmg)
    {
        if (player.getInvulTimer() <= 0)
        {
            player.DamagePlayer(dmg);
            player.setInvulTimer(invulnerabilityTimerSet);
            StartCoroutine(smallPause());
        }

    }

    IEnumerator smallPause()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);

    }

    private void _KillEnemy(Enemy enemy)
    {
        Instantiate(enemyDeathParticles, enemy.transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(enemy.gameObject);
    }

    public static void ShakeCamera()
    {
        gm._ShakeCamera();
    }
    private void _ShakeCamera()
    {
        vcam1.GetComponent<CinemachineCameraShaker>().ShakeCamera(cameraShakeDuration);
    }

    public static void BlinkShake()
    {

        gm._BlinkShake();
    }

    private void _BlinkShake()
    {
        vcam1.GetComponent<CinemachineCameraShaker>().ShakeCamera(cameraShakeBlinkDuration);
    }

    public static void PlaySound(string sound)
    {
        gm._PlaySound(sound);
    }

    private void _PlaySound(string sound)
    {
        audioManager.PlaySound(sound);
    }

    public static void LoadScene(string nextLevel)
    {
        gm._LoadScene(nextLevel);
    }


    private void _LoadScene(string nextLevel)
    {
        Debug.Log("HERE we are again");
        SceneManager.LoadScene(nextLevel);
        if (nextLevel == "Scene1")
        {
            PlaySound("music");


        }
        else if (nextLevel == "Escape")
        {
            audioManager.StopAll();
            PlaySound("escape");
            player.transform.position = new Vector3(-200f, -3.1f, 0f);



        }
    }
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        vcam1 = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        player = FindObjectOfType<Player>();
        Transform lookUp = null;
        Transform lookDown = null;
        if (player != null)
        {
            foreach (Transform child in player.transform)
            {
                if (child.name == "LookUpTest")
                {
                    lookUp = child;
                }
                else if (child.name == "LookDownTest")
                {
                    lookDown = child;
                }

            }
            group.m_Targets[0].target = player.transform;
            group.m_Targets[1].target = lookUp.transform;
            group.m_Targets[2].target = lookDown.transform;
            vcam1.m_Follow = group.transform;
            vcam1.m_LookAt = player.transform;
        }
        if (isLoading)
        {

            player.transform.position = new Vector3(saveData.position[0], saveData.position[1], saveData.position[2]);
            player.playerStats = saveData.stats;
            isLoading = false;
        }

    }


    public static void LookUp(float weight)
    {
        gm._LookUp(weight);
    }

    private void _LookUp(float weight)
    {
        group.m_Targets[1].weight = weight;
    }


    public static void LookDown(float weight)
    {
        gm._LookDown(weight);
    }

    private void _LookDown(float weight)
    {
        group.m_Targets[2].weight = weight;
    }

    public static string GetCurrentSceneName()
    {
        return gm._GetCurrentSceneName();
    }

    private string _GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    /*
    public static void SetPlayerStats(Player.PlayerStats stats)
    {
        gm._SetPlayerStats(stats);
    }

    private void _SetPlayerStats(Player.PlayerStats stats)
    {
        player.playerStats = stats;  
    }
    */
    public static void Loading(SaveData data)
    {
        gm._Loading(data);
    }
    private void _Loading(SaveData data)
    {
        isLoading = true;
        this.saveData = data;
        gm._LoadScene(data.scene);
    }

    public static void EnablePlayerMovement(bool enable)
    {
        gm._EnablePlayerMovement(enable);

    }

    private void _EnablePlayerMovement(bool enable)
    {
        if (enable)
        {
            playerMovement.MovementEnabled = true;
 
        }
        else
        {
            playerMovement.MovementEnabled = false;
            playerMovement.animator.SetFloat("speed", -1);
            playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            playerMovement.animator.SetTrigger("Dialogue");
        }
    }

    public static void SpearDestroyed()
    {
        gm._SpearDestroyed();
    }

    private void _SpearDestroyed()
    {
        playerMovement.SpearDestroyed();
    }

    public static void SetCameraAbyssChase(Cinemachine.CinemachineVirtualCamera vcam2)
    {
        gm._SetCameraAbyssChase(vcam2);
    }

    private void _SetCameraAbyssChase(Cinemachine.CinemachineVirtualCamera vcam2)
    {
        vcam1.gameObject.SetActive(false);
        vcam1 = vcam2;
        vcam1.gameObject.SetActive(true);
        
        
        
    }


    /*
    public static void CanTeleportToSpear(bool can)
    {
        gm._CanTeleportToSpear(can);
    }

    private void _CanTeleportToSpear(bool can)
    {
        playerMovement.setCanTeleportToSpear(can);
    }
    */

}
