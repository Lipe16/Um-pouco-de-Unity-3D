using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPainel : MonoBehaviour {

    public Text nome, descricao;
    public Image image;
    public ItemInfo itemInfo;
    public GameController gameController;
    public FPSController fPSController;
    public Button btnUsar;

    public void usarItem()
    {
        print("usar item");
        itemInfo.usarItem();
        fPSController = FindObjectOfType(typeof(FPSController)) as FPSController;
        fPSController.inventario.Remove(itemInfo.gameObject);
        fPSController.atualizarInventario();
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        gameController.currentState = GameState.INVENTARIO;
        this.gameObject.SetActive(false);
    }


    public void removerItem()
    {
        fPSController = FindObjectOfType(typeof(FPSController)) as FPSController;
        fPSController.inventario.Remove(itemInfo.gameObject);
        fPSController.atualizarInventario();
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        gameController.currentState = GameState.INVENTARIO;
        this.gameObject.SetActive(false);
    }


    public void fechar()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        gameController.currentState = GameState.INVENTARIO;
        this.gameObject.SetActive(false);
        print("fechar");
    }
}
