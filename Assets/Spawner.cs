using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject organism;
    GameObject organismInstance;

    void Start() {
        for (int i = 0; i < 10; i++) {
            organismInstance = Instantiate(organism, new Vector3(UnityEngine.Random.Range(-45, 45), 1, UnityEngine.Random.Range(-45, 45)), transform.rotation);

            // Starting values
            organismInstance.GetComponent<OrganismObject>().speed = UnityEngine.Random.Range(.1f, 1f);
            organismInstance.GetComponent<OrganismObject>().deathValue = 100/*Convert.ToSingle(Math.Pow(organismInstance.GetComponent<OrganismObject>().speed, UnityEngine.Random.Range(-1f, -.8f)) * 10)*/;
            organismInstance.GetComponent<OrganismObject>().metabolism = Convert.ToSingle(Math.Pow(organismInstance.GetComponent<OrganismObject>().speed, UnityEngine.Random.Range(1.7f, -2f)) * 10);
            organismInstance.GetComponent<OrganismObject>().number = i + 1;

            print($"{organismInstance.GetComponent<OrganismObject>().speed}, {organismInstance.GetComponent<OrganismObject>().metabolism}");
        }
    }

    void Update() {
        
    }
}
