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

        private readonly float pixelMagnification = 0.01f;


        public PictureBoxDataManager()
        {
            // ピクセルから大きさを計算するためのデータ
            categoryImageDatas = new Dictionary<StagePartsCategory, PictureBoxData>();
            categoryImageDatas.Add(StagePartsCategory.Scaffold, new PictureBoxData(new Vector2(8, 4), Properties.Resources.scaffold, BasePoint.Top));
            categoryImageDatas.Add(StagePartsCategory.Wall, new PictureBoxData(new Vector2(1, 20), Properties.Resources.wallNorth, Properties.Resources.wallSouth, BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.NormalWall, new PictureBoxData(new Vector2(4, 30), Properties.Resources.floor, BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.JumpRamp, new PictureBoxData(new Vector2(4, 2.5f), Properties.Resources.jumpRampNorth, Properties.Resources.jumpRampSouth, BasePoint.Bottom));
            categoryImageDatas.Add(StagePartsCategory.ItemBox, new PictureBoxData(new Vector2(4, 2.5f), Properties.Resources.itemBox, BasePoint.Bottom));

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
