using UnityEngine;
using System.Collections;

public class CameraGUIMutliMenuScript : MonoBehaviour {

	//Positions
	public enum Position : int
	{ MULTIMENU=0, PARAMETRAGE_HOTE=1, PARAMETRAGE_INVITE=2, SALLE_ATTENTE=3 }
	private float _espacementMenus = -40f;
	private Transform _transform;
	[SerializeField]
	private Position _positionDepart;
	[SerializeField]
	private GlassPaneBlackScript _glassPane;
	
	void Start ()
	{
		_transform = this.transform;
		TeleporterCamera(_positionDepart);
	}
	
	public void GoToPosition(Position position)
	{
		StartCoroutine(WaitAndChangeMenu(_glassPane.Duration, position));
	}
	
	private IEnumerator WaitAndChangeMenu(float seconds, Position position)
	{
		_glassPane.StartGlassPane();
		yield return new WaitForSeconds(seconds);
		_glassPane.StopGlassPane();
		TeleporterCamera(position);
	}
	
	private void TeleporterCamera(Position position)
	{
		_transform.position = new Vector3(
			(int)position * _espacementMenus,
			_transform.position.y,
			_transform.position.z);
	}
	
	/*
	//Positions
	public enum Position : int
	{ MULTIMENU=0, PARAMETRAGE_HOTE=1, PARAMETRAGE_INVITE=2, SALLE_ATTENTE=3 }
	private float _espacementMenus = -40f;
	private Transform _transform;
	private float _vitesse = 1;
	private float _positionAAtteindre = 0;
	
	
	[SerializeField]
	private DisplayHostsScript _displayHostsScript;
	
	// Use this for initialization
	void Start ()
	{
		_transform = this.transform;
		
		//GoToPosition(Position.SALLE_ATTENTE);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(    _vitesse > 0 && _transform.position.x >= _positionAAtteindre
			|| _vitesse < 0 && _transform.position.x <= _positionAAtteindre)
		{
			_transform.position = new Vector3(_positionAAtteindre, 0, 10);
		}
		else
		{
			_transform.position = new Vector3(_transform.position.x + _vitesse, 0, 10);
		}
	}
	
	public void GoToPosition(Position position)
	{
		_positionAAtteindre = (int)position * _espacementMenus;
		
		if(_positionAAtteindre - _transform.position.x > 0)
		{
			_vitesse = Mathf.Abs(_vitesse);
		}
		else
		{
			_vitesse = -Mathf.Abs(_vitesse);
		}
	}
	*/
}
