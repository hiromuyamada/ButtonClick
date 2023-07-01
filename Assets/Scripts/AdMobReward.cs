using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobReward : MonoBehaviour
{
    //��邱��
    //1.�����[�h�L��ID�̓���
    //2.GetReward�֐��ɕ�V���e�����
    //3.�����[�h�N���ݒ�@ShowAdMobReward()���g��

    //�L���̓ǂݍ��݂��������Ă�����{�^���\��

    public ClickManager clickManager; //�C���X�y�N�^�[��ŃA�^�b�`

    private RewardedAd rewardedAd;//RewardedAd�^�̕ϐ� rewardedAd��錾 ���̒��Ƀ����[�h�L���̏�񂪓���

    private string adUnitId;

    public GameObject RewardButton;

    private void Start()
    {
        //Android��iOS�ōL��ID���Ⴄ�̂Ńv���b�g�t�H�[���ŏ����𕪂��܂��B
        // �Q�l
        //�yUnity�zAndroid��iOS�ŏ����𕪂�����@
        // https://marumaro7.hatenablog.com/entry/platformsyoriwakeru

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3286497755430429/7457547256";//������Android�̃����[�h�L��ID�����
#elif UNITY_IPHONE
        adUnitId = ca-app-pub-3286497755430429/5788979037";//������iOS�̃����[�h�L��ID�����
#else
        adUnitId = "unexpected_platform";
#endif

        Debug.Log("�����[�h �ǂݍ��݊J�n");
        LoadRewardedAd();//�����[�h�L���ǂݍ���
    }

    //�����[�h�L����\������֐�
    //�{�^���Ɋ��t�����Ďg�p
    public void ShowAdMobReward()
    {
        //�ϐ�rewardedAd�̒��g�����݂��Ă���A�L���̓ǂݍ��݂��������Ă�����L���\��
        if (rewardedAd != null && rewardedAd.CanShowAd() == true)
        {
            //�����[�h�L�� �\�������{�@��V�̎󂯎��̊֐�GetReward�������ɐݒ�
            rewardedAd.Show(GetReward);
        }
        else
        {
            Debug.Log("�����[�h�L���ǂݍ��ݖ�����");
        }
    }

    //��V�󂯎�菈��
    private void GetReward(Reward reward)
    {
        Debug.Log("��V�󂯎��");

        //�����ɕ�V�̏���������
        //�X�R�A2�{
        int score = 2 * clickManager.count;

        //�X�R�A�Ƀe�L�X�g�\��
        clickManager.countText.text = "Score:" + score;

    }


    //�����[�h�L����ǂݍ��ފ֐� �ēǂݍ��݂ɂ��g�p
    public void LoadRewardedAd()
    {
        //�L���̍ēǂݍ��݂̂��߂̏���
        //rewardedAd�̒��g�������Ă����ꍇ����
        if (rewardedAd != null)
        {
            //�����[�h�L���͎g���̂ĂȂ̂ň�U�j��
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        //���N�G�X�g�𐶐�
        AdRequest request = new AdRequest();

        //�L���̃L�[���[�h��ǉ�
        //===================================================================
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

        //�L�������[�h  ���̌�A�֐�OnRewardedAdLoaded���Ăяo��
        RewardedAd.Load(adUnitId, request, OnRewardedAdLoaded);

        //RewardedAd�^�̕ϐ� rewardedAd�̊e���� �Ɋ֐���o�^
        rewardedAd.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;//rewardedAd�̏�Ԃ� �����[�h�L�� �\���@   �ƂȂ������ɋN������֐�(�֐���OnAdFullScreenContentOpened)��o�^
        rewardedAd.OnAdFullScreenContentFailed += OnAdFullScreenContentFailed;//rewardedAd�̏�Ԃ� �����[�h�L�� �\�����s�@�ƂȂ������ɋN������֐�(�֐���OnAdFullScreenContentFailed)��o�^
        rewardedAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;//rewardedAd�̏�Ԃ� �����[�h�L�� �\���I���@�ƂȂ������ɋN������֐�(OnAdFullScreenContentClosed)��o�^
    }


    // �L���̃��[�h�����{������ɌĂяo�����֐�
    private void OnRewardedAdLoaded(RewardedAd ad, LoadAdError error)
    {
        //�ϐ�error�ɏ�񂪓����Ă��Ȃ��A�܂��́A�ϐ�ad�ɏ�񂪂͂����Ă��Ȃ���������s
        if (error != null || ad == null)
        {
            Debug.LogError("�����[�h �ǂݍ��ݎ��s" + error);//error:�G���[���e 
            return;//���̎��_�ł��̊֐��̎��s�͏I��
        }

        Debug.Log("�����[�h �ǂݍ��݊���");

        //RewardedAd.Load(~��~)�֐������s���邱�Ƃɂ��ARewardedAd�^�̕ϐ�ad��RewardedAd�̃C���X�^���X�𐶐�����B
        //��������RewardedAd�^�̃C���X�^���X��ϐ�rewardedAd�֊��蓖��
        rewardedAd = ad;
    }



    //�����[�h�L�����\�����ꂽ���ɋN������֐�
    private void OnAdFullScreenContentOpened()
    {
        Debug.Log("�����[�h �\������");
    }

    //�����[�h�L���̕\�����s �ƂȂ������ɋN������֐�
    private void OnAdFullScreenContentFailed(AdError error)
    {
        Debug.Log("�����[�h �\�����s" + error);//error:�G���[���e

        Debug.Log("�����[�h �ēǂݍ���");
        LoadRewardedAd();
    }

    //�����[�h�L�����\���I�� �ƂȂ������ɋN������֐�
    private void OnAdFullScreenContentClosed()
    {
        Debug.Log("�����[�h �\������");

        Debug.Log("�����[�h �ēǂݍ���");
        LoadRewardedAd();
    }
}
