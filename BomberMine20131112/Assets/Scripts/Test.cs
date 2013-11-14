using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    private ManagerScript _manager;

    void Start ()
    {
        _manager = ManagerScript.Instance;
        Debug.Log(_manager.Players[0].Nom);
    }
}
