using System.Net.Mime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RelatorioController : MonoBehaviour
{
    public Animator transicaoTela;//animação de troca de tela
    public Text tabela1;//texto na tela que vai ser alterado conforme passa o tempo
    public AudioSource audio2;//ferramente de execução de audio
    public AudioClip encimaBotao;
    public AudioClip clickBotao;
    public Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        carregarRelatorio();
        scrollbar.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void carregarRelatorio(){
        print("carregarRelatorio");
        string path = Application.dataPath + "/Dados/relatorio.txt";
        if(File.Exists(path)){
            string[] lines = System.IO.File.ReadAllLines(path);
            int linha=0;
            foreach (string line in lines)
            {
                if(linha>1)
                    tabela1.text += line+"\n";
                linha++;
            }
            if(linha==0)
                tabela1.text = "Nenhum registro.";
        }
    }

    IEnumerator LoadLevel(string tela){
        transicaoTela.SetTrigger("Start");//dispara a trigger para escurecer a tela 

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(tela);//carrega a tela
    }

    public void voltarMenuInicial(){
        StartCoroutine(LoadLevel("MenuInicial"));//volta ao inicio
    }

    public void encimaBotaoSom(){
        audio2.clip = encimaBotao;
        audio2.Play();
    }

    public void clickBotaoSom(){
        audio2.clip = clickBotao;
        audio2.Play();
    }

    private class PessoaRelatorio{
        public String nome;
        public TimeSpan tempo;

    }

}
