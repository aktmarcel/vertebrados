using System.Net.Mime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text tempoTotalTexto;//texto na tela que vai ser alterado conforme passa o tempo
    private static float tempo;//tempo passado 
    private static float velocidade = 1;//velocidade do tempo
    public static bool menuFases;//velocidade do tempo
    private bool correrTempo;//flag para identificar se é para parar o tempo ou continuar
    public static GameController instance;//metodologia para deixar essa classe estatica em todas as telas
    public int qtdAnimaisCorretos;//total de animais corretos na fase
    public int acertos = 0;//quantidade de acertos de animais
    public int erros = 0;//quantidade de erros de animais
    private static int errosPeixes = 0, errosAnfibios = 0, errosRepteis = 0, errosAves = 0, errosMamiferos = 0;
    private static String faseAnterior = null;
    public string proximaTela;//o nome da proxima tela
    public Animator transicaoTela;//animação de troca de tela
    public Animator menu;//animação de menu
    public float distanciaTela;//distancia que permite encaixar o animal com a sombra
    public InputField inputNome;
    public Button botaoFinalizar;
    public float tempoAparecerContinue = 2f;
    public GameObject botaoContinue;
    public AudioSource audioExplica;

    // Start is called before the first frame update
    void Start(){
        audioExplica.volume = 1f;
        instance = this;//instancia essa classe para ser estatica para todas as telas

        if(SceneManager.GetActiveScene().name == "Fase1_Peixes"){
            tempo = 0;
        }

        if(SceneManager.GetActiveScene().name.Contains("Fase")){
            correrTempo = true;//libera o para correr o tempo
            // if(Mathf.Approximately(Camera.main.aspect, 1.249211f))//5.4 = 1.249211 1.249211
            //     distanciaTela = 50.32F;
            // if(Mathf.Approximately(Camera.main.aspect, 1.332808f))//4.3 = 1.332808
            //     distanciaTela = 49.0085F;
            // if(Mathf.Approximately(Camera.main.aspect, 1.5f))//3.2 = 1.5
            //     distanciaTela = 90.0009f;
            // if(Mathf.Approximately(Camera.main.aspect, 1.599369f))//16.10 = 1.599369
            //     distanciaTela = 90.0007f;
            // if(Mathf.Approximately(Camera.main.aspect, 1.777603f))//16.9 = 1.777603
                distanciaTela = 90.0015f;//distancia entre animal e sombra permitida para encaixar
        }
    }

    void Update() {
        // print(SceneManager.GetActiveScene().name.Contains("Intro"));
        if(SceneManager.GetActiveScene().name.Contains("Fase")){
            TempoCorrido();//vai ficar alterando o tempo
            VerificaAcertos();//fica verificando os acertos
            if(SceneManager.GetActiveScene().name == "Fase3_Mamiferos")
                liberarBotao();//fica verificando se o input tem 3 letar para liberar o bota
        } else if (SceneManager.GetActiveScene().name.Contains("Intro")) {
            StartCoroutine( mostrarContinue() );
            if(Input.GetKeyDown(KeyCode.Return)){
                if(menuFases && (SceneManager.GetActiveScene().name == "Intro_ComoJogar" || SceneManager.GetActiveScene().name == "Intro_Vertebrados" || SceneManager.GetActiveScene().name == "Intro_Desenvolvedores"))
                    voltarMenuInicial();
                else {
                    faseAnterior = SceneManager.GetActiveScene().name;
                    carregarProximaTela();//carrega a proxima tela
                }
            }
        }
    }
    IEnumerator mostrarContinue(){
        yield return new WaitForSeconds(tempoAparecerContinue);
        botaoContinue.SetActive(true);
    }   

    private void TempoCorrido(){
        if(correrTempo){
            tempo += Time.deltaTime * velocidade;
            string horas    = Math.Floor( (tempo % 216000) / 3600 ).ToString("00");
            string minutos  = Math.Floor( (tempo % 3600) / 60 ).ToString("00");
            string segundos = (tempo % 60).ToString("00");
            tempoTotalTexto.text = horas +":"+ minutos +":"+ segundos;//altera o texto cronometro da tela
        }
    }

    public void voltarTela(){
        correrTempo = false;//parar o tempo até carregar a outra tela
        StartCoroutine(LoadLevel(faseAnterior));
    }

    private void VerificaAcertos(){
        if(qtdAnimaisCorretos == acertos){
            acertos = 0;//0 para não entrar na função mais a cada update
            correrTempo = false;//parar o tempo até carregar a outra tela
            if(!menuFases){
                if(SceneManager.GetActiveScene().name == "Fase3_Peixes"){
                    errosPeixes = erros;
                    print(errosPeixes);
                }
                if(SceneManager.GetActiveScene().name == "Fase3_Anfibios"){
                    errosAnfibios = erros;
                    print(errosAnfibios);
                }
                if(SceneManager.GetActiveScene().name == "Fase3_Repteis"){
                    errosRepteis = erros;
                    print(errosRepteis);
                }
                if(SceneManager.GetActiveScene().name == "Fase3_Aves"){
                    errosAves = erros;
                    print(errosAves);
                }
                if(SceneManager.GetActiveScene().name == "Fase3_Mamiferos"){
                    errosMamiferos = erros;
                    print(errosMamiferos);
                }

                if(SceneManager.GetActiveScene().name == "Fase3_Mamiferos"){//se for a ultima tela, cria relatorio
                    menu.SetBool("MenuStart", true);//dispara a trigger para fazer a animação de descer o menu de finalização
                } else{
                    carregarProximaTela();//carrega a proxima tela
                }
            } else {
                if(!SceneManager.GetActiveScene().name.Contains("Fase3"))
                    carregarProximaTela();//carrega a proxima tela
                else
                    voltarMenuInicial();
            }
        }
    }

    public void carregarProximaTela(){
        StartCoroutine(LoadLevel(proximaTela));
    }

    IEnumerator LoadLevel(string tela){
        transicaoTela.SetTrigger("Start");//dispara a trigger para escurecer a tela 

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(tela);//carrega a tela
    }

    public void voltarMenuInicial(){
        StartCoroutine(LoadLevel("MenuInicial"));//volta ao inicio
    }

    public void finalizarJogo(){// metodo para o botao de finalizar o jogo
        relatorio();
        carregarProximaTela();
    }
    public void liberarBotao(){//metodo para verificar se ao menos foi digitado 3 letras no input pra liberar o botao e finalizar o jogo
        if(inputNome.text.Length >= 3){
            botaoFinalizar.interactable = true;            
        } else {
            botaoFinalizar.interactable = false;
        }
    }

    private void relatorio(){//cria o relatorio
        String folder = Application.dataPath + "/Dados";
        string path = Application.dataPath + "/Dados/rankAlunos.txt";
        string pathRelatorio = Application.dataPath + "/Dados/relatorio.txt";
        if(!Directory.Exists(folder)){
            Directory.CreateDirectory(folder);
        }
        
        //================================================================================
        //cria rank
        if(!File.Exists(path)){
            File.WriteAllText(path, "Rank\n");
        }
        string linhas = "Data: " + System.DateTime.Now + " | Tempo para finalizar o jogo: "+ tempoTotalTexto.text + " | Aluno: "+ inputNome.text +"\n";
        File.AppendAllText(path, linhas);

        //================================================================================
        //cria relatorio
        if(!File.Exists(pathRelatorio)){
            File.WriteAllText(pathRelatorio, "Relatório\n\n");
        }
        string linhasRelatorio = "Aluno: "+ inputNome.text +"\n";
        linhasRelatorio += "Numero de tentativas erradas na fase 3 dos Peixes: "+ errosPeixes +"\n";
        linhasRelatorio += "Numero de tentativas erradas na fase 3 dos Anfibios: "+ errosAnfibios +"\n";
        linhasRelatorio += "Numero de tentativas erradas na fase 3 dos Répteis: "+ errosRepteis +"\n";
        linhasRelatorio += "Numero de tentativas erradas na fase 3 das Aves: "+ errosAves +"\n";
        linhasRelatorio += "Numero de tentativas erradas na fase 3 dos Mamíferos: "+ errosMamiferos +"\n";
        linhasRelatorio += "--------------------------------\n";
        File.AppendAllText(pathRelatorio, linhasRelatorio);
    }
}
