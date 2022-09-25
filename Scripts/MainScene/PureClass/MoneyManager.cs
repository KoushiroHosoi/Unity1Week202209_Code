using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System.Numerics;

namespace HamuGame
{
    public class MoneyManager 
    {
        //合計金額（スコア）
        private BigInteger moneyAmount;

        //最大値(1不可思議にする(1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0))
        private BigInteger maxMoneyAmount;

        public BigInteger MoneyAmount { get => moneyAmount; }

        //初期化
        public MoneyManager()
        {
            moneyAmount = new BigInteger(decimal.MaxValue);
            moneyAmount = 0;
            maxMoneyAmount = BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000");
        }

        //変化量と数学記号を渡して計算する
        public void ChangeMoneyAmount(int value, MathematicalSymbolType mathematicalSymbol)
        {
            switch (mathematicalSymbol)
            {
                case MathematicalSymbolType.plus:
                    moneyAmount += value;
                    break;
                case MathematicalSymbolType.minus:
                    moneyAmount -= value;
                    break;
                case MathematicalSymbolType.multiplied:
                    moneyAmount *= value;
                    break;
                case MathematicalSymbolType.divided:
                    moneyAmount /= value;
                    break;
            }

            //0～9999那由多に収まるように調整する
            if(moneyAmount < 0)
            {
                moneyAmount = 0;
            }
            else if(moneyAmount > maxMoneyAmount)
            {
                moneyAmount = BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000") - 1;
            }
        }
    }
}
