using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public GameObject FloatingTextPrefab;
    // Start is called before the first frame update
    public float waitTime;

    public Vector3 Offset = new Vector3(0,4,0);
    void start(){
        LoopFunction(waitTime);
        transform.localPosition += Offset;
    }
    private IEnumerator LoopFunction(float waitTime)
    {
        while(true){
        ShowFloatingText();
        yield return new WaitForSeconds(waitTime);
        }
    }

    // Update is called once per frame
    void ShowFloatingText(){
        Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
    }
}
