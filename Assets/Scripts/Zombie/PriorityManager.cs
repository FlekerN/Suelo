using System.Collections.Generic;
using UnityEngine;

public class PriorityManager : MonoBehaviour
{
    [SerializeField]
    private int maxAttackers = 3;

    public List<EnemyAI> enemyList =
        new List<EnemyAI>();

    private List<EnemyAI> priorityList =
        new List<EnemyAI>();

    private void Awake()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        enemyList.AddRange(enemies);
    }

    private void Update()
    {
        priorityList.Clear();

        foreach (EnemyAI enemy in enemyList)
        {
            if (enemy == null)
                continue;

            enemy.isAttackPriority = false;
            

            if (enemy.distanceToPlayer <= enemy.attackRange)
            {
                priorityList.Add(enemy);
            }
        }

        // Ordenar por distancia
        priorityList.Sort((a, b) =>
        {
            float distA =
                a.distanceToPlayer - a.priorityBonus;

            float distB =
                b.distanceToPlayer - b.priorityBonus;

            return distA.CompareTo(distB);
        });

        // Dar prioridad
        for (int i = 0;
             i < priorityList.Count;
             i++)
        {
            if (i < maxAttackers)
            {
                priorityList[i]
                    .isAttackPriority = true;
            }
        }
    }
}