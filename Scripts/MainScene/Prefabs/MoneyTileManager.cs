using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace HamuGame
{
    //������Tile�̃N���X
    public class MoneyTileManager : MonoBehaviour
    {
        //�����̂����̃I�u�W�F�N�g�Ɋւ��郂�m
        private GameObject imageGameObject;
        [SerializeField] private GameObject[] imagePrefabs;
        //�F��ς���p
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Color defaltColor;
        [SerializeField] private Color passedColor;
        [SerializeField] private Color goalColor;

        //���z�ƃS�[���Ɋւ���f�[�^
        private int moneyAmount;
        private bool isGoal;
        //�ʉ߂ł��邩�ǂ���
        private bool canPassing;
        //�S�[�����ɃJ������h�炷�p
        private CameraShaker cameraShaker;

        public event Action onGoal;

        //Tile�̏������������Ȃ�
        public void SetData(int amount,float tileSize)
        {
            //�X�V���ꂽ�Ƃ��̃A�j���[�V����
            //���[����L�т�C���[�W
            //���[�ɃX�P�[��0�ňړ����g�債�Ȃ��獶�����ɐi�߂Ă݂�
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

        //���z�ɍ��킹�Ă����̃I�u�W�F�N�g�𐶐�����
        private void SetImage(int amount)
        {
            //�O�̃X�e�[�W�̃I�u�W�F�N�g��j��
            if (imageGameObject != null)
            {
                Destroy(imageGameObject);
                //imageGameObject = null;
            }

            //���z�ɍ��킹�Đ����I
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
            
            //�A�j���[�V����������
            float scaleX = imageGameObject.transform.localScale.x;
            float scaleY = imageGameObject.transform.localScale.y;
            float scaleZ = imageGameObject.transform.localScale.z;
            Sequence seq = DOTween.Sequence();
            seq.Append(imageGameObject.transform.DOScale(new Vector3(scaleX * 1.25f, scaleY * 0.8f, scaleZ), 0.25f))
                .Append(imageGameObject.transform.DOScale(new Vector3(scaleX * 0.8f, scaleY * 1.25f, scaleZ), 0.25f))
                .Append(imageGameObject.transform.DOScale(new Vector3(scaleX, scaleY, scaleZ), 0.1f))
                .SetLink(imageGameObject);
        }

        //Tile���S�[���ɕύX����
        public void SetGoal()
        {
            isGoal = true;
            sprite.color = goalColor;
        }

        //�ʍs�\�Ȃ�true��Ԃ�
        public bool ReturnCanPassing()
        {
            return canPassing;
        }

        //Player�Ƀ|�C���g��n������
        public int ReturnTilePoint()
        {
            canPassing = false;
            sprite.color = passedColor;
            Destroy(imageGameObject);

            return moneyAmount;
        }

        //Goal���̋����̏���
        public void ReturnGoal()
        {
            if (isGoal)
            {
                //Debug.Log("�S�[��");
                cameraShaker.ShakeCamera(0.25f, 0.1f);
                onGoal();
            }
        }

        //�j�󂳂��Ƃ��̋����i�A�j���[�V�����̏����j
        public IEnumerator DestroyTileCoroutine()
        {
            //�Ó]�����ė���������(��������悤�ȃC���[�W)
            //���ΓI�ɕ������Ă��獶�E�ɗ���������
            //�������ɂ��悤��
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
