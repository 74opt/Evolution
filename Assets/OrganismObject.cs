using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObject : MonoBehaviour {
    // Standard Traits
    public float hunger, hungerTick, number;
    

    // Evolution Traits 
    public float speed, deathValue, metabolism;

    // ETC
    IEnumerator HungerCoroutine;

    void Awake() {
        hunger = deathValue/2;
        hungerTick = 1;

        // Starts hunger tick 
        HungerCoroutine = HungerTick();
        StartCoroutine(HungerCoroutine);
    }

    void Update() {
        // Death
        if (hunger >= 100) {
            print($"Organism #{number} has died after {Time.realtimeSinceStartup} seconds!");
            Destroy(gameObject);
        }

        // Reproduce
        if (hunger <= 0) {

        }        
    }

    IEnumerator HungerTick() {
        while (true) {
            yield return new WaitForSeconds(hungerTick);
                hunger += metabolism;
        }
    }

    private void FixedUpdate() {
        
    }
}
