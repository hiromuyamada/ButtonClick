using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickManager : MonoBehaviour
{
    //カウント用の変数を用意
    public int count = 0;//int型の変数　初期値＝0

    //テキスト型の変数を用意
    public Text countText;

    //float型の変数を用意する
    private float time = ButtonManager.gameTime;//初期値10

    //Text型の変数を用意する
    public Text timeText;

    //GameObject型の変数を用意する
    public GameObject panelObject;

    //bool型の変数を用意する
    private bool gameEnd = false;

    public GameObject canvas;
    public GameObject slimePrefab;
    float posX;
    float posY;

    //変数を1増やす関数を作成
    public void PushButton()
    {
        count++;
        //Debug.Log(count);

        //増えた数字をテキストで表示
        countText.text = "Score:" + count;

        //キャラ表示
        //1/25で15体発生
        if(GenerateRandom(25)){
            for(int i = 0; i<15; i++)
            {
                GenerateSlime();
                count++;
            }
        }
        GenerateSlime();

    }


    private void Start()
    {
        GameObject prefab = (GameObject)Instantiate(slimePrefab);
        prefab.transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //gameEndがfalseのときに処理
        if(gameEnd == false)
        {
            //timeが0以下のとき
            if (time <= 0)
            {
                //テキストにカウントダウンの数値を表示する
                timeText.text = "Time:0.00";

                //GameObjectを表示する
                panelObject.SetActive(true);

                //gameEndをtrueへ変更
                gameEnd = true;


                /*GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("slime");
                foreach (GameObject obj in tagobjs)
                {
                    Destroy(obj);
                }*/
            }
            else
            {
                //カウントダウンさせる
                time -= Time.deltaTime;
                //Debug.Log(time);

                //テキストにカウントダウンの数値を表示する
                timeText.text = "Time:" + time.ToString("f2");
            }
        } 
    }

    //シーン移動
    public void ChangeScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void GenerateSlime()
    {
        setPosition();
        GameObject obj = (GameObject)Resources.Load("slime");
        GameObject slime = Instantiate(obj, new Vector3(posX, posY, 1.0f), Quaternion.identity);
        slime.transform.SetParent(canvas.transform);
        if (GenerateRandom(15))
        {
            slime.transform.localScale = new Vector3(60, 60, 0);

        }
        else
        {
            slime.transform.localScale = new Vector3(20, 20, 0);

        }
    }

    void setPosition()
    {
        posX = GenerateRandom(100, 900);
        posY = GenerateRandom(100, 1700);
    }

    int GenerateRandom(int min, int max)
    {
        System.Random rand = new System.Random();
        int r = rand.Next(min, max);
        return (int)(float)r;
    }

    /**
     * percent分の1でtrueを返す
     */
    bool GenerateRandom(int percent)
    {
        System.Random rand = new System.Random();
        int r = rand.Next(percent);
        if (r == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
