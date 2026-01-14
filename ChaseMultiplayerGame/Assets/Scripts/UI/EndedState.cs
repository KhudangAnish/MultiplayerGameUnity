using TMPro;
using UnityEngine;

public class EndedState : MonoBehaviour
{
    [SerializeField] private GameObject endedUI;
    [SerializeField] private TMP_Text endedText;
    private bool runOnce = false;

    public void UpdateState()
    {
        if(runOnce is false)
        {
            runOnce = true;
            endedUI.SetActive(true);
            if (GameManager.Instance.allIsInfected.Value)
            {
                endedText.text = "EVERYONE GOT INFECTED - Zombies won!";
            }
            else
            {
                endedText.text = "SOME PEOPLE MANAGED TO ESCAPE - Humans won!";
            }
        }
    }
}
