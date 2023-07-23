using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdMobInterstitial : MonoBehaviour
{
    //やること
    //1.インタースティシャル広告IDの入力
    //2.インタースティシャル起動設定　ShowAdMobInterstitial()を使う


    private InterstitialAd interstitialAd;//InterstitialAd型の変数interstitialAdを宣言　この中にインタースティシャル広告の情報が入る

    private string adUnitId;

    private void Start()
    {
        //AndroidとiOSで広告IDが違うのでプラットフォームで処理を分けます。
        // 参考
        //【Unity】AndroidとiOSで処理を分ける方法
        // https://marumaro7.hatenablog.com/entry/platformsyoriwakeru

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3286497755430429/3354387382";//ここにAndroidのインタースティシャル広告IDを入力          
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3286497755430429/2368608777";//ここにiOSのインタースティシャル広告IDを入力
#else
        adUnitId = "unexpected_platform";
#endif

        Debug.Log("インタースティシャル 読み込み開始");
        LoadInterstitialAd();
    }

    //インタースティシャル広告を表示する関数
    //ボタンなどに割付けして使用
    public void ShowAdMobInterstitial()
    {
        //変数interstitialの中身が存在しており、広告の読み込みが完了していたら広告表示
        if (interstitialAd != null && interstitialAd.CanShowAd() == true)
        {
            //インタースティシャル広告 表示を実施
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("インタースティシャル 読み込み未完了");
            SceneManager.LoadScene("TitleScene");

        }
    }


    //インタースティシャル広告を読み込む関数 再読み込みにも使用
    private void LoadInterstitialAd()
    {
        //広告の再読み込みのための処理
        //interstitialAdの中身が入っていた場合処理
        if (interstitialAd != null)
        {
            //インタースティシャル広告は使い捨てなので一旦破棄
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        //リクエストを生成
        AdRequest request = new AdRequest();

        //広告のキーワードを追加
        //==================================================================
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

        //広告をロード  その後、関数OnInterstitialAdLoadedを呼び出す
        InterstitialAd.Load(adUnitId, request, OnInterstitialAdLoaded);

        //InterstitialAd型の変数 interstitialAdの各種状態 に関数を登録
        interstitialAd.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;//interstitialの状態が　インタースティシャル広告 表示   　となった時に起動する関数(関数名OnAdFullScreenContentOpened)を登録
        interstitialAd.OnAdFullScreenContentFailed += OnAdFullScreenContentFailed;//interstitialの状態が　インタースティシャル広告 表示失敗  となった時に起動する関数(関数名OnAdFullScreenContentFailed)を登録
        interstitialAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;//interstitialの状態が  インタースティシャル広告 表示終了　となった時に起動する関数(OnAdFullScreenContentClosed)を登録               
    }


    // 広告のロードを実施した後に呼び出される関数
    private void OnInterstitialAdLoaded(InterstitialAd ad, LoadAdError error)
    {
        //変数errorに情報が入っていない、または、変数adに情報がはいっていなかったら実行
        if (error != null || ad == null)
        {
            Debug.LogError("インタースティシャル 読み込み失敗" + error);//error:エラー内容 
            return;//この時点でこの関数の実行は終了
        }

        Debug.Log("インタースティシャル 読み込み完了");

        //InterstitialAd.Load(~略~)関数を実行することにより、InterstitialAd型の変数adにInterstitialAdのインスタンスを生成する。
        //生成したInterstitialAd型のインスタンスを変数interstitialAdへ割り当て
        interstitialAd = ad;
    }



    //インタースティシャル広告が表示された時に起動する関数
    private void OnAdFullScreenContentOpened()
    {
        Debug.Log("インタースティシャル 表示完了");
    }

    //インタースティシャル広告の表示失敗 となった時に起動する関数
    private void OnAdFullScreenContentFailed(AdError error)
    {
        Debug.Log("インタースティシャル 表示失敗" + error);//error:エラー内容

        Debug.Log("インタースティシャル 再読み込み");
        LoadInterstitialAd();
    }


    //インタースティシャル広告が表示終了 となった時に起動する関数
    private void OnAdFullScreenContentClosed()
    {
        Debug.Log("インタースティシャル 表示閉じる");

        Debug.Log("インタースティシャル 再読み込み");
        LoadInterstitialAd();

        // シーン移動
        SceneManager.LoadScene("FreeSelectScene");

    }

}
