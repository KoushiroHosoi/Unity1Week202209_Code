using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System.Numerics;
using System;
using UnityEngine.SceneManagement;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    public class EndSceneManager : MonoBehaviour
    {
        private int rankNumber;
        private string rankName;

        public int RankNumber { get => rankNumber; }
        public string RankName { get => rankName; }

        //初期化
        private void Awake()
        {
            rankNumber = 0;
            rankName = "";
        }

        // Start is called before the first frame update
        void Start()
        {
            SePlayer.Instance.Volume = SoundVolumeController.SeVolume;

            //ランクを計算
            rankNumber = CalculateDigits(MainAndEneConnecter.TotalScore);
            //ランク名を算出
            rankName = CalculateRank(rankNumber);
            SePlayer.Instance.Play(0);
        }

        //桁数(ランク)を返す
        public int CalculateDigits(BigInteger num)
        {
            int digit = 0;

            while (num >= 10)
            {
                digit++;
                num /= 10;
            }

            return digit;
        }

        //ランクを返す
        private string CalculateRank(int num)
        {
            //Maxの時のみ特殊な処理
            if (MainAndEneConnecter.TotalScore == BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000") - 1)
            {
                rankNumber = 64;
                return "ハム国の神";
            }
            else
            {
                switch (rankNumber)
                {
                    case 0:
                        return "地下送り";
                    case int n when (n >= 1 && n <= 3):
                        return "大大貧民";
                    case int n when (n >= 4 && n <= 7):
                        return "大貧民";
                    case int n when (n >= 8 && n <= 11):
                        return "貧民";
                    case int n when (n >= 12 && n <= 15):
                        return "一般市民";
                    case int n when (n >= 16 && n <= 19):
                        return "成金";
                    case int n when (n >= 20 && n <= 23):
                        return "小金持ち";
                    case int n when (n >= 24 && n <= 27):
                        return "普通の金持ち";
                    case int n when (n >= 28 && n <= 31):
                        return "大金持ち";
                    case int n when (n >= 32 && n <= 35):
                        return "石油王";
                    case int n when (n >= 36 && n <= 39):
                        return "全人類にお金配れるレベル";
                    case int n when (n >= 40 && n <= 43):
                        return "全人類を恋人にできるレベル";
                    case int n when (n >= 44 && n <= 47):
                        return "前〇社長が羨むレベル";
                    case int n when (n >= 48 && n <= 51):
                        return "イーロンマ〇クが羨むレベル";
                    case int n when (n >= 52 && n <= 55):
                        return "兵〇和尊が羨むレベル";
                    case int n when (n >= 56 && n <= 59):
                        return "ハム国副国王";
                    case int n when (n >= 60 && n <= 63):
                        return "ハム国国王";

                    default:
                        return "エラー";
                }
            }
        }

        //ランキングを表示（改良しろ！！）
        public void ShowRanking()
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(rankNumber);
        }

        //Twitterを表示
        public void ShowTweet()
        {
            naichilab.UnityRoomTweet.Tweet("hamu_earngiantmoney", "あなたは " + JapaneseNumberNotationConverter.ToJapaneseNumberNotation(MainAndEneConnecter.TotalScore) + " 円集めました！！" + "\nあなたのランクは " + rankName + " です！！", "unityroom", "unity1week");
        }

        //StartSceneに戻す
        public void LoadStartScene()
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            //SoundVolumeController.SetVolume(BgmPlayer.Instance.Volume, SePlayer.Instance.Volume);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("StartScene");
        }
    }
}
