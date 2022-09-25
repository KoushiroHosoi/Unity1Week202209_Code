using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System.Numerics;

namespace HamuGame
{
    public class MoneyManager 
    {
        //���v���z�i�X�R�A�j
        private BigInteger moneyAmount;

        //�ő�l(1�s�v�c�ɂ���(1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0))
        private BigInteger maxMoneyAmount;

        public BigInteger MoneyAmount { get => moneyAmount; }

        //������
        public MoneyManager()
        {
            moneyAmount = new BigInteger(decimal.MaxValue);
            moneyAmount = 0;
            maxMoneyAmount = BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000");
        }

        //�ω��ʂƐ��w�L����n���Čv�Z����
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

            //0�`9999�ߗR���Ɏ��܂�悤�ɒ�������
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
