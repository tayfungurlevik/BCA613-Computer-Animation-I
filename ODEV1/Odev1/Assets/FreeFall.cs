using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour
{
    [Tooltip("İlk Hız(y ekseninde)")]
    [SerializeField]
    private float V0 = 0f;
    [Tooltip("İlk konum(y ekseninde)")]
    [SerializeField]
    private float x0 = 100f;
    [Tooltip("Simulasyon maksimum zamanı")]
    [SerializeField]
    private float t_max = 5f;
    [Tooltip("Zaman adımı(timestep)")]
    [SerializeField]
    private float h = 0.01f;
    [Tooltip("ivme")]
    [SerializeField]
    private float a = -10;
    private float t = 0;
    private int n = 0;

    private void Start()
    {
        //topu baslangicta belirlenen konum noktasına taşıyorum
        transform.position = new Vector3(0, x0, 0);
        //Her h kadar saniyede FreeFallingBall metodunu çalıştırıyorum
        StartCoroutine(FreeFallingBall(h));
    }
    private IEnumerator FreeFallingBall(float timeStep)
    {
        while (t < t_max)
        {
            float Vnew = V0 + a * timeStep;
            float xnew = x0 + V0 * timeStep;
            //Topun konumunu güncelliyorum
            transform.position = new Vector3(0, xnew, 0);
            n++;
            t = n * timeStep;
            V0 = Vnew;
            x0 = xnew;
            yield return new WaitForSeconds(timeStep);
        }

    }
}
