using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    [SerializeField] InputField dataInput;
    [SerializeField] string []dataName;
    [SerializeField] Text foundText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Found()
    {
        if (dataInput.text == dataName[0])
        {
            foundText.text = "[" + "<color=green>" + dataInput.text + "</color>" + "]" + " Is Found.";
        }
        else if (dataInput.text == dataName[1])
        {
            foundText.text = "[" +"<color=green>" + dataInput.text + "</color>" + "]" + " Is Found.";
        }
        else if (dataInput.text == dataName[2])
        {
            foundText.text = "[" + "<color=green>" + dataInput.text + "</color>" + "]" + " Is Found.";
        }
        else if (dataInput.text == dataName[3])
        {
            foundText.text = "[" + "<color=green>" + dataInput.text + "</color>" + "]" + " Is Found.";
        }
        else if (dataInput.text == dataName[4])
        {
            foundText.text = "[" + "<color=green>" + dataInput.text + "</color>" + "]" + " Is Found.";
        }
        else
        {
            foundText.text = "[" + "<color=red>" + dataInput.text + "</color>" + "]" + " Is Not Found.";
        }
    }

}
