using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireBullet : MonoBehaviour
{
    public GameObject explosion;

    public float bulletSpeed;
    public float bulletLifespan;

    private Rigidbody rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Debug.Log(rigidBody);
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody.AddForce(rigidBody.transform.forward * bulletSpeed);
        Destroy(gameObject, bulletLifespan);
    }

    void OnCollisionEnter()
    {
        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        Destroy(expl, 1);
    }
}
