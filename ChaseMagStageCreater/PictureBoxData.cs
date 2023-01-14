using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{

    public enum BasePoint
    {
        Top,
        Center,
        Bottom
    }

    public class PictureBoxData
    {

        public Vector2 scale { get; set; }

        public Bitmap[] bitmap { get; set; }

        public BasePoint basePoint { get; set; }

        public PictureBoxData(Vector2 scale,Bitmap bitmap, BasePoint basePoint)
        {
            this.scale = scale;
            this.bitmap = new Bitmap[1] { bitmap };
            this.basePoint = basePoint;
        }
        public PictureBoxData(Vector2 scale, Bitmap bitmap, Bitmap altBitmap, BasePoint basePoint)
        {
            this.scale = scale;
            this.bitmap = new Bitmap[2] { bitmap, altBitmap };
            this.basePoint = basePoint;

        }



    }
}
