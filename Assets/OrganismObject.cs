using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObject : MonoBehaviour {
    // Standard Traits
    public float hunger, hungerTick, number;
    

    // Evolution Traits 
    public float speed, deathValue, metabolism, detectionRadius;

    // ETC
    IEnumerator HungerCoroutine;
    GameObject closestFood;

    IEnumerator HungerTick() {
        while (true) {
            yield return new WaitForSeconds(hungerTick);
                hunger += metabolism;
        }
    }

    void Awake() {
        hunger = 50f/*deathValue/2*/;
        hungerTick = 1;

        // Starts hunger tick 
        HungerCoroutine = HungerTick();
        StartCoroutine(HungerCoroutine);

        // To get rid of starting errors
        // TODO Create gameobject to default to when no other object exists nearby?
        closestFood = gameObject;
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

        // Food searching
        if (0 < hunger && hunger < 100) { 
            closestFood = SearchForClosest("Food");
        }

        // Physical traits
        transform.localScale = new Vector3(System.Convert.ToSingle(System.Math.Pow(metabolism, -1f)), speed * 20, System.Convert.ToSingle(System.Math.Pow(metabolism, -1f)));
    }

    List<GameObject> Search(string tag) {
        List<GameObject> filteredList = new List<GameObject>();

        Collider[] nearbyObjects = Physics.OverlapSphere(new Vector3(0, 0, 0), Mathf.Infinity);
        foreach (Collider collider in nearbyObjects) {
            if (collider.gameObject.CompareTag(tag)) {
                filteredList.Add(collider.gameObject);
            }
        }

        return filteredList;
    }

    GameObject SearchForClosest(string tag) {
        List<GameObject> findingList = Search(tag);
        GameObject closest;

        if (findingList.Count > 0) { 
            closest = findingList[0];
        } else {
            closest = gameObject;  // in case of no nearby gameobjects of that type
        }


        foreach (GameObject item in findingList) {
            if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closest.transform.position)) {
                closest = item;
            }
        }

        return closest;
    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, closestFood.transform.position, speed);
    }

    private void OnCollisionEnter(Collision collider) {
        if (collider.gameObject.CompareTag("Food")) {
            hunger -= collider.gameObject.GetComponent<Berry>().value;
            print($"Organism #{number} ate. Hunger is {hunger}.");
            Destroy(collider.gameObject);
        }
    }
}
