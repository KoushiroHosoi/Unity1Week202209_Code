using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //�i�ޕ����̎��
    public enum MoveVector
    {
        Up,
        Right,
        Down,
        Left
    }

    public class PlayerManager : MonoBehaviour
    {
        //Player�̍��W
        private Vector2 playerPos;

        public Vector2 PlayerPos { get => playerPos; }

        //Player�̑傫�������������ɂ��郁�\�b�h
        public void ChangePlayerSize(int stageSize)
        {
            switch (stageSize)
            {
                case 4:
                    this.transform.localScale = new Vector3(3f, 3f, 1);
                    break;
                case 6:
                    this.transform.localScale = new Vector3(2.4f, 2.4f, 1);
                    break;
                case 8:
                    this.transform.localScale = new Vector3(1.8f, 1.8f, 1);
                    break;
                case 10:
                    this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                    break;
                default:
                    Debug.Log("�v���C���[�̑傫�����s���ł�");
                    break;
            }

        }

        //�v���C���[���ړ������鏈��
        public void PlayerMove(MoveVector moveVec,float distance)
        {
            switch (moveVec)
            {
                case MoveVector.Up:
                    //�f�[�^��̈ړ�
                    playerPos += new Vector2(0, -1);
                    //�����ڏ�̈ړ�
                    this.transform.position += new Vector3(0, distance, 0);
                    break;

                case MoveVector.Right:
                    playerPos += new Vector2(1, 0);
                    this.transform.position += new Vector3(distance, 0, 0);
                    break;

                case MoveVector.Down:
                    playerPos += new Vector2(0, 1);
                    this.transform.position += new Vector3(0, -distance, 0);
                    break;

                case MoveVector.Left:
                    playerPos += new Vector2(-1, 0);
                    this.transform.position += new Vector3(-distance, 0, 0);
                    break;
            }

            SePlayer.Instance.Play(2);
        }


        //Player���X�^�[�g�̈ʒu�i0,0�j�ɖ߂�
        public void ResetPlayerPos()
        {
            playerPos = new Vector2(0, 0);
        }
    }
}
