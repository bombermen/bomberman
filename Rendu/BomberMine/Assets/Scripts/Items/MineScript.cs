using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineScript : MonoBehaviour
{

	public Rigidbody _rigidbody;
	public bool _collante = false;
	public float _seconds = 3;
	public MineExplosionScript _mineExplosionScript;
	
	Transform _transform;
	
	bool _mineEnclenchee = false;

	public bool MineEnclenchee
	{
		get { return this._mineEnclenchee; }
		set { _mineEnclenchee = value; }
	}	
	void Start()
	{
		_transform = this.transform;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(!MineEnclenchee)
		{
			if(_collante)
			{
				_transform.parent.parent = col.transform;
				_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
			}
			
			MineEnclenchee = true;
		}
	}
	
	public void Explode()
	{
		List<GameObject> objectsToDestroy = _mineExplosionScript.ObjectsToDestroy;
		
		foreach(GameObject ob in objectsToDestroy)
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
	}
}
