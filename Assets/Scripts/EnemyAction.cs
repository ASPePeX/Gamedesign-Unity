using UnityEngine;
using System.Collections;

public class EnemyAction : MonoBehaviour
{
    public int HealthPoints;

    public GameObject Weapon;

    public void MessageHandler(string msg)
    {
        if (msg.Equals("NewRound"))
        {
            if (HealthPoints <= 0)
            {
                Debug.Log(this.gameObject.name + " died!");
                Destroy(this.gameObject);
                
            }
        }
    }
}
