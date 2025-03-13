using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Prefabs")]
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject initialChunk;
    [SerializeField] GameObject chunkWithPowerups;
    [Header("Chunk Distance")]
    [SerializeField] int[] ChunkDistancesY;
    [SerializeField] int[] chunkDistancesX;
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
    List<GameObject> chunkSpawned = new List<GameObject>();
    int counter;

    private void Awake()
    {
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing, OnDifficultyIncreasing);
        currentChunkIndexToChoose = minimumChunkIndexToChoose;
        spawnPosition = new Vector2(0, ChunkDistancesY[Random.Range(0, ChunkDistancesY.Length)]);
        counter = 0;
        currentPowerupApperanceRate = Random.Range(minPowerupAppearanceRate, maxPowerupAppearanceRate + 1);
    }
    void Start()
    {
        SpawnStartingChunks();
    }

    private void Update()
    {
        DestroyChunk();
    }
    void SpawnStartingChunks()
    {
        chunkSpawned.Add(SpawnChunk(spawnPosition, initialChunk));

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
            chunkSpawned.Add(chunkToSpawn);
            ++counter;
        }
    }
   
    void DestroyChunk()
    {
        for(int i=0;i<chunkSpawned.Count;i++)
        {
            GameObject currentChunk = chunkSpawned[i];
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
                chunkSpawned.Remove(currentChunk);
                chunkSpawned.Add(chunkToSpawn);
                Destroy(currentChunk);
                ++counter;
            }
        }
    }

    GameObject SpawnChunk(Vector3 spawnPosition, GameObject chunkPrefab)
    {
        return Instantiate(chunkPrefab, spawnPosition, Quaternion.identity, this.transform);
    }
    Vector3 CalculateSpawnPosition()
    {
        Transform endPoint = chunkSpawned[chunkSpawned.Count - 1].transform.GetChild(endPointIndex);
        int chunkDistanceX = chunkDistancesX[Random.Range(0, chunkDistancesX.Length)];
        if(chunkDistanceX == 0)
        {
            float prevChunkPositionY = chunkSpawned[chunkSpawned.Count - 1].transform.position.y;
            return new Vector2(endPoint.position.x, prevChunkPositionY);
        }
        else
        {
            return new Vector3(endPoint.position.x + chunkDistanceX, ChunkDistancesY[Random.Range(0, ChunkDistancesY.Length)]);
        }
    }

    void OnDifficultyIncreasing(object[] datas)
    {
        float timeElapsed = (float)datas[0];
        float difficultyDuration = (float)datas[1];
        currentChunkIndexToChoose = (Mathf.FloorToInt(Mathf.Lerp(minimumChunkIndexToChoose, chunkPrefabs.Length, timeElapsed / difficultyDuration)));
    }
}