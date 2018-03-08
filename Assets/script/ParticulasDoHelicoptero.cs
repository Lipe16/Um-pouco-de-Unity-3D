using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulasDoHelicoptero : MonoBehaviour {
    public GameObject emissor; //emissor de particulas
    public float altitudeMax;
    public float altitude, percentualDistancia;

    public ParticleSystem terra, folhas;
    ParticleSystem.EmissionModule terraEmissor;
    ParticleSystem.EmissionModule folhasEmissor;

    public float tMax, tMin, fMax, fMin;//variaveis para serem usadas junto aos modulos emissores para controlar sua emissão

	// Use this for initialization
	void Start () {

        //acesando modulo de emissão dos particleSystem
        terraEmissor = terra.emission;
        folhasEmissor = folhas.emission;

        tMin = terraEmissor.rateOverTime.constantMin;
        tMax = terraEmissor.rateOverTime.constantMax;

        fMin = folhasEmissor.rateOverTime.constantMin;
        fMax = folhasEmissor.rateOverTime.constantMax;

    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit raycastHit;

        //condicional retorna distância caso Raycast funcione, Vector3.down(0,-1,0) é para mirar para baixo 
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, altitudeMax*5)) {
            //quando raucastHit colidir com terreno, recebe a distancia entre o tereno e o helicoptero
            if (raycastHit.collider.GetComponent<Terrain>()) {
                altitude = raycastHit.distance;

                percentualDistancia = altitude / altitudeMax;//calcula percentual da distância
                atualizarPosicao();//atualiza posição do particleSystemManeger

                //condicionais usam percentual de distância para emitir ou não as particulas dependendo da distância
                if (percentualDistancia > 1)
                {
                    terraEmissor.rateOverTime = new  ParticleSystem.MinMaxCurve(0,0);
                    folhasEmissor.rateOverTime = new ParticleSystem.MinMaxCurve(0, 0);
                }
                else {
                    terraEmissor.rateOverTime = new ParticleSystem.MinMaxCurve(tMin, tMax);
                    folhasEmissor.rateOverTime = new ParticleSystem.MinMaxCurve(fMin, fMax);
                }
            }

            
            
        }
	}

    //metodo responsavel por manter o emissor de particulas sempre abaixo do helicoptero
    // e mantendo-o abaixo da altidude(no chão)
    void atualizarPosicao() {
        emissor.transform.position = new Vector3(transform.position.x, transform.position.y - altitude, transform.position.z);
    }
}
