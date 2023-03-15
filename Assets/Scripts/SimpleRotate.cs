using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 rotationVec;
    public float rotationSpeed;
    private float maxRotationSpeed;
    void Start()
    {
        maxRotationSpeed = rotationSpeed;
        rotationSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotationVec * rotationSpeed) ; 
    }


    public void SpinUp()
    {
        StopAllCoroutines();
        StartCoroutine(Spin(true));

    }

    public void SpinDown()
    {
        StopAllCoroutines();
        StartCoroutine(Spin(false));
    }

    IEnumerator Spin(bool up)
    {
        float t = 0.01f;
        while (t < 1)
        {
            t *= 1.2f;
            if (up)
                this.rotationSpeed = Mathf.Lerp(0, maxRotationSpeed, t);
            else
                this.rotationSpeed = Mathf.Lerp(maxRotationSpeed, 0, t);
            yield return new WaitForEndOfFrame();
        }
    }
}
