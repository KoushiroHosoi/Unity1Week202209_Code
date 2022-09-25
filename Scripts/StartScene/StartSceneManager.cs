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
        //�w�i�ɗ���������Prefab
        [SerializeField] private GameObject[] moneyPrefabs;
        //Prefab�𐶐��E�j������ʒu
        [SerializeField] private Transform[] createPos;
        [SerializeField] private Transform[] endPos;

        private void Start()
        {
            //BGM�𗬂�
            BgmPlayer.Instance.Play(0);
            StartCoroutine(ProductionMoneyCoroutine());
        }

        //�{�^���������ꂽ����s���鏈��
        public void ChangeStartScene()
        {
            StartCoroutine(LoadMainScene());
        }

        //���C���V�[����ǂݍ���
        private IEnumerator LoadMainScene()
        {
            SePlayer.Instance.Play(0);
            //�T�E���h�̑傫����ۑ�����
            SoundVolumeController.SetVolume(BgmPlayer.Instance.Volume, SePlayer.Instance.Volume);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("MainGameScene");
        }

        //���D���R�C�����������񓮂���
        private IEnumerator ProductionMoneyCoroutine()
        {
            while (true)
            {
                //���J��Ԃ�
                for(int i = 0; i < createPos.Length; i++)
                {
                    //��������I�u�W�F�N�g�������_���Ɍ��߂�
                    int random = Random.Range(0, moneyPrefabs.Length);
                    GameObject moneyObject = Instantiate(moneyPrefabs[random], createPos[i].position, Quaternion.identity);

                    //������������
                    StartCoroutine(MoveMoneyCoroutine(moneyObject, endPos[i]));
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        //�����𓮂�������
        private IEnumerator MoveMoneyCoroutine(GameObject gameObj,Transform pos)
        {
            yield return gameObj.transform.DOMove(pos.position, 6f).SetLink(gameObj).SetEase(Ease.Linear).WaitForCompletion();

            Destroy(gameObj);

            yield break;
        }
    }
}
