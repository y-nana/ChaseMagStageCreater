using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ChaseMagStageCreater
{
    public enum Pole
    {
        North,
        South
    }

    class StageDataManager
    {
        // 操作しているステージデータ
        public StageData stageData { get; private set; }
        // 表示しているピクチャーボックスのリスト
        public List<PictureBox> pictures{ get; private set; }

        // サイズなどのデータ取得用
        private PictureBoxDataManager pictureBoxDataManager;

        // ステージのピクチャーボックス
        private PictureBox inStage;
        private PictureBox zoomStage;

        private Point preLocation;
        private Point deltaMove;

        private RadioButton movePartsButtom;

        private readonly int changeDelta = 20;

        private int dragPictureIndex;

        // 選択状況を変更する処理
        private Action<int> changeSelect;

        public Label debugLabel;

        public StageDataManager(StageData stageData, PictureBox stage, PictureBox zoomStage, Action<int> changeSelect, RadioButton movePartsButtom)
        {
            this.stageData = stageData;
            this.inStage = stage;
            this.zoomStage = zoomStage;
            this.zoomStage.SizeChanged += ZoomStage_SizeChanged;
            this.zoomStage.LocationChanged += ZoomStage_LocationChanged;
            this.inStage.MouseUp += InStage_MouseUP;
            this.changeSelect = changeSelect;
            pictures = new List<PictureBox>();
            pictureBoxDataManager = new PictureBoxDataManager();

            preLocation = zoomStage.Location;
            deltaMove = Point.Empty;

            dragPictureIndex = -1;
            this.movePartsButtom = movePartsButtom;
        }

        private void InStage_MouseUP(object sender, MouseEventArgs e)
        {
            if (inStage.Size == zoomStage.Size|| deltaMove.IsEmpty)
            {
                return;
            }

            UpdateAllParts();
            deltaMove = Point.Empty;
            preLocation = zoomStage.Location;

        }

        private void ZoomStage_LocationChanged(object sender, EventArgs e)
        {

            Point delta = new Point(zoomStage.Location.X - preLocation.X, zoomStage.Location.Y - preLocation.Y);
            deltaMove.Offset(delta);
            preLocation = zoomStage.Location;
            
           
            if (Math.Abs(deltaMove.X) > changeDelta || Math.Abs(deltaMove.Y) > changeDelta)
            {
                debugLabel.Text = delta.X.ToString();
                UpdateAllParts();
                deltaMove = Point.Empty;
            }
        }


        private void UpdateAllParts()
        {
            foreach (var partPicture in pictures)
            {
                PictureBox realPictureBox = GetInStagePicture(partPicture);
                Point location = realPictureBox.Location;
                realPictureBox.Location = location;
                

                if (!StageOfRangeIn(realPictureBox, inStage))
                {
                    partPicture.Visible = false;

                }
                else
                {
                    partPicture.Visible = true;
                    realPictureBox = GetInStageViewPicture(realPictureBox, inStage);
                    partPicture.Size = realPictureBox.Size;
                    partPicture.Location = realPictureBox.Location;
                }
            }

        }


        // パーツを新しく追加する
        public void AddNewPart(StagePartsCategory category, Vector2 position)
        {
        
            StagePart part = new StagePart();
            part.category = category;
            part.position = position;
            part.sizeMagnification = Vector2.one;
            stageData.stageParts.Add(part);
            AddPictureBox(part);
            
        }

        // 指定したインデックスのパーツを削除する
        public void DeletePart(int index)
        {

            stageData.stageParts.RemoveAt(index);
            pictures[index].Dispose();
            pictures.RemoveAt(index);

        }


        // インポート時などステージデータを丸ごと入れ替える
        public void SetNewData(StageData data, Control.ControlCollection controls)
        {
            this.stageData = data;
            // すべての表示の更新
            UpdateAllView();

        }

        // すべての表示の更新
        public void UpdateAllView()
        {
            foreach (var item in pictures)
            {
                item.Dispose();
            }
            pictures.Clear();
            for (int i = 0; i < stageData.stageParts.Count; i++)
            {
                AddPictureBox(stageData.stageParts[i]);
                if (inStage.Size != zoomStage.Size)
                {
                    if (!StageOfRangeIn(pictures[i], inStage))
                    {
                        pictures[i].Visible = false;

                    }
                    else
                    {
                        PictureBox viewPic = GetInStageViewPicture(pictures[i], inStage);
                        pictures[i].Location = viewPic.Location;
                        pictures[i].Size = viewPic.Size;
                    }

                }
            }
        }

        // パーツに応じた一つのピクチャーボックスを作り追加する
        private void AddPictureBox(StagePart part)
        {
            PictureBox picture = new PictureBox();
            float magnification = GetMagnification();
            // 大きさの設定
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            // 位置の設定
            Point location =
            LocationConverter.StagePositionToLocation(part.position, size, pictureBoxDataManager.GetBasePoint(part.category), GetBasePoint(zoomStage), magnification);

            picture.Location = location;
            picture.Size = size;

            // 画像の設定
            if (StagePart.IsSetPole(part.category) && !part.isNorth)
            {
                picture.BackgroundImage = pictureBoxDataManager.GetBitmap(part.category, Pole.South);
            }
            else
            {
                picture.BackgroundImage = pictureBoxDataManager.GetBitmap(part.category,Pole.North);
            }

            picture.BackgroundImageLayout = ImageLayout.Stretch;
            picture.BackColor = Color.White;

            // ドラッグイベントの追加
            AddEvent(picture);
            

            // 追加処理
            pictures.Add(picture);
            inStage.FindForm().Controls.Add(picture);
            picture.BringToFront();

        }


        // 大きさの更新
        public void UpdateSize(int index , Vector2 value)
        {
            stageData.stageParts[index].sizeMagnification = value;

            StagePart part = stageData.stageParts[index];
            float magnification = GetMagnification();

            // 大きさの設定
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            pictures[index].Size = size;
            SetPicturePosition(index);
        }

        // 位置の更新
        public void UpdatePosition(int index, Vector2 value)
        {
            stageData.stageParts[index].position = value;
            SetPicturePosition(index);

        }

        // 磁力の向きの更新
        public void UpdatePole(int index, bool isNorth)
        {
            stageData.stageParts[index].isNorth = isNorth;
            StagePart part = stageData.stageParts[index];
            if (part.isNorth)
            {
                pictures[index].BackgroundImage = pictureBoxDataManager.GetBitmap(part.category, Pole.North);
            }
            else
            {
                pictures[index].BackgroundImage = pictureBoxDataManager.GetBitmap(part.category, Pole.South);

            }
            
        }

        // 選択状態をわかりやすくする
        public void SelectedPicture(int index)
        {
            pictures[index].Image = Properties.Resources.SelectMask;
        }

        // 選択状態を解除
        internal void DeSelected(int preIndex)
        {
            if (preIndex >= 0&& preIndex < pictures.Count)
            {
                pictures[preIndex].Image = null;
                GetInStageViewPicture(pictures[preIndex], inStage);
            }
        }

        // 移動させたピクチャーボックスの位置をデータとして反映させる
        public void PictureLocationApply(PictureBox partPicture)
        {
            int index = pictures.IndexOf(partPicture);
            PictureBox realPart = GetInStagePicture(pictures[index]);
            Point partLocation = pictures[index].Location;
            partLocation.Offset(-(realPart.Width-pictures[index].Width), -(realPart.Height - pictures[index].Height));


            // 位置のセット
            stageData.stageParts[index].position = 
                LocationConverter.LocationToStagePosition(
                partLocation,
                realPart.Size,
                pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                GetBasePoint(zoomStage), GetMagnification());
            SetPicturePosition(index);
            
        }

        // 指定したインデックスのピクチャーボックスの位置をデータ通りに更新する
        private void SetPicturePosition(int index)
        {
            Point partLocation =
                LocationConverter.StagePositionToLocation(
                    stageData.stageParts[index].position,
                    GetInStagePicture(pictures[index]).Size,
                    pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                    GetBasePoint(zoomStage), GetMagnification());
            pictures[index].Location = partLocation;

        }

        // パーツの取得
        public StagePart GetPart(int index)
        {
            return stageData.stageParts[index];
        }

        public StagePart GetPart(PictureBox partPicture)
        {
            int index = pictures.IndexOf(partPicture);
            return stageData.stageParts[index];
        }

        public int GetPartsCouont()
        {
            return stageData.stageParts.Count;
        }

        private void ResetPictureSize(PictureBox partPicture)
        {
            // 大きさの設定
            partPicture.Size = GetInStagePicture(partPicture).Size;
        }

        // 見切れていない本来の大きさを取得
        private PictureBox GetInStagePicture(PictureBox partPicture)
        {
            PictureBox box = new PictureBox();
            StagePart part = GetPart(partPicture);
            float magnification = GetMagnification();
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            box.Size = size;

            Point location =
                LocationConverter.StagePositionToLocation(
                    part.position, 
                    size, 
                    pictureBoxDataManager.GetBasePoint(part.category), 
                    GetBasePoint(zoomStage),
                    magnification);
            box.Location = location;
            return box;
        }

        // パーツの再配置
        private void ZoomStage_SizeChanged(object sender, EventArgs e)
        {


            // パーツのピクチャーボックスを再配置
            UpdateAllParts();
            
            preLocation = zoomStage.Location;

            deltaMove = Point.Empty;
            
        }

        private float GetMagnification()
        {
            return zoomStage.Width / stageData.width;

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
            int index = pictures.IndexOf((PictureBox)sender);
            // 選択状況変更処理
            if (changeSelect!= null)
            {
                changeSelect.Invoke(index);
            }
            if (movePartsButtom.Checked && e.Button == MouseButtons.Left)
            {
                dragPictureIndex = index;
                ResetPictureSize((PictureBox)sender);
            }


        }

        // ドラッグでパーツを移動させる
        private void StagePartMouseMove(object sender, MouseEventArgs e)
        {
            if (dragPictureIndex < 0)
            {
                return;
            }

            Point mouseLocation = e.Location;
            mouseLocation.Offset(pictures[dragPictureIndex].Location);
            PointF picLocation = mouseLocation;
            picLocation.X -= pictures[dragPictureIndex].Width * 0.5f;
            picLocation.Y -= pictures[dragPictureIndex].Height * 0.5f;
            pictures[dragPictureIndex].Location = Point.Round(picLocation);

        }

        // パーツの移動終了
        private void StagePartMouseUp(object sender, MouseEventArgs e)
        {
            if (dragPictureIndex < 0)
            {
                return;
            }
            if (movePartsButtom.Checked)
            {
                // データへ反映する
                PictureLocationApply((PictureBox)sender);

            }
            dragPictureIndex = -1;

        }

        //*******************************************************************************************

        // パーツを表示するかどうか
        // 完全に外側だったら表示しない
        private bool StageOfRangeIn(PictureBox picture, PictureBox range)
        {

            if (picture.Location.X + picture.Width < range.Location.X)
            {
                return false;
            }
            if (picture.Location.X > range.Location.X + range.Width)
            {
                return false;
            }
            if (picture.Location.Y > range.Location.Y + range.Height)
            {
                return false;
            }
            if (picture.Location.Y + picture.Height < range.Location.Y)
            {
                return false;
            }


            return true;
        }

        // 範囲内に収まるようにサイズ調整
        public PictureBox GetInStageViewPicture(PictureBox picture, PictureBox range)
        {
            PictureBox returnPictureBox = new PictureBox();
            returnPictureBox.Location = picture.Location;
            returnPictureBox.Size = picture.Size;
            if (picture.Left < range.Left)
            {
                returnPictureBox.Size = new Size(returnPictureBox.Right - range.Left, returnPictureBox.Height);
                returnPictureBox.Location = new Point(range.Left, returnPictureBox.Location.Y);
            }

            if (picture.Right > range.Right)
            {
                returnPictureBox.Size = new Size(range.Right - returnPictureBox.Left, returnPictureBox.Height);
            }
            if (picture.Top < range.Top)
            {
                returnPictureBox.Size = new Size(returnPictureBox.Width, returnPictureBox.Bottom - range.Top);
                returnPictureBox.Location = new Point(returnPictureBox.Location.X, range.Top);
            }
            if (picture.Bottom > range.Bottom)
            {
                returnPictureBox.Size = new Size(returnPictureBox.Width, range.Bottom - returnPictureBox.Top);
            }
            return returnPictureBox;
        }

        // 下中心のロケーションを取得する
        public static Point GetBasePoint(PictureBox pictureBox)
        {
            Point location = pictureBox.Location;
            // 原点へ
            location.Offset((int)Math.Round(pictureBox.Size.Width / 2.0f), pictureBox.Size.Height);
            return location;
        }


    }

}
