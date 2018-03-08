using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarTexturaDoChao : MonoBehaviour {
    //classe que armazena grupo de texturtassendo usada como vetor, para ter várias instancias da mesma(vários grupos)
    public TextureType[] textureTypes;

    //Audio sourde da classe
    public AudioSource audioS;

    public GameObject personagem;

    //Váriaveis para controle do terreno
    public TerrainData terrainData;
    int alphaMapWidth, alphaMapHeight;
    float[,,] splatData;
    int numeroDeTexturas;


	// Use this for initialization
	void Start () {
        //recupera audiosource do personagem
        //audioS = GetComponent<AudioSource>();
        //audioS.volume = 0.3f;

        //essas linhas abaixo recuperam valores do terreno e calculam a quantidade de texturas contém nele, mesmo que não usado todos
        //terrainData = Terrain.activeTerrain.terrainData;//recupera terreno ativo
        alphaMapHeight = terrainData.alphamapHeight;//altura do terreno
        alphaMapWidth = terrainData.alphamapWidth;//largura do terreno

        splatData = terrainData.GetAlphamaps(0,0,alphaMapWidth, alphaMapHeight);

        numeroDeTexturas = splatData.Length / (alphaMapWidth*alphaMapHeight);//retorna numero de texturas presentes no terreno da sena
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //metodo será usado em FPSController para emitir som onde personagem pisa
    public void playSoundStep() {
        //essa técnica é chamada de raycast, fica pegando as colisões com o chão
        RaycastHit raycastHit;
        Vector3 start = personagem.transform.position;//transform.position;
        Vector3 dir = Vector3.down;

        //condiconal responsável por verificar se há colisão com o chão
        if (Physics.Raycast(start, dir, out raycastHit, 1.3f)) {
            print(raycastHit.collider.name);//pegar nome do que esta colidindo no chão

            // se estiver colidindo com uma mesh
            if (raycastHit.collider.GetComponent<MeshRenderer>()) {
                playMesh(raycastHit.collider.GetComponent<MeshRenderer>());
                print("mesh");//pegar nome do que esta colidindo no chão
            }
            //se estiver colidindo apenas com terreno
            else if (raycastHit.collider.GetComponent<Terrain>()) {
                print("terreno");
                playTerrain();
            }
        }
    }

    //Metodo recebe em qual mesh do chão o personagem colide
    // e usa dois laços de repetição para descobrir qual som corresponde a essa mesh
    //para representar passos...
    void playMesh(MeshRenderer mesh) {
        if (textureTypes.Length > 0) {
            foreach (TextureType type in textureTypes) {
                foreach (Texture texture in type.textures) {
                    if (mesh.material.mainTexture == texture) {
                        AudioClip somAux = type.sons[Random.Range(0, type.sons.Length)];
                        audioS.PlayOneShot(somAux);
                    }
                }
            }
        }
    }

    //localiza o som da textura no terreno, para aplicar aos passos do personagem
    void playTerrain()
    {
        int idDaTexturaNoTerreno = GetActiveTerrainTextureId();//recebe id da textura no terreno, com metodo criado logo abaixo

        //compara todas as texturas disponiveis até achar a certa no grupo instanciado da classe "TextureTypes" e executa som
        if (textureTypes.Length > 0)
        {
            foreach (TextureType type in textureTypes)
            {
                foreach (Texture texture in type.textures)
                {
                    if (terrainData.splatPrototypes[idDaTexturaNoTerreno].texture == texture)
                    {
                        AudioClip somAux = type.sons[Random.Range(0, type.sons.Length)];
                        audioS.PlayOneShot(somAux);
                    }

                }
            }
        }
    }


    //identifica e retorna o id da posição no terreno onde personagem pisa
    private int GetActiveTerrainTextureId() {
        int id = 0;
        Vector3 playerPosition = personagem.transform.position;
        Vector3 terrainPosition = ConvertToSplatMapCoordinate(playerPosition);

        for(int i = 0; i < numeroDeTexturas; i++) {
            if (0 < splatData[(int)terrainPosition.z, (int)terrainPosition.x, i]) {//tem retorno maior que 0 na hora que bater a textura certa
                id = i;
            }
        }
        return id;
    }


    //retorna coordenadas da posição do personagem
    private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPosition) {
        Vector3 vecRet = new Vector3();
        Terrain ter = Terrain.activeTerrain;
        Vector3 terPosition = ter.transform.position;

        vecRet.x = ((playerPosition.x - terPosition.x) / ter.terrainData.size.x)*ter.terrainData.alphamapWidth;
        vecRet.z = ((playerPosition.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;

        return vecRet;
    }

}

//essa classe armazena sons e textures de um mesmo tipo, para serem usados juntos, 
//cada vez que a mesma for instanciada representará um novo grupo de texturas e sons.
[System.Serializable]// essa linha é para classe poder ser instanciada várias vezes
public class TextureType {
    public string nomeDoGrupo;
    public Texture[] textures;
    public AudioClip[] sons;
}
