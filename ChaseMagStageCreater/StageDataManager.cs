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

        // 倍率
        //public float magnification { get; set; }
        private StageSizeManager sizeManager;

        // ズームマネージャー
        //private ZoomManager zoomManager;

        // ドラッグイベントを追加するメソッド
        private Action<PictureBox> addEventAction;



        public StageDataManager(StageData stageData, PictureBox stage, PictureBox zoomStage, StageSizeManager sizeManager, Action<PictureBox> addEventAction)
        {
            this.stageData = stageData;
            this.inStage = stage;
            this.zoomStage = zoomStage;
            this.zoomStage.SizeChanged += ZoomStage_SizeChanged;
            this.sizeManager = sizeManager;
            pictures = new List<PictureBox>();
            pictureBoxDataManager = new PictureBoxDataManager();
            this.addEventAction = addEventAction;

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
                        ResizePartPicture(pictures[i], inStage);
                    }

                }
            }
        }

        // パーツに応じた一つのピクチャーボックスを作り追加する
        private void AddPictureBox(StagePart part)
        {
            PictureBox picture = new PictureBox();
            //float magnification = sizeManager.stageDataRatio;
            float magnification = zoomStage.Width / stageData.width;
            // 大きさの設定
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            // 位置の設定
            Point location =
            LocationConverter.StagePositionToLocation(part.position, size, pictureBoxDataManager.GetBasePoint(part.category), GetBasePoint(inStage), magnification);
            //location.Offset((int)(zoomManager.zoomLocation), 0);

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
            if (addEventAction != null)
            {
                addEventAction.Invoke(picture);
            }
            

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
            float magnification = sizeManager.stageDataRatio;
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
                ResizePartPicture(pictures[preIndex], inStage);
            }
        }

        // 移動させたピクチャーボックスの位置をデータとして反映させる
        public void PictureLocationApply(PictureBox partPicture)
        {
            int index = pictures.IndexOf(partPicture);
            PictureBox realPart = GetInStagePicture(pictures[index]);
            Point partLocation = pictures[index].Location;
            partLocation.Offset(-(realPart.Width-pictures[index].Width), -(realPart.Height - pictures[index].Height));
            //partLocation.Offset(-(int)zoomManager.zoomLocation, 0);
            float magnification = sizeManager.stageDataRatio;
            // 位置のセット
            stageData.stageParts[index].position = 
                LocationConverter.LocationToStagePosition(
                partLocation,
                realPart.Size,
                pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                GetBasePoint(zoomStage), magnification);
            SetPicturePosition(index);
            
        }

        // 指定したインデックスのピクチャーボックスの位置をデータ通りに更新する
        private void SetPicturePosition(int index)
        {
            float magnification = sizeManager.stageDataRatio;
            Point partLocation =
                LocationConverter.StagePositionToLocation(
                    stageData.stageParts[index].position,
                    //pictures[index].Size,
                    GetInStagePicture(pictures[index]).Size,
                    pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                    GetBasePoint(zoomStage), magnification);
            //partLocation.Offset((int)zoomManager.zoomLocation, 0);
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

        public void ResetPictureSize(PictureBox partPicture)
        {
            StagePart part = GetPart(partPicture);

            // 大きさの設定
            partPicture.Size = GetInStagePicture(partPicture).Size;
        }

        // 見切れていない本来の大きさを取得
        private PictureBox GetInStagePicture(PictureBox partPicture)
        {
            PictureBox box = new PictureBox();
            StagePart part = GetPart(partPicture);
            float magnification = sizeManager.stageDataRatio;
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
            //location.Offset((int)(zoomManager.zoomLocation), 0);
            box.Location = location;
            return box;
        }

        // パーツの再配置
        private void ZoomStage_SizeChanged(object sender, EventArgs e)
        {

            // ステージのピクチャーボックスの大きさを調整
            //SettingStageLocation();

            // パーツのピクチャーボックスを再配置
            UpdateAllView();

            // ステージ表示の端の位置を知らせるラベルの更新
            //UpdateCornerLabel();
            // 選択状況
            //if (IndexOutOfRange()) return;

            //stageDataManager.SelectedPicture(partsListBox.SelectedIndex);
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
        public void ResizePartPicture(PictureBox picture, PictureBox range)
        {
            
            if (picture.Left < range.Left)
            {
                picture.Size = new Size(picture.Right - range.Left, picture.Height);
                picture.Location = new Point(range.Left, picture.Location.Y);
            }

            if (picture.Right > range.Right)
            {
                picture.Size = new Size(range.Right - picture.Left, picture.Height);
            }
            if (picture.Top < range.Top)
            {
                picture.Size = new Size(picture.Width, picture.Bottom - range.Top);
                picture.Location = new Point(picture.Location.X, range.Top);
            }
            if (picture.Bottom > range.Bottom)
            {
                picture.Size = new Size(picture.Width, range.Bottom - picture.Top);
            }
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
