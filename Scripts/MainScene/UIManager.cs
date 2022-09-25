using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;
using DG.Tweening;

namespace HamuGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainSceneManager mainSceneManager;
        [SerializeField] private Canvas canvas;

        [SerializeField] private Text moneyText;
        [SerializeField] private Text timerText;
        [SerializeField] private Text lebelText;

        [SerializeField] private Text popUpTextPrefab;

        [SerializeField] private Text countDownText;

        [SerializeField] private GameObject pausePanel;

        [SerializeField] private Transform[] mathObjectPos;
        [SerializeField] private GameObject[] mathSynbolObjectPrefab;

        [SerializeField] private Sprite[] progressImage;

        //[SerializeField] private Image[] mathSynbolImages;
        //[SerializeField] private Sprite[] mathSynbolSprites;

        // Start is called before the first frame update
        void Awake()
        {
            mainSceneManager.onChangePointUI += ChangePointUI;
            mainSceneManager.onPopUpPointText += PopUpPointText;
            mainSceneManager.onChangeMathTypeUI += ChangeMathSynbols;
            mainSceneManager.onChengeStageLebelUI += ChangeLebelText;
            mainSceneManager.onChangePausePanel += ChangePausePanel;
            mainSceneManager.onCountDownUI += CountDownUI;
            mainSceneManager.onEndCountDownUI += EndCountDownUI;
        }

        //ポーズパネルは非表示に
        private void Start()
        {
            pausePanel.SetActive(false);
        }

        //タイマーは常に表示
        void Update()
        {
            timerText.text = "タイム：" + mainSceneManager.GameTime.ToString("f2").PadLeft(6, '0');
        }

        //ゲーム開始時のカウントダウン用
        private void CountDownUI(int num)
        {
            if(num == 0)
            {
                countDownText.text = "スタート！！";
            }
            else
            {
                countDownText.text = num.ToString();
            }
        }

        //カウントダウンが終了したとき用
        private void EndCountDownUI()
        {
            countDownText.gameObject.SetActive(false);
        }

        //Spaceキーが押されたときの処理
        private void ChangePausePanel()
        {
            if (pausePanel.activeSelf)
            {
                //SePlayer.Instance.Play(1);
                pausePanel.SetActive(false);
            }
            else
            {
                //SePlayer.Instance.Play(0);
                pausePanel.SetActive(true);
            }
        }

        //金額が変わったときの処理
        private void ChangePointUI()
        {
            //moneyText.text = "合計金額：\n" + mainSceneManager.Money.BigMoneyAmount.ToString("N0");
            moneyText.text = "合計金額：　" + mainSceneManager.Money.MoneyAmount.ToString("N0");
        }

        //moneyText周りにUIをポップアップさせる処理
        private void PopUpPointText(MathematicalSymbolType mathematicalSymbolType,int amount)
        {
            StartCoroutine(PouUpCoroutine(mathematicalSymbolType, amount));
        }
        
        //実際に生成して動かすコルーチン
        private IEnumerator PouUpCoroutine(MathematicalSymbolType mathematicalSymbolType,int amount)
        {
            //生成(本来は最初からRectTransform用の位置に生成したかった)
            Text pouUpText = Instantiate(popUpTextPrefab);
            pouUpText.transform.SetParent(canvas.transform);

            //記号によって色を変える(文字も変更)
            string colorCode = null;
            string mathematicalSymbol = null;
            Color textColor;
            switch (mathematicalSymbolType)
            {
                case MathematicalSymbolType.plus:
                    colorCode = "#C24B2B";
                    mathematicalSymbol = "+";
                    break;
                case MathematicalSymbolType.minus:
                    colorCode = "#56ECD0";
                    mathematicalSymbol = "-";
                    break;
                case MathematicalSymbolType.multiplied:
                    colorCode = "#604AAE";
                    mathematicalSymbol = "×";
                    break;;
                case MathematicalSymbolType.divided:
                    colorCode = "#7CA420";
                    mathematicalSymbol = "÷";
                    break;
            }
            
            if(ColorUtility.TryParseHtmlString(colorCode, out textColor))
            {
                pouUpText.color = textColor;
                pouUpText.text = mathematicalSymbol + amount.ToString();
            }            

            //位置をUI用に直す
            float randomPosX = Random.Range(-255, 255); float randomPosY = Random.Range(290, 330);
            Vector3 createPos = new Vector3(randomPosX, randomPosY, 0);
            pouUpText.rectTransform.anchoredPosition = createPos;

            //動かす
            Sequence seq = DOTween.Sequence();

            yield return seq.Append(pouUpText.rectTransform.DOAnchorPosY(createPos.y - 15f, 0.5f))
                            .Append(pouUpText.rectTransform.DOAnchorPosY(createPos.y + 20f, 1f))
                            .SetLink(pouUpText.gameObject)
                            .WaitForCompletion();

            Destroy(pouUpText.gameObject);

            yield break;
        }


        //数学記号を入れ替えるときの処理
        private void ChangeMathSynbols()
        {
            //現在の番号を取得する
            int number = mainSceneManager.CalculationNumber;

            //mathObjectPosの先頭（一番上）から実行をおこなう
            foreach (var pos in mathObjectPos)
            {
                //すでに子オブジェクトがあったら破壊する（オブジェクトが被ってしまうため）
                for(var i = pos.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(pos.transform.GetChild(i).gameObject);
                }
                
                //生成をおこなって位置を調整する
                GameObject obj = Instantiate(ReturnMathSynbolImage(mainSceneManager.MathematicalSymbols[number]));
                obj.transform.SetParent(pos);
                obj.transform.localPosition = new Vector3(0, 0, 0);

                //順番に並べるために加算をおこなう
                if (number < mainSceneManager.MathematicalSymbols.Length - 1)
                {
                    number++;
                }
                else
                {
                    number = 0;
                }
            }
            
        }

        //指定された数学記号に対応するゲームオブジェクトを返す
        private GameObject ReturnMathSynbolImage(MathematicalSymbolType mathematicalSymbolType)
        {
            switch (mathematicalSymbolType)
            {
                case MathematicalSymbolType.plus:
                    return mathSynbolObjectPrefab[0];
                case MathematicalSymbolType.minus:
                    return mathSynbolObjectPrefab[1];
                case MathematicalSymbolType.multiplied:
                    return mathSynbolObjectPrefab[2];
                case MathematicalSymbolType.divided:
                    return mathSynbolObjectPrefab[3];
                default:
                    return null;
            }
        }

        private void ChangeLebelText()
        {
            lebelText.text = "レベル：" +　mainSceneManager.StageLebel.StageLebel.ToString().PadLeft(2, '0');
        }
    }
}
