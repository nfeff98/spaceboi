using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRotLock : MonoBehaviour
{
    public LayerMask LayerMask;
    private SpaceBoyController sbc;
    // Start is called before the first frame update
    void Start()
    {
        sbc = FindObjectOfType<SpaceBoyController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(this.transform.position + new Vector3(0, 10, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 12f, LayerMask)){
            this.transform.up = hit.normal;
           
        }
    }
}
