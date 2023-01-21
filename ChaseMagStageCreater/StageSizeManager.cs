using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ChaseMagStageCreater
{

    

    class StageSizeManager
    {

        private StageData stageData;    // ステージデータ

        private PictureBox outStage;    // 最大サイズののピクチャーボックス
        private PictureBox inStage;     // ステージのピクチャーボックス
        private PictureBox zoomStage;   // ズーム時に大きくなるピクチャーボックス

        private float stageDataRatio;   // ステージデータとの倍率

        public Label debugLabel;        // デバッグ用

        public StageSizeManager(StageData stageData, PictureBox outStage, PictureBox inStage, PictureBox zoomStage)
        {
            this.stageData = stageData;
            this.outStage = outStage;
            this.inStage = inStage;
            this.zoomStage = zoomStage;
            // イベント登録
            this.outStage.SizeChanged += OutStageSizeChanged;
            this.zoomStage.SizeChanged += ZoomStage_SizeChanged;
            zoomStage.FindForm().SizeChanged += OutStageSizeChanged;
            // サイズ、位置セット
            ChangeStageSize();
        }

        // フォーム自体のサイズ変更など
        public void OutStageSizeChanged(object sender, EventArgs e)
        {
            ChangeStageSize();
        }

        // ズームに伴うサイズ変更
        private void ZoomStage_SizeChanged(object sender, EventArgs e)
        {

            Size size = new Size();
            Point location = new Point();
            if (zoomStage.Width < outStage.Width)
            {
                size.Width = zoomStage.Width;
                location.X = zoomStage.Location.X;
            }
            else
            {
                size.Width = outStage.Width;
                location.X = outStage.Location.X;

            }
            if (zoomStage.Height < outStage.Height)
            {
                size.Height = zoomStage.Height;
                location.Y = zoomStage.Location.Y;

            }
            else
            {
                size.Height = outStage.Height;
                location.Y = outStage.Location.Y;

            }

            inStage.Location = location;
            inStage.Size = size;

        }



        // 外側のステージサイズが変わった時に変更される
        // ステージピクチャーボックスの最大サイズ
        // データのステージサイズの変更に伴って大きさを変える
        public void ChangeStageSize()
        {

            // 横をマックスにする
            if (IsWidthMax())
            {
                // 倍率を入れなおす
                stageDataRatio = outStage.Width / stageData.width;

            }
            // 縦をマックスにする
            else
            {
                stageDataRatio = outStage.Height / stageData.height;

            }
            Size size = new Size();
            size.Width = (int)Math.Round(stageDataRatio * stageData.width);
            size.Height = (int)Math.Round(stageDataRatio * stageData.height);

            // 位置変更
            // 中心を求める
            PointF newLocation = outStage.Location;
            newLocation.X += outStage.Width * 0.5f;
            newLocation.Y += outStage.Height * 0.5f;
            // 中心から左上へ
            newLocation.X -= size.Width * 0.5f;
            newLocation.Y -= size.Height* 0.5f;

            inStage.Location = Point.Round(newLocation);
            // サイズ変更
            inStage.Size = size;


        }



        // 横幅をぎりぎりにするか 縦横比率から求める
        private bool IsWidthMax()
        {
            float outStageAspectRatio = (float)outStage.Width / outStage.Height;
            float dataStageSizeAspectRatio = (float)stageData.width / stageData.height;
            return outStageAspectRatio < dataStageSizeAspectRatio;
        }




    }
}
