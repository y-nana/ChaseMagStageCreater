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
//using Microsoft.WindowsAPICodePack.Dialogs;


namespace ChaseMagStageCreater
{
    public partial class Form1 : Form
    {

        private StageData stageData;
        private int preIndex;
        private string nowOpenPath;

        private Dictionary<RadioButton, StagePartsCategory> categoryPairs;
        private Dictionary<StagePartsCategory, ImageData> categoryImageDatas;

        private List<PictureBox> pictures;

        private float stageSizeMagnification;

        private int dragPictureIndex;

        private ZoomManager zoomManager;

        private readonly int insideStageMargin = 10;

        private readonly float pixelMagnification = 0.01f;

        private float stageBoxRatio;

        private Size maxStagePictureSize;

        

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ChaseMag StageCreater";

            // ピクセルから大きさを計算するためのデータ
            categoryImageDatas = new Dictionary<StagePartsCategory, ImageData>();
            categoryImageDatas.Add(StagePartsCategory.Scaffold, new ImageData( new Vector2(8,4),Properties.Resources.scaffold,BasePoint.Top));
            categoryImageDatas.Add(StagePartsCategory.Wall, new ImageData( new Vector2(1,20),Properties.Resources.wallNorth,Properties.Resources.wallSouth ,BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.NormalWall, new ImageData( new Vector2(4,30), Properties.Resources.floor, BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.JumpRamp, new ImageData( new Vector2(4,2.5f), Properties.Resources.jumpRampNorth,Properties.Resources.jumpRampSouth,BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.ItemBox, new ImageData(new Vector2(4,2.5f), Properties.Resources.itemBox,BasePoint.Bottom));


            maxStagePictureSize = new Size(stageBox.Size.Width - insideStageMargin * 2, stageBox.Size.Height - insideStageMargin * 2);

            stageBoxRatio = maxStagePictureSize.Width / (float)maxStagePictureSize.Height;


            categoryPairs = new Dictionary<RadioButton, StagePartsCategory>();
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                categoryPairs.Add((RadioButton)flowLayoutPanel1.Controls[i],(StagePartsCategory)i);
            }
            msgText.Text = string.Empty;
            stageData = new StageData();
            pictures = new List<PictureBox>();
            zoomManager = new ZoomManager(new Vector2(InStagePicture.Size.Width, InStagePicture.Size.Height));

            stageData.width = 80;
            stageData.height = 20;
            widthSize.Value = 80;
            heightSize.Value = 20;


            preIndex = -1;
            dragPictureIndex = -1;
            nowOpenPath = string.Empty;



            ViewUpdate();
        }

        // ファイルからインポート
        public void ImportData(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            string datastr = reader.ReadToEnd();
            reader.Close();
            // jsonからデシリアライズ
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            StageData data = JsonSerializer.Deserialize<StageData>(datastr, options);

            stageData = data;

            foreach (var item in pictures)
            {
                item.Dispose();
            }
            pictures.Clear();

            msgText.Text = filePath + "読み込みました";
            nowOpenPath = filePath;
            this.Text += nowOpenPath;
            for (int i = 0; i < stageData.stageParts.Count; i++)
            {
                AddStagePartsImage(stageData.stageParts[i]);

            }

            // 描画の更新処理
            ViewDataClear();
            ViewUpdate();
        }




        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            openFileDialog1.ShowDialog();

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            
            if (partsListBox.SelectedIndex < 0)
            {
                msgText.Text = "リストから削除する項目を選択してください";
                return;
            }

            ViewDataClear();

            stageData.stageParts.RemoveAt(partsListBox.SelectedIndex);
            pictures[partsListBox.SelectedIndex].Dispose();
            pictures.RemoveAt(partsListBox.SelectedIndex);

            msgText.Text = (partsListBox.SelectedIndex+1).ToString() + "削除しました";

            ViewUpdate();

        }

        private void ViewUpdate()
        {

            Dictionary<StagePartsCategory, int> categoryCnt = new Dictionary<StagePartsCategory, int>();
            
            partsListBox.Items.Clear();
            for (int i = 0; i < stageData.stageParts.Count; i++)
            {
                partsListBox.Items.Add((i + 1).ToString() + stageData.stageParts[i].category.ToString());
            }

            heightSize.Value = (decimal)stageData.height;
            widthSize.Value = (decimal)stageData.width;
        }

        private void PreDataSave()
        {
            if (preIndex >= 0)
            {
                stageData.stageParts[preIndex].position = new Vector2((float)positionX.Value, (float)positionY.Value);
                stageData.stageParts[preIndex].sizeMagnification = new Vector2((float)sizeX.Value, (float)sizeY.Value);

                if (IsSetPole(stageData.stageParts[preIndex].category))
                {
                    stageData.stageParts[preIndex].isNorth = northButton.Checked;
                }

                pictures[preIndex].Image = null;

            }
        }

        private void ViewDataClear()
        {


            preIndex = -1;
            positionX.Value = 0;
            positionY.Value = 0;
            sizeX.Value = 1;
            sizeY.Value = 1;

            polePanel.Visible = false;


        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ImportData(openFileDialog1.FileName);

        }

        private void partsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreDataSave();

            if (partsListBox.SelectedIndex < 0)
            {
                return;
            }

            pictures[partsListBox.SelectedIndex].Image = Properties.Resources.SelectMask;
            preIndex = partsListBox.SelectedIndex;
            StagePart part = stageData.stageParts[partsListBox.SelectedIndex];

            positionX.Value = (decimal)part.position.x;
            positionY.Value = (decimal)part.position.y;
            sizeX.Value = (decimal)part.sizeMagnification.x;
            sizeY.Value = (decimal)part.sizeMagnification.y;
            if (IsSetPole(part.category))
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

        // 磁力の向きを設定するかどうか
        private bool IsSetPole(StagePartsCategory categry)
        {
            return categry == StagePartsCategory.JumpRamp || categry == StagePartsCategory.Wall;
        }

        private void OverWriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stageData.stageParts.Count <= 0)
            {

                msgText.Text = "リストにデータを入れてください";
                return;
            }

            PreDataSave();

            if (nowOpenPath == string.Empty)
            {
                saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";
                saveFileDialog1.ShowDialog();
                return;

            }

            ExportData(nowOpenPath);


        }

        private void ExportData(string path)
        {


            // jsonに変換
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            string jsonstr = JsonSerializer.Serialize(stageData, options);
            // 保存先を開く
            StreamWriter writer = new StreamWriter(path, false);
            // jsonデータ書き込み
            writer.WriteLine(jsonstr);
            // バッファのクリア、クローズ
            writer.Flush();
            writer.Close();
            nowOpenPath = path;
            msgText.Text = path + "保存しました";
            this.Text += "  " + nowOpenPath;

        }

        private void addButton_Click(object sender, EventArgs e)
        {

            AddStageParts();


        }

        private bool AddStageParts()
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
            StagePart addpart = new StagePart();
            addpart.category = categoryPairs[checkedCategory];
            stageData.stageParts.Add(addpart);
            AddStagePartsImage(addpart);

            ViewUpdate();
            partsListBox.SelectedIndex = stageData.stageParts.Count - 1;
            return true;
        }

        // ステージパーツをビューに追加表示する
        private void AddStagePartsImage(StagePart part)
        {

            pictures.Add(new PictureBox());
            this.Controls.Add(pictures[pictures.Count - 1]);

            Size size = categoryImageDatas[part.category].bitmap[0].Size;
            size = new Size((int)Math.Round(categoryImageDatas[part.category].scale.x * pixelMagnification * size.Width * stageSizeMagnification* part.sizeMagnification.x),
                (int)Math.Round(categoryImageDatas[part.category].scale.y * pixelMagnification * size.Height * stageSizeMagnification * part.sizeMagnification.y));
            Point location = InStagePicture.Location;

            location.Offset((int)Math.Round(InStagePicture.Size.Width / 2.0f), InStagePicture.Size.Height);

            Vector2 position = new Vector2(part.position.x * stageSizeMagnification, -part.position.y * stageSizeMagnification);
            location.Offset(StagePositionToLocation(position, size, categoryImageDatas[part.category].basePoint));
            location.Offset((int)(zoomManager.zoomLocation), 0);
            
            pictures[pictures.Count - 1].Location = location;
            pictures[pictures.Count - 1].Size = size;


            if (IsSetPole(part.category)&& !part.isNorth)
            {
                pictures[pictures.Count - 1].BackgroundImage = categoryImageDatas[part.category].bitmap[1];
            }
            else
            {
                pictures[pictures.Count - 1].BackgroundImage = categoryImageDatas[part.category].bitmap[0];

            }

            pictures[pictures.Count - 1].BackgroundImageLayout = ImageLayout.Stretch;
            pictures[pictures.Count - 1].BringToFront();
            pictures[pictures.Count - 1].MouseDown += StagePartMouseDown;
            pictures[pictures.Count - 1].MouseUp += StagePartMouseUp;
            pictures[pictures.Count - 1].MouseMove += StagePartMouseMove;

            if (location.X < InStagePicture.Location.X || location.X+size.Width > InStagePicture.Location.X + InStagePicture.Width)
            {
                pictures[pictures.Count - 1].Visible = false;

            }


        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ExportData(saveFileDialog1.FileName);

        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            saveFileDialog1.ShowDialog();

        }

        private void CreateNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "新規作成";

            saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            saveFileDialog1.ShowDialog();
        }

        private Point StagePositionToLocation(Vector2 pos, Size size, BasePoint basePoint)
        {
            PointF delta = Point.Empty;
            switch (basePoint)
            {
                case BasePoint.Top:
                    delta = new PointF(-size.Width / 2.0f, 0.0f);

                    break;
                case BasePoint.Center:
                    delta = new PointF(-size.Width / 2.0f, -size.Height / 2.0f);
                    break;
                case BasePoint.Bottom:
                    delta = new PointF(-size.Width / 2.0f, -size.Height);
                    break;

            }

            PointF pointf = new PointF(pos.x + delta.X, pos.y + delta.Y);
            return Point.Round(pointf);
        }

        // 位置の値を変更する
        private void position_ValueChanged(object sender, EventArgs e)
        {
            if (partsListBox.SelectedIndex < 0 || partsListBox.SelectedIndex >= pictures.Count)
            {
                return;
            }
            Point location = InStagePicture.Location;
            // 原点へ
            location.Offset((int)Math.Round(InStagePicture.Size.Width / 2.0f), InStagePicture.Size.Height);
            Vector2 pos = new Vector2((float)positionX.Value * stageSizeMagnification,
                (float)-positionY.Value * stageSizeMagnification);
            BasePoint basePoint = categoryImageDatas[stageData.stageParts[partsListBox.SelectedIndex].category].basePoint;
            location.Offset(StagePositionToLocation(pos, pictures[partsListBox.SelectedIndex].Size, basePoint));
            location.Offset((int)zoomManager.zoomLocation,0);
            pictures[partsListBox.SelectedIndex].Location = location;

            pictures[partsListBox.SelectedIndex].Visible = StageOfRangeIn(pictures[partsListBox.SelectedIndex]);






        }

        // サイズの値を変更する
        private void size_ValueChanged(object sender, EventArgs e)
        {
            if (partsListBox.SelectedIndex < 0|| partsListBox.SelectedIndex >= pictures.Count )
            {
                return;
            }
            StagePartsCategory category = stageData.stageParts[partsListBox.SelectedIndex].category;
            Size size = categoryImageDatas[category].bitmap[0].Size;
            size = new Size((int)Math.Round(categoryImageDatas[category].scale.x * pixelMagnification * size.Width * stageSizeMagnification*(float)sizeX.Value),
                (int)Math.Round(categoryImageDatas[category].scale.y * pixelMagnification * size.Height * stageSizeMagnification * (float)sizeY.Value));
            pictures[partsListBox.SelectedIndex].Size = size;


            Point location = InStagePicture.Location;
            // 原点へ
            location.Offset((int)Math.Round(InStagePicture.Size.Width / 2.0f), InStagePicture.Size.Height);
            Vector2 pos = new Vector2((float)positionX.Value * stageSizeMagnification,
                (float)-positionY.Value * stageSizeMagnification);
            BasePoint basePoint = categoryImageDatas[stageData.stageParts[partsListBox.SelectedIndex].category].basePoint;
            location.Offset(StagePositionToLocation(pos, pictures[partsListBox.SelectedIndex].Size, basePoint));
            pictures[partsListBox.SelectedIndex].Location = location;

        }

        // ドラッグでパーツを移動し始める
        private void StagePartMouseDown(object sender, EventArgs e)
        {
            int index = pictures.IndexOf((PictureBox)sender);
            partsListBox.SelectedIndex = index;
            dragPictureIndex = index;
        }

        // ドラッグでパーツを移動させる
        private void StagePartMouseMove(object sender, EventArgs e)
        {
            if (dragPictureIndex < 0)
            {
                return;
            }

            Point mouseLocation = PointToClient(MousePosition);
            // ドラッグでステージ外へ出せないように
            if (mouseLocation.X < InStagePicture.Location.X ||
                mouseLocation.X + pictures[dragPictureIndex].Width > InStagePicture.Location.X + InStagePicture.Size.Width||
                mouseLocation.Y < InStagePicture.Location.Y || 
                mouseLocation.Y + pictures[dragPictureIndex].Height> InStagePicture.Location.Y + InStagePicture.Size.Height)
            {
                return;
            }

            PointF picLocation = PointToClient(MousePosition);
            picLocation.X -= pictures[dragPictureIndex].Width * 0.5f;
            picLocation.Y -= pictures[dragPictureIndex].Height * 0.5f;
            pictures[dragPictureIndex].Location = Point.Round(picLocation);

        }

        // パーツの移動終了
        private void StagePartMouseUp(object sender, EventArgs e)
        {
            int index = pictures.IndexOf((PictureBox)sender);

            PictureLocationApply(index);


            dragPictureIndex = -1;
        }

        // 移動させた場所をデータとして反映させる
        private void PictureLocationApply(int index)
        {
            BasePoint basePoint = categoryImageDatas[stageData.stageParts[index].category].basePoint;

            Point partLocation = pictures[index].Location;
            partLocation.Offset(-(int)zoomManager.zoomLocation,0);

            // 位置のセット
            stageData.stageParts[index].position = LocationToStagePosition(
                partLocation,
                pictures[index].Size,
                 basePoint);
            positionX.Value = (decimal)stageData.stageParts[index].position.x;
            positionY.Value = (decimal)stageData.stageParts[index].position.y;
        }

        // アプリケーション上の座標をUnity内の座標へ変換する
        private Vector2 LocationToStagePosition(Point point, Size size,BasePoint basePoint)
        {
            Point location = InStagePicture.Location;
            // 原点へ
            location.Offset((int)Math.Round(InStagePicture.Size.Width / 2.0f), InStagePicture.Size.Height);
            switch (basePoint)
            {
                case BasePoint.Top:
                    point.Offset((int)Math.Round(size.Width / 2.0f), 0);

                    break;
                case BasePoint.Center:
                    point.Offset((int)Math.Round(size.Width / 2.0f), (int)Math.Round(size.Height / 2.0f));

                    break;
                case BasePoint.Bottom:
                    point.Offset((int)Math.Round(size.Width / 2.0f), size.Height);

                    break;

            }

            Vector2 deltaMove = new Vector2(point.X - location.X, point.Y - location.Y);

            // ステージ上のポジションへ変換
            Vector2 returnValue = new Vector2(
                deltaMove.x / stageSizeMagnification,
                deltaMove.y / stageSizeMagnification
                );
            // 0.5刻みにする
            float increment = 0.5f;
            returnValue = new Vector2(IncrementOfValue( returnValue.x, increment), -IncrementOfValue(returnValue.y, increment));
            return returnValue;

        }


        // 指定の値刻みの値を返す
        private float IncrementOfValue(float value ,float increment)
        {

            float r = (float)(Math.Floor(value / increment) * increment);
            if (r < value)
                r += increment;
            return r;


        }

        // ステージ内をクリックする
        private void InStagePicture_Click(object sender, EventArgs e)
        {
            // 追加モードだったらパーツを追加する

            if (addModeButtom.Checked)
            {
                if (AddStageParts())
                {
                    pictures[pictures.Count-1].Location = PointToClient(MousePosition);
                    PictureLocationApply(pictures.Count - 1);

                }
            }
            // 視点移動モードでズーム中だったら視点を移動する
            else if (viewMoveModeButton.Checked)
            {
                PreDataSave();
                Point mouseLocation = PointToClient(MousePosition);
                int value = 20;
                if (mouseLocation.X > InStagePicture.Location.X + InStagePicture.Width * 0.5f)
                {
                    value *= -1;
                }
                zoomManager.ChangeFocus(value);
                PictureViewReflesh();

            }


        }


        // ズームする
        private void InStagePicture_MouseWheel(object sender, MouseEventArgs e)
        {
            PreDataSave();

            zoomManager.ZoomChange(e.Delta);
            PictureViewReflesh();
            
            
        }

        // パーツの磁力の向きを変更する
        private void poleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (partsListBox.SelectedIndex < 0)
            {
                return;
            }

            if (southButton.Checked)
            {
                pictures[partsListBox.SelectedIndex].BackgroundImage = categoryImageDatas[stageData.stageParts[partsListBox.SelectedIndex].category].bitmap[0];
            }
            else
            {
                pictures[partsListBox.SelectedIndex].BackgroundImage = categoryImageDatas[stageData.stageParts[partsListBox.SelectedIndex].category].bitmap[1];

            }
        }

        // ステージのサイズを変更する
        private void Size_ValueChanged(object sender, EventArgs e)
        {

            stageData.width = (float)widthSize.Value;
            stageData.height = (float)heightSize.Value;


            if (stageData.width >= stageData.height * stageBoxRatio)
            {
                stageSizeMagnification = maxStagePictureSize.Width / stageData.width;
                maxStagePictureSize.Height = (int)(stageData.height * stageSizeMagnification);
            }
            else
            {
                
                stageSizeMagnification = maxStagePictureSize.Height / stageData.height;
                maxStagePictureSize.Width = (int)(stageData.width * stageSizeMagnification);
            }
            InStagePicture.Size = maxStagePictureSize;
            Point stagePictureLocation = stageBox.Location;
            stagePictureLocation.Offset((int)(stageBox.Size.Width * 0.5f), (int)(stageBox.Size.Height * 0.5f));
            stagePictureLocation.Offset(-(int)(maxStagePictureSize.Width * 0.5f), -(int)(maxStagePictureSize.Height * 0.5f));
            InStagePicture.Location = stagePictureLocation;
            zoomManager.ResetZoom(new Vector2(InStagePicture.Size.Width, InStagePicture.Size.Height));
            PictureViewReflesh();

        }



        // パーツの再配置
        private void PictureViewReflesh()
        {

            if (stageData.width >= stageData.height * stageBoxRatio)
            {
                stageSizeMagnification = zoomManager.viewStagePictureSize.x / stageData.width;
                maxStagePictureSize.Height = (int)(stageData.height * stageSizeMagnification);
            }
            else
            {

                stageSizeMagnification = zoomManager.viewStagePictureSize.y / stageData.height;
                maxStagePictureSize.Width = (int)(stageData.width * stageSizeMagnification);
            }

            InStagePicture.Size = maxStagePictureSize;
            Point stagePictureLocation = stageBox.Location;
            stagePictureLocation.Offset((int)(stageBox.Size.Width * 0.5f), (int)(stageBox.Size.Height * 0.5f));
            stagePictureLocation.Offset(-(int)(maxStagePictureSize.Width * 0.5f), -(int)(maxStagePictureSize.Height * 0.5f));
            InStagePicture.Location = stagePictureLocation;


            foreach (var item in pictures)
            {
                item.Dispose();
            }
            pictures.Clear();

            for (int i = 0; i < stageData.stageParts.Count; i++)
            {
                AddStagePartsImage(stageData.stageParts[i]);

            }
            UpdateCornerLabel();
        }


        // ステージ表示の端の位置を知らせるラベルの更新
        private void UpdateCornerLabel()
        {

            float temp = (stageData.width * 0.5f) / zoomManager.zoomValue;
            float move = -zoomManager.zoomLocation / stageSizeMagnification;

            topleftLabel.Text = (-temp + move).ToString() + ", " + stageData.height.ToString();
            topRightLabel.Text = (temp + move).ToString() + ", " + stageData.height.ToString();
            bottomLeftLabel.Text = (-temp + move).ToString() + ", " + 0.ToString();
            bottomRightLabel.Text = (temp + move).ToString() + ", " + 0.ToString();



            // 表示位置の調整
            int margin = 5;

            Point location = InStagePicture.Location;
            location.Offset(0, -(topleftLabel.Height + margin));
            topleftLabel.Location = location;

            location = InStagePicture.Location;
            location.Offset(InStagePicture.Width - topRightLabel.Width, -(topRightLabel.Height + margin));
            topRightLabel.Location = location;

            location = InStagePicture.Location;
            location.Offset(0, (InStagePicture.Height + margin));
            bottomLeftLabel.Location = location;

            location = InStagePicture.Location;
            location.Offset(InStagePicture.Width - topRightLabel.Width, (InStagePicture.Height + margin));
            bottomRightLabel.Location = location;

        }

        // パーツを表示するかどうか
        private bool StageOfRangeIn(PictureBox picture)
        {
            if (picture.Location.X < InStagePicture.Location.X)
            {
                return false;
            }
            if (picture.Location.X + picture.Width > InStagePicture.Location.X + InStagePicture.Width)
            {
                return false;
            }
            if (picture.Location.Y < InStagePicture.Location.Y)
            {
                return false;
            }
            if (picture.Location.Y + picture.Height > InStagePicture.Location.Y + InStagePicture.Height)
            {
                return false;
            }
            return true;
        }


    }



}
