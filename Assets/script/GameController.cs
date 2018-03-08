using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState{
    GAMEPLAY,
    INVENTARIO,
    PAINELINFO
    }

public class GameController : MonoBehaviour {
    public GameState currentState;
    public List<GameObject> inventario;
    public Image[] slot;



    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //metodo controla o tempo do jogo, se o jogo está em start ou pause
    public void changeState(GameState newState) {
        currentState = newState;

        if (currentState == GameState.INVENTARIO)//jogo pausa ao abrir inventário
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        } else if (currentState == GameState.PAINELINFO) {//jogo pausa ao abrir painel
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else {
            Time.timeScale = 1;//jogo corre normal
            Cursor.visible = false;
        }

    }
}
