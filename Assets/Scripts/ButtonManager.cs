using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public static float gameTime = 10;

    //シーン移動
    public void VsChangeScene()
    {
        SceneManager.LoadScene("VsSelectScene");
    }

    public void FreeChangeScene()
    {
        SceneManager.LoadScene("FreeSelectScene");
    }

    public void TitleChangeScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void FreeMainChangeScene(float time)
    {
        gameTime = time;
        SceneManager.LoadScene("FreeMainScene");
    }
}
