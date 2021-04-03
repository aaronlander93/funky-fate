using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    public List<GameObject> enemies;

    public GameObject hecklerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Hard-coding this for now
        Vector2 pos = new Vector2(17, 2);
        Vector2 pos2 = new Vector2(53, 84);
        Spawn(pos, 1);
        Spawn(pos2, 1);
    }

    void Spawn(Vector2 pos, int charType)
    {
        enemies.Add(Instantiate(hecklerPrefab, pos, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(enemy => enemy.GetComponent<Enemy>().isDead() == true);
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }
}
