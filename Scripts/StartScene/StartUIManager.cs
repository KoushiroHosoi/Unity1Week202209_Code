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

    //表示したいパネルを格納
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject creditPanel;

    //On・Off用のボタンを格納
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
        //イベントを設定
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
        //パネルを非表示に！
        startPanel.SetActive(false);
        creditPanel.SetActive(false);
        soundPanel.SetActive(false);
        explanationPanel.SetActive(false);
    }

    //スタートパネルを表示する
    private void OpenStartPanel()
    {
        startPanel.SetActive(true);
        SePlayer.Instance.Play(0);
        ChangeExplanationPanel();
    }

    //ゲーム説明パネルを表示・非表示する
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

    //サウンドパネルを表示・非表示する
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

    //クレジット用パネルを表示・非表示する
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
