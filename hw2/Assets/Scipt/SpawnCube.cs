using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour{

    public GameObject cube;

    public void Spawn(Vector3 position) {
        Instantiate(cube).transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Spawn(Vector3.zero);
        }
    }
}
