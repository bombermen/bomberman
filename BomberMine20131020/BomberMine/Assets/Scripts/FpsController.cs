/*
 * Controleur du Personnage
 * 
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FpsController : MonoBehaviour {
	
	//inputs
	bool _forward;
	bool _backward;
	bool _right;
	bool _left;
	bool _jump;
	
	//vitesse maximale sur X ou Z
	float _speed = 4;
	
	//gameObject properties
	Rigidbody _rigidbody;
	Transform _transform;
	
	//initialization
	void Start() {
		_transform = this.transform;
		_rigidbody = this.rigidbody;
	}
	
	//called once per frame
	void Update() {
		
		_forward = false;
		_backward = false;
		_right = false;
		_left = false;
		_jump = false;
		
		//directions
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z)) {
			_forward = true;
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			_backward = true;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			_right = true;
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) {
			_left = true;
		}
	}
	
	//called at a fixed rate
	void FixedUpdate() {
		
		//Vitesse du point de vue du perso
		Vector3 localSpeed = Vector3.zero;
		//Vitesse globale (incluant la rotation du perso)
		Vector3 speed = Vector3.zero;
		
		if(_forward) {
			localSpeed.z += 1;
		}
		if(_backward) {
			localSpeed.z -= 1;
		}
		if(_right) {
			localSpeed.x += 1;
		}
		if(_left) {
			localSpeed.x -= 1;
		}
		
		//Normalisation du vecteur (pour éviter le problème du straff)
		localSpeed = localSpeed.normalized * _speed;
		//Retour aux coordonées globales
		speed = _transform.TransformDirection(localSpeed);
		//Déplacement du personnage
		_rigidbody.velocity = speed;
	}
}
