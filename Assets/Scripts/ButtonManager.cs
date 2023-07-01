using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    //シーン移動
    public void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
