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
using Microsoft.WindowsAPICodePack.Dialogs;


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

        private readonly float pixelMagnification = 0.01f;

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



            categoryPairs = new Dictionary<RadioButton, StagePartsCategory>();
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                categoryPairs.Add((RadioButton)flowLayoutPanel1.Controls[i],(StagePartsCategory)i);
            }
            msgText.Text = string.Empty;
            stageData = new StageData();
            pictures = new List<PictureBox>();
            stageData.width = 50.0f;
            stageData.height = 20.0f;
            stageSizeMagnification = pictureBox1.Size.Width / stageData.width;
            preIndex = -1;
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
                /*
                using (var fileDialog = new CommonOpenFileDialog()
                {
                    Title = "フォルダを選択してください",
                    InitialDirectory = openPath,
                    IsFolderPicker = true
                })
                {
                    if (fileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                    {
                        return;
                    }
                    nowOpenPath = fileDialog.FileName + "/StageData.json";
                }
                */
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
            // 指定したグループ内のラジオボタンでチェックされている物を取り出す
            RadioButton checkedCategory = flowLayoutPanel1.Controls.OfType<RadioButton>()
                .SingleOrDefault(rb => rb.Checked == true);

            if (checkedCategory == null)
            {
                msgText.Text = "パーツの種類を選択してください";
                scaffoldButton.Checked = true;
                return;
            }

            StagePart addpart = new StagePart();
            addpart.category = categoryPairs[checkedCategory];
            stageData.stageParts.Add(addpart);
            ViewUpdate();
            partsListBox.SelectedIndex = stageData.stageParts.Count - 1;

            AddStagePartsImage(addpart);

        }

        private void AddStagePartsImage(StagePart part)
        {
            // test

            pictures.Add(new PictureBox());
            this.Controls.Add(pictures[pictures.Count - 1]);

            Size size = categoryImageDatas[part.category].bitmap[0].Size;
            size = new Size((int)Math.Round(categoryImageDatas[part.category].scale.x * pixelMagnification * size.Width * stageSizeMagnification),
                (int)Math.Round(categoryImageDatas[part.category].scale.y * pixelMagnification * size.Height * stageSizeMagnification));
            Point location = pictureBox1.Location;

            location.Offset((int)Math.Round(pictureBox1.Size.Width / 2.0f), pictureBox1.Size.Height);

            Vector2 position = new Vector2(part.position.x * stageSizeMagnification, -part.position.y * stageSizeMagnification);
            location.Offset(StagePositionToLocation(position, size, categoryImageDatas[part.category].basePoint));
            pictures[pictures.Count - 1].Location = location;
            pictures[pictures.Count - 1].Size = size;
            pictures[pictures.Count - 1].BackgroundImage = categoryImageDatas[part.category].bitmap[0];
            pictures[pictures.Count - 1].BackgroundImageLayout = ImageLayout.Stretch;
            pictures[pictures.Count - 1].BringToFront();

            msgText.Text = size.ToString();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            ExportData(saveFileDialog1.FileName);

        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jsonファイル(*.json)|*.json";

            saveFileDialog1.ShowDialog();
            /*
            using (var fileDialog = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",
                InitialDirectory = openPath,
                IsFolderPicker = true
            })
            {
                if (fileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }
                nowOpenPath = fileDialog.FileName + "/StageData.json";
            }
            */
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

        private void position_ValueChanged(object sender, EventArgs e)
        {
            if (partsListBox.SelectedIndex < 0|| partsListBox.SelectedIndex >= pictures.Count )
            {
                return;
            }
            Point location = pictureBox1.Location;
            // 原点へ
            location.Offset((int)Math.Round(pictureBox1.Size.Width / 2.0f), pictureBox1.Size.Height);
            Vector2 pos = new Vector2((float)positionX.Value * stageSizeMagnification,
                (float)-positionY.Value * stageSizeMagnification);
            BasePoint basePoint = categoryImageDatas[stageData.stageParts[partsListBox.SelectedIndex].category].basePoint;
            location.Offset(StagePositionToLocation(pos, pictures[partsListBox.SelectedIndex].Size, basePoint));
            pictures[partsListBox.SelectedIndex].Location = location;

        }

    }


}
