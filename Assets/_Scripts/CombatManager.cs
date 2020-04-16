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

            //Debug.Log(enemyHp + " " + enemyEvade + " " + enemyDmg);
            do
            {
                if (Random.Range(0, 101) > enemyEvade)
                {
                    enemyHp -= currentPawn.dmg;
                }

                if (Random.Range(0, 100) > currentPawn.evadeProb)
                {
                    currentPawn.hp -= enemyDmg;
                }

            } while (enemyHp > 0 || currentPawn.hp > 0);
            if (enemyHp < 0)
                Debug.Log("Player Won");
            else
                Debug.Log("Player Lost");
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
