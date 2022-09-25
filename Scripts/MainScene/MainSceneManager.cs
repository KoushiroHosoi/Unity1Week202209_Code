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
    //今回の神クラス
    public class MainSceneManager : MonoBehaviour
    {
        //カウントダウン中かどうか
        private bool isCountingDown;

        //Tile関連
        [SerializeField] private MoneyTileManager moneyTilePrefab;
        private int[] amountTypes;
        [SerializeField] private int[] lv1AmountArray;
        [SerializeField] private int[] lv2AmountArray;
        [SerializeField] private int[] lv3AmountArray;
        [SerializeField] private int[] lv4AmountArray;
        private int[] randomAmounts;
        private MoneyTileManager[,] moneyTiles;

        //プレイヤー関連
        [SerializeField] private PlayerManager playerPrefab;
        private PlayerManager player;

        //ゲームデータ関連
        [SerializeField] private float gameTime;
        private StageLebelManager stageLebel;
        private MoneyManager money;
        private bool canPlaying;
        private float tileSize;

        //演算子を扱う
        public MathematicalSymbolType[] defaltMathematicalSymbols;
        private MathematicalSymbolType[] mathematicalSymbols;
        private int calculationNumber;

        //シーン遷移用
        private MainAndEneConnecter mainAndEneConnecter;

        //MoneyAmountのUIを変える用の委譲
        public event Action onChangePointUI;
        //ポイントをポップアップさせる用
        public event Action<MathematicalSymbolType,int> onPopUpPointText;
        //計算記号のUIを変える用
        public event Action onChangeMathTypeUI;
        //ステージレベルのUIを変える用
        public event Action onChengeStageLebelUI;

        //PausePanelを表示する用
        public event Action onChangePausePanel;
        //カウントダウン時の用
        public event Action<int> onCountDownUI;
        public event Action onEndCountDownUI;

        public float GameTime { get => gameTime; }
        public StageLebelManager StageLebel { get => stageLebel; }
        public MoneyManager Money { get => money; }
        public MathematicalSymbolType[] MathematicalSymbols { get => mathematicalSymbols; }
        public int CalculationNumber { get => calculationNumber; }

        private void Awake()
        {
            //初期化
            isCountingDown = true;

            money = new MoneyManager();
            mainAndEneConnecter = new MainAndEneConnecter();
            stageLebel = new StageLebelManager(50, 4);

            //登場する金額を登録する
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
            //Startシーンからサウンドの大きさを引き継ぐ
            BgmPlayer.Instance.Volume = SoundVolumeController.BgmVolume;
            SePlayer.Instance.Volume = SoundVolumeController.SeVolume;
            StartCoroutine(GameStartCoroutine());
        }

        void Update()
        {
            //カウントダウン中は操作不可能にする
            if (isCountingDown) return;

            //Spaceキーを押すとゲームを一時停止させる
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canPlaying = !canPlaying;
                onChangePausePanel();
            }

            //一時停止中はここでリターン
            if (!canPlaying) return;

            //時間に関する処理、時間が0になったらシーン移動
            if(gameTime > 0)
            {
                gameTime -= Time.deltaTime;
            }
            else
            {
                mainAndEneConnecter.LoadEndScene(money.MoneyAmount);
            }

            //移動に関する処理
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //場外に行かないようにする
                if (player.PlayerPos.y == 0) return;
                //移動先が通行不可なら移動しない
                if (!moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y - 1].ReturnCanPassing()) return;
                //プレイヤーを移動させる
                player.PlayerMove(MoveVector.Up, tileSize);
                //ポイントを計算する
                CalculateMoney();
                //周りに移動可能なTileがあるか調べる
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
            //カウントダウン開始
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

            //ゲームをスタートさせる
            //計算記号をランダムに並び替える
            mathematicalSymbols = defaltMathematicalSymbols.OrderBy(i => Guid.NewGuid()).ToArray();
            //UIの変更（生成）をおこなう
            onChengeStageLebelUI();
            onChangePointUI();
            onChangeMathTypeUI();
            //ステージを生成
            CreateTileView();
            SetTilesData();
            CreatePlayer();

            onEndCountDownUI();
            BgmPlayer.Instance.Play(0);

            isCountingDown = false;
        }

        //ステージを作り直す処理
        private void ChangeNextStage()
        {
            //ステージレベル周り(タイル破壊とかもここでやってる)
            stageLebel.StageLebelUp(money.MoneyAmount);
            onChengeStageLebelUI();
            //Tileにデータを入れ直す
            SetTilesData();
            CreatePlayer();
            //数学記号を並べ直す
            mathematicalSymbols = defaltMathematicalSymbols.OrderBy(i => Guid.NewGuid()).ToArray();
            calculationNumber = 0;
            onChangeMathTypeUI();
        }

        //Tileの見た目・データすべて破壊する
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

        //Tileの見た目を生成する処理
        private void CreateTileView()
        {
            //2次元配列の要素数が変更したので新しく宣言する
            moneyTiles = new MoneyTileManager[stageLebel.FieldSize, stageLebel.FieldSize];

            //FieldSizeに合わせてTileの大きさを変更する
            //縦横12*12に収まるようなサイズにする
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
                    Debug.Log("Tileの大きさが不明です");
                    break;
            }

            //生成用のPrefabの大きさを変更する
            moneyTilePrefab.transform.localScale = new Vector3(tileSize, tileSize, 1);

            //Tileの横幅を取得する
            float distance = moneyTilePrefab.transform.localScale.x;

            for (int i = 0; i < stageLebel.FieldSize; i++)
            {
                for (int k = 0; k < stageLebel.FieldSize; k++)
                {
                    //生成する(中心を基準に生成させる)
                    Vector3 createPos = new Vector3(-6f + (distance / 2) + (distance * k), 5.4f - (distance / 2) - (distance * i), 0);
                    MoneyTileManager numberTile = Instantiate(moneyTilePrefab, createPos, Quaternion.identity);
                    numberTile.transform.SetParent(this.transform);
                    moneyTiles[k, i] = numberTile;
                }
            }
        }

        //プレイヤーの大きさを変更する
        private void ChangePlayerSize()
        {
            player.ChangePlayerSize(stageLebel.FieldSize);
        }

        //エンドシーンに移動する際、操作不可にする処理
        private void EndGame()
        {
            canPlaying = false;
        }

        //lvArray{4,2,4,2,2,0,0,0,0}を渡すと{1,1,1,1,10,10,100,100,100,100,500,500,…}のように変換してくれるメソッド
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

        //Tileにデータを入れる処理
        private void SetTilesData()
        {
            //前のアイデアの遺産
            /*
            //フィールド上のTileに入れるポイントを用意する
            float maxPoint = Mathf.Pow(stageLebel.FieldSize, 2) * 5;
            //n^2分の個数のポイントを用意(36コ)
            randomPoints = new int[(int)Mathf.Pow(stageLebel.FieldSize, 2)];

            //一旦すべてに5を入れる
            for (int i = 0; i < randomPoints.Length; i++)
            {
                randomPoints[i] = (int)maxPoint / randomPoints.Length;
            }

            //1～4の数を生成して足し引きを繰り返す
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

            //現在のステージのレベルを取得する
            int lebel = stageLebel.StageLebel;

            //レベルに合わせたタイルを取得
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
            //シャッフルする
            randomAmounts = randomAmounts.OrderBy(i => Guid.NewGuid()).ToArray();

            //ゴールを設定する
            int goalPosX = UnityEngine.Random.Range(1, stageLebel.FieldSize);
            int goalPosY = UnityEngine.Random.Range(1, stageLebel.FieldSize);

            //データをTileに設定する
            for (int i = 0; i < stageLebel.FieldSize; i++)
            {
                for (int k = 0; k < stageLebel.FieldSize; k++)
                {
                    //一旦onGoalをすべて空にする(これしないとレベルアップ(onGoal())が複数呼ばれて大変なことになる)
                    //moneyTiles[k, i].onGoal?.Invoke();
                    moneyTiles[k, i].onGoal -= ChangeNextStage;
                    
                    //データをそれぞれ設定する
                    moneyTiles[k, i].onGoal += ChangeNextStage;

                    moneyTiles[k, i].SetData(randomAmounts[i * 6 + k], tileSize);

                    //乱数と一致したTileをゴールとして設定する
                    if (k == goalPosX && i == goalPosY)
                    {
                        moneyTiles[k, i].SetGoal();
                    }
                }
            }
        }

        //Playerの位置を初期化するクラス
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

        //四方が囲まれていないかチェックする
        private void CheckCanMove()
        {
            //配列の要素数以上・以下の数にならないようにする
            int posX = (int)player.PlayerPos.x;
            int posY = (int)player.PlayerPos.y;
            int rightPos = Mathf.Clamp(posX + 1, 0, stageLebel.FieldSize - 1);
            int leftPos = Mathf.Clamp(posX - 1, 0, stageLebel.FieldSize - 1);
            int upPos = Mathf.Clamp(posY + 1, 0, stageLebel.FieldSize - 1);
            int downPos = Mathf.Clamp(posY - 1, 0, stageLebel.FieldSize - 1);

            //周りが囲まれていないかチェックする
            //囲まれていたらゲームオーバーにする
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

        //金額を計算する処理
        private void CalculateMoney()
        {
            //UIをポップアップ表示させる
            onPopUpPointText(mathematicalSymbols[calculationNumber], moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnTilePoint());
            //演算子によって計算記号を変える
            money.ChangeMoneyAmount(moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnTilePoint(), mathematicalSymbols[calculationNumber]);

            //演算子を順番に入れ替えるための処理
            if (calculationNumber < mathematicalSymbols.Length - 1)
            {
                calculationNumber++;
            }
            else
            {
                calculationNumber = 0;
            }

            //ゴールかどうか調べる
            moneyTiles[(int)player.PlayerPos.x, (int)player.PlayerPos.y].ReturnGoal();

            //UIの変更をおこなう
            onChangePointUI();
            onChangeMathTypeUI();
        }
    }
}