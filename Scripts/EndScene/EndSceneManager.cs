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

        //������
        private void Awake()
        {
            rankNumber = 0;
            rankName = "";
        }

        // Start is called before the first frame update
        void Start()
        {
            SePlayer.Instance.Volume = SoundVolumeController.SeVolume;

            //�����N���v�Z
            rankNumber = CalculateDigits(MainAndEneConnecter.TotalScore);
            //�����N�����Z�o
            rankName = CalculateRank(rankNumber);
            SePlayer.Instance.Play(0);
        }

        //����(�����N)��Ԃ�
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

        //�����N��Ԃ�
        private string CalculateRank(int num)
        {
            //Max�̎��̂ݓ���ȏ���
            if (MainAndEneConnecter.TotalScore == BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000") - 1)
            {
                rankNumber = 64;
                return "�n�����̐_";
            }
            else
            {
                switch (rankNumber)
                {
                    case 0:
                        return "�n������";
                    case int n when (n >= 1 && n <= 3):
                        return "���n��";
                    case int n when (n >= 4 && n <= 7):
                        return "��n��";
                    case int n when (n >= 8 && n <= 11):
                        return "�n��";
                    case int n when (n >= 12 && n <= 15):
                        return "��ʎs��";
                    case int n when (n >= 16 && n <= 19):
                        return "����";
                    case int n when (n >= 20 && n <= 23):
                        return "��������";
                    case int n when (n >= 24 && n <= 27):
                        return "���ʂ̋�����";
                    case int n when (n >= 28 && n <= 31):
                        return "�������";
                    case int n when (n >= 32 && n <= 35):
                        return "�Ζ���";
                    case int n when (n >= 36 && n <= 39):
                        return "�S�l�ނɂ����z��郌�x��";
                    case int n when (n >= 40 && n <= 43):
                        return "�S�l�ނ���l�ɂł��郌�x��";
                    case int n when (n >= 44 && n <= 47):
                        return "�O�Z�В����A�ރ��x��";
                    case int n when (n >= 48 && n <= 51):
                        return "�C�[�����}�Z�N���A�ރ��x��";
                    case int n when (n >= 52 && n <= 55):
                        return "���Z�a�����A�ރ��x��";
                    case int n when (n >= 56 && n <= 59):
                        return "�n����������";
                    case int n when (n >= 60 && n <= 63):
                        return "�n��������";

                    default:
                        return "�G���[";
                }
            }
        }

        //�����L���O��\���i���ǂ���I�I�j
        public void ShowRanking()
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(rankNumber);
        }

        //Twitter��\��
        public void ShowTweet()
        {
            naichilab.UnityRoomTweet.Tweet("hamu_earngiantmoney", "���Ȃ��� " + JapaneseNumberNotationConverter.ToJapaneseNumberNotation(MainAndEneConnecter.TotalScore) + " �~�W�߂܂����I�I" + "\n���Ȃ��̃����N�� " + rankName + " �ł��I�I", "unityroom", "unity1week");
        }

        //StartScene�ɖ߂�
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
