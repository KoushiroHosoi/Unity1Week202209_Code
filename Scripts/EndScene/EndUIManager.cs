using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace HamuGame
{
    public class EndUIManager : MonoBehaviour
    {
        [SerializeField] private EndSceneManager endSceneManager;
        
        [SerializeField] private Text totalAmountText;
        [SerializeField] private Text rankNumText;
        [SerializeField] private Text rankText;
        [SerializeField] private Text thankyouText;

        //[SerializeField] private Button rankingButton;
        [SerializeField] private Button tweetButton;
        [SerializeField] private Button reStartButton;


        void Awake()
        {
            //TotalText�̒��g�����
            totalAmountText.text = "";

            //���Text���\���ɂ���
            totalAmountText.gameObject.SetActive(false);
            rankNumText.gameObject.SetActive(false);
            rankText.gameObject.SetActive(false);
            thankyouText.gameObject.SetActive(false);

            //�C�x���g���{�^���ɒǉ�
            tweetButton.OnClickAsObservable().Subscribe(_ => endSceneManager.ShowTweet());
            reStartButton.OnClickAsObservable().Subscribe(_ => endSceneManager.LoadStartScene());

            StartCoroutine(ShowUICoroutine());
        }

        private void ChangeRankingActive()
        {
            /*
            if (explanationPanel.activeSelf)
            {
                //SePlayer.Instance.Play(1);
                explanationPanel.SetActive(false);
            }
            else
            {
                //SePlayer.Instance.Play(0);
                explanationPanel.SetActive(true);
            }
            */
        }

        private IEnumerator ShowUICoroutine()
        {
            yield return new WaitForSeconds(0.3f);

            //�X�R�A��\������
            totalAmountText.gameObject.SetActive(true);
            string resultStr = "���v���z��\n" + JapaneseNumberNotationConverter.ToJapaneseNumberNotation(MainAndEneConnecter.TotalScore) + "\n�~�ł���!";
            yield return totalAmountText.DOText(resultStr, 3).SetEase(Ease.Linear).WaitForCompletion();

            //RankNum��\������
            rankNumText.gameObject.SetActive(true);
            rankNumText.text = "�����N�� " + endSceneManager.RankNumber.ToString();

            yield return new WaitForSeconds(0.5f);

            //Rank��\������
            rankText.gameObject.SetActive(true);
            rankText.text = "���Ȃ��� " + endSceneManager.RankName + " �ł��I";

            yield return new WaitForSeconds(1f);

            //�����L���O��\��������
            endSceneManager.ShowRanking();

            //ThankYouText��\��
            thankyouText.gameObject.SetActive(true);

            yield break;
        }
    }
}
