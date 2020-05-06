using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballCarpmaTesti : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        basketball = GetComponent<Basketball>();
        duzlemNormal = Duzlem.transform.up;
        HizVektoru = basketball.Hiz * basketball.transform.forward;
        StartCoroutine(CarpmaTesti());
    }

    private IEnumerator CarpmaTesti()
    {
        while (t<tmax)
        {
            float timestepRemaining = h;
            float timestep = timestepRemaining;
            while (timestepRemaining>0)
            {
                var yenikonum = Integrate(transform.position, HizVektoru, timestep);
                float d = Vector3.Dot((transform.position - Duzlem.transform.position), duzlemNormal) - basketball.Cap/2;
                float dyeni= Vector3.Dot((yenikonum - Duzlem.transform.position), duzlemNormal) - basketball.Cap/2;
                if (CollisionBetween(d, dyeni))
                {

                    float f = Calculatef(d, dyeni);
                    timestep *= f;
                    yenikonum = Integrate(transform.position, HizVektoru, timestep);
                    HizVektoru = CollisionResponse(HizVektoru);
                }
                timestepRemaining -= timestep;
                transform.position = yenikonum;

            }
            n++;
            t = n * h;
            yield return new WaitForSeconds(h);
        }
    }

    private Vector3 CollisionResponse(Vector3 hizVektoru)
    {
        Vector3 NormalYonundeHiz= -Cr * (Vector3.Dot(hizVektoru, duzlemNormal)) * duzlemNormal;
        Vector3 tegethiz = hizVektoru - (Vector3.Dot(hizVektoru, duzlemNormal)) * duzlemNormal;
        return NormalYonundeHiz + tegethiz;
    }

    private bool CollisionBetween(float d, float d2)
    {
        return (d * d2) < 0;
    }

    private Vector3 Integrate(Vector3 currentKonum,Vector3 hiz,float Timestep)
    {
        Vector3 yeniKonum= currentKonum + hiz * Timestep;
        return yeniKonum;
    }
    // Update is called once per frame
    //void Update()
    //{
    //    transform.position += transform.forward * basketball.Hiz * Time.deltaTime;
    //    float d = Vector3.Dot((transform.position - Duzlem.transform.position), duzlemNormal)-basketball.Yaricap;
    //    Debug.Log(string.Format("Düzlem ile top arasındaki uzaklık: {0}", d));
    //}
    private float Calculatef(float dnow,float dnext)
    {
        return dnow / (dnow - dnext);
    }
}
