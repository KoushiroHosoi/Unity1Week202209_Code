using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.SceneManagement;
using System.Numerics;
using naichilab.EasySoundPlayer.Scripts;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace HamuGame
{
    //MainScene‚ÆEndScene‚Ì‚â‚èŽæ‚è‚ð‚¨‚±‚È‚¤
    public  class MainAndEneConnecter
    {
        private static BigInteger totalScore;

        private CancellationToken token;

        public event Action onEndGame;

        public static BigInteger TotalScore { get => totalScore; }

        public MainAndEneConnecter()
        {
            totalScore = 0;

            var cts = new CancellationTokenSource();
            token = cts.Token;
        }

        public void LoadEndScene(BigInteger score)
        {
            onEndGame();
            totalScore = score;
            SoundVolumeController.SetVolume(BgmPlayer.Instance.Volume, SePlayer.Instance.Volume);
            AsyncLoadEnd(token);
        }

        private async UniTask AsyncLoadEnd(CancellationToken token)
        {
            SePlayer.Instance.Play(4);
            await UniTask.Delay(TimeSpan.FromSeconds(1.2f), cancellationToken: token);
            SceneManager.LoadScene("EndScene");
        }
    }
}
