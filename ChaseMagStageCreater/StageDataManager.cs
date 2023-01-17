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
        private PictureBox stage;

        // 倍率
        public float magnification { get; set; }

        // ズームマネージャー
        private ZoomManager zoomManager;

        // ドラッグイベントを追加するメソッド
        private Action<PictureBox> addEventAction;



        public StageDataManager(float width, float height, PictureBox stage, ZoomManager zoomManager, Action<PictureBox> addEventAction)
        {
            stageData = new StageData();
            stageData.width = width;
            stageData.height = height;
            this.stage = stage;
            this.zoomManager = zoomManager;

            pictures = new List<PictureBox>();
            pictureBoxDataManager = new PictureBoxDataManager();
            this.addEventAction = addEventAction;

        }

        // パーツを新しく追加する
        public void AddNewPart(StagePartsCategory category, Vector2 position, Control.ControlCollection controls)
        {
        
            StagePart part = new StagePart();
            part.category = category;
            part.position = position;
            part.sizeMagnification = Vector2.one;
            stageData.stageParts.Add(part);
            AddPictureBox(part,controls);
            
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
            UpdateAllView(controls, false);

        }

        // すべての表示の更新
        public void UpdateAllView(Control.ControlCollection controls, bool isZoom)
        {
            foreach (var item in pictures)
            {
                item.Dispose();
            }
            pictures.Clear();
            for (int i = 0; i < stageData.stageParts.Count; i++)
            {
                AddPictureBox(stageData.stageParts[i], controls);
                if (isZoom && !StageOfRangeIn(pictures[i], stage))
                {

                    pictures[i].Visible = false;

                }
            }
        }

        // パーツに応じた一つのピクチャーボックスを作り追加する
        private void AddPictureBox(StagePart part, Control.ControlCollection controls)
        {
            PictureBox picture = new PictureBox();
            // 大きさの設定
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            // 位置の設定
            Point location =
            LocationConverter.StagePositionToLocation(part.position, size, pictureBoxDataManager.GetBasePoint(part.category), GetBasePoint(stage), magnification);
            location.Offset((int)(zoomManager.zoomLocation), 0);

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
            controls.Add(picture);
            picture.BringToFront();

        }


        // 大きさの更新
        public void UpdateSize(int index , Vector2 value)
        {
            stageData.stageParts[index].sizeMagnification = value;

            StagePart part = stageData.stageParts[index];

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
            }
        }

        // 移動させたピクチャーボックスの位置をデータとして反映させる
        public void PictureLocationApply(PictureBox partPicture)
        {
            int index = pictures.IndexOf(partPicture);
            Point partLocation = pictures[index].Location;
            partLocation.Offset(-(int)zoomManager.zoomLocation, 0);
            // 位置のセット
            stageData.stageParts[index].position = 
                LocationConverter.LocationToStagePosition(
                partLocation,
                pictures[index].Size,
                pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                GetBasePoint(stage), magnification);
            SetPicturePosition(index);
            
        }

        // 指定したインデックスのピクチャーボックスの位置をデータ通りに更新する
        private void SetPicturePosition(int index)
        {
            Point partLocation =
                LocationConverter.StagePositionToLocation(
                    stageData.stageParts[index].position,
                    pictures[index].Size,
                    pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                    GetBasePoint(stage), magnification);
            partLocation.Offset((int)zoomManager.zoomLocation, 0);
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
