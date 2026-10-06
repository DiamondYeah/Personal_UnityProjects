using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class LasersInLevel
{
    public int level;
    public float spawnTimer;
    public GameObject[] possibleLasers;
}



public class SpawnController : MonoBehaviour
{

    [Header("Platform Spawn Settings")]
    [SerializeField]
    private GameObject panelSpawnZone;
    [SerializeField]
    private GameObject[] panels;
    [SerializeField]
    private int maxPanelSpawn;

    [Space(15)]
    [Header("Laser Spawn Settings")]
    [SerializeField]
    private GameObject laserSpawnZone;

    [Header("Lasers Per Level")]
    [SerializeField]
    private LasersInLevel[] lasers;

    [Header("Lasers Horizontal Positions")]
    [SerializeField]
    private Transform startSideLoc;
    [SerializeField]
    private Transform endSideLoc;
    
    [Header("Lasers Vertical Positions")]
    [SerializeField]
    private Transform startVertLoc;
    [SerializeField]
    private Transform endVertLoc;

    private Collider panelSpawnCollider;
    private Collider laserSpawnCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelSpawnCollider = panelSpawnZone.GetComponent<Collider>();
        laserSpawnCollider = laserSpawnZone.GetComponent<Collider>();

        SpawnPanels();
        StartCoroutine(SpawnLaser());
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.PlayerIsAtEnd += SpawnPanels;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.PlayerIsAtEnd -= SpawnPanels;
    }



    private Vector3 RandomizeLaserSpawnLocation()
    {
        return new Vector3(UnityEngine.Random.Range(laserSpawnCollider.bounds.min.x, laserSpawnCollider.bounds.max.x),
                           UnityEngine.Random.Range(laserSpawnCollider.bounds.min.y, laserSpawnCollider.bounds.max.y),
                           laserSpawnZone.transform.position.z);
    }

    private Vector3 RandomizePanelSpawnLocation()
    {
        return new Vector3(UnityEngine.Random.Range(panelSpawnCollider.bounds.min.x, panelSpawnCollider.bounds.max.x),
                           panelSpawnZone.transform.position.y,
                           UnityEngine.Random.Range(panelSpawnCollider.bounds.min.z, panelSpawnCollider.bounds.max.z));

    }

    private void SpawnPanels()
    {
        int count = 0;
        while (count < maxPanelSpawn)
        {
            int randomNum = UnityEngine.Random.Range(0, panels.Length);
            Instantiate(panels[randomNum], RandomizePanelSpawnLocation(), panelSpawnZone.transform.rotation);
            count++;
        }
    }

    private IEnumerator SpawnLaser()
    { 
        while (true)
        {
            int currentLevel = GameController.currentLevel;
            yield return new WaitForSeconds(lasers[currentLevel - 1].spawnTimer);

            currentLevel = GameController.currentLevel;
            int randomNum = UnityEngine.Random.Range(0, lasers[currentLevel - 1].possibleLasers.Length);

            GameObject selectedLaser = lasers[currentLevel - 1].possibleLasers[randomNum];
            GameObject laser = Instantiate(selectedLaser, RandomizeLaserSpawnLocation(), selectedLaser.transform.rotation);
            laser.GetComponent<LaserMove>().Initialize(startSideLoc, endSideLoc, startVertLoc, endVertLoc);
        }
    }
}
