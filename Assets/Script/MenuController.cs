using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Animator animcaoMenu;//animação de menu
    public Animator transicaoTela;//animação de troca de tela
    public AudioSource audio;//ferramente de execução de audio
    public AudioSource audio2;//ferramente de execução de audio
    public AudioClip correntes;//audio da corrente;
    public AudioClip encimaBotao;
    public AudioClip clickBotao;
    public GameObject botoesPrincipais;
    public GameObject botoesFases;
    public GameObject botoesOpcoes;
    public Slider sliderVolume;


    // Start is called before the first frame update
    void Start()
    {
        // audio.clip = correntes;
        // audio.Play();
        animcaoMenu.SetBool("MenuStart", true);
        sliderVolume.value = MusicPlayer.musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadLevel(string tela){
        transicaoTela.SetTrigger("Start");//dispara a trigger para escurecer a tela 

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(tela);//carrega a tela
    }

    public void ComecarJogo(){
        GameController.menuFases = false;
        audio.Play();
        animcaoMenu.SetBool("MenuStart", false);
        StartCoroutine(LoadLevel("Intro1_Peixes"));//volta ao inicio
    }

    public void VerRank(){
        audio.Play();
        animcaoMenu.SetBool("MenuStart", false);
        StartCoroutine(LoadLevel("Rank"));//volta ao inicio
    }

    public void VerRelatório(){
        audio.Play();
        animcaoMenu.SetBool("MenuStart", false);
        StartCoroutine(LoadLevel("Relatorio"));//volta ao inicio
    }

    public void sairDoJogo(){
        Application.Quit();
    }

    public void encimaBotaoSom(){
        audio2.clip = encimaBotao;
        audio2.Play();
    }

    public void clickBotaoSom(){
        audio2.clip = clickBotao;
        audio2.Play();
    }

    public void voltarBotoesPrincipais(){
        botoesFases.SetActive(false);
        botoesOpcoes.SetActive(false);
        botoesPrincipais.SetActive(true);
    }

    public void carregarBotoesFases(){
        botoesPrincipais.SetActive(false);
        botoesFases.SetActive(true);
    }

    public void carregarBotoesOpcoes(){
        botoesPrincipais.SetActive(false);
        botoesOpcoes.SetActive(true);
    }

    public void ComecarPeixes(){
        comecarFase("Intro1_Peixes");
    }
    public void ComecarAnfibios(){
        comecarFase("Intro1_Anfibios");
    }
    public void ComecarRepteis(){
        comecarFase("Intro1_Repteis");
    }
    public void ComecarAves(){
        comecarFase("Intro1_Aves");
    }
    public void ComecarMamiferos(){
        comecarFase("Intro1_Mamiferos");
    }
    public void ComecarVertebrados(){
        comecarFase("Intro_Vertebrados");
    }
    public void ComecarComoJogar(){
        comecarFase("Intro_ComoJogar");
    }

    public void VerContato(){
        comecarFase("Intro_Desenvolvedores");
    }

    public void comecarFase(String fase){
        GameController.menuFases = true;
        audio.Play();
        animcaoMenu.SetBool("MenuStart", false);
        StartCoroutine(LoadLevel(fase));
    }

    public void SetVolume(float vol){
        // MusicPlayer.instance.SetVolume(vol);
        MusicPlayer.musicVolume = vol;
    }
}
