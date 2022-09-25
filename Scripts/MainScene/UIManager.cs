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

        //�|�[�Y�p�l���͔�\����
        private void Start()
        {
            pausePanel.SetActive(false);
        }

        //�^�C�}�[�͏�ɕ\��
        void Update()
        {
            timerText.text = "�^�C���F" + mainSceneManager.GameTime.ToString("f2").PadLeft(6, '0');
        }

        //�Q�[���J�n���̃J�E���g�_�E���p
        private void CountDownUI(int num)
        {
            if(num == 0)
            {
                countDownText.text = "�X�^�[�g�I�I";
            }
            else
            {
                countDownText.text = num.ToString();
            }
        }

        //�J�E���g�_�E�����I�������Ƃ��p
        private void EndCountDownUI()
        {
            countDownText.gameObject.SetActive(false);
        }

        //Space�L�[�������ꂽ�Ƃ��̏���
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

        //���z���ς�����Ƃ��̏���
        private void ChangePointUI()
        {
            //moneyText.text = "���v���z�F\n" + mainSceneManager.Money.BigMoneyAmount.ToString("N0");
            moneyText.text = "���v���z�F�@" + mainSceneManager.Money.MoneyAmount.ToString("N0");
        }

        //moneyText�����UI���|�b�v�A�b�v�����鏈��
        private void PopUpPointText(MathematicalSymbolType mathematicalSymbolType,int amount)
        {
            StartCoroutine(PouUpCoroutine(mathematicalSymbolType, amount));
        }
        
        //���ۂɐ������ē������R���[�`��
        private IEnumerator PouUpCoroutine(MathematicalSymbolType mathematicalSymbolType,int amount)
        {
            //����(�{���͍ŏ�����RectTransform�p�̈ʒu�ɐ�������������)
            Text pouUpText = Instantiate(popUpTextPrefab);
            pouUpText.transform.SetParent(canvas.transform);

            //�L���ɂ���ĐF��ς���(�������ύX)
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
                    mathematicalSymbol = "�~";
                    break;;
                case MathematicalSymbolType.divided:
                    colorCode = "#7CA420";
                    mathematicalSymbol = "��";
                    break;
            }
            
            if(ColorUtility.TryParseHtmlString(colorCode, out textColor))
            {
                pouUpText.color = textColor;
                pouUpText.text = mathematicalSymbol + amount.ToString();
            }            

            //�ʒu��UI�p�ɒ���
            float randomPosX = Random.Range(-255, 255); float randomPosY = Random.Range(290, 330);
            Vector3 createPos = new Vector3(randomPosX, randomPosY, 0);
            pouUpText.rectTransform.anchoredPosition = createPos;

            //������
            Sequence seq = DOTween.Sequence();

            yield return seq.Append(pouUpText.rectTransform.DOAnchorPosY(createPos.y - 15f, 0.5f))
                            .Append(pouUpText.rectTransform.DOAnchorPosY(createPos.y + 20f, 1f))
                            .SetLink(pouUpText.gameObject)
                            .WaitForCompletion();

            Destroy(pouUpText.gameObject);

            yield break;
        }


        //���w�L�������ւ���Ƃ��̏���
        private void ChangeMathSynbols()
        {
            //���݂̔ԍ����擾����
            int number = mainSceneManager.CalculationNumber;

            //mathObjectPos�̐擪�i��ԏ�j������s�������Ȃ�
            foreach (var pos in mathObjectPos)
            {
                //���łɎq�I�u�W�F�N�g����������j�󂷂�i�I�u�W�F�N�g������Ă��܂����߁j
                for(var i = pos.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(pos.transform.GetChild(i).gameObject);
                }
                
                //�����������Ȃ��Ĉʒu�𒲐�����
                GameObject obj = Instantiate(ReturnMathSynbolImage(mainSceneManager.MathematicalSymbols[number]));
                obj.transform.SetParent(pos);
                obj.transform.localPosition = new Vector3(0, 0, 0);

                //���Ԃɕ��ׂ邽�߂ɉ��Z�������Ȃ�
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

        //�w�肳�ꂽ���w�L���ɑΉ�����Q�[���I�u�W�F�N�g��Ԃ�
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
            lebelText.text = "���x���F" +�@mainSceneManager.StageLebel.StageLebel.ToString().PadLeft(2, '0');
        }
    }
}
