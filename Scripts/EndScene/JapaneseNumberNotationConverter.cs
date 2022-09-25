using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using UnityEngine;

public static class JapaneseNumberNotationConverter
{
    // 無量大数を宣言する
    //BigInteger.Parse(変換したい数値,数値に関するカルチャ固有の書式情報を提供するオブジェクト)でBigIntegerを返す
    private static readonly BigInteger 無量大数 = BigInteger.Parse("100000000000000000000000000000000000000000000000000000000000000000000", CultureInfo.InvariantCulture);

    //桁単位の文字列の配列、Stringよりもstringを使うべし
    // 9999以下も扱えるように、末尾に空文字列を追加しておきます。
    // ところで「𥝱」だけUTF-16で4バイト必要になる文字(いわゆる「サロゲートペア」が必要になる文字)なので、表示されない
    private static string[] units = { "無量大数", "不可思議", "那由他", "阿僧祇", "恒河沙", "極", "載", "正", "澗", "溝", "穣", "予", "垓", "京", "兆", "億", "万", string.Empty };

    /// <summary>万、億等の日本の補助単位を用いた文字列表現に変換します。</summary>
    public static string ToJapaneseNumberNotation(BigInteger value)
    {
        //0が渡されたら0を返す
        if (value == BigInteger.Zero)
        {
            return "0";
        }
        
        //StringBuilderはstringの強化版
        StringBuilder builder = new StringBuilder();

        if (value < 0)
        {
            //"マイナス○○と表示するようにする"
            // ありえるかは分かりませんが、何か適当に最初に付き足しておきます。
            builder.Append("マイナス");
            // valueを正の数に直す
            value = BigInteger.Abs(value);
        }
        
        //divisor：約数
        BigInteger divisor = 無量大数;

        //例えば10012000000(100億1200万)渡されたら
        //100億1200万 >= divisor になるまでdivisorを1万で割る かつ 桁を一つ下げる(兆→億など)
        //100億1200万 >= divisor になったら 100億1200万/1億 をしてそれのあまり(1200万)をvalueに格納する かつ 答えの100に億をつける
        //今度は1200万 >= divisor になったので 1200万/1万 をして答えの1200に万をつける
        for (int i = 0; i < units.Length && value > 0; i++, divisor /= 10000)
        {
            if (value >= divisor)
            {
                //Debug.Log("value is " + value + " divisor is " + divisor);
                //Debug.Log("結果は " + (value / divisor) + units[i] + " 余りは " + (value % divisor));
                // 無量大数よりも上の補助単位はないので、"10000無量大数"等の表記を許容することにします
                builder.Append((value / divisor).ToString(CultureInfo.InvariantCulture) + units[i]);
                value %= divisor;
            }
        }
        
        return builder.ToString();
    }
}
