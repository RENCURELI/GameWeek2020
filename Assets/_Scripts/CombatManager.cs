using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 1. Get current player Index
 * 2. Get player stats
 * 3. Determine random enemy
 * 4. Get Enemy stats (random)
 * 5. Random test + stat modifiers for resolution
 * 6. Send result and make player pawn act accordignly
 */

public class CombatManager : MonoBehaviour
{
    PlayerStats currentPawn;

    public bool combatStart = false;

    private int enemyHp = -1;
    private int enemyEvade = -1;
    private int enemyDmg = -1;

    // Start is called before the first frame update
    void Start()
    {
        //Will have to add index checking when second player is added
        currentPawn = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatStart && enemyDmg < 0 && enemyEvade < 0 && enemyHp < 0)
        {
            enemyHp = Random.Range(25, 100);
            enemyEvade = Random.Range(10, 26);
            enemyDmg = Random.Range(5, 11);

            Debug.Log(enemyHp + " " + enemyEvade + " " + enemyDmg);
            do
            {
                enemyHp -= 5;
                Debug.Log(enemyHp);
            } while (enemyHp > 0);
            EndFight();
        }
    }

    void EndFight()
    {
        combatStart = false;
        enemyHp = -1;
        enemyEvade = -1;
        enemyDmg = -1;
    }

}
