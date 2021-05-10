using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObject : MonoBehaviour {
    // Standard Traits
    public float hunger, hungerTick, number, generation, hungerColor, ageAlpha;
    public int minOffspring, maxOffspring;
    public bool ageSet;
    

    // Evolution Traits 
    public float speed, deathValue, metabolism, age, detectionRadius;

    // ETC
    MeshRenderer mesh;

    IEnumerator HungerCoroutine, AgeCoroutine, SetAgeCoroutine;
    GameObject closestEntity;
    GameObject closestFood, closestOrganism;

    IEnumerator HungerTick() {
        while (true) {
            yield return new WaitForSeconds(hungerTick);
            hunger += metabolism;
        }
    }

    IEnumerator SetAge(float metabolism) {
        yield return new WaitForSeconds(.01f);
        yield return Random.Range(60f, 80f) * metabolism;
    }

    // IEnumerator AgeHandler(float ageTotal) {
    //     yield return new WaitForSeconds(ageTotal);
    //     Destroy(gameObject);
    // }

    void Awake() {
        transform.parent = null;

        // Components
        mesh = GetComponent<MeshRenderer>();

        // Hunger
        hunger = 50f/*deathValue/2*/;
        hungerTick = 1;

        // Offspring
        minOffspring = 1;
        maxOffspring = 3;

        // Starts hunger tick 
        HungerCoroutine = HungerTick();
        StartCoroutine(HungerCoroutine);

        // initial age so error doesnt happen
        //age = 100f;

        // To get rid of starting errors
        // TODO Create gameobject to default to when no other object exists nearby? maybe.
        closestFood = gameObject;
    }

    void Update() {
        // Age
        age -= Time.deltaTime;

        // Death
        if (hunger >= 100 || age <= 0) {
            //print($"{gameObject.name} has died after {Time.realtimeSinceStartup} seconds!");
            Destroy(gameObject);
        }

        // Reproduce
        if (hunger <= 0) {
            closestOrganism = SearchForClosest("Organism", detectionRadius);

            if (closestOrganism.gameObject.GetComponent<OrganismObject>().hunger <= 0) {
                closestEntity = closestOrganism;
            } else {
                closestEntity = SearchForClosest("Food", detectionRadius);  // if cant find possible mate to reproduce, just keep eating food
            }
        }

        // Food searching
        if (0 < hunger && hunger < 100) { 
            closestEntity = SearchForClosest("Food", detectionRadius);
        }

        // Scaling main body(based on speed + metabolism)
        float bodyScale = System.Convert.ToSingle(System.Math.Pow(metabolism - detectionRadius/2, -1f));
        transform.localScale = new Vector3(bodyScale, speed * 20, bodyScale);

        // Scaling cube (based on detection radius)
        float cubeScale = .6f * (detectionRadius/1.5f);
        transform.GetChild(0).localScale = new Vector3(cubeScale, cubeScale, cubeScale);

        // Color (color is based on hunger, alpha is based on age)
        hungerColor = -System.Math.Abs((.02f * hunger) - 1) + 1;
        // ageAlpha = age / 100; // TODO: create physical trait for age

        if (hunger > 0 && hunger < 50) {
            //hungerColor = .02f * hunger;
            mesh.material.SetColor("_Color", new Color(hungerColor, 1, 0, 1));
        } else if (hunger > 50 && hunger < 100) {
            //hungerColor = System.Math.Abs((-.02f * hunger) + .02f);
            mesh.material.SetColor("_Color", new Color(1, hungerColor, 0, 1)); 
        } else if (hunger <= 0) {
            mesh.material.SetColor("_Color", new Color(1, 0, 1, 1)); 
        }
    }

    public static List<GameObject> Search(string tag, float detectionRadius) {
        List<GameObject> filteredList = new List<GameObject>();

        Collider[] nearbyObjects = Physics.OverlapSphere(new Vector3(0, 0, 0), Mathf.Infinity);
        foreach (Collider collider in nearbyObjects) {
            if (collider.gameObject.CompareTag(tag)) {
                filteredList.Add(collider.gameObject);
            }
        }

        return filteredList;
    }

    GameObject SearchForClosest(string tag, float detectionRadius) {
        List<GameObject> findingList = Search(tag, detectionRadius);
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
        try {
            transform.position = Vector3.MoveTowards(transform.position, closestEntity.transform.position, speed);
        } catch (System.Exception) {}
    }

    private void OnCollisionEnter(Collision collider) {
        // Eat Food
        if (collider.gameObject.CompareTag("Food")) {
            hunger -= collider.gameObject.GetComponent<Berry>().value;
            //print($"{gameObject.name} ate. Hunger is {hunger}.");
            Destroy(collider.gameObject);
        }

        // Create baby
        if (collider.gameObject.CompareTag("Organism")) {
            if (collider.gameObject.GetComponent<OrganismObject>().hunger <= 0 && hunger <= 0) {
                for (int i = 0; i < Random.Range(minOffspring, maxOffspring + 1); i++) {
                    Spawner.totalNumber += 1;

                    GameObject organismInstance = Instantiate(gameObject, transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3)), transform.rotation);

                    organismInstance.GetComponent<OrganismObject>().speed = ((gameObject.GetComponent<OrganismObject>().speed + collider.gameObject.GetComponent<OrganismObject>().speed) / 2) * Random.Range(.85f, 1.15f);
                    organismInstance.GetComponent<OrganismObject>().deathValue = 100;
                    organismInstance.GetComponent<OrganismObject>().metabolism = organismInstance.GetComponent<OrganismObject>().speed * UnityEngine.Random.Range(15f, 20f);

                    organismInstance.GetComponent<OrganismObject>().detectionRadius = ((gameObject.GetComponent<OrganismObject>().detectionRadius + collider.gameObject.GetComponent<OrganismObject>().detectionRadius) / 2) * Random.Range(.85f, 1.15f);
                    organismInstance.GetComponent<OrganismObject>().metabolism += organismInstance.GetComponent<OrganismObject>().detectionRadius;

                    organismInstance.name = $"Organism {Spawner.totalNumber}";
                    organismInstance.GetComponent<OrganismObject>().generation = generation + 1;

                    organismInstance.GetComponent<OrganismObject>().age = (organismInstance.GetComponent<OrganismObject>().metabolism / 1.5f) * UnityEngine.Random.Range(90f, 120f);

                    print($"{gameObject.name} and {collider.gameObject.name} have given birth to {organismInstance.gameObject.name}:\nSpeed - {organismInstance.GetComponent<OrganismObject>().speed}. Metabolism - {organismInstance.GetComponent<OrganismObject>().metabolism}");
                }

                hunger = Random.Range(60f, 75f);
            }
        }
    }
}
