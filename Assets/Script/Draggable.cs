using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Draggable : MonoBehaviour
{
    Vector2 difference = Vector2.zero;
    Vector2 posicaoInicial, posicaoInicialVolatil;
    // private bool podeDestruir = false;
    // private bool soltouBotao = false;
    private int errou = 0;
    private float distancia;
    private void Start() {
        posicaoInicial          = transform.position;
        posicaoInicialVolatil   = transform.position;
        //distancia               = GameController.instance.distanciaTela;
    }

    private void OnMouseUp() {//evento quando solta botao do mouse
        // soltouBotao = true;
        // if(podeDestruir){
        //     Destroy(this.GetComponent<Rigidbody2D>());
        //     Destroy(this.GetComponent<PolygonCollider2D>());
        // }
        if(CompareTag("Errado")){
            GameController.instance.erros += 1;
            errou++;
        }
        if(errou == 3){
            Destroy(this.GetComponent<SpriteRenderer>());
        }
        // transform.position = posicaoInicialVolatil;

        if(SceneManager.GetActiveScene().name.Contains("Fase3")){
            GameObject[] respawns = null;
            if( CompareTag("Correto1") )
                respawns = GameObject.FindGameObjectsWithTag("Sombra1");

            else if( CompareTag("Correto2") )
                respawns = GameObject.FindGameObjectsWithTag("Sombra2");

            else if( CompareTag("Correto3") )
                respawns = GameObject.FindGameObjectsWithTag("Sombra3");

            else if( CompareTag("Correto4") )
                respawns = GameObject.FindGameObjectsWithTag("Sombra4");

            else if( CompareTag("Correto5") )
                respawns = GameObject.FindGameObjectsWithTag("Sombra5");

            if(respawns != null && respawns.Length > 0){
                float Distance = Vector3.Distance(transform.position, respawns[0].transform.position);
                distancia      = GameController.instance.distanciaTela;

                if(Distance<distancia){
                    transform.position = respawns[0].transform.position;
                    Destroy(this.GetComponent<Rigidbody2D>());
                    Destroy(this.GetComponent<PolygonCollider2D>());
                    GameController.instance.acertos +=1;
                } else {
                    transform.position = posicaoInicialVolatil;
                }
            }else {
                transform.position = posicaoInicialVolatil;
            }
        }
    }
    private void OnMouseDown() {//evento quando segura botao do mouse
        // soltouBotao = false;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    private void OnMouseDrag() {//evento quando mouse segura e arrasta
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void OnMouseExit() {//evento quando mouse sai do collider
        
    }

    private void OnMouseEnter() {//evento quando mouse entra no collider
        //print(CompareTag("Correto"));
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(this.tag.Contains("Correto") && other.gameObject.CompareTag("LugarCorreto")){
            GameController.instance.acertos +=1;
        }
        if(this.tag.Contains("Errado") && other.gameObject.CompareTag("LugarCorreto")){
            GameController.instance.acertos -=1;
        }
        /* if( (other.gameObject.CompareTag("Sombra1") && CompareTag("Correto1")) ||
            (other.gameObject.CompareTag("Sombra2") && CompareTag("Correto2")) ||
            (other.gameObject.CompareTag("Sombra3") && CompareTag("Correto3")) ||
            (other.gameObject.CompareTag("Sombra4") && CompareTag("Correto4")) ||
            (other.gameObject.CompareTag("Sombra5") && CompareTag("Correto5")) ){

                podeDestruir = true;
                posicaoInicialVolatil = other.transform.position;
        } */
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(this.tag.Contains("Correto") && other.gameObject.CompareTag("LugarCorreto")){
            GameController.instance.acertos -=1;
        }
        if(this.tag.Contains("Errado") && other.gameObject.CompareTag("LugarCorreto")){
            GameController.instance.acertos +=1;
        }
        //foi colocado booleano pra verificar se o botao foi solto, pois o evento de soltar é mais rápido, porem pra destruir ele demora e trocava a posição aqui antes de trocar la
        /* if(!soltouBotao){
            podeDestruir = false;
            posicaoInicialVolatil = posicaoInicial;
        } */
    }
}
