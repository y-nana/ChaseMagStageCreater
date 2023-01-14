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


        public StageDataManager(float width, float height, PictureBox stage)
        {
            stageData = new StageData();
            stageData.width = width;
            stageData.height = height;
            this.stage = stage;
            pictures = new List<PictureBox>();
            pictureBoxDataManager = new PictureBoxDataManager();

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
            UpdateAllView(controls);

        }

        // すべての表示の更新
        public void UpdateAllView(Control.ControlCollection controls)
        {
            foreach (var item in pictures)
            {
                item.Dispose();
            }
            pictures.Clear();
            foreach (StagePart part in stageData.stageParts)
            {
                AddPictureBox(part, controls);
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
            //Point location = stage.Location;

            //location.Offset((int)Math.Round(stage.Size.Width / 2.0f), stage.Size.Height);

            //Vector2 position = new Vector2(part.position.x * magnification, -part.position.y * magnification);
            Point location =
            LocationConverter.StagePositionToLocation(part.position, size, pictureBoxDataManager.GetBasePoint(part.category), GetBasePoint(stage), magnification);
            // **********************************************************zoom未実装
            //location.Offset((int)(zoomManager.zoomLocation), 0);

            picture.Location = location;
            picture.Size = size;


            if (StagePart.IsSetPole(part.category) && !part.isNorth)
            {
                picture.BackgroundImage = pictureBoxDataManager.GetBitmap(part.category, Pole.South);
            }
            else
            {
                picture.BackgroundImage = pictureBoxDataManager.GetBitmap(part.category,Pole.North);

            }

            picture.BackgroundImageLayout = ImageLayout.Stretch;
            /*
            picture.MouseDown += StagePartMouseDown;
            picture.MouseUp += StagePartMouseUp;
            picture.MouseMove += StagePartMouseMove;
            */
            if (!StageOfRangeIn(picture,stage))
            {
                //picture.Visible = false;

            }
            /*
            if (location.X < stage.Location.X || location.X + pictureSize.Width > stage.Location.X + stage.Width)
            {
                picture.Visible = false;

            }
            */

            pictures.Add(picture);
            controls.Add(picture);
            picture.BringToFront();


        }

        public void SelectedPicture(int index)
        {
            pictures[index].Image = Properties.Resources.SelectMask;
        }

        // 指定したインデックスのパーツのデータを更新する
        public void UpdatePartData(StagePart part, int index)
        {
            stageData.stageParts[index].position = part.position;
            stageData.stageParts[index].sizeMagnification = part.sizeMagnification;
            stageData.stageParts[index].isNorth = part.isNorth;
            // いる？
            pictures[index].Image = null;
        }

        public void UpdateSize(int index , Vector2 value)
        {
            stageData.stageParts[index].sizeMagnification = value;

            StagePart part = stageData.stageParts[index];

            // 大きさの設定
            Vector2 pictureSize = pictureBoxDataManager.GetPictureSize(part.category);
            Size size = new Size((int)Math.Round(pictureSize.x * magnification * part.sizeMagnification.x),
                (int)Math.Round(pictureSize.y * magnification * part.sizeMagnification.y));
            pictures[index].Size = size;
            pictures[index].Location =
                LocationConverter.StagePositionToLocation(
                    value,
                    pictures[index].Size,
                    pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                    GetBasePoint(stage), magnification);
        }

        public void UpdatePosition(int index, Vector2 value)
        {
            stageData.stageParts[index].position = value;
            pictures[index].Location = 
                LocationConverter.StagePositionToLocation(
                    value,
                    pictures[index].Size, 
                    pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category), 
                    GetBasePoint(stage), magnification);

        }

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

        // 移動させたピクチャーボックスの位置をデータとして反映させる
        public void PictureLocationApply(PictureBox partPicture)
        {
            //partLocation.Offset(-(int)zoomManager.zoomLocation, 0);
           int index = pictures.IndexOf(partPicture);
            // 位置のセット
            stageData.stageParts[index].position = 
                LocationConverter.LocationToStagePosition(
                pictures[index].Location,
                pictures[index].Size,
                pictureBoxDataManager.GetBasePoint(stageData.stageParts[index].category),
                GetBasePoint(stage), magnification);
            
        }

        public StagePart GetPart(PictureBox partPicture)
        {
            int index = pictures.IndexOf(partPicture);
            return stageData.stageParts[index];
        }

        //*******************************************************************************************

        // パーツを表示するかどうか
        private bool StageOfRangeIn(PictureBox picture, PictureBox range)
        {
            if (picture.Location.X < range.Location.X)
            {
                return false;
            }
            if (picture.Location.X + picture.Width > range.Location.X + range.Width)
            {
                return false;
            }
            if (picture.Location.Y < range.Location.Y)
            {
                return false;
            }
            if (picture.Location.Y + picture.Height > range.Location.Y + range.Height)
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


        public static Point ToCenter(PictureBox pictureBox)
        {
            Point location = pictureBox.Location;
            location.Offset(-(int)Math.Round(pictureBox.Width * 0.5f),-(int)Math.Round(pictureBox.Height * 0.5f));
            return location;
        }

    }

}
