using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System.Numerics;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    public class ProductionManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] fontPrefabs;
        private bool[] isChecks;
        [SerializeField] private MainSceneManager mainSceneManager;

        [SerializeField] private Transform[] movePos;

        // Start is called before the first frame update
        void Awake()
        {
            int length = fontPrefabs.Length;
            isChecks = new bool[length];

            for(int i = 0; i < isChecks.Length; i++)
            {
                isChecks[i] = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(isChecks[0] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("10000"))
            {
                CreateAndMoveFont(fontPrefabs[0]);
                isChecks[0] = false;
            }
            else if (isChecks[1] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("100000000"))
            {
                CreateAndMoveFont(fontPrefabs[1]);
                isChecks[1] = false;
            }
            else if (isChecks[2] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("1000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[2]);
                isChecks[2] = false;
            }
            else if (isChecks[3] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("10000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[3]);
                isChecks[3] = false;
            }
            else if (isChecks[4] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("100000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[4]);
                isChecks[4] = false;
            }
            else if (isChecks[5] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("1000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[5]);
                isChecks[5] = false;
            }
            else if (isChecks[6] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("10000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[6]);
                isChecks[6] = false;
            }
            else if (isChecks[7] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("100000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[7]);
                isChecks[7] = false;
            }
            else if (isChecks[8] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("1000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[8]);
                isChecks[8] = false;
            }
            else if (isChecks[9] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("10000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[9]);
                isChecks[9] = false;
            }
            else if (isChecks[10] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("100000000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[10]);
                isChecks[10] = false;
            }
            else if (isChecks[11] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("1000000000000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[11]);
                isChecks[11] = false;
            }
            else if (isChecks[12] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("10000000000000000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[12]);
                isChecks[12] = false;
            }
            else if (isChecks[13] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("100000000000000000000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[13]);
                isChecks[13] = false;
            }
            else if (isChecks[14] && mainSceneManager.Money.MoneyAmount >= BigInteger.Parse("1000000000000000000000000000000000000000000000000000000000000"))
            {
                CreateAndMoveFont(fontPrefabs[14]);
                isChecks[14] = false;
            }
        }

        private void CreateAndMoveFont(GameObject gameObject)
        {
            SePlayer.Instance.Play(3);
            GameObject gameObj = Instantiate(gameObject, movePos[0].position, UnityEngine.Quaternion.identity);
            StartCoroutine(MoveFont(gameObj));
        }

        private IEnumerator MoveFont(GameObject gameObj)
        {
            Sequence seq = DOTween.Sequence();
            yield return seq.Append(gameObj.transform.DOMove(new UnityEngine.Vector3((movePos[0].position.x + movePos[1].position.x) / 2, movePos[1].position.y, movePos[1].position.z), 1.5f).SetEase(Ease.OutQuad))
                            .Append(gameObj.transform.DOMove(new UnityEngine.Vector3(movePos[1].position.x, movePos[1].position.y, movePos[1].position.z), 1.5f).SetEase(Ease.InQuad))
                            .SetLink(gameObj)
                            .WaitForCompletion();

            Destroy(gameObj);

            yield break;
        }
    }
}
