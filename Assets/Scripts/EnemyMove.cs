using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
	public NavMeshAgent agent;
	public GameObject bashta;
	private bool _isReloading = false;
	private GameObject closestEnemy = null;
	[SerializeField] private GameObject misleEffect;
	[SerializeField] private AudioSource missleAudio;
    void Update()
    {
	    if (closestEnemy == null)
	    {
		    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Tank");
		    foreach (GameObject en in enemies)
		    {
			    if (en.name != transform.name)
			    {
				    closestEnemy = en;
				    break;
			    }
		    }

		    float path = (transform.position - enemies[0].transform.position).magnitude;
		    foreach (GameObject en in enemies)
		    {
			    if ((transform.position - en.transform.position).magnitude < path && en.name != transform.name)
			    {
				    closestEnemy = en;
			    }
		    }
	    }

	    RaycastHit hit;
	    if (closestEnemy)
	    {
		    Vector3 bashtaEuler = bashta.transform.eulerAngles;
		    bashta.transform.LookAt(closestEnemy.transform.position);
		    bashta.transform.eulerAngles = new Vector3(bashtaEuler.x, bashta.transform.eulerAngles.y, bashtaEuler.z);
		    agent.SetDestination(closestEnemy.transform.position);
		    if (Physics.Raycast(transform.position, closestEnemy.transform.position - transform.position, out hit))
		    {
			    if (hit.transform.tag == "Tank" && hit.distance < 100 && !_isReloading)
			    {
				    agent.isStopped = true;
				    StartCoroutine(Shoot(closestEnemy));
			    }
			    else
				    agent.isStopped = false;
		    }
		    else
			    agent.isStopped = false;
	    }
	}

	private IEnumerator Shoot(GameObject enemy)
	{
		_isReloading = true;
		Vector3 target = enemy.transform.position + new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f));
		RaycastHit hit;
		GameObject missle;
		if (Physics.Raycast(bashta.transform.position, target - bashta.transform.position, out hit))
		{
			missleAudio.Play();
			if (hit.transform.tag == "Tank")
			{
				hit.transform.gameObject.GetComponent<HPcontroller>().Hit(10, gameObject);
			}
			missle = Instantiate(misleEffect, hit.point, Quaternion.identity);
			StartCoroutine(Destroyer(missle));
		}
		yield return new WaitForSeconds(1);
		_isReloading = false;
	}
	
	private IEnumerator Destroyer(GameObject missle)
	{
		yield return new WaitForSeconds(1);
		Destroy(missle);
	}

	public void ChangeEnemy(GameObject enemy)
	{
		closestEnemy = enemy;
	}
}
