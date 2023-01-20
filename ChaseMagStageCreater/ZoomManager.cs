using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChaseMagStageCreater
{
    // ズームを管理するクラス
    public class ZoomManager
    {
        public float zoomValue { get; private set; }     // 現在何倍か
        public bool isZoomed { get; private set; }
        public int zoomLocation { get; private set; }      // どこが中心か
        //public Size viewStagePictureSize { get; private set; }
        private Size baseStageSize;

        private PictureBox inStage;
        private PictureBox outStage;
        private PictureBox zoomStage;

        public float maxZoomValue = 2.0f;

        private readonly float defaultPercent = 1.0f;

        private readonly float wheelPercentValue = 0.1f;

        private readonly int moveValue = 40;

        //private Control control;

        public ZoomManager(PictureBox inStage, PictureBox outStage, PictureBox zoomStage)
        {
            zoomValue = defaultPercent;
            this.baseStageSize = inStage.Size;
            this.inStage = inStage;
            this.outStage = outStage;
            this.zoomStage = zoomStage;
            //viewStagePictureSize = baseStageSize;
            baseStageSize = inStage.Size;
            this.zoomStage.Location = inStage.Location;
            this.zoomStage.Size = inStage.Size;
            isZoomed = false;

            this.inStage.SizeChanged += InStageSizeChanged;
            this.inStage.MouseWheel += InStagePicture_MouseWheel;
        }


        // ********************************************************
        // ズーム
        // ********************************************************

        // ホイールでズームする
        private void InStagePicture_MouseWheel(object sender, MouseEventArgs e)
        {
            bool zoomIn = e.Delta > 0;
            /*
            if (zoomIn && (!CanZoom()||zoomManager.isZoomed))
            {
                return;
            }
            */
            ZoomChange(zoomIn);
            Point mouseLocation = e.Location;
            mouseLocation = inStage.FindForm().PointToClient(mouseLocation);
            // マウスの位置のところへフォーカス
            float delta = mouseLocation.X - (inStage.Location.X + inStage.Width * 0.5f);
            ChangeFocus(-(int)Math.Round(delta));
            //PictureViewReflesh();


        }

        //ズームする
        public void ZoomChange(int deltaWheel)
        {

            if (deltaWheel > 0)
            {
                isZoomed = true;
                zoomValue += wheelPercentValue;
                if (zoomValue > maxZoomValue)
                {
                    zoomValue = maxZoomValue;
                }

            }
            else if(deltaWheel < 0)
            {
                zoomValue -= wheelPercentValue;
                if (zoomValue < defaultPercent)
                {
                    isZoomed = false;
                    zoomValue = defaultPercent;
                }

                zoomLocation = 0;

            }
            //ZoomAplly();
        }

        //ズームする
        public void ZoomChange(bool ZoomIn)
        {
            if (!isZoomed)
            {
                baseStageSize = inStage.Size;
            }
            if (ZoomIn)
            {
                isZoomed = true;
                zoomValue = maxZoomValue;

            }
            else
            {
                /*
                isZoomed = false;
                zoomValue = defaultPercent;
                zoomLocation = 0.0f;
                */
                ResetZoom();
            }
            Size size = new Size((int)Math.Round(inStage.Width * zoomValue), (int)Math.Round(inStage.Height * zoomValue));

            ChangeZoomStageSize(size);



            //ZoomAplly();
        }


        public void InStageSizeChanged(object sender, EventArgs e)
        {
            //ResetZoom();
            //baseStageSize = inStage.Size;
            if (isZoomed)
            {
                return;
            }
            zoomStage.Location = inStage.Location;
            zoomStage.Size = inStage.Size;
            float heightRatio = outStage.Height / inStage.Height;
            float widthRatio = outStage.Width / inStage.Width;

            if (heightRatio < widthRatio)
            {
                maxZoomValue = widthRatio;
                return;
            }
            maxZoomValue = heightRatio;
        }
        //private void ZoomAplly()
        //{
        //viewStagePictureSize = new Vector2(baseStageSize.x * zoomValue, baseStageSize.y * zoomValue);
        //viewStagePictureSize = new Size(baseStageSize.Width * zoomValue, baseStageSize.Height * zoomValue);
        //}

        // ズームをリセットする
        public void ResetZoom()
        {
            
            //baseStageSize = size;
            zoomLocation = 0;

            isZoomed = false;
            zoomValue = defaultPercent;

            ChangeZoomStageSize(baseStageSize);

            //ZoomAplly();
        }

        // ズーム中の視点を変更する
        public void ChangeFocus(bool isRight)
        {
            if (!isZoomed)
            {
                return;
            }

            if (isRight)
            {
                ChangeFocus(-moveValue);
            }
            else
            {
                ChangeFocus(moveValue);
            }

            
        }

        public void ChangeFocus(int value)
        {
            if (!isZoomed)
            {
                return;
            }

            zoomLocation += value;

            CheckRange();
            zoomStage.Location.Offset(zoomLocation, 0);

        }

        public void SetForcus(int value)
        {
            if (!isZoomed)
            {
                return;
            }

            zoomLocation = value;
            CheckRange();
            zoomStage.Location.Offset(zoomLocation, 0);


        }
        private void CheckRange()
        {
            SizeF viewStagePictureSize = new SizeF(baseStageSize.Width * zoomValue, baseStageSize.Height);
            // 表示範囲外まで移動しないようにする
            float maxWidth = (viewStagePictureSize.Width - baseStageSize.Width) *0.5f;
            if (zoomLocation > maxWidth)
            {
                zoomLocation = (int)Math.Round(maxWidth);
            }
            else if (zoomLocation < -maxWidth)
            {
                zoomLocation = -(int)Math.Round(maxWidth);

            }
        }

        /*
        public void ChangeBaseStageSize(Size baseSize, float maxZoom)
        {
            this.baseStageSize = baseSize;
            maxZoomValue = maxZoom;

        }
        */

        private void ChangeZoomStageSize(Size size)
        {
            PointF location = inStage.Location;
            location.X += inStage.Width * 0.5f;
            location.Y += inStage.Height * 0.5f;
            location.X -= size.Width * 0.5f;
            location.Y -= size.Height * 0.5f;
            zoomStage.Location = Point.Round(location);
            zoomStage.Size = size;
        }









    }
}
