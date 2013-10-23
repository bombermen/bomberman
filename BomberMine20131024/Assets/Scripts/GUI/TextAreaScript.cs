using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TextAreaScript : MonoBehaviour {
	
	public int _caracteresParLigne = 20;
	public int _nbLignes = 10;
	public float _espacement = .2f;
	public bool _afficherFin = true;
	public bool _afficherFond = false;
	public GameObject _fond;
	public AlphabetScript _alphabet;
	
	public string _text;
	private string _lignes;
	private Transform _transform;
	private GameObject[] _objetsInstancies;
	private GameObject[][] _tuileFondInstancies;
	
	// Use this for initialization
	void Start ()
	{
		_transform = this.transform;
		_objetsInstancies = new GameObject[_caracteresParLigne * _nbLignes];
		_tuileFondInstancies = new GameObject[_nbLignes][];
		for(int i=0; i<_nbLignes; i++)
			_tuileFondInstancies[i] = new GameObject[_caracteresParLigne];
		
		if(_afficherFond)
		{
			AfficherFond();
		}
		SetText(_text);
	}
	
	private void AfficherFond()
	{
		for(int i=0; i<_nbLignes; i++)
		{
			for(int j=0; j<_caracteresParLigne; j++)
			{
				Vector3 position = _transform.position;
				position.x -= j * _espacement;
				position.y -= i * 1;
				_tuileFondInstancies[i][j] = Instantiate(_fond, position, Quaternion.identity) as GameObject;
			}
		}
	}
	
	/// <summary>
	/// Detruit les objets instanciés.
	/// </summary>
	void ReinitialiserObjetsInstancies()
	{
		for(int i=0; i<_objetsInstancies.Length; i++)
			if(_objetsInstancies[i])
				Destroy(_objetsInstancies[i]);
	}
	
	/// <summary>
	/// Sets the text.
	/// Affiche le texte dans les limites spécifiées dans les attributs publics.
	/// </summary>
	/// <param name='text'>
	/// _text.
	/// </param>
	void SetText(string text)
	{
		Vector3 scale = _transform.localScale;
		_text = text;
		int debut, fin;
		
		_transform.localScale = new Vector3(1f,1f,1f);
		
		ReinitialiserObjetsInstancies();
		
		if(_afficherFin)
		{
			debut = _text.Length - _caracteresParLigne * _nbLignes;
			fin = _text.Length;
			if(debut < 0)
			{
				debut = 0;
			}
		}
		else
		{
			debut = 0;
			fin = _caracteresParLigne * _nbLignes;
			if(fin > _text.Length)
			{
				fin = _text.Length;
			}
		}
		
		for(int i=debut; i<fin; i++)
		{
			Vector3 position = _transform.position;
			position.x -= (i % _caracteresParLigne) * _espacement;
			position.y -= (i / _caracteresParLigne) * 1;
			_objetsInstancies[i] = _alphabet.CharToGameObject(_text[i]);
			if(_objetsInstancies[i] != null)
			{
				_objetsInstancies[i] = Instantiate(_objetsInstancies[i], position, Quaternion.identity) as GameObject;
				_objetsInstancies[i].transform.parent = _transform;
			}	
		}
		
		_transform.localScale = scale;
	}
	

	/// <summary>
	/// Mises en forme.
	/// Permet de mettre a la ligne les mots qui seraient coupés
	/// sans cela.
	/// </summary>
	/// <returns>
	/// Texte en forme.
	/// </returns>
	/// <param name='text'>
	/// minuscule à mettre en forme.
	/// </param>
	/// 
	private string MiseEnForme(string text)
	{
		string resultat = "";
		int ligne = 0;
		int caractereDansLigneCourante = 0;
		
		_text = MiseEnForme(text);
		
		string[] mots = text.Split(new char[]{' '});
		
		for(int i=0; i<mots.Length; i++)
		{
			caractereDansLigneCourante += mots[i].Length + 1;
			
			if(caractereDansLigneCourante <= _caracteresParLigne)
			{
				resultat += mots[i] + " ";
			}
			else
			{
				int nbEspaces = _caracteresParLigne - caractereDansLigneCourante + mots[i].Length + 1;
				for(int espace=0; espace<nbEspaces; espace++)
				{
					resultat += " ";
				}
				resultat += mots[i] + " ";
				caractereDansLigneCourante = mots[i].Length + 1;
				ligne++;
			}
		}
		
		return resultat;
	}
	
	public void Append(string str)
	{
		_text += str;
		SetText(_text);
	}
	
	public void EffacerDerniereLettre()
	{
		if(_text != "")
		{
			_text = _text.Substring(0, _text.Length - 1);
		}
		SetText(_text);
	}
	
	public void SetFocused(bool focus)
	{
		Color color = focus ? Color.red : Color.black;
		for(int i=0; i<_nbLignes; i++)
			for(int j=0; j < _caracteresParLigne; j++)
				_tuileFondInstancies[i][j].renderer.material.color = color;
	}
}
