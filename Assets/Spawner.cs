using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject organism;
    public GameObject berry;
    GameObject organismInstance;
    
    IEnumerator FoodSpawnCoroutine;

    IEnumerator FoodSpawn(int foodAmount, float time) {
        while (true) {
            for (int i = 0; i < foodAmount; i++) {
                Instantiate(berry, new Vector3(UnityEngine.Random.Range(-45, 45), 1, UnityEngine.Random.Range(-45, 45)), transform.rotation);
            }

            print($"{foodAmount} food entities have spawned.");

            yield return new WaitForSeconds(time);
        }
    }

    void Start() {
        for (int i = 0; i < 10; i++) {
            organismInstance = Instantiate(organism, new Vector3(UnityEngine.Random.Range(-45, 45), 1, UnityEngine.Random.Range(-45, 45)), transform.rotation);

            // Starting values
            organismInstance.GetComponent<OrganismObject>().speed = UnityEngine.Random.Range(.01f, .1f);
            organismInstance.GetComponent<OrganismObject>().deathValue = 100/*Convert.ToSingle(Math.Pow(organismInstance.GetComponent<OrganismObject>().speed, UnityEngine.Random.Range(-1f, -.8f)) * 10)*/;
            organismInstance.GetComponent<OrganismObject>().metabolism = organismInstance.GetComponent<OrganismObject>().speed * UnityEngine.Random.Range(15f, 20f);
            organismInstance.GetComponent<OrganismObject>().number = i + 1;

            print($"Organism {organismInstance.GetComponent<OrganismObject>().number}: Speed - {organismInstance.GetComponent<OrganismObject>().speed}. Metabolism - {organismInstance.GetComponent<OrganismObject>().metabolism}");
        }

        FoodSpawnCoroutine = FoodSpawn(10, 15f);
        StartCoroutine(FoodSpawnCoroutine);
    }

    void Update() {
        
    }
}
