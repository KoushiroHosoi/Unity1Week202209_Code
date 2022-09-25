using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;
using UniRx;
using naichilab.EasySoundPlayer.Scripts;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private StartSceneManager startSceneManager;

    //�\���������p�l�����i�[
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject creditPanel;

    //On�EOff�p�̃{�^�����i�[
    [SerializeField] private Button openStartPanelButton;

    [SerializeField] private Button startButton;

    [SerializeField] private Button explanationButton;
    [SerializeField] private Button explanationCancelButton;

    [SerializeField] private Button soundButton;
    [SerializeField] private Button soundCancelButton;

    [SerializeField] private Button creditButton;
    [SerializeField] private Button creditCancelButton;

    // Start is called before the first frame update
    void Awake()
    {
        //�C�x���g��ݒ�
        openStartPanelButton.OnClickAsObservable().Subscribe(_ => OpenStartPanel());

        startButton.OnClickAsObservable().Subscribe(_ => startSceneManager.ChangeStartScene());

        explanationButton.OnClickAsObservable().Subscribe(_ => ChangeExplanationPanel());
        explanationCancelButton.OnClickAsObservable().Subscribe(_ => ChangeExplanationPanel());

        soundButton.OnClickAsObservable().Subscribe(_ => ChangeSoundPanel());
        soundCancelButton.OnClickAsObservable().Subscribe(_ => ChangeSoundPanel());

        creditButton.OnClickAsObservable().Subscribe(_ => ChangeCreditPanel());
        creditCancelButton.OnClickAsObservable().Subscribe(_ => ChangeCreditPanel());
    }

    private void Start()
    {
        //�p�l�����\���ɁI
        startPanel.SetActive(false);
        creditPanel.SetActive(false);
        soundPanel.SetActive(false);
        explanationPanel.SetActive(false);
    }

    //�X�^�[�g�p�l����\������
    private void OpenStartPanel()
    {
        startPanel.SetActive(true);
        SePlayer.Instance.Play(0);
        ChangeExplanationPanel();
    }

    //�Q�[�������p�l����\���E��\������
    private void ChangeExplanationPanel()
    {
        if (explanationPanel.activeSelf)
        {
            SePlayer.Instance.Play(1);
            explanationPanel.SetActive(false);
        }
        else
        {
            SePlayer.Instance.Play(1);
            explanationPanel.SetActive(true);
        }
    }

    //�T�E���h�p�l����\���E��\������
    private void ChangeSoundPanel()
    {
        if (soundPanel.activeSelf)
        {
            SePlayer.Instance.Play(1);
            soundPanel.SetActive(false);
        }
        else
        {
            SePlayer.Instance.Play(1);
            soundPanel.SetActive(true);
        }
    }

    //�N���W�b�g�p�p�l����\���E��\������
    private void ChangeCreditPanel()
    {
        if (creditPanel.activeSelf)
        {
            SePlayer.Instance.Play(1);
            creditPanel.SetActive(false);
        }
        else
        {
            SePlayer.Instance.Play(1);
            creditPanel.SetActive(true);
        }
    }
}
