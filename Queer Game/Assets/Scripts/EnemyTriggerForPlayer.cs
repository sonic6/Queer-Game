using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerForPlayer : MonoBehaviour
{
    [SerializeField] EnemyController myParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            myParent.WaitForPlayerAttack(other.transform);
            Verses.myEnemy = myParent;
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            myParent.FindNewTarget();
            Verses.myEnemy = null;
        }
    }
}
