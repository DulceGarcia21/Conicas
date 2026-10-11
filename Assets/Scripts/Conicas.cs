using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Conicas : MonoBehaviour
{
    public TMP_Text txtConicas;
    private int conicasSeleccionadas = 0; //0-sin seleccionar, 1-recta, 2-circunferencia, 3-elipse, 4-parabola,5-hiperbola
    public Slider sl_a;
    private float a = 5;

    public Slider sl_b;
    private float b = 3;

    public Slider sl_h;
    private float h = 1;

    public Slider sl_k;
    private float k = 1;

    public Slider sl_t;
    private float t = 45;

    private int resolucion = 1000;
    public TMP_Text lbl_a, lbl_b, lbl_h, lbl_k, lbl_t;
    public Material matRec, matCircunferencia, matElipse, matParabola, matHiperbola;

    private Vector3[] posPuntos;

    public void DibujaConicas(){
        if (conicasSeleccionadas != 0){

            LineRenderer lr = GetComponent<LineRenderer>();
            lr.SetVertexCount(resolucion+1);

            a = sl_a.value;
            b = sl_b.value;
            h = sl_h.value;
            k = sl_k.value;
            t = sl_t.value;

            switch(conicasSeleccionadas){
                case 1: //Recta
                    txtConicas.text = "Recta";
                    lr.material = matRec;
                    ResetSliders();
                    lbl_a.text = "ax";
                    lbl_b.text = "ay";
                    lbl_h.text = "bx";
                    lbl_k.text = "by";
                    lbl_t.gameObject.SetActive(false);
                    sl_t.gameObject.SetActive(false);
                    posPuntos = CrearRecta(a, b, h, k, resolucion);
                    break;

                case 2: //Circunferencia
                    txtConicas.text = "Circunferencia";
                    lr.material = matCircunferencia;
                    ResetSliders();
                    lbl_a.gameObject.SetActive(false);
                    sl_a.gameObject.SetActive(false);
                    lbl_t.gameObject.SetActive(false);
                    sl_t.gameObject.SetActive(false);
                    lbl_b.text = "r";
                    posPuntos = CrearCircunferencia(b, h, k, resolucion);
                    break;

                case 3: //Elipse
                    txtConicas.text = "Elipse";
                    lr.material = matElipse;
                    ResetSliders();
                    posPuntos = CrearElipse(a, b, h, k, t, resolucion);
                    break;

                case 4: //Parabola
                    txtConicas.text = "Parabola";
                    lr.material = matParabola;
                    ResetSliders();
                    sl_a.gameObject.SetActive(false);
                    lbl_a.gameObject.SetActive(false);
                    lbl_b.text = "p";
                    posPuntos = CrearParabola(b, h, k, t, resolucion);
                    break;

                case 5: //Hiperbola
                    txtConicas.text = "Hiperbola";
                    lr.material = matHiperbola;
                    ResetSliders();
                    break;
            }
            for(int i=0;i<=resolucion;i++){
                lr.SetPosition(i, posPuntos[i]);
            }
        }
    }

    public void ResetSliders(){
        sl_a.gameObject.SetActive(true);
        sl_b.gameObject.SetActive(true);
        sl_h.gameObject.SetActive(true);
        sl_k.gameObject.SetActive(true);
        sl_t.gameObject.SetActive(true);

        lbl_a.gameObject.SetActive(true);
        lbl_b.gameObject.SetActive(true);
        lbl_h.gameObject.SetActive(true);
        lbl_k.gameObject.SetActive(true);
        lbl_t.gameObject.SetActive(true);

        lbl_a.text = "a";
        lbl_b.text = "b";
        lbl_h.text = "h";
        lbl_k.text = "k";
        lbl_t.text = "t";
        }

    public void BtnRecta(){
        conicasSeleccionadas = 1;
        DibujaConicas();
    }
    private Vector3[] CrearRecta(float ax, float ay, float bx, float by, int resolucion){
        posPuntos = new Vector3[resolucion+1];
        float dx = bx-ax;
        float dy = by-ay;
        for (int i=0;i<=resolucion;i++){
            posPuntos[i]= new Vector3(ax +dx*i/resolucion, ay + dy*i/resolucion, 0);
        }
        return posPuntos;
    }

    public void BtnCircunferencia(){
        conicasSeleccionadas = 2;
        DibujaConicas();
    }
    private Vector3[] CrearCircunferencia(float r, float h, float k, int resolucion){
        posPuntos = new Vector3[resolucion+1];
        Vector3 centro = new Vector3(h, k, 0);
        for (int i=0;i<=resolucion;i++){
            float angulo = (float)i/(float)resolucion *2*Mathf.PI;
            posPuntos[i] = new Vector3(r*Mathf.Cos(angulo),r*Mathf.Sin(angulo),0);
            posPuntos[i] += posPuntos[i]+centro;
        }
        return posPuntos;
    }

    public void BtnElipse(){
        conicasSeleccionadas = 3;
        DibujaConicas();
    }
    private Vector3[] CrearElipse(float a, float b, float h, float k,float theta, int resolucion){
        posPuntos = new Vector3[resolucion+1];
        Quaternion q = Quaternion.AngleAxis(theta, Vector3.forward);
        Vector3 centro = new Vector3(h, k, 0);
        for (int i=0;i<=resolucion;i++){
            float angulo = (float)i/(float)resolucion *2*Mathf.PI;
            posPuntos[i] = new Vector3(a*Mathf.Cos(angulo),b*Mathf.Sin(angulo),0);
            posPuntos[i] += q * posPuntos[i]+centro;
        }
        return posPuntos;
    }

    public void BtnParabola(){
        conicasSeleccionadas = 4;
        DibujaConicas();
    }
    private Vector3[] CrearParabola(float p, float h, float k,float theta, int resolucion){
        posPuntos = new Vector3[resolucion+1];
        Quaternion q = Quaternion.AngleAxis(theta, Vector3.forward);
        Vector3 vertice = new Vector3(h, k, 0);
        for (int i=0;i<=resolucion;i++){
            float angulo = (float)i/(float)resolucion *2*Mathf.PI;
            posPuntos[i] = new Vector3(i-resolucion/2,(1/4*p)*Mathf.Pow(i-(resolucion/2),2),0);
            posPuntos[i] += q * posPuntos[i]+vertice;
        }
        return posPuntos;
    }

    public void BtnHiperbola(){
        conicasSeleccionadas = 5;
        DibujaConicas();
    }
 
}
