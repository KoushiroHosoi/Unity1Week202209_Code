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
            //TotalTextの中身を空に
            totalAmountText.text = "";

            //一回Textを非表示にする
            totalAmountText.gameObject.SetActive(false);
            rankNumText.gameObject.SetActive(false);
            rankText.gameObject.SetActive(false);
            thankyouText.gameObject.SetActive(false);

            //イベントをボタンに追加
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

            //スコアを表示する
            totalAmountText.gameObject.SetActive(true);
            string resultStr = "合計金額は\n" + JapaneseNumberNotationConverter.ToJapaneseNumberNotation(MainAndEneConnecter.TotalScore) + "\n円でした!";
            yield return totalAmountText.DOText(resultStr, 3).SetEase(Ease.Linear).WaitForCompletion();

            //RankNumを表示する
            rankNumText.gameObject.SetActive(true);
            rankNumText.text = "ランクは " + endSceneManager.RankNumber.ToString();

            yield return new WaitForSeconds(0.5f);

            //Rankを表示する
            rankText.gameObject.SetActive(true);
            rankText.text = "あなたは " + endSceneManager.RankName + " です！";

            yield return new WaitForSeconds(1f);

            //ランキングを表示させる
            endSceneManager.ShowRanking();

            //ThankYouTextを表示
            thankyouText.gameObject.SetActive(true);

            yield break;
        }
    }
}
