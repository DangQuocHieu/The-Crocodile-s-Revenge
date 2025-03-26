using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour, IDifficultyScaler
{
    [Header("Chunk Prefabs")]
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject initialChunk;
    [SerializeField] GameObject chunkWithPowerups;
    [Header("Chunk Distance")]
    [SerializeField] int[] ChunkDistancesY;
    [SerializeField] int minDistaneX = 2;
    [SerializeField] int maxDistanceX = 8;
    [SerializeField] float zeroDistanceXRate = 0.5f;
    [SerializeField] int currentMaxDistanceX;
    [Header("Some Chunk Properties")]
    [SerializeField] int minimumChunkIndexToChoose;
    [SerializeField] int currentChunkIndexToChoose;
    [SerializeField] int chunkCount;
    [SerializeField] float destroyPositionX;
    [SerializeField] int minPowerupAppearanceRate = 5;
    [SerializeField] int maxPowerupAppearanceRate = 20;
    [SerializeField] int endPointIndex = 0;
    int currentPowerupApperanceRate;
    private Vector2 spawnPosition;
    LinkedList<GameObject> chunkSpawned = new LinkedList<GameObject>();
    int counter;

    private void Awake()
    {
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
        currentChunkIndexToChoose = minimumChunkIndexToChoose;
        spawnPosition = new Vector2(0, ChunkDistancesY[Random.Range(0, ChunkDistancesY.Length)]);
        counter = 0;
        currentPowerupApperanceRate = Random.Range(minPowerupAppearanceRate, maxPowerupAppearanceRate + 1);
    }
    void Start()
    {
        SpawnStartingChunks();
    }

    void LateUpdate()
    {
        DestroyChunk();
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }
    void SpawnStartingChunks()
    {
        chunkSpawned.AddLast(SpawnChunk(spawnPosition, initialChunk));

        for(int i=0;i<chunkCount;i++)
        {
            spawnPosition = CalculateSpawnPosition();
            GameObject chunkToSpawn;
            if (counter != 0 && counter % currentPowerupApperanceRate == 0)
            {
                chunkToSpawn = SpawnChunk(spawnPosition, chunkWithPowerups);
                currentPowerupApperanceRate = Random.Range(minPowerupAppearanceRate, maxPowerupAppearanceRate + 1);
                counter = 0;
            }
            else chunkToSpawn = SpawnChunk(spawnPosition, chunkPrefabs[Random.Range(0, currentChunkIndexToChoose)]);
            chunkSpawned.AddLast(chunkToSpawn);
            ++counter;
        }
    }
   
    void DestroyChunk()
    {
            GameObject currentChunk = chunkSpawned.First.Value;
            if (currentChunk.transform.position.x <= destroyPositionX)
            {
                spawnPosition = CalculateSpawnPosition();
                GameObject chunkToSpawn;
                if (counter != 0 && counter % currentPowerupApperanceRate == 0)
                {
                    chunkToSpawn = SpawnChunk(spawnPosition, chunkWithPowerups);
                    currentPowerupApperanceRate = Random.Range(minPowerupAppearanceRate,maxPowerupAppearanceRate + 1);
                    counter = 0;
                }
                else
                {
                    chunkToSpawn = SpawnChunk(spawnPosition, chunkPrefabs[Random.Range(0, currentChunkIndexToChoose)]);
                }
                chunkSpawned.RemoveFirst();
                chunkSpawned.AddLast(chunkToSpawn);
                Destroy(currentChunk);
                ++counter;
            }
    }

    GameObject SpawnChunk(Vector3 spawnPosition, GameObject chunkPrefab)
    {
        return Instantiate(chunkPrefab, spawnPosition, Quaternion.identity, this.transform);
    }
    Vector3 CalculateSpawnPosition()
    {
        GameObject currentChunk = chunkSpawned.Last.Value;
        Transform endPoint = currentChunk.transform.GetChild(endPointIndex);
        
        if(Random.value <= zeroDistanceXRate)
        {
            float prevChunkPositionY = currentChunk.transform.position.y;
            return new Vector2(endPoint.position.x - 0.1f, prevChunkPositionY);
        }
        else
        {
            int chunkDistanceX = Random.Range(minDistaneX, maxDistanceX + 1);
            return new Vector3(endPoint.position.x + chunkDistanceX, ChunkDistancesY[Random.Range(0, ChunkDistancesY.Length)]);
        }
    }

    public void OnIncreaseDifficulty(object[] datas)
    {
        float t = (float)datas[0];
        float difficultyScale = (float)datas[1];
        currentChunkIndexToChoose = Mathf.RoundToInt(Mathf.Lerp(minimumChunkIndexToChoose, chunkPrefabs.Length, t));
        currentMaxDistanceX = Mathf.RoundToInt(Mathf.Lerp(maxDistanceX, maxDistanceX * difficultyScale, t));
    }
}