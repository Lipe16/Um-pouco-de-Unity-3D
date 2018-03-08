using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public ItemInfo itemInfo;
    public GameObject painelInfo;
    public GameController gameController;



    void Start()
    { 
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

        public void OnPointerDown(PointerEventData eventData)
    {
        if (itemInfo != null) {
            gameController.changeState(GameState.PAINELINFO);
            if (itemInfo.clicavel == false)
            {
                painelInfo.GetComponent<ItemInfoPainel>().btnUsar.interactable = false;
            }
            else {
                painelInfo.GetComponent<ItemInfoPainel>().btnUsar.interactable = true;
            }
            painelInfo.SetActive(true);
            painelInfo.GetComponent<ItemInfoPainel>().nome.text = itemInfo.nome;
            painelInfo.GetComponent<ItemInfoPainel>().descricao.text = itemInfo.descricao;
            painelInfo.GetComponent<ItemInfoPainel>().image.sprite = itemInfo.imagem;
            painelInfo.GetComponent<ItemInfoPainel>().itemInfo = itemInfo;
        }
    }
}
