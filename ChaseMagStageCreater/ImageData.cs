using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{
    class ImageData
    {

        public Vector2 scale { get; set; }

        public Bitmap[] bitmap { get; set; }

        

        public ImageData(Vector2 scale,Bitmap bitmap)
        {
            this.scale = scale;
            this.bitmap = new Bitmap[1] { bitmap };
        }
        public ImageData(Vector2 scale, Bitmap bitmap, Bitmap altBitmap)
        {
            this.scale = scale;
            this.bitmap = new Bitmap[2] { bitmap, altBitmap };
        }


    }
}
