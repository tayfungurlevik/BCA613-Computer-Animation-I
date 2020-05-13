using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BasketballCarpmaTesti2 : MonoBehaviour
{
    [SerializeField]
    private GameObject Duzlem;
    [SerializeField]
    [Tooltip("Timestep")]
    private float h = 0.01f;
    [SerializeField]
    [Tooltip("Tmax: Simulasyon uzunluğu")]
    private float tmax;
    [SerializeField]
    [Tooltip("Esneklik katsayısı")]
    private float Cr = 0.8163f;
    private int n = 0;
    private float t = 0;
    private Basketball basketball;
    Vector3 duzlemNormal;

    private Vector3 HizVektoru;
    private Vector3[] PoligonVertices;
    // Start is called before the first frame update
    void Start()
    {
        basketball = GetComponent<Basketball>();
        duzlemNormal = Duzlem.transform.forward*-1;
        HizVektoru = basketball.Hiz * basketball.transform.forward;
        PoligonVertices=Duzlem.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < PoligonVertices.Length; i++)
        {
            PoligonVertices[i].x *= Duzlem.transform.localScale.x;
            PoligonVertices[i].y *= Duzlem.transform.localScale.y;
            PoligonVertices[i].z *= Duzlem.transform.localScale.z;
        }
        StartCoroutine(CarpmaTesti());
    }

    private IEnumerator CarpmaTesti()
    {
        while (t < tmax)
        {
            float timestepRemaining = h;
            float timestep = timestepRemaining;
            while (timestepRemaining > 0)
            {
                var yenikonum = Integrate(transform.position, HizVektoru, timestep);
                float d = Vector3.Dot((transform.position - Duzlem.transform.position), duzlemNormal) - basketball.Cap / 2;
                float dyeni = Vector3.Dot((yenikonum - Duzlem.transform.position), duzlemNormal) - basketball.Cap / 2;
                if (CollisionBetween(d, dyeni))
                {

                    float f = Calculatef(d, dyeni);
                    timestep *= f;
                    yenikonum = Integrate(transform.position, HizVektoru, timestep);
                    if (PoligonIcindemi(PoligonVertices, yenikonum, duzlemNormal)) 
                    {
                        HizVektoru = CollisionResponse(HizVektoru);
                    }
                    
                }
                timestepRemaining -= timestep;
                transform.position = yenikonum;

            }
            n++;
            t = n * h;
            yield return new WaitForSeconds(h);
        }
        if (t == tmax)
        {
            StopCoroutine(CarpmaTesti());
        }
    }

    private Vector3 CollisionResponse(Vector3 hizVektoru)
    {
        Vector3 NormalYonundeHiz = -Cr * (Vector3.Dot(hizVektoru, duzlemNormal)) * duzlemNormal;
        Vector3 tegethiz = hizVektoru - (Vector3.Dot(hizVektoru, duzlemNormal)) * duzlemNormal;
        return NormalYonundeHiz + tegethiz;
    }

    private bool CollisionBetween(float d, float d2)
    {
        return (d * d2) < 0;
    }

    private Vector3 Integrate(Vector3 currentKonum, Vector3 hiz, float Timestep)
    {
        Vector3 yeniKonum = currentKonum + hiz * Timestep;
        return yeniKonum;
    }
   
    private float Calculatef(float dnow, float dnext)
    {
        return dnow / (dnow - dnext);
    }
    private bool PoligonIcindemi(Vector3[] vertices,Vector3 xhit,Vector3 PoligonNormal)
    {
        Vector2[] vertices2D;
        Vector2 projectedXhit;
        (vertices2D,projectedXhit) = IkıBoyutluDuzlemeDusur(vertices, PoligonNormal,xhit);
        Vector2[] kenarlar = new Vector2[vertices.Length];
        kenarlar[0] = vertices[1] - vertices[0];
        kenarlar[1] = vertices[3] - vertices[1];
        kenarlar[2] = vertices[2] - vertices[3];
        kenarlar[3] = vertices[0] - vertices[2];
        Vector2[] vectorsFromCorners = new Vector2[4];
        for (int i = 0; i < kenarlar.Length; i++)
        {
            vectorsFromCorners[i] = projectedXhit - kenarlar[i];
        }
        float[] z = new float[4];
        for (int i = 0; i < z.Length; i++)
        {
            z[i] = Vector3.Cross(kenarlar[i], vectorsFromCorners[i]).z;
        }
        float carpim = 0;
        bool isaretDegisti = false;
        for (int i = 0; i < z.Length; i++)
        {
           
            if (i > 0 && !isaretDegisti)
            {
                carpim = z[i] * z[i-1];
                if (carpim < 0)
                {

                    isaretDegisti = true;
                }

            }

        }
        return !isaretDegisti;
    }

    private (Vector2[],Vector2) IkıBoyutluDuzlemeDusur(Vector3[] vertices, Vector3 poligonNormal, Vector3 xhit)
    {
        Vector2[] Vectorsof2D = new Vector2[vertices.Length];
        Vector2 xhit2d;
        var normalizedNormal = poligonNormal.normalized;
        var max = Mathf.Max(Mathf.Abs(normalizedNormal.x), Mathf.Abs(normalizedNormal.y), Mathf.Abs(normalizedNormal.z));
        if (max== Mathf.Abs(normalizedNormal.x))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vectorsof2D[i] = new Vector2(vertices[i].y, vertices[i].z);
            }
            xhit2d = new Vector2(xhit.y, xhit.z);
        }
        else if (max == Mathf.Abs(normalizedNormal.y))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vectorsof2D[i] = new Vector2(vertices[i].z, vertices[i].x);
            }
            xhit2d = new Vector2(xhit.z, xhit.x);
        }
        else
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vectorsof2D[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            xhit2d = new Vector2(xhit.x, xhit.y);
        }
        return (Vectorsof2D,xhit2d);
    }
}
