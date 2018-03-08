using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilha : MonoBehaviour {
    public float qtdCarga;
    BateriaLanterna script;

	// Use this for initialization
	void Start () {
        script = FindObjectOfType(typeof(BateriaLanterna)) as BateriaLanterna; // recupera o script da lanterna
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void interacao() {
        if (script != null) {
            script.recarregarBateria(qtdCarga);
            Destroy(this.gameObject);// destroy esse script com seu gameobject
        }

    }
}
