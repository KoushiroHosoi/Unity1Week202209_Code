using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;
using System.Linq;
using naichilab.EasySoundPlayer.Scripts;
using DG.Tweening;
using UniRx;

namespace HamuGame
{
    //����̐_�N���X
    public class MainSceneManager : MonoBehaviour
    {
        //�J�E���g�_�E�������ǂ���
        private bool isCountingDown;

        //Tile�֘A
        [SerializeField] private MoneyTileManager moneyTilePrefab;
        private int[] amountTypes;
        [SerializeField] private int[] lv1AmountArray;
        [SerializeField] private int[] lv2AmountArray;
        [SerializeField] private int[] lv3AmountArray;
        [SerializeField] private int[] lv4AmountArray;
        private int[] randomAmounts;
        private MoneyTileManager[,] moneyTiles;

        //�v���C���[�֘A
        [SerializeField] private PlayerManager playerPrefab;
        private PlayerManager player;

        //�Q�[���f�[�^�֘A
        [SerializeField] private float gameTime;
        private StageLebelManager stageLebel;
        private MoneyManager money;
        private bool canPlaying;
        private float tileSize;

        //���Z�q������
        public MathematicalSymbolType[] defaltMathematicalSymbols;
        private MathematicalSymbolType[] mathematicalSymbols;
        private int calculationNumber;

        //�V�[���J�ڗp
        private MainAndEneConnecter mainAndEneConnecter;

        //MoneyAmount��UI��ς���p�̈Ϗ�
        public event Action onChangePointUI;
        //�|�C���g���|�b�v�A�b�v������p
        public event Action<MathematicalSymbolType,int> onPopUpPointText;
        //�v�Z�L����UI��ς���p
        public event Action onChangeMathTypeUI;
        //�X�e�[�W���x����UI��ς���p
        public event Action onChengeStageLebelUI;

        //PausePanel��\������p
        public event Action onChangePausePanel;
        //�J�E���g�_�E�����̗p
        public event Action<int> onCountDownUI;
        public event Action onEndCountDownUI;

        public float GameTime { get => gameTime; }
        public StageLebelManager StageLebel { get => stageLebel; }
        public MoneyManager Money { get => money; }
        public MathematicalSymbolType[] MathematicalSymbols { get => mathematicalSymbols; }
        public int CalculationNumber { get => calculationNumber; }

        private void Awake()
        {
            //������
            isCountingDown = true;

            money = new MoneyManager();
            mainAndEneConnecter = new MainAndEneConnecter();
            stageLebel = new StageLebelManager(50, 4);

            //�o�ꂷ����z��o�^����
            amountTypes = new int[] { 1, 10, 100, 500, 1000, 5000, 10000, 1000000, 100000000 };

            stageLebel.onDestroyField += DestroyTile;
            stageLebel.onChangeFieldSize += CreateTileView;
            stageLebel.onChangeFieldSize += ChangePlayerSize;
            stageLebel.onChangeScene += mainAndEneConnecter.LoadEndScene;

            mainAndEneConnecter.onEndGame += EndGame;

            lv1AmountArray = ChangeArray(lv1AmountArray);
            lv2AmountArray = ChangeArray(lv2AmountArray);
            lv3AmountArray = ChangeArray(lv3AmountArray);
            lv4AmountArray = ChangeArray(lv4AmountArray);

            calculationNumber = 0;
            canPlaying = true;
        }

        void Start()
        {
            //Start�V�[������T�E���h�̑傫���������p��
            BgmPlayer.Instance.Volume = SoundVolumeController.BgmVolume;
            SePlayer.Instance.Volume = SoundVolumeController.SeVolume;
            StartCoroutine(GameStartCoroutine());
        }

        void Update()
        {
            //�J�E���g�_�E�����͑���s�\�ɂ���
            if (isCountingDown) return;

            //Space�L�[�������ƃQ�[�����ꎞ��~������
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canPlaying = !canPlaying;
                onChangePausePanel();
            }

            //�ꎞ��~���͂����Ń��^�[��
            if (!canPlaying) return;

            //���ԂɊւ��鏈���A���Ԃ�0�ɂȂ�����V�[���ړ�
            if(gameTime > 0)
            {
                gameTime -= Time.deltaTime;
            }
            else
            {
                mainAndEneConnecter.LoadEndScene(money.MoneyAmount);
            }

            //�ړ��Ɋւ��鏈��
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //��O�ɍs���Ȃ��悤�ɂ���
                if (player.PlayerPos.y == 0) return;
                //�ړ��悪�ʍs�s�Ȃ�ړ����Ȃ�
                if (!moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y - 1].ReturnCanPassing()) return;
                //�v���C���[���ړ�������
                player.PlayerMove(MoveVector.Up, tileSize);
                //�|�C���g���v�Z����
                CalculateMoney();
                //����Ɉړ��\��Tile�����邩���ׂ�
                CheckCanMove();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (player.PlayerPos.x == stageLebel.FieldSize - 1) return;
                if (!moneyTiles[(int)player.PlayerPos.x + 1, (int)player.PlayerPos.y].ReturnCanPassing()) return;
                player.PlayerMove(MoveVector.Right, tileSize);
                CalculateMoney();
                CheckCanMove();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (player.PlayerPos.y == stageLebel.FieldSize - 1) return;
                if (!moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y + 1].ReturnCanPassing()) return;
                player.PlayerMove(MoveVector.Down, tileSize);
                CalculateMoney();
                CheckCanMove();

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (player.PlayerPos.x == 0) return;
                if (!moneyTiles[(int)player.PlayerPos.x - 1, (int)player.PlayerPos.y].ReturnCanPassing()) return;
                player.PlayerMove(MoveVector.Left, tileSize);
                CalculateMoney();
                CheckCanMove();
            }
        }

        private IEnumerator GameStartCoroutine()
        {
            //�J�E���g�_�E���J�n
            for (int i = 3; i > -1; i--)
            {
                if (i != 0)
                {
                    SePlayer.Instance.Play(0);
                }
                else
                {
                    SePlayer.Instance.Play(1);
                }
                
                onCountDownUI(i);
                yield return new WaitForSeconds(1);
            }

            //�Q�[�����X�^�[�g������
            //�v�Z�L���������_���ɕ��ёւ���
            mathematicalSymbols = defaltMathematicalSymbols.OrderBy(i => Guid.NewGuid()).ToArray();
            //UI�̕ύX�i�����j�������Ȃ�
            onChengeStageLebelUI();
            onChangePointUI();
            onChangeMathTypeUI();
            //�X�e�[�W�𐶐�
            CreateTileView();
            SetTilesData();
            CreatePlayer();

            onEndCountDownUI();
            BgmPlayer.Instance.Play(0);

            isCountingDown = false;
        }

        //�X�e�[�W����蒼������
        private void ChangeNextStage()
        {
            //�X�e�[�W���x������(�^�C���j��Ƃ��������ł���Ă�)
            stageLebel.StageLebelUp(money.MoneyAmount);
            onChengeStageLebelUI();
            //Tile�Ƀf�[�^����꒼��
            SetTilesData();
            CreatePlayer();
            //���w�L������ג���
            mathematicalSymbols = defaltMathematicalSymbols.OrderBy(i => Guid.NewGuid()).ToArray();
            calculationNumber = 0;
            onChangeMathTypeUI();
        }

        //Tile�̌����ځE�f�[�^���ׂĔj�󂷂�
        private void DestroyTile()
        {
            for (int i = 0; i < stageLebel.FieldSize; i++)
            {
                for (int k = 0; k < stageLebel.FieldSize; k++)
                {
                    //Destroy(moneyTiles[k, i].gameObject);
                    StartCoroutine(moneyTiles[k, i].DestroyTileCoroutine());
                    moneyTiles[k, i] = null;
                }
            }
        }

        //Tile�̌����ڂ𐶐����鏈��
        private void CreateTileView()
        {
            //2�����z��̗v�f�����ύX�����̂ŐV�����錾����
            moneyTiles = new MoneyTileManager[stageLebel.FieldSize, stageLebel.FieldSize];

            //FieldSize�ɍ��킹��Tile�̑傫����ύX����
            //�c��12*12�Ɏ��܂�悤�ȃT�C�Y�ɂ���
            switch (stageLebel.FieldSize)
            {
                case 4:
                    tileSize = 3f;
                    break;
                case 6:
                    tileSize = 2f;
                    break;
                case 8:
                    tileSize = 1.5f;
                    break;
                case 10:
                    tileSize = 1.2f;
                    break;
                default:
                    Debug.Log("Tile�̑傫�����s���ł�");
                    break;
            }

            //�����p��Prefab�̑傫����ύX����
            moneyTilePrefab.transform.localScale = new Vector3(tileSize, tileSize, 1);

            //Tile�̉������擾����
            float distance = moneyTilePrefab.transform.localScale.x;

            for (int i = 0; i < stageLebel.FieldSize; i++)
            {
                for (int k = 0; k < stageLebel.FieldSize; k++)
                {
                    //��������(���S����ɐ���������)
                    Vector3 createPos = new Vector3(-6f + (distance / 2) + (distance * k), 5.4f - (distance / 2) - (distance * i), 0);
                    MoneyTileManager numberTile = Instantiate(moneyTilePrefab, createPos, Quaternion.identity);
                    numberTile.transform.SetParent(this.transform);
                    moneyTiles[k, i] = numberTile;
                }
            }
        }

        //�v���C���[�̑傫����ύX����
        private void ChangePlayerSize()
        {
            player.ChangePlayerSize(stageLebel.FieldSize);
        }

        //�G���h�V�[���Ɉړ�����ہA����s�ɂ��鏈��
        private void EndGame()
        {
            canPlaying = false;
        }

        //lvArray{4,2,4,2,2,0,0,0,0}��n����{1,1,1,1,10,10,100,100,100,100,500,500,�c}�̂悤�ɕϊ����Ă���郁�\�b�h
        private int[] ChangeArray(int[] lvArray)
        {
            List<int> testList = new List<int>();

            for (int i = 0; i < amountTypes.Length; i++)
            {
                for (int k = lvArray[i]; k > 0; k--)
                {
                    testList.Add(amountTypes[i]);
                }
            }

            int[] testArray = testList.ToArray();
            return testArray;
        }

        //Tile�Ƀf�[�^�����鏈��
        private void SetTilesData()
        {
            //�O�̃A�C�f�A�̈�Y
            /*
            //�t�B�[���h���Tile�ɓ����|�C���g��p�ӂ���
            float maxPoint = Mathf.Pow(stageLebel.FieldSize, 2) * 5;
            //n^2���̌��̃|�C���g��p��(36�R)
            randomPoints = new int[(int)Mathf.Pow(stageLebel.FieldSize, 2)];

            //��U���ׂĂ�5������
            for (int i = 0; i < randomPoints.Length; i++)
            {
                randomPoints[i] = (int)maxPoint / randomPoints.Length;
            }

            //1�`4�̐��𐶐����đ����������J��Ԃ�
            for (int i = 0; i < randomPoints.Length; i += 2)
            {
                int point = UnityEngine.Random.Range(0, 5);
                int randomLR = UnityEngine.Random.Range(0, 100);

                if (randomLR % 2 == 0)
                {
                    randomPoints[i] += point;
                    randomPoints[i + 1] -= point;
                }
                else
                {
                    randomPoints[i] -= point;
                    randomPoints[i + 1] += point;
                }
            }
            */

            //���݂̃X�e�[�W�̃��x�����擾����
            int lebel = stageLebel.StageLebel;

            //���x���ɍ��킹���^�C�����擾
            switch (lebel)
            {
                case int n when (n >= 1 && n <= 14):
                    randomAmounts = lv1AmountArray;
                    break;
                case int n when (n >= 15 && n <= 29):
                    randomAmounts = lv2AmountArray;
                    break;
                case int n when (n >= 30 && n <= 44):
                    randomAmounts = lv3AmountArray;
                    break;
                case int n when (n >= 45 && n <= stageLebel.StageMaxLebel):
                    randomAmounts = lv4AmountArray;
                    break;
            }
            //�V���b�t������
            randomAmounts = randomAmounts.OrderBy(i => Guid.NewGuid()).ToArray();

            //�S�[����ݒ肷��
            int goalPosX = UnityEngine.Random.Range(1, stageLebel.FieldSize);
            int goalPosY = UnityEngine.Random.Range(1, stageLebel.FieldSize);

            //�f�[�^��Tile�ɐݒ肷��
            for (int i = 0; i < stageLebel.FieldSize; i++)
            {
                for (int k = 0; k < stageLebel.FieldSize; k++)
                {
                    //��UonGoal�����ׂċ�ɂ���(���ꂵ�Ȃ��ƃ��x���A�b�v(onGoal())�������Ă΂�đ�ςȂ��ƂɂȂ�)
                    //moneyTiles[k, i].onGoal?.Invoke();
                    moneyTiles[k, i].onGoal -= ChangeNextStage;
                    
                    //�f�[�^�����ꂼ��ݒ肷��
                    moneyTiles[k, i].onGoal += ChangeNextStage;

                    moneyTiles[k, i].SetData(randomAmounts[i * 6 + k], tileSize);

                    //�����ƈ�v����Tile���S�[���Ƃ��Đݒ肷��
                    if (k == goalPosX && i == goalPosY)
                    {
                        moneyTiles[k, i].SetGoal();
                    }
                }
            }
        }

        //Player�̈ʒu������������N���X
        private void CreatePlayer()
        {
            if (player == null)
            {
                player = Instantiate(playerPrefab, moneyTiles[0, 0].transform.position, Quaternion.identity);
            }
            else
            {
                player.transform.position = moneyTiles[0, 0].transform.position;
            }

            player.ResetPlayerPos();
        }

        //�l�����͂܂�Ă��Ȃ����`�F�b�N����
        private void CheckCanMove()
        {
            //�z��̗v�f���ȏ�E�ȉ��̐��ɂȂ�Ȃ��悤�ɂ���
            int posX = (int)player.PlayerPos.x;
            int posY = (int)player.PlayerPos.y;
            int rightPos = Mathf.Clamp(posX + 1, 0, stageLebel.FieldSize - 1);
            int leftPos = Mathf.Clamp(posX - 1, 0, stageLebel.FieldSize - 1);
            int upPos = Mathf.Clamp(posY + 1, 0, stageLebel.FieldSize - 1);
            int downPos = Mathf.Clamp(posY - 1, 0, stageLebel.FieldSize - 1);

            //���肪�͂܂�Ă��Ȃ����`�F�b�N����
            //�͂܂�Ă�����Q�[���I�[�o�[�ɂ���
            if (
                !moneyTiles[rightPos, posY].ReturnCanPassing()
                && !moneyTiles[leftPos, posY].ReturnCanPassing()
                && !moneyTiles[posX, upPos].ReturnCanPassing()
                && !moneyTiles[posX, downPos].ReturnCanPassing()
               )
            {
                mainAndEneConnecter.LoadEndScene(money.MoneyAmount);
            }
        }

        //���z���v�Z���鏈��
        private void CalculateMoney()
        {
            //UI���|�b�v�A�b�v�\��������
            onPopUpPointText(mathematicalSymbols[calculationNumber], moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnTilePoint());
            //���Z�q�ɂ���Čv�Z�L����ς���
            money.ChangeMoneyAmount(moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnTilePoint(), mathematicalSymbols[calculationNumber]);

            //���Z�q�����Ԃɓ���ւ��邽�߂̏���
            if (calculationNumber < mathematicalSymbols.Length - 1)
            {
                calculationNumber++;
            }
            else
            {
                calculationNumber = 0;
            }

            //�S�[�����ǂ������ׂ�
            moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnGoal();

            //UI�̕ύX�������Ȃ�
            onChangePointUI();
            onChangeMathTypeUI();
        }
    }
}