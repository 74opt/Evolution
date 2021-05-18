using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject organism;
    public GameObject berry;
    public static int totalNumber;
    GameObject organismInstance;
    
    IEnumerator FoodSpawnCoroutine;

    IEnumerator FoodSpawn(int foodAmount, float time, int capacity) {
        while (true) {
        
            int foodCount = OrganismObject.Search("Food", Mathf.Infinity).Count;

            if (foodCount + 500 > capacity) {
                foodAmount = capacity - foodCount;
            }

            for (int i = 0; i < foodAmount; i++) {
                    Instantiate(berry, new Vector3(UnityEngine.Random.Range(-110, 110), .3f, UnityEngine.Random.Range(-110, 110)), transform.rotation);
                }

            //print($"{foodAmount} food entities have spawned.");

            yield return new WaitForSeconds(time);
        }
    }

    void Start() {
        totalNumber = 0;

        for (int i = 0; i < 20; i++) {
            totalNumber += 1;

            organismInstance = Instantiate(organism, new Vector3(UnityEngine.Random.Range(-110, 110), 1, UnityEngine.Random.Range(-110, 110)), transform.rotation);

            // Starting values
            organismInstance.GetComponent<OrganismObject>().speed = UnityEngine.Random.Range(.005f, .1f);
            organismInstance.GetComponent<OrganismObject>().deathValue = 100;
            organismInstance.GetComponent<OrganismObject>().metabolism = organismInstance.GetComponent<OrganismObject>().speed * UnityEngine.Random.Range(15f, 20f);

            organismInstance.GetComponent<OrganismObject>().detectionRadius = UnityEngine.Random.Range(1f, 3f);
            organismInstance.GetComponent<OrganismObject>().metabolism += organismInstance.GetComponent<OrganismObject>().detectionRadius;

            organismInstance.name = $"Organism {Spawner.totalNumber}";
            organismInstance.GetComponent<OrganismObject>().generation = 1;

            organismInstance.GetComponent<OrganismObject>().age = (organismInstance.GetComponent<OrganismObject>().metabolism / 1.5f) * UnityEngine.Random.Range(90f, 120f);

            print($"{organismInstance.name}: Speed - {organismInstance.GetComponent<OrganismObject>().speed}. Metabolism - {organismInstance.GetComponent<OrganismObject>().metabolism}");
        }

        FoodSpawnCoroutine = FoodSpawn(500, 20f, 1500);
        StartCoroutine(FoodSpawnCoroutine);
    }
}
