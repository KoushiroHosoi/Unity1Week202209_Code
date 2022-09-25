using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace HamuGame
{
    //足元のTileのクラス
    public class MoneyTileManager : MonoBehaviour
    {
        //足元のお金のオブジェクトに関するモノ
        private GameObject imageGameObject;
        [SerializeField] private GameObject[] imagePrefabs;
        //色を変える用
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Color defaltColor;
        [SerializeField] private Color passedColor;
        [SerializeField] private Color goalColor;

        //金額とゴールに関するデータ
        private int moneyAmount;
        private bool isGoal;
        //通過できるかどうか
        private bool canPassing;
        //ゴール時にカメラを揺らす用
        private CameraShaker cameraShaker;

        public event Action onGoal;

        //Tileの初期化をおこなう
        public void SetData(int amount,float tileSize)
        {
            //更新されたときのアニメーション
            //左端から伸びるイメージ
            //左端にスケール0で移動→拡大しながら左方向に進めてみる
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMoveX(-tileSize / 2f, 0).SetRelative())
                .Join(transform.DOScaleX(0, 0))
                .Append(transform.DOMoveX(tileSize / 2f, 0.25f).SetRelative())
                .Join(transform.DOScaleX(tileSize, 0.25f));

            moneyAmount = amount;
            canPassing = true;
            isGoal = false;
            sprite.color = defaltColor;
            cameraShaker = GameObject.FindObjectOfType<CameraShaker>();
            SetImage(amount);
        }

        //金額に合わせてお金のオブジェクトを生成する
        private void SetImage(int amount)
        {
            //前のステージのオブジェクトを破棄
            if (imageGameObject != null)
            {
                Destroy(imageGameObject);
                //imageGameObject = null;
            }

            //金額に合わせて生成！
            GameObject createObject = null;
            switch (amount)
            {
                case 1:
                    createObject = imagePrefabs[0];
                    break;
                case 10:
                    createObject = imagePrefabs[1];
                    break;
                case 100:
                    createObject = imagePrefabs[2];
                    break;
                case 500:
                    createObject = imagePrefabs[3];
                    break;
                case 1000:
                    createObject = imagePrefabs[4];
                    break;
                case 5000:
                    createObject = imagePrefabs[5];
                    break;
                case 10000:
                    createObject = imagePrefabs[6];
                    break;
                case 1000000:
                    createObject = imagePrefabs[7];
                    break;
                case 1_0000_0000:
                    createObject = imagePrefabs[8];
                    break;
            }

            imageGameObject = Instantiate(createObject, this.transform);
            
            //アニメーションさせる
            float scaleX = imageGameObject.transform.localScale.x;
            float scaleY = imageGameObject.transform.localScale.y;
            float scaleZ = imageGameObject.transform.localScale.z;
            Sequence seq = DOTween.Sequence();
            seq.Append(imageGameObject.transform.DOScale(new Vector3(scaleX * 1.25f, scaleY * 0.8f, scaleZ), 0.25f))
                .Append(imageGameObject.transform.DOScale(new Vector3(scaleX * 0.8f, scaleY * 1.25f, scaleZ), 0.25f))
                .Append(imageGameObject.transform.DOScale(new Vector3(scaleX, scaleY, scaleZ), 0.1f))
                .SetLink(imageGameObject);
        }

        //Tileをゴールに変更する
        public void SetGoal()
        {
            isGoal = true;
            sprite.color = goalColor;
        }

        //通行可能ならtrueを返す
        public bool ReturnCanPassing()
        {
            return canPassing;
        }

        //Playerにポイントを渡す処理
        public int ReturnTilePoint()
        {
            canPassing = false;
            sprite.color = passedColor;
            Destroy(imageGameObject);

            return moneyAmount;
        }

        //Goal時の挙動の処理
        public void ReturnGoal()
        {
            if (isGoal)
            {
                //Debug.Log("ゴール");
                cameraShaker.ShakeCamera(0.25f, 0.1f);
                onGoal();
            }
        }

        //破壊されるときの挙動（アニメーションの処理）
        public IEnumerator DestroyTileCoroutine()
        {
            //暗転させて落下させる(爆発するようなイメージ)
            //相対的に浮かしてから左右に落下させる
            //半透明にしようぜ
            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            sprite.sortingOrder += 1;
            float randomX = UnityEngine.Random.Range(1f, 3f);
            Destroy(imageGameObject);
            Sequence seq = DOTween.Sequence();
            yield return seq.Append(sprite.DOColor(Color.black, 0))
                            .Join(sprite.DOFade(0.5f, 0))
                            .Join(transform.DOScale(transform.localScale * 0.75f, 0))
                            .Append(transform.DOMove(new Vector3(transform.position.x * randomX, 4, 0), 0.5f).SetEase(Ease.OutCubic).SetRelative(true))
                            .Append(transform.DOMove(new Vector3(transform.position.x * (randomX + 2), -32, 0), 1.2f).SetEase(Ease.InCubic).SetRelative(true))
                            .SetLink(gameObject)
                            .WaitForCompletion();
            Destroy(gameObject);
            yield break;
        }
    }
}
