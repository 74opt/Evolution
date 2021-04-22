using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    float cSensitivity, xRotation, yRotation, cVelocity, xVel, yVel, zVel, minVel, maxVel, velIncrease;

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        cSensitivity = 5f;
        cVelocity = .2f;
        minVel = .04f;
        maxVel = 4f;
        velIncrease = .04f;
    }


    void Update() {
        // Mouselook
        yRotation += Input.GetAxis("Mouse X") * cSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * cSensitivity;

        if (xRotation < -90) {
            xRotation = -90;
        }

        if (xRotation > 90) {
            xRotation = 90;
        }

        // W and S movement
        if (Input.GetKey(KeyCode.W)) {
            zVel = cVelocity;
        } else if (Input.GetKey(KeyCode.S)) {
            zVel = -cVelocity;
        } else {
            zVel = 0;
        }

        // A and D movement
        if (Input.GetKey(KeyCode.A)) {
            xVel = -cVelocity;
        } else if (Input.GetKey(KeyCode.D)) {
            xVel = cVelocity;
        } else {
            xVel = 0;
        }

        // E and Q movement
        if (Input.GetKey(KeyCode.Q)) {
            yVel = -cVelocity;
        } else if (Input.GetKey(KeyCode.E)) {
            yVel = cVelocity;
        } else {
            yVel = 0;
        }

        // Change velocity by mousewheel
        cVelocity += Input.mouseScrollDelta.y * velIncrease;

        if (cVelocity < minVel) {
            cVelocity = minVel;
        }

        if (cVelocity > maxVel) {
            cVelocity = maxVel;
        }
    }

    private void FixedUpdate() {
        transform.Translate(new Vector3(xVel, yVel, zVel), Space.Self);

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}
