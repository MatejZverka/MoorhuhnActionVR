using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChicken : MonoBehaviour
{
    public float secondsBetweenSpawning = 2.5f;
	public float xMinRange = -25.0f;
	public float xMaxRange = 25.0f;
	public float yMinRange = 8.0f;
	public float yMaxRange = 25.0f;
	public float zMinRange = -25.0f;
	public float zMaxRange = 25.0f;

    public GameObject[] spawnObjects; 

    private float nextSpawnTime; 

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time+secondsBetweenSpawning;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.CheckChickenLimit()){
           if (Time.time  >= nextSpawnTime) {
                MakeThingToSpawn();	
                nextSpawnTime = Time.time + secondsBetweenSpawning;
                gameManager.IncreaseChicken();
            } 
        }
    }
 
    void MakeThingToSpawn (){
        Vector3 spawnPosition;
 
        spawnPosition.x = Random.Range (xMinRange, xMaxRange);
        spawnPosition.y = Random.Range (yMinRange, yMaxRange);
        spawnPosition.z = Random.Range (zMinRange, zMaxRange);

 
        int objectToSpawn = Random.Range (0, spawnObjects.Length);

 
        GameObject spawnedObject = Instantiate (spawnObjects [objectToSpawn],  
        spawnPosition, transform.rotation) as GameObject;

        spawnedObject.transform.parent = gameObject.transform;
    }

}
