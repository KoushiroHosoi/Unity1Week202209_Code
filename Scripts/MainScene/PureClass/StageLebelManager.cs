using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;
using System.Numerics;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //ステージのレベルやTileの数を扱う
    public class StageLebelManager
    {
        //レベル
        private int stageLebel;
        private int stageMaxLebel;
        //フィールドのサイズ
        private int fieldSize;

        //ステージのサイズが変わった際にTileを破壊する用
        public event Action onDestroyField;
        //ステージのサイズが変わった際におこなう処理用
        public event Action onChangeFieldSize;
        //レベルがマックスまで達した際にシーンを移動させる用
        public event Action<BigInteger> onChangeScene;

        public int StageLebel { get => stageLebel; }
        public int FieldSize { get => fieldSize; }
        public int StageMaxLebel { get => stageMaxLebel; }

        //データを設定
        public StageLebelManager(int maxLebel,int startSize)
        {
            stageLebel = 1;
            stageMaxLebel = maxLebel;
            fieldSize = startSize;
        }

        //フィールドを拡大させる処理
        private void ExpandFieldSize(int lebel)
        {
            
            //15・30・45のときのみ特殊な処理を走らせる
            if (lebel % 15 == 0)
            {
                SePlayer.Instance.Play(6);

                //15・30のときはTileの数を増やす
                if (lebel != 45)
                {
                    //Fieldをすべて削除する
                    onDestroyField();
                    fieldSize += 2;
                    //新しいサイズでフィールドを作成する
                    onChangeFieldSize();
                }
                else
                {
                    //ここはTileにセットするお金が変わるだけなので何もしない
                }
            }
            else
            {
                SePlayer.Instance.Play(5);
            }
        }

        //レベルアップさせる処理
        public void StageLebelUp(BigInteger totalScore)
        {
            //レベルがmax以下ならレベルアップ
            if (stageLebel < stageMaxLebel)
            {
                //Debug.Log("レベルアップ！");
                stageLebel++;
                ExpandFieldSize(stageLebel);
            }
            //maxレベルに達したらシーンを移動させる
            else
            {
                onChangeScene(totalScore);
            }
        }
    }
}
