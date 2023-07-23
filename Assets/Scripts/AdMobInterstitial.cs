using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdMobInterstitial : MonoBehaviour
{
    //��邱��
    //1.�C���^�[�X�e�B�V�����L��ID�̓���
    //2.�C���^�[�X�e�B�V�����N���ݒ�@ShowAdMobInterstitial()���g��


    private InterstitialAd interstitialAd;//InterstitialAd�^�̕ϐ�interstitialAd��錾�@���̒��ɃC���^�[�X�e�B�V�����L���̏�񂪓���

    private string adUnitId;

    private void Start()
    {
        //Android��iOS�ōL��ID���Ⴄ�̂Ńv���b�g�t�H�[���ŏ����𕪂��܂��B
        // �Q�l
        //�yUnity�zAndroid��iOS�ŏ����𕪂�����@
        // https://marumaro7.hatenablog.com/entry/platformsyoriwakeru

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3286497755430429/3354387382";//������Android�̃C���^�[�X�e�B�V�����L��ID�����          
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3286497755430429/2368608777";//������iOS�̃C���^�[�X�e�B�V�����L��ID�����
#else
        adUnitId = "unexpected_platform";
#endif

        Debug.Log("�C���^�[�X�e�B�V���� �ǂݍ��݊J�n");
        LoadInterstitialAd();
    }

    //�C���^�[�X�e�B�V�����L����\������֐�
    //�{�^���ȂǂɊ��t�����Ďg�p
    public void ShowAdMobInterstitial()
    {
        //�ϐ�interstitial�̒��g�����݂��Ă���A�L���̓ǂݍ��݂��������Ă�����L���\��
        if (interstitialAd != null && interstitialAd.CanShowAd() == true)
        {
            //�C���^�[�X�e�B�V�����L�� �\�������{
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("�C���^�[�X�e�B�V���� �ǂݍ��ݖ�����");
            SceneManager.LoadScene("TitleScene");

        }
    }


    //�C���^�[�X�e�B�V�����L����ǂݍ��ފ֐� �ēǂݍ��݂ɂ��g�p
    private void LoadInterstitialAd()
    {
        //�L���̍ēǂݍ��݂̂��߂̏���
        //interstitialAd�̒��g�������Ă����ꍇ����
        if (interstitialAd != null)
        {
            //�C���^�[�X�e�B�V�����L���͎g���̂ĂȂ̂ň�U�j��
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        //���N�G�X�g�𐶐�
        AdRequest request = new AdRequest();

        //�L���̃L�[���[�h��ǉ�
        //==================================================================
        // �A�v���Ɋ֘A����L�[���[�h�𕶎���Őݒ肷��ƃA�v���ƍL���̊֘A�������܂�܂��B
        // ���ʁA���v���オ��\��������܂��B
        // �C�Ӑݒ�̂��ߕs�v�ł���Ώ����Ă��������Ė��͂���܂���B

        // Application.systemLanguage��OS�̌��ꔻ�ʁ@
        // �Ԃ�l��SystemLanguage.����
        // �[���̌��ꂪ���{��̎�
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            request.Keywords.Add("�Q�[��");
            request.Keywords.Add("���o�C���Q�[��");
        }

        //�[���̌��ꂪ���{��ȊO�̎�
        else
        {
            request.Keywords.Add("game");
            request.Keywords.Add("mobile games");
        }
        //==================================================================

        //�L�������[�h  ���̌�A�֐�OnInterstitialAdLoaded���Ăяo��
        InterstitialAd.Load(adUnitId, request, OnInterstitialAdLoaded);

        //InterstitialAd�^�̕ϐ� interstitialAd�̊e���� �Ɋ֐���o�^
        interstitialAd.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;//interstitial�̏�Ԃ��@�C���^�[�X�e�B�V�����L�� �\��   �@�ƂȂ������ɋN������֐�(�֐���OnAdFullScreenContentOpened)��o�^
        interstitialAd.OnAdFullScreenContentFailed += OnAdFullScreenContentFailed;//interstitial�̏�Ԃ��@�C���^�[�X�e�B�V�����L�� �\�����s  �ƂȂ������ɋN������֐�(�֐���OnAdFullScreenContentFailed)��o�^
        interstitialAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;//interstitial�̏�Ԃ�  �C���^�[�X�e�B�V�����L�� �\���I���@�ƂȂ������ɋN������֐�(OnAdFullScreenContentClosed)��o�^               
    }


    // �L���̃��[�h�����{������ɌĂяo�����֐�
    private void OnInterstitialAdLoaded(InterstitialAd ad, LoadAdError error)
    {
        //�ϐ�error�ɏ�񂪓����Ă��Ȃ��A�܂��́A�ϐ�ad�ɏ�񂪂͂����Ă��Ȃ���������s
        if (error != null || ad == null)
        {
            Debug.LogError("�C���^�[�X�e�B�V���� �ǂݍ��ݎ��s" + error);//error:�G���[���e 
            return;//���̎��_�ł��̊֐��̎��s�͏I��
        }

        Debug.Log("�C���^�[�X�e�B�V���� �ǂݍ��݊���");

        //InterstitialAd.Load(~��~)�֐������s���邱�Ƃɂ��AInterstitialAd�^�̕ϐ�ad��InterstitialAd�̃C���X�^���X�𐶐�����B
        //��������InterstitialAd�^�̃C���X�^���X��ϐ�interstitialAd�֊��蓖��
        interstitialAd = ad;
    }



    //�C���^�[�X�e�B�V�����L�����\�����ꂽ���ɋN������֐�
    private void OnAdFullScreenContentOpened()
    {
        Debug.Log("�C���^�[�X�e�B�V���� �\������");
    }

    //�C���^�[�X�e�B�V�����L���̕\�����s �ƂȂ������ɋN������֐�
    private void OnAdFullScreenContentFailed(AdError error)
    {
        Debug.Log("�C���^�[�X�e�B�V���� �\�����s" + error);//error:�G���[���e

        Debug.Log("�C���^�[�X�e�B�V���� �ēǂݍ���");
        LoadInterstitialAd();
    }


    //�C���^�[�X�e�B�V�����L�����\���I�� �ƂȂ������ɋN������֐�
    private void OnAdFullScreenContentClosed()
    {
        Debug.Log("�C���^�[�X�e�B�V���� �\������");

        Debug.Log("�C���^�[�X�e�B�V���� �ēǂݍ���");
        LoadInterstitialAd();

        // �V�[���ړ�
        SceneManager.LoadScene("FreeSelectScene");

    }

}
