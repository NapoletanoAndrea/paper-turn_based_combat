using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugFunctions : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TakeDamage(InputField inputField)
    {
        BattleSystem.instance.GetCurrentActor().TakeDamage(int.Parse(inputField.text));
    }
}
