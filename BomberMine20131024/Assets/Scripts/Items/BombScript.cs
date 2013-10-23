/*
 * Inactif jusqu'à toucher un objet
 * Attend ensuite _seconds secondes avant de détruire tous les objets
 * dans le rayon _explosionMagnitude
 * Les objets dans ce rayon mais qui sont cachés par un objet indestructible sont protégés
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class BombScript : MonoBehaviour
{
	public float _explosionMagnitude = 2;
	public Rigidbody _rigidbody;
	public bool _collante = false;
	public float _seconds = 3;
	
	bool explode = false;
	
	bool _bombeDeclenchee = false;
	
	void OnTriggerEnter(Collider col)
	{
		
		if(!_bombeDeclenchee)
		{
			if(_collante)
			{
				transform.parent = col.transform;
				_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
			}
			
			_bombeDeclenchee = true;
			StartCoroutine(Declencher(_seconds));
		}
	}
	
	private IEnumerator Declencher(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		destroyAround();
	}
	
	void destroyAround()
	{
		List<GameObject> hitObjects = new List<GameObject>();
		Vector3 center = transform.position;
		int res = 1000;
		
		for(int i=0; i<res; i++)
		{
			Vector3 fwd = new Vector3(Mathf.Cos(i / 2 / Mathf.PI), 0, Mathf.Sin(i / 2 / Mathf.PI));
			
			Ray ray = new Ray(center, fwd);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, _explosionMagnitude))
			{
				GameObject ob = hit.collider.gameObject;
				
				if(ob.layer != 11 && !hitObjects.Contains(ob))
				{
					hitObjects.Add(hit.collider.gameObject);
				}
			}
			
		}
		
		foreach(GameObject ob in hitObjects)
		{
			if(ob.layer == 8)
			{
				ob.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				ob.SetActive(false);
			}
		}
		
		explode = true;
		
		transform.parent.parent.gameObject.SetActive(false);
	}
		
	void Update() {
			
		//DEBUG -- dessin des rayons autour de la bombe
		/*
		if(explode) {
			
			Vector3 center = transform.position;
			int res = 1000;
			
			for(int i=0; i<res; i++)
			{
				
				Vector3 fwd = new Vector3(Mathf.Cos(i / 2 / Mathf.PI), 0, Mathf.Sin(i / 2 / Mathf.PI));
				
				Ray ray = new Ray(center, fwd);
				RaycastHit hit;
				
				Debug.DrawLine(ray.origin, hit.point);
			}
		}
		*/
	}
}
