using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorLuz : MonoBehaviour {
    public Light luz;
	// Use this for initialization
	void Start () {
        luz = GetComponent<Light>();
	}

    //CONTROLE DE COLISÕES
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine("apagarLuz");//para metodo apagarLuz corroutine
            luz.intensity = 5;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            luz.enabled = true;
            StartCoroutine("apagarLuz");// ativa metodo apagarLuz corroutine
        }
    }


    //cria rotinas (corroutine), para ações baseando-se no tempo tipo sleep
    IEnumerator apagarLuz() {

        yield return new WaitForSeconds(0.3f);
        luz.intensity += -0.8f;

        if (luz.intensity > 0) {

            StartCoroutine("apagarLuz");

        }
    }
}
