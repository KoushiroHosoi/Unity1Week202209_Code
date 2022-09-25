using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.SceneManagement;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    public class StartSceneManager : MonoBehaviour
    {
        //背景に流すお金のPrefab
        [SerializeField] private GameObject[] moneyPrefabs;
        //Prefabを生成・破棄する位置
        [SerializeField] private Transform[] createPos;
        [SerializeField] private Transform[] endPos;

        private void Start()
        {
            //BGMを流す
            BgmPlayer.Instance.Play(0);
            StartCoroutine(ProductionMoneyCoroutine());
        }

        //ボタンを押されたら実行する処理
        public void ChangeStartScene()
        {
            StartCoroutine(LoadMainScene());
        }

        //メインシーンを読み込む
        private IEnumerator LoadMainScene()
        {
            SePlayer.Instance.Play(0);
            //サウンドの大きさを保存する
            SoundVolumeController.SetVolume(BgmPlayer.Instance.Volume, SePlayer.Instance.Volume);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("MainGameScene");
        }

        //お札やらコインをたくさん動かす
        private IEnumerator ProductionMoneyCoroutine()
        {
            while (true)
            {
                //二回繰り返す
                for(int i = 0; i < createPos.Length; i++)
                {
                    //生成するオブジェクトをランダムに決める
                    int random = Random.Range(0, moneyPrefabs.Length);
                    GameObject moneyObject = Instantiate(moneyPrefabs[random], createPos[i].position, Quaternion.identity);

                    //動かす＆消す
                    StartCoroutine(MoveMoneyCoroutine(moneyObject, endPos[i]));
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        //お金を動かす処理
        private IEnumerator MoveMoneyCoroutine(GameObject gameObj,Transform pos)
        {
            yield return gameObj.transform.DOMove(pos.position, 6f).SetLink(gameObj).SetEase(Ease.Linear).WaitForCompletion();

            Destroy(gameObj);

            yield break;
        }
    }
}
