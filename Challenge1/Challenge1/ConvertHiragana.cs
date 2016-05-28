using System.Collections.Generic;
using System.Linq;

class ConvertHiragana {

    /// <summary>
    /// ローマ字変換定義ファイル名
    /// </summary>
    private const string RomanTableFileName = "romantable.txt";

    /// <summary>
    /// ローマ字変換テーブル
    /// </summary>
    private readonly Dictionary<string, string> RomanTable;


    public ConvertHiragana() {
        this.RomanTable = this.GetRomanTable();
    }

    /// <summary>
    /// ローマ字文字列をひらがな文字列に変換
    /// </summary>
    /// <param name="roman"></param>
    /// <returns></returns>
    public string RomanToHiragana(string roman) {
        roman = roman.Trim().ToLower();
        string result = "", romanTemporary = "";

        foreach (var r in roman.Select((c, i) => new { Value = c, IsLastLoop = (++i == roman.Length) })) {

            romanTemporary += r.Value;

            //母音チェックと母音以外で例外的に通したい文字列チェック
            var isConvertingTarget = System.Text.RegularExpressions.Regex.IsMatch(romanTemporary, @"[aeiou,.\-\s]|nn|xn");

            if (!isConvertingTarget && !r.IsLastLoop) { continue; }

            if (!this.RomanTable.ContainsKey(romanTemporary)) {
                string firstChar = romanTemporary.Substring(0, 1),
                       secondChar = (romanTemporary.Length > 1) ? romanTemporary.Substring(1, 1) : "";

                //促音判定
                //falseの場合は適当な入力じゃなければ｢n｣or｢\s｣or改行文字(のはず)
                var convertingTarget = firstChar + ((firstChar == secondChar) ? secondChar : "");

                result += this.ToHiragana(convertingTarget);
                romanTemporary = romanTemporary.Substring(1);
            }

            result += this.ToHiragana(romanTemporary);
            romanTemporary = "";
        }

        return (result.Length != 0) ? result : roman;
    }

    /// <summary>
    /// ローマ字文字列をひらがな文字に変換 ※関数名どうにかしたい
    /// </summary>
    /// <param name="roman"></param>
    /// <returns></returns>
    private string ToHiragana(string roman) {
        string result;
        return this.RomanTable.TryGetValue(roman, out result) ? result : roman;
    }

    /// <summary>
    /// Google日本語入力のローマ字変換テーブル(TSV)を元に作成
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, string> GetRomanTable() {
        var romanTable = System.IO.File.ReadAllLines(RomanTableFileName);
        return romanTable.Select(e => e.Split('\t')).ToDictionary(e => e[0], e => e[1]);
    }
}