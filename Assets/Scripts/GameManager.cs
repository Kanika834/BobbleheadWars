using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;

    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;

    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //currentSpawnTime time passed since last spawn call
        currentSpawnTime += Time.deltaTime;

        //condition to generate a new wave of Aliens
        if (currentSpawnTime > generatedSpawnTime)
        {
            //resets the timer after a spawn occurs 
            currentSpawnTime = 0;

            //spawn-time randomizer
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            //ensures number of aliens within limits
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                //this List keeps track of where you have already spawned Aliens
                List<int> previousSpawnLocations = new List<int>();

                //limits number of Aliens to number of Spawnpoints
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;

                }

                //preventative code to make sure you do not spawn more aliens
                //than you've configured.
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                                  aliensPerSpawn - totalAliens : aliensPerSpawn;

                //this code loops once for each spawned Alien
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        //keeps track of numb of aliens spawned
                        aliensOnScreen += 1;

                        //value of -1 means no index has been assigned or found for the spawnpoint
                        int spawnPoint = -1;

                        //while loop keeps looking for a spawning point (index)
                        //that has not been used yet
                        while (spawnPoint == -1)
                        {
                            //create random index of List(array) between 0 and number of spawnpoints
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            //check to see if random spawnpoint has not already been used
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                //add this random number to the List
                                previousSpawnLocations.Add(randomNumber);
                                //use this random number for the spawn location index
                                spawnPoint = randomNumber;
                            }
                        }

                        //actual point(label) on arena to spawn next alien 
                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        //code to actually create a new Alien from a Prefab
                        GameObject newAlien = Instantiate(alien) as GameObject;

                        //position the new alien to that random unused spawned point
                        newAlien.transform.position = spawnLocation.transform.position;

                        //get the "Alein" code from the Alien spawned
                        Alien alienScript = newAlien.GetComponent<Alien>();

                        //set the new alien target to where the player currently is
                        //NOTE : GameManager code affecting Alien code. 
                        alienScript.target = player.transform;

                        //the new Aliens turn towards the player
                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                                                newAlien.transform.position.y,
                                                player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                    }
                }

            }

        }






    }
}
