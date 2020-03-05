using UnityEngine;
using System.Collections;

public class MakeTransparent : MonoBehaviour
{


    void Start()
    {

        System.DateTime CurrentDate = new System.DateTime();
        CurrentDate = System.DateTime.Now;

        if (CurrentDate.Second > 20)
            transform.localScale = new Vector3(0f, 0f, 0f);
        else transform.localScale = new Vector3(.7f, .7f, 1f);
    }
}