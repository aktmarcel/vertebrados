using System.Net.Mime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankController : MonoBehaviour
{
    public Animator transicaoTela;//animação de troca de tela
    public Text tabela1;//texto na tela que vai ser alterado conforme passa o tempo
    public Text tabela2;//texto na tela que vai ser alterado conforme passa o tempo
    public AudioSource audio2;//ferramente de execução de audio
    public AudioClip encimaBotao;
    public AudioClip clickBotao;

    // Start is called before the first frame update
    void Start()
    {
        carregarRank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void carregarRank(){
        string path = Application.dataPath + "/Dados/rankAlunos.txt";
        if(File.Exists(path)){
            string[] lines = System.IO.File.ReadAllLines(path);
            int b = 0;
            string pontuacoes = "";
            string pontuacoes2 = "";
            List<PessoaRank> pessoaRank = new List<PessoaRank>();
            foreach (string line in lines)
            {
                //Data: 15/09/2020 17:56:21 | Tempo para finalizar o jogo: 00:00:40 | Aluno: Brunno Marcel Ferreira Marins
                if(b > 0){
                    string pessoaLinha = line.Substring(75);
                    string tempoLinha = line.Substring(57, 8);
                    TimeSpan ts = TimeSpan.Parse(tempoLinha);
                    pessoaRank.Add(new PessoaRank{nome = pessoaLinha, tempo = ts});
                    //pontuacoes += b + " - " + tempoLinha + " - " + pessoaLinha + "\n";
                }
                b++;
            }
            for(int i = 0; i < pessoaRank.Count; i++){
                for(int j = i + 1; j < pessoaRank.Count; j++){
                    if(pessoaRank[i].tempo > pessoaRank[j].tempo){
                        PessoaRank temp = pessoaRank[i];
                        pessoaRank[i] = pessoaRank[j];
                        pessoaRank[j] = temp;
                    }

                }
                if(i < 15)
                    pontuacoes += (i+1) + " - " + pessoaRank[i].tempo + " - " + pessoaRank[i].nome + "\n";
                else
                    pontuacoes2 += (i+1) + " - " + pessoaRank[i].tempo + " - " + pessoaRank[i].nome + "\n";
            }

            if(pessoaRank.Count > 0 && pessoaRank.Count < 16){
                for (int i = pessoaRank.Count; i < 16; i++)
                {
                    pontuacoes += (i+1) + "\n";
                }
            }
            if(pessoaRank.Count > 15 && pessoaRank.Count < 26){
                for (int i = pessoaRank.Count; i < 26; i++)
                {
                    pontuacoes2 += (i+1) + "\n";
                }
            }
            if(pontuacoes != "")
                tabela1.text = pontuacoes;
            if(pontuacoes2 != "")
                tabela2.text = pontuacoes2;
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

    private class PessoaRank{
        public String nome;
        public TimeSpan tempo;

    }

}
