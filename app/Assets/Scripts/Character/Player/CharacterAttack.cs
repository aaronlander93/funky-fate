using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackRangeHorizontal;
    [SerializeField] private float attackRangeVertical;
    
    [SerializeField] private Animator _anim;
    [SerializeField] private string button = "Attack";

    public PhotonView pv;
    private GameSetupController gsc;

    private bool enemiesSynced;

    // Start is called before the first frame update
    void Start()
    {
        if (!pv.IsMine)
        {
            Destroy(this);
        }

        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();

        if(GameConfig.Multiplayer && !PhotonNetwork.IsMasterClient)
            gsc.FindEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
            _anim.SetBool("isAttacking", true);
        }
        if(Input.GetKeyUp(KeyCode.J))
        {
            _anim.SetBool("isAttacking", false);
        }
    }

    private void Attack()
    {
        List<Rigidbody2D> enemies = gsc.GetEnemies();
        List<Rigidbody2D> enemiesWithinRange = new List<Rigidbody2D>();

        float dirFacing = gameObject.transform.rotation.y;
        float ownX = gameObject.transform.position.x;
        float ownY = gameObject.transform.position.y;

        bool facingLeft = false;

        if (dirFacing == -1)
        {
            facingLeft = true;
        }

        // Find enemies within attack range
        foreach(Rigidbody2D enemy in enemies)
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
        foreach(Rigidbody2D enemy in enemiesWithinRange)
        {
            if (GameConfig.Multiplayer)
                gameObject.GetComponent<MultiplayerSync>().EnemyDamageMessage(enemy.GetComponent<PhotonView>().ViewID, 1);
            else
                enemy.GetComponent<Enemy>().TakeDamage(1, facingLeft);
        }
    }
}
