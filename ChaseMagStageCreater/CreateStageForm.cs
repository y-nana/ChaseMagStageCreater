using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
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

        private int preIndex;                       // 前に選択していたインデックス
        private int dragPictureIndex;               // ドラッグ中のインデックス

        private string nowOpenPath;                 // 現在開いているファイルへのパス

        private float stageBoxRatio;                // 最大の縦横比
        private Size maxStagePictureSize;           // ステージのピクチャーボックスの最大サイズ

        private bool isClicked;                     // クリックされた状態か
        private Point mouseStartPos;                // クリックし始めの位置
        private float startZoomLocation;            // し始めの位置

        private readonly int insideStageMargin = 10;// ステージ外の壁の余白の大きさ

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
            zoomManager = new ZoomManager(new Vector2(InStagePicture.Size.Width, InStagePicture.Size.Height));
            stageDataManager = new StageDataManager(defaultSize.x, defaultSize.y, InStagePicture, zoomManager, AddEvent);

            // フォーム上でステージとして見せるピクチャーボックスの最大サイズを算出
            maxStagePictureSize = new Size(stageBox.Size.Width - insideStageMargin * 2, stageBox.Size.Height - insideStageMargin * 2);
            // ステージのピクチャーボックスの縦横比を算出
            stageBoxRatio = maxStagePictureSize.Width / (float)maxStagePictureSize.Height;


            // ラジオボタンとカテゴリーの関連付け
            categoryPairs = new Dictionary<RadioButton, StagePartsCategory>();
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                categoryPairs.Add((RadioButton)flowLayoutPanel1.Controls[i],(StagePartsCategory)i);
            }

            preIndex = -1;
            dragPictureIndex = -1;
            addModeButtom.Checked = true;

            // 表示の初期化
            widthSize.Value = (decimal)defaultSize.x;
            heightSize.Value = (decimal)defaultSize.y;

            nowOpenPath = string.Empty;
            msgText.Text = string.Empty;

            ViewUpdate();
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
            ViewUpdate();
            ViewDataClear();
            ChangeStageSize();

     

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

        // パーツのデータをフォーム上に表示する
        private void SetViewPartData(StagePart part)
        {
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
        }


        // リストの項目、ステージのサイズを入れなおす
        private void ViewUpdate()
        {

            partsListBox.Items.Clear();
            for (int i = 0; i < stageDataManager.GetPartsCouont(); i++)
            {
                partsListBox.Items.Add((i + 1).ToString() +" "+ stageDataManager.GetPart(i).category.ToString());
            }

            heightSize.Value = (decimal)stageDataManager.stageData.height;
            widthSize.Value = (decimal)stageDataManager.stageData.width;
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
            stageDataManager.AddNewPart(categoryPairs[checkedCategory], position,this.Controls);

            ViewUpdate();
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
                    Point baseLocation = StageDataManager.GetBasePoint(InStagePicture);
                    baseLocation.Offset((int)Math.Round(zoomManager.zoomLocation), 0);

                    AddStageParts(
                        LocationConverter.LocationToStagePosition(PointToClient(MousePosition),
                        baseLocation,
                        stageDataManager.magnification));

                }
                // 視点移動モードでズーム中だったら視点を移動する
                else if (viewMoveModeButton.Checked)
                {

                    Point mouseLocation = PointToClient(MousePosition);

                    // マウスの位置とステージの中心を比較
                    bool isRight = mouseLocation.X > InStagePicture.Location.X + InStagePicture.Width * 0.5f;
                    zoomManager.ChangeFocus(isRight);

                    // 表示の更新
                    PictureViewReflesh();

                }
            }
            //右クリック
            // 視点移動
            else if (e.Button == MouseButtons.Right)
            {
                isClicked = true;
                startZoomLocation = zoomManager.zoomLocation;
                mouseStartPos = PointToClient(MousePosition);
            }


        }
        // ********************************************************
        // 視点移動
        // ********************************************************
        private void InStagePicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isClicked)
            {
                Point mousePos = PointToClient(MousePosition);
                int deltaMove = mousePos.X - mouseStartPos.X;
                zoomManager.SetForcus(deltaMove + startZoomLocation);
                // 不安定
                //PictureViewReflesh();
            }
        }

        private void InStagePicture_MouseUp(object sender, MouseEventArgs e)
        {
            isClicked = false;
            PictureViewReflesh();

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
            if (IndexOutOfRange()) return;

            Vector2 newPosition = new Vector2((float)positionX.Value,stageDataManager.GetPart(partsListBox.SelectedIndex).position.y);
            stageDataManager.UpdatePosition(partsListBox.SelectedIndex,newPosition);


        }
        // 位置の値を変更する(Y)
        private void position_ValueChanged_Y(object sender, EventArgs e)
        {
            if (IndexOutOfRange()) return;

            Vector2 newPosition = new Vector2(stageDataManager.GetPart(partsListBox.SelectedIndex).position.x, (float)positionY.Value);
            stageDataManager.UpdatePosition(partsListBox.SelectedIndex, newPosition);
        }

        // サイズの値を変更する(X)
        private void size_ValueChanged_X(object sender, EventArgs e)
        {
            if (IndexOutOfRange()) return;

            Vector2 newSizeMagnification = new Vector2((float)sizeX.Value, stageDataManager.GetPart(partsListBox.SelectedIndex).sizeMagnification.y);
            stageDataManager.UpdateSize(partsListBox.SelectedIndex, newSizeMagnification);


        }

        // サイズの値を変更する(Y)
        private void size_ValueChanged_Y(object sender, EventArgs e)
        {
            if (IndexOutOfRange()) return;

            Vector2 newSizeMagnification = new Vector2(stageDataManager.GetPart(partsListBox.SelectedIndex).sizeMagnification.x, (float)sizeY.Value);
            stageDataManager.UpdateSize(partsListBox.SelectedIndex, newSizeMagnification);
        }

        // 磁力の向きを変更する
        private void poleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (IndexOutOfRange()) return;

            stageDataManager.UpdatePole(partsListBox.SelectedIndex,northButton.Checked);
        }


        // ********************************************************
        // ピクチャーボックスをドラッグで操作するためのメソッド群
        // ********************************************************

        // ピクチャーボックスにイベントを追加する
        private void AddEvent(PictureBox picture)
        {
            picture.MouseDown += StagePartMouseDown;
            picture.MouseUp += StagePartMouseUp;
            picture.MouseMove += StagePartMouseMove;
        }

        // ドラッグでパーツを移動し始める
        private void StagePartMouseDown(object sender, MouseEventArgs e)
        {
            int index = stageDataManager.pictures.IndexOf((PictureBox)sender);
            partsListBox.SelectedIndex = index;
            if (partsMoveModeButtom.Checked && e.Button == MouseButtons.Left)
            {
                dragPictureIndex = index;
            }


        }

        // ドラッグでパーツを移動させる
        private void StagePartMouseMove(object sender, MouseEventArgs e)
        {
            if (dragPictureIndex < 0)
            {
                return;
            }

            Point mouseLocation = PointToClient(MousePosition);
            // ドラッグでステージ外へ出せないように
            if (mouseLocation.X < InStagePicture.Location.X ||
                mouseLocation.X + stageDataManager.pictures[dragPictureIndex].Width * 0.5f > InStagePicture.Location.X + InStagePicture.Size.Width||
                mouseLocation.Y < InStagePicture.Location.Y || 
                mouseLocation.Y + stageDataManager.pictures[dragPictureIndex].Height * 0.5f> InStagePicture.Location.Y + InStagePicture.Size.Height)
            {
                return;
            }

            PointF picLocation = PointToClient(MousePosition);
            picLocation.X -= stageDataManager.pictures[dragPictureIndex].Width * 0.5f;
            picLocation.Y -= stageDataManager.pictures[dragPictureIndex].Height * 0.5f;
            stageDataManager.pictures[dragPictureIndex].Location = Point.Round(picLocation);

        }

        // パーツの移動終了
        private void StagePartMouseUp(object sender, MouseEventArgs e)
        {
            if (dragPictureIndex < 0)
            {
                return;
            }
            if (partsMoveModeButtom.Checked)
            {
                // データへ反映する
                stageDataManager.PictureLocationApply((PictureBox)sender);
                SetViewPartData(stageDataManager.GetPart((PictureBox)sender));
            }
            dragPictureIndex = -1;

        }


        // ********************************************************
        // ズーム
        // ********************************************************

        // ホイールでズームする
        private void InStagePicture_MouseWheel(object sender, MouseEventArgs e)
        {
            bool zoomIn = e.Delta > 0;
            if (zoomIn && (!CanZoom()||zoomManager.isZoomed))
            {
                return;
            }

            zoomManager.ZoomChange(zoomIn);
            Point mouseLocation = PointToClient(MousePosition);

            // マウスの位置のところへフォーカス
            float delta = mouseLocation.X - (InStagePicture.Location.X + InStagePicture.Width * 0.5f);
            zoomManager.ChangeFocus(-delta);
            PictureViewReflesh();


        }

        // ********************************************************
        // ステージのサイズを変更する
        // ********************************************************
        private void Size_ValueChanged_Width(object sender, EventArgs e)
        {
            stageDataManager.stageData.width = (float)widthSize.Value;

        }
        private void Size_ValueChanged_Height(object sender, EventArgs e)
        {
            //stageDataManager.stageData.width = (float)widthSize.Value;
            stageDataManager.stageData.height = (float)heightSize.Value;

            ChangeStageSize();

        }

        private void ChangeStageSize()
        {
            // 縦と横の比率によってどちらを限界まで伸ばすか決める
            if (stageDataManager.stageData.width >= stageDataManager.stageData.height * stageBoxRatio)
            {
                stageDataManager.magnification = maxStagePictureSize.Width / stageDataManager.stageData.width;
                maxStagePictureSize.Height = (int)(stageDataManager.stageData.height * stageDataManager.magnification);
            }
            else
            {
                stageDataManager.magnification = maxStagePictureSize.Height / stageDataManager.stageData.height;
                maxStagePictureSize.Width = (int)(stageDataManager.stageData.width * stageDataManager.magnification);
            }
            InStagePicture.Size = maxStagePictureSize;
            Point stagePictureLocation = stageBox.Location;
            stagePictureLocation.Offset((int)(stageBox.Size.Width * 0.5f), (int)(stageBox.Size.Height * 0.5f));
            stagePictureLocation.Offset(-(int)(maxStagePictureSize.Width * 0.5f), -(int)(maxStagePictureSize.Height * 0.5f));
            InStagePicture.Location = stagePictureLocation;
            zoomManager.ResetZoom(new Vector2(InStagePicture.Size.Width, InStagePicture.Size.Height));

            // パーツの位置がステージ外へ出ないように
            positionX.Maximum = (decimal)(stageDataManager.stageData.width * 0.5f);
            positionX.Minimum = (decimal)(-stageDataManager.stageData.width * 0.5f);
            positionY.Maximum = (decimal)stageDataManager.stageData.height;

            PictureViewReflesh();
        }


        // ********************************************************
        // ステージのビューを更新する
        // ********************************************************

        // パーツの再配置
        private void PictureViewReflesh()
        {

            // ステージサイズとピクチャーボックスの倍率を算出
            // 縦と横の比率によって変える
            if (stageDataManager.stageData.width >= stageDataManager.stageData.height * stageBoxRatio)
            {
                stageDataManager.magnification = zoomManager.viewStagePictureSize.x / stageDataManager.stageData.width;
                maxStagePictureSize.Height = (int)(stageDataManager.stageData.height * stageDataManager.magnification);
            }
            else
            {
                stageDataManager.magnification = zoomManager.viewStagePictureSize.y / stageDataManager.stageData.height;
                maxStagePictureSize.Width = (int)(stageDataManager.stageData.width * stageDataManager.magnification);
            }

            // ステージのピクチャーボックスの大きさを調整
            InStagePicture.Size = maxStagePictureSize;
            Point stagePictureLocation = stageBox.Location;
            stagePictureLocation.Offset((int)(stageBox.Size.Width * 0.5f), (int)(stageBox.Size.Height * 0.5f));
            stagePictureLocation.Offset(-(int)(maxStagePictureSize.Width * 0.5f), -(int)(maxStagePictureSize.Height * 0.5f));
            InStagePicture.Location = stagePictureLocation;

            // パーツのピクチャーボックスを再配置
            stageDataManager.UpdateAllView(this.Controls, zoomManager.isZoomed);

            // ステージ表示の端の位置を知らせるラベルの更新
            UpdateCornerLabel();
        }


        // ステージ表示の端の位置を知らせるラベルの更新
        private void UpdateCornerLabel()
        {
            // 端のステージの位置
            float viewSidePosition = (stageDataManager.stageData.width * 0.5f) / zoomManager.zoomValue;
            // ズーム時の視点移動を考慮
            float move = -zoomManager.zoomLocation / stageDataManager.magnification;

            topleftLabel.Text = (-viewSidePosition + move).ToString() + ", " + stageDataManager.stageData.height.ToString();
            topRightLabel.Text = (viewSidePosition + move).ToString() + ", " + stageDataManager.stageData.height.ToString();
            bottomLeftLabel.Text = (-viewSidePosition + move).ToString() + ", " + 0.ToString();
            bottomRightLabel.Text = (viewSidePosition + move).ToString() + ", " + 0.ToString();


            // ラベルの表示位置の調整
            int margin = 5;
            // 左上
            Point location = InStagePicture.Location;
            location.Offset(0, -(topleftLabel.Height + margin));
            topleftLabel.Location = location;
            // 右上
            location = InStagePicture.Location;
            location.Offset(InStagePicture.Width - topRightLabel.Width, -(topRightLabel.Height + margin));
            topRightLabel.Location = location;
            // 左下
            location = InStagePicture.Location;
            location.Offset(0, (InStagePicture.Height + margin));
            bottomLeftLabel.Location = location;
            // 右下
            location = InStagePicture.Location;
            location.Offset(InStagePicture.Width - bottomRightLabel.Width, (InStagePicture.Height + margin));
            bottomRightLabel.Location = location;

        }


        // ********************************************************
        // チェック
        // ********************************************************

        // リストの添え字が範囲外か
        private bool IndexOutOfRange()
        {
            return partsListBox.SelectedIndex < 0 || partsListBox.SelectedIndex >= stageDataManager.GetPartsCouont();
        }

        // ズームできるステージの大きさか
        private bool CanZoom()
        {

            return InStagePicture.Height * zoomManager.maxZoomValue < stageBox.Height - insideStageMargin * 2.0f;
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

        private void CreateStageForm_SizeChanged(object sender, EventArgs e)
        {
            // フォーム上でステージとして見せるピクチャーボックスの最大サイズを算出
            maxStagePictureSize = new Size(stageBox.Size.Width - insideStageMargin * 2, stageBox.Size.Height - insideStageMargin * 2);
            // ステージのピクチャーボックスの縦横比を算出
            stageBoxRatio = maxStagePictureSize.Width / (float)maxStagePictureSize.Height;
            //Size_ValueChanged_Height(sender,e);
            ChangeStageSize();
            msgText.Text = "changed";

        }
    }


}
