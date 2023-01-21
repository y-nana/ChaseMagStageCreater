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
        private float zoomValue;        // 現在何倍か
        private bool isZoomed;          // 現在ズームしている状態かどうか
        private bool isZooming;         // ズームによる処理の実行中か
        private bool isClicked;         // クリック中か
        private Size baseStageSize;     // ズーム前のサイズ

        private PictureBox inStage;     // ステージのピクチャーボックス
        private PictureBox outStage;    // 最大サイズのピクチャーボックス
        private PictureBox zoomStage;   // ズーム時に大きくなるピクチャーボックス

        private RadioButton moveButtom; // 視点移動モードにするラジオボタン

        private Point startZoomLocation;    // 視点移動し始めのピクチャーボックスの位置
        private Point startMouseLocation;   // 視点移動し始めのマウスの位置

        public float maxZoomValue = 2.0f;   // 最大倍率

        private readonly float defaultPercent = 1.0f;   // 初期倍率

        private readonly float wheelPercentValue = 0.5f;//１ ホイールによる増加

        public  Label debugLabel;       // デバッグ用

        public ZoomManager(PictureBox inStage, PictureBox outStage, PictureBox zoomStage, RadioButton moveButtom)
        {
            zoomValue = defaultPercent;
            this.baseStageSize = inStage.Size;
            this.inStage = inStage;
            this.outStage = outStage;
            this.zoomStage = zoomStage;
            this.moveButtom = moveButtom;
            baseStageSize = inStage.Size;
            this.zoomStage.Location = inStage.Location;
            this.zoomStage.Size = inStage.Size;
            isZoomed = false;

            this.inStage.SizeChanged += InStageSizeChanged;
            this.inStage.MouseWheel += InStagePicture_MouseWheel;
            this.inStage.MouseDown += InStagePicture_Clicked;
            this.inStage.MouseMove += InStagePicture_MouseMove;
            this.inStage.MouseUp += InStagePicture_MouseUp;
            SetMaxZoomValue();

        }


        // ********************************************************
        // ステージの大きさが変更された
        // ********************************************************
        public void InStageSizeChanged(object sender, EventArgs e)
        {

            if (isZooming)
            {
                return;
            }

            baseStageSize = inStage.Size;
            ResetZoom();
            SetMaxZoomValue();
        }


        // ********************************************************
        // ズーム
        // ********************************************************

        // ホイールでズームする
        private void InStagePicture_MouseWheel(object sender, MouseEventArgs e)
        {
            bool zoomIn = e.Delta > 0;
            // マウスの位置のところへフォーカス
            PointF delta = new PointF(e.Location.X - (inStage.Width * 0.5f), e.Location.Y - (inStage.Height * 0.5f));
            //if (!ZoomChange(e.Delta))
            if (!ZoomChange(zoomIn))
            {
                return;
            }

            // マウスの位置のところへフォーカス

            delta.X = -delta.X * zoomValue + delta.X;
            delta.Y = -delta.Y * zoomValue + delta.Y;
            ChangeFocus(Point.Round(delta));


        }

        // ********************************************************
        // 視点移動
        // ********************************************************

        private void InStagePicture_Clicked(object sender, MouseEventArgs e)
        {


            if (e.Button == MouseButtons.Left && moveButtom.Checked)
            {
                // マウスの位置のところへフォーカス

                PointF delta = new PointF( e.Location.X - (inStage.Width * 0.5f), -(e.Location.Y - (inStage.Height* 0.5f)));
                ChangeFocus(Point.Round(delta));
            }

            // 右クリックでドラッグによる視点移動を開始
            if (e.Button == MouseButtons.Right)
            {
                isClicked = true;
                startZoomLocation =zoomStage.Location;
                startMouseLocation = inStage.FindForm().PointToClient(e.Location);
            }



        }
        // ドラッグ中の処理
        private void InStagePicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isClicked)
            {
                Point mousePos = inStage.FindForm().PointToClient(e.Location);
                mousePos.Offset(-startMouseLocation.X, -startMouseLocation.Y);
                mousePos.Offset(startZoomLocation);
                SetForcus(mousePos);

            }
        }
        // ドラッグ終了
        private void InStagePicture_MouseUp(object sender, MouseEventArgs e)
        {
            isClicked = false;

        }

        //ズームする ホイールで徐々にバージョン
        // ちらつきが気になるので未使用
        public bool ZoomChange(int deltaWheel)
        {

            if (!isZoomed)
            {
                baseStageSize = inStage.Size;
                
            }
            if (deltaWheel > 0)
            {
                if (zoomValue >= maxZoomValue)
                {
                    return false;
                }
                isZoomed = true;
                zoomValue += wheelPercentValue;
                if (zoomValue > maxZoomValue)
                {
                    zoomValue = maxZoomValue;
                }

            }
            else if(deltaWheel < 0)
            {
                if (zoomValue <= defaultPercent)
                {
                    return false;
                }
                zoomValue -= wheelPercentValue;
                if (zoomValue < defaultPercent)
                {
                    isZoomed = false;
                    zoomValue = defaultPercent;
                }


            }
            isZooming = true;
            Size size = new Size((int)Math.Round(baseStageSize.Width * zoomValue), (int)Math.Round(baseStageSize.Height * zoomValue));

            ChangeZoomStageSize(size);

            isZooming = false;
            return true;
        }

        //ズームする 一気に最大バージョン
        public bool ZoomChange(bool ZoomIn)
        {


            if (!isZoomed)
            {
                baseStageSize = inStage.Size;
            }
            if (ZoomIn)
            {

                if (zoomValue == maxZoomValue)
                {
                    return false;
                }
                isZoomed = true;
                zoomValue = maxZoomValue;

            }
            else
            {

                ResetZoom();
            }

            isZooming = true;
            Size size = new Size((int)Math.Round(inStage.Width * zoomValue), (int)Math.Round(inStage.Height * zoomValue));

            ChangeZoomStageSize(size);

            isZooming = false;
            return true;


        }

        // ズームをリセットする
        public void ResetZoom()
        {

            isZoomed = false;
            zoomValue = defaultPercent;

            ChangeZoomStageSize(baseStageSize);

        }

        // 最大倍率を算出しなおす
        private void SetMaxZoomValue()
        {
            float heightRatio = (float)outStage.Height / inStage.Height;
            float widthRatio = (float)outStage.Width / inStage.Width;

            if (heightRatio < widthRatio)
            {
                maxZoomValue = widthRatio;
                return;
            }
            maxZoomValue = heightRatio;
        }


        // ズーム中の視点を変更する

        // 現在からの加算移動
        public void ChangeFocus(Point value)
        {
            if (!isZoomed)
            {
                return;
            }

            Point location = zoomStage.Location;
            location.Offset(value.X, value.Y);

            debugLabel.Text = value.ToString();
            zoomStage.Location = GetInRangeLocation(location);
            

        }

        // 右上のロケーションをそのままセット
        public void SetForcus(Point value)
        {
            if (!isZoomed)
            {
                return;
            }

            zoomStage.Location = GetInRangeLocation(value);


        }

        // 描画する範囲内のロケーションにして返す
        private Point GetInRangeLocation(Point location)
        {
            
            if (inStage.Right > location.X + zoomStage.Width)
            {
                location.X = inStage.Right - zoomStage.Width;
            }
            if (inStage.Left < location.X)
            {
                location.X = inStage.Left;

            }
            if (inStage.Top < location.Y)
            {
                location.Y = inStage.Top;
            }
            if (inStage.Bottom > location.Y + zoomStage.Height)
            {
                location.Y = inStage.Bottom - zoomStage.Height;

            }
            return location;
        }



        // ズーム後の大きさによって位置を調整して変更する
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
