using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobReward : MonoBehaviour
{
    //やること
    //1.リワード広告IDの入力
    //2.GetReward関数に報酬内容を入力
    //3.リワード起動設定　ShowAdMobReward()を使う

    //広告の読み込みが完了していたらボタン表示

    public ClickManager clickManager; //インスペクター上でアタッチ

    private RewardedAd rewardedAd;//RewardedAd型の変数 rewardedAdを宣言 この中にリワード広告の情報が入る

    private string adUnitId;

    public GameObject RewardButton;

    private void Start()
    {
        //AndroidとiOSで広告IDが違うのでプラットフォームで処理を分けます。
        // 参考
        //【Unity】AndroidとiOSで処理を分ける方法
        // https://marumaro7.hatenablog.com/entry/platformsyoriwakeru

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3286497755430429/7457547256";//ここにAndroidのリワード広告IDを入力
#elif UNITY_IPHONE
        adUnitId = ca-app-pub-3286497755430429/5788979037";//ここにiOSのリワード広告IDを入力
#else
        adUnitId = "unexpected_platform";
#endif

        Debug.Log("リワード 読み込み開始");
        LoadRewardedAd();//リワード広告読み込み
    }

    //リワード広告を表示する関数
    //ボタンに割付けして使用
    public void ShowAdMobReward()
    {
        //変数rewardedAdの中身が存在しており、広告の読み込みが完了していたら広告表示
        if (rewardedAd != null && rewardedAd.CanShowAd() == true)
        {
            //リワード広告 表示を実施　報酬の受け取りの関数GetRewardを引数に設定
            rewardedAd.Show(GetReward);
        }
        else
        {
            Debug.Log("リワード広告読み込み未完了");
        }
    }

    //報酬受け取り処理
    private void GetReward(Reward reward)
    {
        Debug.Log("報酬受け取り");

        //ここに報酬の処理を書く
        //スコア2倍
        int score = 2 * clickManager.count;

        //スコアにテキスト表示
        clickManager.countText.text = "Score:" + score;

    }


    //リワード広告を読み込む関数 再読み込みにも使用
    public void LoadRewardedAd()
    {
        //広告の再読み込みのための処理
        //rewardedAdの中身が入っていた場合処理
        if (rewardedAd != null)
        {
            //リワード広告は使い捨てなので一旦破棄
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        //リクエストを生成
        AdRequest request = new AdRequest();

        //広告のキーワードを追加
        //===================================================================
        // アプリに関連するキーワードを文字列で設定するとアプリと広告の関連性が高まります。
        // 結果、収益が上がる可能性があります。
        // 任意設定のため不要であれば消していただいて問題はありません。

        // Application.systemLanguageでOSの言語判別　
        // 返り値はSystemLanguage.言語
        // 端末の言語が日本語の時
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            request.Keywords.Add("ゲーム");
            request.Keywords.Add("モバイルゲーム");
        }

        //端末の言語が日本語以外の時
        else
        {
            request.Keywords.Add("game");
            request.Keywords.Add("mobile games");
        }
        //==================================================================

        //広告をロード  その後、関数OnRewardedAdLoadedを呼び出す
        RewardedAd.Load(adUnitId, request, OnRewardedAdLoaded);

        //RewardedAd型の変数 rewardedAdの各種状態 に関数を登録
        rewardedAd.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;//rewardedAdの状態が リワード広告 表示　   となった時に起動する関数(関数名OnAdFullScreenContentOpened)を登録
        rewardedAd.OnAdFullScreenContentFailed += OnAdFullScreenContentFailed;//rewardedAdの状態が リワード広告 表示失敗　となった時に起動する関数(関数名OnAdFullScreenContentFailed)を登録
        rewardedAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;//rewardedAdの状態が リワード広告 表示終了　となった時に起動する関数(OnAdFullScreenContentClosed)を登録
    }


    // 広告のロードを実施した後に呼び出される関数
    private void OnRewardedAdLoaded(RewardedAd ad, LoadAdError error)
    {
        //変数errorに情報が入っていない、または、変数adに情報がはいっていなかったら実行
        if (error != null || ad == null)
        {
            Debug.LogError("リワード 読み込み失敗" + error);//error:エラー内容 
            return;//この時点でこの関数の実行は終了
        }

        Debug.Log("リワード 読み込み完了");

        //RewardedAd.Load(~略~)関数を実行することにより、RewardedAd型の変数adにRewardedAdのインスタンスを生成する。
        //生成したRewardedAd型のインスタンスを変数rewardedAdへ割り当て
        rewardedAd = ad;
    }



    //リワード広告が表示された時に起動する関数
    private void OnAdFullScreenContentOpened()
    {
        Debug.Log("リワード 表示完了");
    }

    //リワード広告の表示失敗 となった時に起動する関数
    private void OnAdFullScreenContentFailed(AdError error)
    {
        Debug.Log("リワード 表示失敗" + error);//error:エラー内容

        Debug.Log("リワード 再読み込み");
        LoadRewardedAd();
    }

    //リワード広告が表示終了 となった時に起動する関数
    private void OnAdFullScreenContentClosed()
    {
        Debug.Log("リワード 表示閉じる");

        Debug.Log("リワード 再読み込み");
        LoadRewardedAd();
    }
}
