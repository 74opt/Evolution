using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    private void OnCollisionEnter(Collision collider) {
        Destroy(collider.gameObject);
    }
}
