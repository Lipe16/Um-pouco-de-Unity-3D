using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPorta : MonoBehaviour {

    public bool aberta;
    public float step, abertura;
    float rotationZ = 0;
    public bool precisaDeChave, requerPainelEletronico;
    public string chaveNome;
    public FPSController scriptPlayer;

    public PainelEletronico painelEletronico;
   
    

    // Use this for initialization
    void Start () {

        scriptPlayer = FindObjectOfType(typeof(FPSController)) as FPSController;
        painelEletronico = FindObjectOfType(typeof(PainelEletronico)) as PainelEletronico;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void interacao()
    {

        //verifica se a porta abre ou se precisa checar inventário e ver se personagem tem a chave dela
        if (precisaDeChave == true)
        {
            foreach (GameObject item in scriptPlayer.inventario)
            {
                if (item.name == chaveNome)//caso precise de chave, aqui confere se o nome da chave bate com a porta
                {
                    StartCoroutine("abrirFecharPorta");//se tudo der certo ele starta a corroutina de abrir porta
                    precisaDeChave = false;
                    break;
                }
            }
        } else if (requerPainelEletronico == true) {
            if (painelEletronico.aberto) {
                StartCoroutine("abrirFecharPorta");//se tudo der certo ele starta a corroutina de abrir porta
            }
        }
        else
        {
            StartCoroutine("abrirFecharPorta");//se tudo der certo ele starta a corroutina de abrir porta
        }
        

    }


    //corrotina de abrir a porta
    IEnumerator abrirFecharPorta() {


        yield return new WaitForEndOfFrame();// a cada freme a porta gira um pouco, ate completar abertura ou fechamento

        // esse bloco de código faz com que a porta abra ou feche suavemente
        if(aberta == true) {
            rotationZ += step;

            transform.localRotation = Quaternion.Euler(-90,0,rotationZ);

            if (rotationZ < abertura) {
                StartCoroutine("abrirFecharPorta");
            }
            else{
                transform.localRotation = Quaternion.Euler(-90, 0, 40);
                aberta = false;
            }

        } else if (aberta == false) {
            rotationZ -= step;

            transform.localRotation = Quaternion.Euler(-90, 0, rotationZ);
            print("entrou");
            if (rotationZ > -abertura)
            {
                StartCoroutine("abrirFecharPorta");
            }
            else
            {
                transform.localRotation = Quaternion.Euler(-90, 0, -40);
                aberta = true;
            }
        }
    }
}
