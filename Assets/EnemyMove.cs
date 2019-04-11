using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	public NavMeshAgent agent;
    void Update()
    {
		float h = Screen.height;
		float w = Screen.width;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(w / 2, h / 2));
		RaycastHit hit;
		if (Input.GetKey(KeyCode.Space))
		{
			if (Physics.Raycast(ray, out hit, 90))
			{
				Debug.Log(hit.point);
				agent.SetDestination(hit.point);
			}
		}
	}
}
