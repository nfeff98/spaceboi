using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{

    private SpaceBoyController sbc;
    // Start is called before the first frame update
    void Start()
    {
        sbc = this.GetComponentInParent<SpaceBoyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitObj()
    {
        sbc.InteractWithResource();
    }
}
