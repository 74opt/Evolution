using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCommunicator : MonoBehaviour {
    /* 
     * File Format:
     * Time since startup
     * Organism Number
     * Berry Number
    */

    const string file = "Assets\\data.txt";
    StreamWriter sw;
    IEnumerator DataCoroutine;
    IEnumerator DataUpdater(int update) {
        while (true) {
            using (StreamWriter sw = File.CreateText(file)) {
                sw.WriteLine(Time.realtimeSinceStartup);

                List<GameObject> organismList = OrganismObject.Search("Organism");
                sw.WriteLine(organismList.Count);

                List<GameObject> foodList = OrganismObject.Search("Food");
                sw.WriteLine(foodList.Count);
            }

            yield return new WaitForSeconds(update);

            File.WriteAllText(file, String.Empty);
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

        DataCoroutine = DataUpdater(30);
        StartCoroutine(DataCoroutine);

        // using (StreamWriter sw = File.CreateText(file)) {
        //     sw.WriteLine("TESTING");
        // }
    }
}
