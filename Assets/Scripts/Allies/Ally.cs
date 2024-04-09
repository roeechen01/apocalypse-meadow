using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : AiEntity
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        goodEntity = true;
        EntityStart();
    }

    private void Update()
    {
       
    }
}
