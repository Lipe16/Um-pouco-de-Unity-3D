using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInfo : MonoBehaviour {
    public string nome;
    public string descricao;
    public Sprite imagem;
    public bool clicavel;


    public void usarItem() {
        float qtdCarga = 30;
        BateriaLanterna script;
        script = FindObjectOfType(typeof(BateriaLanterna)) as BateriaLanterna; // recupera o script da lanterna


        if (script != null)
        {
            script.recarregarBateria(qtdCarga);
            //Destroy(this.gameObject);// destroy esse script com seu gameobject
        }
    }

}
