using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ChaseMagStageCreater
{
    class GuideViewManager
    {
        private Label topleftLabel;
        private Label topRightLabel;
        private Label bottomLeftLabel;
        private Label bottomRightLabel;

        private PictureBox inStage;
        private PictureBox zoomStage;

        private StageData stageData;

        public GuideViewManager(Label topleftLabel, Label topRightLabel, Label bottomLeftLabel, Label bottomRightLabel, PictureBox inStage, PictureBox zoomStage, StageData stageData)
        {
            this.topleftLabel = topleftLabel;
            this.topRightLabel = topRightLabel;
            this.bottomLeftLabel = bottomLeftLabel;
            this.bottomRightLabel = bottomRightLabel;
            this.inStage = inStage;
            this.zoomStage = zoomStage;
            this.stageData = stageData;

            this.inStage.SizeChanged += InStage_SizeChanged;
            this.zoomStage.LocationChanged += ZoomStage_LocationChanged;
            UpdateCornerLabel();
        }

        private void ZoomStage_LocationChanged(object sender, EventArgs e)
        {
            UpdateLabelValue();
        }

        private void InStage_SizeChanged(object sender, EventArgs e)
        {
            UpdateCornerLabel();
        }

        private void UpdateLabelValue()
        {
            // 端のステージの位置
            float magnification = zoomStage.Width / stageData.width;
            int right = inStage.Right;
            int left = inStage.Left;
            int top = inStage.Top;
            int bottom = inStage.Bottom;
            Point basePoint = StageDataManager.GetBasePoint(zoomStage);

            Vector2 topLeft = LocationConverter.LocationToStagePosition(
                new Point(left, top),
                basePoint,
                magnification);

            Vector2 bottomRight = LocationConverter.LocationToStagePosition(
                new Point(right, bottom),
                basePoint,
                magnification);


            topleftLabel.Text = topLeft.x.ToString() + ", " + (-topLeft.y).ToString();
            topRightLabel.Text = bottomRight.x.ToString() + ", " + (-topLeft.y).ToString();
            bottomLeftLabel.Text = topLeft.x.ToString() + ", " + bottomRight.y.ToString();
            bottomRightLabel.Text = bottomRight.x.ToString() + ", " + bottomRight.y.ToString();

        }

        private void UpdateCornerLabel()
        {

            UpdateLabelValue();


            // ラベルの表示位置の調整
            int margin = 5;
            // 左上
            Point location = inStage.Location;
            location.Offset(0, -(topleftLabel.Height + margin));
            topleftLabel.Location = location;
            // 右上
            location = inStage.Location;
            location.Offset(inStage.Width - topRightLabel.Width, -(topRightLabel.Height + margin));
            topRightLabel.Location = location;
            // 左下
            location = inStage.Location;
            location.Offset(0, (inStage.Height + margin));
            bottomLeftLabel.Location = location;
            // 右下
            location = inStage.Location;
            location.Offset(inStage.Width - bottomRightLabel.Width, (inStage.Height + margin));
            bottomRightLabel.Location = location;


        }







    }
}
