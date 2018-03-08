using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

public class FPSController : MonoBehaviour {
    //canvas
    public GameObject painelInteracao;

    //controles da camera
    private GameObject cameraFPS;
    Vector3 moveDirection = Vector3.zero;

    //controles do personagem
    public CharacterController characterController;
    public float moveSpeed, speedCam, jumpForce;
    public float velocidadeBase, incrementoCorrida;
    public float rotationX, rotationY;
    public GameController gameController;
    public Animator animator;
    public int idAnimation = 0;
    public Transform Arissa;
    public bool estavaCorrendoAntesDoPulo, pulando;
    public float distanciaDeInteracaoComObjetos;

    //animação ao usar objetos
    public Transform cotoveloDireito, auxCotoveloDireito;

    //controles do inventario
    public bool inventarioAberto;
    public GameObject inventarioWindow;
    public List<GameObject> inventario;
    public Image[] slot;

    //controles do audio
    public AudioClip[] audioStep;
    public AudioSource audioSource;
    public AudioClip somPulando, somAterrisando;
    public float delayEntrePassos, fatorDelay=1;
    public float tempoEntrePassos;


    //pegar classes dentro de outros arquivos
    public PegarTexturaDoChao pegarTexturaDoChao;//esse arquivo tem classe responsável para detectar texturas e meshs de onde personagem pisa 
    public GameObject lanterna;

    // Use this for initialization
    void Start () {
        
        // pega o arquivo na inicialização para usar suas classes responsáveis por detectar texturas mashs do chão como dito acima
        

        //pega game controles do jogo e seu inventário
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        characterController = GetComponent<CharacterController>();
        inventario = gameController.inventario;
        slot = gameController.slot;
        transform.tag = "Player";//da uma tag ao personagem

        cameraFPS = GetComponentInChildren<Camera>().transform.gameObject;//pega a camera do personagem


        //recupera audio
        audioSource = GetComponent<AudioSource>();//recupera o controlador de audio no personagem

        //zera as posições do vetor da camera
        cameraFPS.transform.localRotation = Quaternion.identity;
        cameraFPS.transform.localPosition = new Vector3(0, 3.94f, -2.5f);
    }
	
	// Update is called once per frame
	void Update () {
        animator.SetInteger("estadoAnimacao", idAnimation);

        //para abrir ou fechar janela do inventario 
        if (Input.GetButtonDown("Inventario") && gameController.currentState != GameState.PAINELINFO){
            gameController.changeState(GameState.INVENTARIO);
            inventarioAberto = !inventarioAberto;
            inventarioWindow.SetActive(inventarioAberto);
            abrirInventario();
        }
  

        //nessa parte é pegada a direção pra onde a camera está olhando para a frente
        Vector3 directFront = new Vector3(cameraFPS.transform.forward.x, 0, cameraFPS.transform.forward.z);
        directFront *= Input.GetAxis("Vertical");

        //nessa parte é pegada a direção pra onde a camera está olhando para o lado
        Vector3 directRight = new Vector3(cameraFPS.transform.right.x, 0, cameraFPS.transform.right.z);
        directRight *= Input.GetAxis("Horizontal");

        Vector3 directFinal = directFront + directRight;


        //Rotação do personagem
        if (Input.GetAxis("Horizontal") > 0)
        {
            Quaternion QuatenionR = Quaternion.AngleAxis(30, Vector3.up);
            Arissa.transform.localRotation = QuatenionR;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            Quaternion QuatenionR = Quaternion.AngleAxis(-30, Vector3.up);
            Arissa.transform.localRotation = QuatenionR;
        }
        else {
            Quaternion QuatenionR = Quaternion.AngleAxis(0, Vector3.up);
            Arissa.transform.localRotation = QuatenionR;
        }
  

        //verifica se o personagem toca o chão
        if (characterController.isGrounded) {   //está tocando no chão
            // direção que o personagem olha(camera), gurda esses valores na variavel
            moveDirection = new Vector3(directFinal.x, 0, directFinal.z) * moveSpeed;
            //controle para controlar o pulo
            if (pulando == true)
            {
                audioSource.PlayOneShot(somAterrisando);
                pulando = false;
            }
            else {//finalizar o pulo
                    //controles para estado do personagem, animações
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)//se não mover o mouse
                    {
                        if (idAnimation != 2 && estavaCorrendoAntesDoPulo == false)
                        {
                            idAnimation = 1;
                        } else if (estavaCorrendoAntesDoPulo) {
                            moveSpeed = velocidadeBase + incrementoCorrida;
                            fatorDelay = 0.5f;
                            idAnimation = 2;
                        }
                    }
                    else {//se o mouse estiver se movendo
                        if (idAnimation != 2) {
                            idAnimation = 0;
                        }
                    }
            }

            if (Input.GetButtonDown("Jump")) {// ao aperta boão de pulo (ex: space)
                idAnimation = 3;
                moveDirection.y = jumpForce;  
                pulando = true;
                audioSource.PlayOneShot(somPulando);
            }
            //esse metodo faz som de passos no chão
            tocarSomAoCaminhar();
        }

        // aplicar gravidade
        moveDirection.y += Physics.gravity.y * Time.deltaTime;
        //Pega valores obtidos acima para mover a direção que o personagem olha(camera)
        characterController.Move(moveDirection * Time.deltaTime);
        FPSCamController();

        //implementando corrida
        //quando botão de corrida dor pressionado
        if (Input.GetButtonDown("Corrida")){
            moveSpeed = velocidadeBase + incrementoCorrida;
            fatorDelay = 0.5f;
            idAnimation = 2;
            estavaCorrendoAntesDoPulo = true;
        }//quando botão de corrida for solto
        else if (Input.GetButtonUp("Corrida"))
        {
            moveSpeed = velocidadeBase;
            fatorDelay = 1;
            idAnimation = 0;
           estavaCorrendoAntesDoPulo = false;
        }

        //neste trecho utilizo a tecnica de Raycast para saber se o personagem está olhando (camera está olhando) para objeto de interação
        //para isso utilizei a posição do personagem e também a posição para onde a camera aponta(forward)
        RaycastHit raycastHit;
        if (Physics.Raycast(new Vector3(transform.position.x, 61.5f, transform.position.z), cameraFPS.transform.forward, out raycastHit, distanciaDeInteracaoComObjetos)) {
            if (raycastHit.collider.tag == "Interacao")
            {

                painelInteracao.SetActive(true);//quando a camera olha para um objeto de interacao aparece um painel na tela
                //ao aperatar botão configurado como interação, ele pode mexer com objetos caso estejam na area de interação
                if (Input.GetButtonDown("Interacao"))
                {
                    //manda uma mensagem, dispara o metodo interacao do objeto que está interagindo(metodo fica em outro script no objeto interagindo)
                    raycastHit.collider.gameObject.SendMessage("interacao", SendMessageOptions.DontRequireReceiver);
                }
            }
            else {
                painelInteracao.SetActive(false);
            }
        }else
        {
            painelInteracao.SetActive(false);
        }//Debug mostra a sena onde raycast está mirando
         Debug.DrawRay(new Vector3(transform.position.x, 61.5f, transform.position.z), cameraFPS.transform.forward*distanciaDeInteracaoComObjetos, Color.red);

    }




    void FPSCamController(){

        rotationX += Input.GetAxis("Mouse X") * speedCam;
        rotationX = clampAngle(rotationX, -360, 360);

        rotationY += Input.GetAxis("Mouse Y") * speedCam;
        rotationY = clampAngle(rotationY, -35, 35);

        Quaternion xQuatenion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuatenion = Quaternion.AngleAxis(rotationY, -Vector3.right);// negativo corrige  movimento da camera para cima
        Quaternion rotationFinal = Quaternion.identity * xQuatenion * yQuatenion;

        //suavisa rotação da camera, baseado na variavel speedCam
        //não usei variavel rotação final não deixar a camera girar sozinha em X,
        //evitando assim que ela e o personagem apontem para locais diferenes... Quando personagem for girado a camera obrigatoriamente gira junto
        cameraFPS.transform.localRotation = Quaternion.Lerp(cameraFPS.transform.localRotation, yQuatenion, speedCam * Time.deltaTime);

        //rotaciona o personagem, fazendo que a camera gire junto em X, como foi dito logo acima
        Arissa.parent.localRotation = xQuatenion;

        // cameraFPS.transform.localRotation = new Quaternion(0, 0, 0,0);// zera posições de rotacao após girar personagem

    }
    
    //metodo responsável por limitar a rotação da camera
    float clampAngle(float angle, float min, float max)
    {
        if (angle < -360) { angle += 360; }
        if (angle > 360) { angle += -360; }
        return Mathf.Clamp(angle, min, max);
    }




    //CONTROLE DE COLISÕES
    private void OnCollisionEnter(Collision collision)
    {
    }
    private void OnCollisionStay(Collision collision)
    {
    }
    private void OnCollisionExit(Collision collision)
    {
    }

    //CONTROLE DE TRIGGERS (semelhante a colisões)
    private void OnTriggerEnter(Collider collider)
    {
        //se o objeto disparando a triger tiver com a tag "coletavel", adicionará referencia ao array inventário
        if (collider.gameObject.tag == "Coletavel")
        {
            if (inventario.Count < slot.Length) {
                inventario.Add(collider.gameObject);
                collider.gameObject.SetActive(false);
            }  
        }
    }

    private void OnTriggerStay(Collider other)
    {    
    }
    //quando objeto engatilhando a triger deixa de engatilha-lo
    private void OnTriggerExit(Collider collision)
    {
    }

    public void abrirInventario() {
        //verifica se o inventário foi chamado
        switch (inventarioAberto)
        {
            //se for chamado , popula o inventário
            case true:
                int i = 0;
                foreach (GameObject item in inventario)
                {
                    slot[i].sprite = item.GetComponent<ItemInfo>().imagem;
                    slot[i].GetComponent<Slot>().itemInfo = item.GetComponent<ItemInfo>();
                    i++;
                }
                break;

            case false:
                //se foi chamado, mais já estava em execução, ele coloca velocidade do jogo normal(gameplay) e esvazia imagens dos slots
                gameController.changeState(GameState.GAMEPLAY);
                foreach (Image img in slot)
                {
                    img.sprite = null;
                }
                break;
        }

    }

    //esvazia inventário e repopula novamente
    public void atualizarInventario()
    {
        foreach (Image img in slot)
        {
            img.sprite = null;
            img.GetComponent<Slot>().itemInfo = null;

            //popular novamente
            int i = 0;
            foreach (GameObject item in inventario)
            {
                slot[i].sprite = item.GetComponent<ItemInfo>().imagem;
                slot[i].GetComponent<Slot>().itemInfo = item.GetComponent<ItemInfo>();
                i++;
            }
        }
    }



    //tocar som ao caminhar
    public void tocarSomAoCaminhar()
    {
        //verifica se personagem está caminhando
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (tempoEntrePassos == 0)
            {
                //int i = Random.Range(0, audioStep.Length);
                //audioSource.PlayOneShot(audioStep[i]);//toca o som
                pegarTexturaDoChao.playSoundStep();
            }
                tempoEntrePassos += Time.deltaTime;
                if (tempoEntrePassos >= delayEntrePassos*fatorDelay)//verifica se já terminou o deley para gerar novo som de passos
                {
                    tempoEntrePassos = 0;
                }
        }
        //em else tempoentrepassos é zerado para gerar novo 
        else
        {
            tempoEntrePassos = 0;
        }
    }



}


