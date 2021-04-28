using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObject : MonoBehaviour {
    // Standard Traits
    public float hunger, hungerTick, number, minOffspring, maxOffspring;
    

    // Evolution Traits 
    public float speed, deathValue, metabolism, detectionRadius;

    // ETC
    IEnumerator HungerCoroutine, AgeCoroutine;
    GameObject closestEntity;
    GameObject closestFood, closestOrganism;

    IEnumerator HungerTick() {
        while (true) {
            yield return new WaitForSeconds(hungerTick);
            hunger += metabolism;
        }
    }

    IEnumerator AgeHandler(float ageTotal) {
        yield return new WaitForSeconds(ageTotal);
        Destroy(gameObject);
    }

    void Awake() {
        // Hunger
        hunger = 50f/*deathValue/2*/;
        hungerTick = 1;

        // Offspring
        minOffspring = 1;
        maxOffspring = 2;

        // Starts hunger tick 
        HungerCoroutine = HungerTick();
        StartCoroutine(HungerCoroutine);

        // Kills off organism after specific amount of time
        AgeCoroutine = AgeHandler(Random.Range(40, 50));
        StartCoroutine(AgeCoroutine);

        // To get rid of starting errors
        // TODO Create gameobject to default to when no other object exists nearby?
        closestFood = gameObject;
    }

    void Update() {
        // Death
        if (hunger >= 100) {
            print($"{gameObject.name} has died after {Time.realtimeSinceStartup} seconds!");
            Destroy(gameObject);
        }

        // Reproduce
        if (hunger <= 0) {
            closestOrganism = SearchForClosest("Organism");

            if (closestOrganism.gameObject.GetComponent<OrganismObject>().hunger <= 0) {
                closestEntity = closestOrganism;
            } else {
                closestEntity = SearchForClosest("Food");  // if cant find possible mate to reproduce, just keep eating food
            }
        }

        // Food searching
        if (0 < hunger && hunger < 100) { 
            closestEntity = SearchForClosest("Food");
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

        closest = gameObject;

        // Gets rid of error where closest would not be in reference, while making sure closest is never assigned to self unless there's no other organisms around
        if (findingList.Count > 0) { 
            foreach (GameObject item in findingList) {
                if (item != gameObject) {
                    closest = item;
                    break;
                }
            }
        } else {
            closest = gameObject;  // in case of no nearby gameobjects of that type
        }


        foreach (GameObject item in findingList) {
            if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closest.transform.position) && item != gameObject) { //For precaution with finding self in organism search, added "&& item != gameObject"
                closest = item;
            }
        }

        return closest;
    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, closestEntity.transform.position, speed);
    }

    private void OnCollisionEnter(Collision collider) {
        // Eat Food
        if (collider.gameObject.CompareTag("Food")) {
            hunger -= collider.gameObject.GetComponent<Berry>().value;
            print($"{gameObject.name} ate. Hunger is {hunger}.");
            Destroy(collider.gameObject);
        }

        // Create baby
        if (collider.gameObject.CompareTag("Organism")) {
            if (collider.gameObject.GetComponent<OrganismObject>().hunger <= 0) {  // no && to avoid any errors about not being able to get OrganismObject script
                for (int i = 0; i < Random.Range(minOffspring, maxOffspring); i++) {
                    Spawner.totalNumber += 1;

                    GameObject organismInstance = Instantiate(gameObject, transform);

                    organismInstance.GetComponent<OrganismObject>().speed = (gameObject.GetComponent<OrganismObject>().speed + collider.gameObject.GetComponent<OrganismObject>().speed) / 2 * Random.Range(.85f, 1.15f);
                    organismInstance.GetComponent<OrganismObject>().deathValue = 100;
                    organismInstance.GetComponent<OrganismObject>().metabolism = organismInstance.GetComponent<OrganismObject>().speed * UnityEngine.Random.Range(15f, 20f);
                    organismInstance.name = $"Organism {Spawner.totalNumber}";

                    print($"{gameObject.name} and {collider.gameObject.name} have given birth to {organismInstance.gameObject.name}:\nSpeed - {organismInstance.GetComponent<OrganismObject>().speed}. Metabolism - {organismInstance.GetComponent<OrganismObject>().metabolism}");
                    transform.DetachChildren();
                }

                hunger += Random.Range(60f, 75f);
            }
        }
    }
}
