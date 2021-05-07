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

    IEnumerator FoodSpawn(int foodAmount, float time) {
        while (true) {
            for (int i = 0; i < foodAmount; i++) {
                Instantiate(berry, new Vector3(UnityEngine.Random.Range(-45, 45), .3f, UnityEngine.Random.Range(-45, 45)), transform.rotation);
            }

            //print($"{foodAmount} food entities have spawned.");

            yield return new WaitForSeconds(time);
        }
    }

    void Start() {
        totalNumber = 0;

        for (int i = 0; i < 10; i++) {
            totalNumber += 1;

            organismInstance = Instantiate(organism, new Vector3(UnityEngine.Random.Range(-45, 45), 1, UnityEngine.Random.Range(-45, 45)), transform.rotation);

            // Starting values
            organismInstance.GetComponent<OrganismObject>().speed = UnityEngine.Random.Range(.005f, .1f);
            organismInstance.GetComponent<OrganismObject>().deathValue = 100;
            organismInstance.GetComponent<OrganismObject>().metabolism = organismInstance.GetComponent<OrganismObject>().speed * UnityEngine.Random.Range(15f, 20f);
            organismInstance.name = $"Organism {Spawner.totalNumber}";
            organismInstance.GetComponent<OrganismObject>().generation = 1;

            print($"{organismInstance.name}: Speed - {organismInstance.GetComponent<OrganismObject>().speed}. Metabolism - {organismInstance.GetComponent<OrganismObject>().metabolism}");
        }

        FoodSpawnCoroutine = FoodSpawn(100, 15f);
        StartCoroutine(FoodSpawnCoroutine);
    }
}
