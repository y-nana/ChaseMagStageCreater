using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{
    class PictureBoxDataManager
    {

        private Dictionary<StagePartsCategory, PictureBoxData> categoryImageDatas;
        // unityはpixelの100分の1
        private readonly float pixelMagnification = 0.01f;

        //unityプレハブで設定したスケールの値と同じ
        private readonly Vector2 scaffoldScale = new Vector2(8, 4);
        private readonly Vector2 wallScale = new Vector2(1, 20);
        private readonly Vector2 normalWallScale = new Vector2(4, 30);
        private readonly Vector2 jumpRampScale = new Vector2(4, 2.5f);
        private readonly Vector2 itemBoxScale = new Vector2(4, 2.5f);
        private readonly Vector2 poleScaffoldScale = new Vector2(10, 1);
        private readonly Vector2 clipScale = new Vector2(0.6f, 0.6f);


        public PictureBoxDataManager()
        {
            // ピクセルから大きさを計算するためのデータ
            categoryImageDatas = new Dictionary<StagePartsCategory, PictureBoxData>();
            PictureBoxData scaffoldData = new PictureBoxData(
                scaffoldScale, 
                Properties.Resources.scaffold, 
                BasePoint.Top);
            categoryImageDatas.Add(StagePartsCategory.Scaffold, scaffoldData);

            PictureBoxData wallData = new PictureBoxData(
                wallScale, 
                Properties.Resources.wallNorth, 
                Properties.Resources.wallSouth, 
                BasePoint.Bottom);
            categoryImageDatas.Add(StagePartsCategory.Wall, wallData);

            PictureBoxData normalWallData = new PictureBoxData(
                normalWallScale, 
                Properties.Resources.floor, 
                BasePoint.Bottom);
            categoryImageDatas.Add(StagePartsCategory.NormalWall, normalWallData);

            PictureBoxData jumpRampData = new PictureBoxData(
                jumpRampScale, 
                Properties.Resources.jumpRampNorth, 
                Properties.Resources.jumpRampSouth, 
                BasePoint.Bottom);
            categoryImageDatas.Add(StagePartsCategory.JumpRamp,jumpRampData);

            PictureBoxData itemBoxData = new PictureBoxData(
                itemBoxScale, 
                Properties.Resources.itemBox, 
                BasePoint.Bottom);
            categoryImageDatas.Add(StagePartsCategory.ItemBox,itemBoxData);
            
            PictureBoxData poleScaffoldData = new PictureBoxData(
                poleScaffoldScale, 
                Properties.Resources.wallNorth, 
                Properties.Resources.wallSouth, 
                BasePoint.Bottom);
            categoryImageDatas.Add(StagePartsCategory.PoleScaffold,poleScaffoldData);
            
            PictureBoxData clipData = new PictureBoxData(
                clipScale, 
                Properties.Resources.clip, 
                BasePoint.Center);
            categoryImageDatas.Add(StagePartsCategory.Clip, clipData);

        }

        

        public Vector2 GetScale(StagePartsCategory category)
        {
            return categoryImageDatas[category].scale;
        }
        // pictureboxとして使うときの大きさを返す
        public Vector2 GetPictureSize(StagePartsCategory category)
        {

            Vector2 size = new Vector2();
            size.x = categoryImageDatas[category].scale.x * pixelMagnification * categoryImageDatas[category].bitmap[0].Width;
            size.y = categoryImageDatas[category].scale.y * pixelMagnification * categoryImageDatas[category].bitmap[0].Height;

            return size;
        }

        public Bitmap GetBitmap(StagePartsCategory category, Pole pole)
        {
            return categoryImageDatas[category].bitmap[(int)pole];

        }
        public Bitmap GetBitmap(StagePartsCategory category)
        {
            return categoryImageDatas[category].bitmap[0];

        }
        public BasePoint GetBasePoint(StagePartsCategory category)
        {
            return categoryImageDatas[category].basePoint;

        }


    }
}
