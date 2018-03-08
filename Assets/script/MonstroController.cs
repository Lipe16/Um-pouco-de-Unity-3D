using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonstroController : MonoBehaviour {
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GameObject[] pontosDeRonda; 
    public GameObject alvo;

    public PegarTexturaDoChao pegarTexturaDoChao;//esse arquivo tem classe responsável para detectar texturas e meshs de onde personagem pisa 

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = pontosDeRonda[0].transform.position;
        animator.SetInteger("idAnimacao", 0);
	}
	
	// Update is called once per frame
	void Update () {

        if (navMeshAgent.remainingDistance < 5 && alvo == null) {
            navMeshAgent.destination = pontosDeRonda[Random.Range(0, 4)].transform.position;
        } else if (alvo != null) {
            navMeshAgent.destination = alvo.transform.localPosition;
        }

        if (navMeshAgent.remainingDistance > 20)
        {
            alvo = null;
        }


        if (navMeshAgent.remainingDistance < 2)
        {
            animator.SetInteger("idAnimacao", 1);
        }
        else {
            animator.SetInteger("idAnimacao", 0);
        }


    }

    public void tocarSom()
    {
        pegarTexturaDoChao.playSoundStep();
        print("tocar som da criatura");
    }



    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player") {
            alvo = trigger.gameObject;
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arma")
        {
          
                // toma dano
            
        }
    }
}
