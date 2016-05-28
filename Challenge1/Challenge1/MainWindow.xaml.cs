using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Challenge1 {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow {

        private const string DialogFilter = "Text Files (.txt)|*.txt";

        public MainWindow() {
            this.InitializeComponent();
            //ハンドラ設定
            this.SetHandlers();
            //画面サイズ設定
            this.DataContext = new { WindowHeight = 550, WindowWidth = 725 };
        }
        

        #region イベント設定

        /// <summary>
        /// ハンドラ設定
        /// </summary>
        private void SetHandlers() {
            //エラーハンドラ設定
            Application.Current.DispatcherUnhandledException += this.Application_UnhandledException;
            //ボタンイベントハンドラ設定
            this.inputButton.Click += this.InputButton_Click;
            this.outputButton.Click += this.OutputButton_Click;
        }

        /// <summary>
        /// エラーハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            this.ShowErrorMessage();
            this.previewTextBlock.Text = e.Exception.Message;
            e.Handled = true;
        }
        
        /// <summary>
        /// 変換ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputButton_Click(object sender, RoutedEventArgs e) {
            this.DoInput();
        }

        /// <summary>
        /// 出力ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputButton_Click(object sender, RoutedEventArgs e) {
            this.DoOutput();
        }
        
        #endregion

        /// <summary>
        /// テキストファイルを読み込み､ひらがなに変換し､textBlockに表示
        /// </summary>
        private void DoInput() {
            var openFileDialog = new OpenFileDialog() { FilterIndex = 1, Filter = DialogFilter };
            if (openFileDialog.ShowDialog() != true) { return; }
            
            string readText;
            using (var fileStream = openFileDialog.OpenFile())
            using (var reader = new StreamReader(fileStream, true)) {
                readText = reader.ReadToEnd();
            }

            this.previewTextBlock.Text = new ConvertHiragana().RomanToHiragana(readText);

            this.ShowSuccessMessage();
        }

        /// <summary>
        /// textBlockに表示されている値でテキストファイルを作成､保存
        /// </summary>
        private void DoOutput() {
            var saveText = this.previewTextBlock.Text;
            if (String.IsNullOrWhiteSpace(saveText)) { return; }

            var saveFileDialog = new SaveFileDialog() { FilterIndex = 1, Filter = DialogFilter };
            if (saveFileDialog.ShowDialog() != true) { return; }

            using (var fileStream = saveFileDialog.OpenFile())
            using (var writer = new StreamWriter(fileStream)) {
                writer.Write(saveText);
            }

            this.ShowSuccessMessage();
        }

        #region メッセージ関数

        /// <summary>
        /// 正常終了メッセージ
        /// </summary>
        private void ShowSuccessMessage() {
            this.ShowMessage("正常終了したよ");
        }

        /// <summary>
        /// 異常終了メッセージ
        /// </summary>
        private void ShowErrorMessage() {
            this.ShowMessage("異常終了したよ");
        }

        /// <summary>
        /// メッセージ出力用
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message) {
            MessageBox.Show(this.GetMoomin(message));
        }

        /// <summary>
        /// 遊び心を忘れてはいけない
        /// </summary>
        /// <returns></returns>
        private string GetMoomin(string message) {
            var moomin = String.Format(@"
　　　　　　　　　　　 ∧　 ∧ 
　　　　　　　　　　　 |1/　|1/ 
　　　　　　　　　 ／￣￣￣｀ヽ、 
　　　　　　　　 /　　　　　　　ヽ 
　　　　　　　　/　  ⌒　 ⌒　　　| 
　　　　　　　　|　（●） （●）　  | 　　>{0}
　　　　　　　  /　　　　　　　　 | 
　　　　　　   /　　　　　　　　　| 
　　　　　　（　　　　　　　　＿ | 
　　　　　　（ヽ、　　　　　/      )| 
　　　　　　   |　｀`ー――‐''"" |    ヽ |
                           ゝノ                     ヽ  ノ
", message);
            return moomin;
        }

        #endregion

    }
}