using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DeterminerTailleTransform : Editor {
	
	[Serializable]
	class Toto
	{
		public Vector3 toto;
		public Vector3 tota;
		public Vector3 toti;
	}
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[MenuItem("CustomButton/GetBytCountInFile")]
	static void GetByteCount()
	{
		var go = (GameObject) Selection.activeObject;
		if (go != null)
		{
			var toto = new BinaryFormatter();
			var stream = new FileStream(Application.persistentDataPath + "/test.txt", FileMode.Append);
			
			toto.Serialize(stream, new Toto());
			
			
			
			stream.Close();
		}
		
		
		
	}
	
}
