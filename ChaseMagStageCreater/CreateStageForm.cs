using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ChaseMagStageCreater
{
    public partial class CreateStageForm : Form
    {                                                    
        // ラジオボタンとカテゴリーの関連
        private Dictionary<RadioButton, StagePartsCategory> categoryPairs;

        // 管理クラス
        private ZoomManager zoomManager;            // ズームを管理
        private StageDataManager stageDataManager;  // ステージデータと対応するピクチャーボックスを管理
        private StageSizeManager sizeManager;       // ステージのピクチャーボックスを管理するクラス
        private GuideViewManager guideManager;       // ステージのピクチャーボックスを管理するクラス

        private int preIndex;                       // 前に選択していたインデックス

        private string nowOpenPath;                 // 現在開いているファイルへのパス

        private bool noModFrag;


        private readonly Vector2 defaultSize = new Vector2(80,20);
        // ステージサイズの初期値

        private readonly string formTitle = "ChaseMag StageCreater";

        public CreateStageForm()
        {
            InitializeComponent();
        }

        // 初期化
        private void CreateStageForm_Load(object sender, EventArgs e)
        {
            this.Text = formTitle;

            // 必要なクラスの生成
            StageData stageData = new StageData();
            stageData.width = defaultSize.x;
            stageData.height = defaultSize.y;

            sizeManager = new StageSizeManager(stageData, maxStagePicture, InStageViewPicture, zoomStagePicture);
            zoomManager = new ZoomManager(InStageViewPicture, maxStagePicture, zoomStagePicture, viewMoveModeButton);
            stageDataManager = new StageDataManager(stageData, InStageViewPicture, zoomStagePicture,ChangeSelect, partsMoveModeButtom);
            guideManager = new GuideViewManager(
                topleftLabel,
                topRightLabel,
                bottomLeftLabel,
                bottomRightLabel,
                InStageViewPicture,
                zoomStagePicture,
                stageData);
            zoomManager.debugLabel = debuglabel2;
            stageDataManager.debugLabel = label1;
            sizeManager.debugLabel = label2;


            // ラジオボタンとカテゴリーの関連付け
            categoryPairs = new Dictionary<RadioButton, StagePartsCategory>();
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                categoryPairs.Add((RadioButton)flowLayoutPanel1.Controls[i],(StagePartsCategory)i);
            }

            preIndex = -1;
            addModeButtom.Checked = true;

            // 表示の初期化
            widthSize.Value = (decimal)defaultSize.x;
            heightSize.Value = (decimal)defaultSize.y;

            nowOpenPath = string.Empty;
            msgText.Text = string.Empty;
        }

        // 終了する
        private void ExitAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        // 終了時
        private void CreateStageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
              "終了してもいいですか？", "確認",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question
                ) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


        // ********************************************************
        // ファイル操作のメソッド群
        // ********************************************************

        #region fileManageMethods
        // ファイルダイアログを開く
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            openFileDialog1.ShowDialog();

        }

        // jsonファイルからデータをインポート
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {


            StageData data = JsonManager.ImportData(openFileDialog1.FileName);

            stageDataManager.SetNewData(data, this.Controls);

            msgText.Text = openFileDialog1.FileName + "読み込みました";
            nowOpenPath = openFileDialog1.FileName;
            this.Text = formTitle + "  " + nowOpenPath;

            // 描画の更新処理
            sizeManager.ChangeStageSize();
            ViewDataClear();
            ViewUpdate();



        }


        // 上書き保存を選択
        private void OverWriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stageDataManager.stageData.stageParts.Count <= 0)
            {

                msgText.Text = "リストにデータを入れてください";
                return;
            }

            if (nowOpenPath == string.Empty)
            {
                saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";
                saveFileDialog1.ShowDialog();
                return;

            }

            JsonManager.ExportData(nowOpenPath,stageDataManager.stageData);
            msgText.Text = nowOpenPath + "保存しました";
            this.Text = formTitle + "  " + nowOpenPath;

        }

        // jsonファイルへ出力
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            JsonManager.ExportData(saveFileDialog1.FileName, stageDataManager.stageData);
            nowOpenPath = saveFileDialog1.FileName;
            msgText.Text = nowOpenPath + "保存しました";
            this.Text = formTitle + "  " + nowOpenPath;

        }

        // 保存ダイアログを開く
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            saveFileDialog1.ShowDialog();

        }

        // jsonファイル新規作成
        private void CreateNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "新規作成";

            saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            saveFileDialog1.ShowDialog();
        }

        #endregion



        // ********************************************************
        // フォーム上の表示更新
        // ********************************************************

        // リスト内の選択状況が変わった
        private void partsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            stageDataManager.DeSelected(preIndex);

            if (IndexOutOfRange()) return;

            stageDataManager.SelectedPicture(partsListBox.SelectedIndex);

            preIndex = partsListBox.SelectedIndex;

            StagePart part = stageDataManager.GetPart(partsListBox.SelectedIndex);
            SetViewPartData(part);
        }

        private void ChangeSelect(int index)
        {
            partsListBox.SelectedIndex = index;
        }

        // パーツのデータをフォーム上に表示する
        private void SetViewPartData(StagePart part)
        {
            noModFrag = true;
            partsListBox.SelectedIndex = stageDataManager.stageData.stageParts.IndexOf(part);
            positionX.Value = (decimal)part.position.x;
            positionY.Value = (decimal)part.position.y;
            sizeX.Value = (decimal)part.sizeMagnification.x;
            sizeY.Value = (decimal)part.sizeMagnification.y;
            if (StagePart.IsSetPole(part.category))
            {
                polePanel.Visible = true;
                if (part.isNorth)
                {
                    northButton.Checked = true;
                }
                else
                {
                    southButton.Checked = true;
                }
                return;
            }

            polePanel.Visible = false;
            noModFrag = false;
        }


        // リストの項目、ステージのサイズを入れなおす
        private void ViewUpdate()
        {

            partsListBox.Items.Clear();
            for (int i = 0; i < stageDataManager.GetPartsCouont(); i++)
            {
                partsListBox.Items.Add((i + 1).ToString() +" "+ stageDataManager.GetPart(i).category.ToString());
            }

        }


        // フォーム上のデータの数値を初期化する
        private void ViewDataClear()
        {
            preIndex = -1;
            SetViewPartData(new StagePart());

        }

        // ********************************************************
        // 新しくパーツを追加する処理
        // ********************************************************

        // 追加ボタンをクリックする
        private void addButton_Click(object sender, EventArgs e)
        {
            Vector2 defaultPosition = new Vector2(0.0f, 1.0f);
            AddStageParts(defaultPosition);
        }

        // 新しいパーツを追加する
        private bool AddStageParts(Vector2 position)
        {

            // 指定したグループ内のラジオボタンでチェックされている物を取り出す
            RadioButton checkedCategory = flowLayoutPanel1.Controls.OfType<RadioButton>()
                .SingleOrDefault(rb => rb.Checked == true);

            if (checkedCategory == null)
            {
                msgText.Text = "パーツの種類を選択してください";
                scaffoldButton.Checked = true;
                return false;
            }
            stageDataManager.AddNewPart(categoryPairs[checkedCategory], position);

            partsListBox.Items.Add((partsListBox.Items.Count).ToString() + " " + categoryPairs[checkedCategory].ToString());

            partsListBox.SelectedIndex = partsListBox.Items.Count - 1;
            return true;
        }


        // ステージ内をクリックする
        private void InStagePicture_Click(object sender, MouseEventArgs e)
        {
            // 左クリック
            if (e.Button == MouseButtons.Left)
            {
                // 追加モードだったらパーツを追加する
                if (addModeButtom.Checked)
                {
                    // 位置の調整
                    float magnification = zoomStagePicture.Width / stageDataManager.stageData.width;
                    Point baseLocation = StageDataManager.GetBasePoint(zoomStagePicture);

                    AddStageParts(
                        LocationConverter.LocationToStagePositionIncrement(PointToClient(MousePosition),
                        baseLocation,
                        magnification));


                }

            }



        }

        // ********************************************************
        // 削除処理
        // ********************************************************

        // 削除ボタンをクリック
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            
            if (IndexOutOfRange())
            {
                msgText.Text = "削除するパーツを選択してください";
                return;
            }

            stageDataManager.DeletePart(partsListBox.SelectedIndex);
            ViewDataClear();

            msgText.Text = (partsListBox.SelectedIndex+1).ToString() + "削除しました";

            ViewUpdate();

        }

        // すべてのパーツを削除する
        private void AllDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                  "すべてのパーツを削除しますか？", "確認",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    ) == DialogResult.No)
            {
                return;
            }

            // 後ろから回す
            for (int i = stageDataManager.GetPartsCouont()-1; i >= 0; i--)
            {
                stageDataManager.DeletePart(i);

            }
            ViewDataClear();

            msgText.Text = "削除しました";

            ViewUpdate();


        }

        // ********************************************************
        // フォーム上からの値変更
        // ********************************************************

        // 位置の値を変更する(X)
        private void position_ValueChanged_X(object sender, EventArgs e)
        {
            if (IndexOutOfRange() || noModFrag) return;

            Vector2 newPosition = new Vector2((float)positionX.Value,stageDataManager.GetPart(partsListBox.SelectedIndex).position.y);
            stageDataManager.UpdatePosition(partsListBox.SelectedIndex,newPosition);


        }
        // 位置の値を変更する(Y)
        private void position_ValueChanged_Y(object sender, EventArgs e)
        {
            if (IndexOutOfRange() || noModFrag) return;

            Vector2 newPosition = new Vector2(stageDataManager.GetPart(partsListBox.SelectedIndex).position.x, (float)positionY.Value);
            stageDataManager.UpdatePosition(partsListBox.SelectedIndex, newPosition);
        }

        // サイズの値を変更する(X)
        private void size_ValueChanged_X(object sender, EventArgs e)
        {
            if (IndexOutOfRange() || noModFrag) return;

            Vector2 newSizeMagnification = new Vector2((float)sizeX.Value, stageDataManager.GetPart(partsListBox.SelectedIndex).sizeMagnification.y);
            stageDataManager.UpdateSize(partsListBox.SelectedIndex, newSizeMagnification);


        }

        // サイズの値を変更する(Y)
        private void size_ValueChanged_Y(object sender, EventArgs e)
        {
            if (IndexOutOfRange() || noModFrag) return;

            Vector2 newSizeMagnification = new Vector2(stageDataManager.GetPart(partsListBox.SelectedIndex).sizeMagnification.x, (float)sizeY.Value);
            stageDataManager.UpdateSize(partsListBox.SelectedIndex, newSizeMagnification);
        }

        // 磁力の向きを変更する
        private void poleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (IndexOutOfRange() || noModFrag) return;

            stageDataManager.UpdatePole(partsListBox.SelectedIndex,northButton.Checked);
        }


        // ********************************************************
        // ステージのサイズを変更する
        // ********************************************************
        private void Size_ValueChanged_Width(object sender, EventArgs e)
        {
            stageDataManager.stageData.width = (float)widthSize.Value;
            sizeManager.ChangeStageSize();


        }
        private void Size_ValueChanged_Height(object sender, EventArgs e)
        {
            stageDataManager.stageData.height = (float)heightSize.Value;
            sizeManager.ChangeStageSize();

        }


        // ********************************************************
        // チェック
        // ********************************************************

        // リストの添え字が範囲外か
        private bool IndexOutOfRange()
        {
            return partsListBox.SelectedIndex < 0 || partsListBox.SelectedIndex >= stageDataManager.GetPartsCouont();
        }


        // ********************************************************
        // ショートカット
        // ********************************************************
        // キー入力でモード切替、パーツ削除
        private void CreateStageForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    addModeButtom.Checked = true;
                    break;
                case Keys.E:
                    partsMoveModeButtom.Checked = true;
                    break;
                case Keys.R:
                    viewMoveModeButton.Checked = true;
                    break;
                case Keys.Delete:
                case Keys.Back:
                    DeleteButton_Click(sender,e);

                    break;
            }

        }

    }


}
