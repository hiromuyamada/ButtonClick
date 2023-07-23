using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class slime : MonoBehaviour
{

    float transX;
    float transY;
    
    // Start is called before the first frame update
    void Start()
    {
        setPosition();
        if(transX == 0.0f || transY == 0.0f)
        {
            transX = 0.5f;
            transY = 0.5F;
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(transX, transY, 0);
        Vector3 pos = transform.localPosition;
        if(pos.x > 520 || pos.x < 10)
        {
            transX = -(transX);
        }
        if(pos.y > 910 || pos.y < -800)
        {
            transY = -(transY);
        }
    }
    
    /**
     * percent•ª‚Ì1‚Åtrue‚ð•Ô‚·
     */
    bool GenerateRandom(int percent)
    {
        System.Random rand = new System.Random();
        int r = rand.Next(percent);
        if(r == 1)
        {
            return true;
        }
        return false;
    }

    void setPosition()
    {
        transX = UnityEngine.Random.Range(-1.5f, 1.5f);
        transY = UnityEngine.Random.Range(-1.5f, 1.5f);
    }
}
