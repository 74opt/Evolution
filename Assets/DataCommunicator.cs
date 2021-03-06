using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCommunicator : MonoBehaviour {
    /* 
     * File Format:
     * Time between each recorded point
     * Time since startup
     * Organism Number
     * Food Number
    */

    const string file = "Assets\\data.txt";
    StreamWriter sw;
    IEnumerator DataCoroutine;
    IEnumerator DataUpdater(int update) {
        float totalTime = 0;

        while (true) {
            using (StreamWriter sw = File.CreateText(file)) {
                sw.WriteLine(update);

                sw.WriteLine(totalTime);

                yield return new WaitForSeconds(.01f);  // just to fix a thing

                List<GameObject> organismList = OrganismObject.Search("Organism", Mathf.Infinity);
                sw.WriteLine(organismList.Count);

                List<GameObject> foodList = OrganismObject.Search("Food", Mathf.Infinity);
                sw.WriteLine(foodList.Count);
            }

            yield return new WaitForSeconds(update);

            File.WriteAllText(file, String.Empty);
            totalTime = Time.realtimeSinceStartup;
        }
    }

    void Start() {
        // test
        if (File.Exists(file)) {
            print("yes");
        } else {
            print("no");
        }

        // clear file
        File.WriteAllText(file, String.Empty);

        DataCoroutine = DataUpdater(20);
        StartCoroutine(DataCoroutine);

        // using (StreamWriter sw = File.CreateText(file)) {
        //     sw.WriteLine("TESTING");
        // }
    }
}
