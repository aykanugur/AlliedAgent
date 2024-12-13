using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverTriggerArea : MonoBehaviour
{
    public GameObject text;
    
    public void ActiveOrDeactiveText(bool input)
    {
        text.SetActive(input);
    }
}
