using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CommandWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private GameObject content;

    private List<string> commands = new List<string>(); //Commands or text


    public void AddText(string text)
    {
        commands.Add(text);
        if(commands.Count > 10)
        {
            commands.RemoveAt(0);
        }
        DrawWindow();
    }
    public void DrawWindow()
    {
        //Clear current text
       // content.

       //i    i     i 
       //a     a    a ei

        //while(content.transform.GetChild(0) != null)
        //{
        //    Destroy(content.transform.GetChild(0).transform);
        //}

        foreach(string command in commands)
        {

        }    
    }
}
