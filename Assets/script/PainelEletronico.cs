using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelEletronico : MonoBehaviour {
    //recebe canvas painel eletronico, para abrir algumas portas
    public GameController gameController;
    public GameObject painelEletronico;
    public Text displayPainel;
    public string senha;
    string senhaDigitada;
    public bool aberto =false;


    void Start()
    {
        gameController = FindObjectOfType<GameController>() as GameController;

        painelEletronico.SetActive(false);
        senhaDigitada = "";
        displayPainel.text = senhaDigitada;


    }
    // Update is called once per frame
    void Update () {
		
	}


    void interacao()
    {
        gameController.changeState(GameState.PAINELINFO);
        painelEletronico.SetActive(true);
    }

    public void btnNumber(int n) {
        senhaDigitada += n;
        displayPainel.text = senhaDigitada;
    }

    public void btnCancelar() {
        gameController.changeState(GameState.GAMEPLAY);
        painelEletronico.SetActive(false);
        senhaDigitada = "";
    }

    public void btnConffirmar() {
        StartCoroutine("confirmar");
    }

    IEnumerator confirmar()
    {
        if (senhaDigitada == senha)
        {
            aberto = true;
            displayPainel.text = "OK";
            yield return new WaitForSecondsRealtime(0.2f);
            displayPainel.text = "";
            yield return new WaitForSecondsRealtime(0.2f);
            displayPainel.text = "OK";
            yield return new WaitForSecondsRealtime(0.2f);
            senhaDigitada = "";
            displayPainel.text = senhaDigitada;
        }
        else
        {
            displayPainel.text = "ERROR";
            yield return new WaitForSecondsRealtime(0.2f);
            displayPainel.text = "";
            yield return new WaitForSecondsRealtime(0.2f);
            displayPainel.text = "ERROR";
            yield return new WaitForSecondsRealtime(0.2f);
            senhaDigitada = "";
            displayPainel.text = senhaDigitada;
        }
    }
}
