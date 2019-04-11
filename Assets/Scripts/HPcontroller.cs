using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HPcontroller : MonoBehaviour
{
    [SerializeField] private Text lifes;
    public int HP = 100;
    
    public void Hit(int damage, GameObject enemy)
    {
        HP -= damage;
        if (transform.name != "Tank")
        {
            GetComponent<EnemyMove>().ChangeEnemy(enemy);
            if (HP < 0)
                Destroy(gameObject);
        }
        else
        {
            if (HP <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            lifes.text = HP.ToString();
        }
    }
}
