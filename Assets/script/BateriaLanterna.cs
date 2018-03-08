using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BateriaLanterna : MonoBehaviour {

    public bool ligado;
    public float cargaMax,carga, gastoS;
    public Light lanterna;
    public Image imagemEnergia;
    public AudioClip audioLanterna;
    public AudioSource audioS;

    // Use this for initialization
    void Start () {
       
        ligado = false;
        lanterna.enabled = ligado;
        StartCoroutine("usarCarga");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Lanterna")) {
            audioS.PlayOneShot(audioLanterna);
            ligado = !ligado;
            lanterna.enabled = ligado;

        }
        updateImagemEnergia();


    }

    void updateImagemEnergia()
    {
        imagemEnergia.rectTransform.localScale = new Vector3((carga/100f),1,1);
    }

    IEnumerator usarCarga() {
        yield return new WaitForSeconds(1);

        if (ligado == true)
        {
            carga -= gastoS;
            if (carga < 0 || carga == 0)
            {
                carga = 0;
                ligado = false;
                lanterna.enabled = ligado;
            }
        }
        else {
            carga += gastoS;
            if (carga > cargaMax)
            {
                carga = cargaMax;
            }
        }

     StartCoroutine("usarCarga");
    }

    public void recarregarBateria(float recarga) {

        carga += recarga;

        if(carga > cargaMax){
            carga = cargaMax;
        }

    }
}
