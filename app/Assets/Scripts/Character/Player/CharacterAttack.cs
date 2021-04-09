using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackRangeHorizontal;
    [SerializeField] private float attackRangeVertical;
    private GameSetupController gsc;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        List<GameObject> enemies = gsc.GetEnemies();
        List<GameObject> enemiesWithinRange = new List<GameObject>();

        float dirFacing = gameObject.transform.rotation.y;
        float ownX = gameObject.transform.position.x;
        float ownY = gameObject.transform.position.y;

        bool facingLeft = false;

        if (dirFacing == -1)
        {
            facingLeft = true;
        }

        // Find enemies within attack range
        foreach(GameObject enemy in enemies)
        {
            float enemyX= enemy.transform.position.x;
            float enemyY = enemy.transform.position.y;

            float dX = ownX - enemyX;
            float dY = ownY - enemyY;

            if (facingLeft)
            {
                if (dX > 0 && dX < attackRangeHorizontal && dY <= 0 && Math.Abs(dY) < attackRangeVertical)
                {
                    enemiesWithinRange.Add(enemy);
                }

            }
            else
            {
                if (dX < 0 && Math.Abs(dX) < attackRangeHorizontal && dY <= 0 && Math.Abs(dY) < attackRangeVertical)
                {
                    enemiesWithinRange.Add(enemy);
                }
            }
        }

        // Apply damage to enemies
        foreach(GameObject enemy in enemiesWithinRange)
        {
            enemy.GetComponent<Enemy>().TakeDamage(1, facingLeft);
        }
    }
}
