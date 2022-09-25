using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;
using System.Numerics;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //�X�e�[�W�̃��x����Tile�̐�������
    public class StageLebelManager
    {
        //���x��
        private int stageLebel;
        private int stageMaxLebel;
        //�t�B�[���h�̃T�C�Y
        private int fieldSize;

        //�X�e�[�W�̃T�C�Y���ς�����ۂ�Tile��j�󂷂�p
        public event Action onDestroyField;
        //�X�e�[�W�̃T�C�Y���ς�����ۂɂ����Ȃ������p
        public event Action onChangeFieldSize;
        //���x�����}�b�N�X�܂ŒB�����ۂɃV�[�����ړ�������p
        public event Action<BigInteger> onChangeScene;

        public int StageLebel { get => stageLebel; }
        public int FieldSize { get => fieldSize; }
        public int StageMaxLebel { get => stageMaxLebel; }

        //�f�[�^��ݒ�
        public StageLebelManager(int maxLebel,int startSize)
        {
            stageLebel = 1;
            stageMaxLebel = maxLebel;
            fieldSize = startSize;
        }

        //�t�B�[���h���g�傳���鏈��
        private void ExpandFieldSize(int lebel)
        {
            
            //15�E30�E45�̂Ƃ��̂ݓ���ȏ����𑖂点��
            if (lebel % 15 == 0)
            {
                SePlayer.Instance.Play(6);

                //15�E30�̂Ƃ���Tile�̐��𑝂₷
                if (lebel != 45)
                {
                    //Field�����ׂč폜����
                    onDestroyField();
                    fieldSize += 2;
                    //�V�����T�C�Y�Ńt�B�[���h���쐬����
                    onChangeFieldSize();
                }
                else
                {
                    //������Tile�ɃZ�b�g���邨�����ς�邾���Ȃ̂ŉ������Ȃ�
                }
            }
            else
            {
                SePlayer.Instance.Play(5);
            }
        }

        //���x���A�b�v�����鏈��
        public void StageLebelUp(BigInteger totalScore)
        {
            //���x����max�ȉ��Ȃ烌�x���A�b�v
            if (stageLebel < stageMaxLebel)
            {
                //Debug.Log("���x���A�b�v�I");
                stageLebel++;
                ExpandFieldSize(stageLebel);
            }
            //max���x���ɒB������V�[�����ړ�������
            else
            {
                onChangeScene(totalScore);
            }
        }
    }
}
