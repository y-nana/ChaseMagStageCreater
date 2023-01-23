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

        private StageDataManager stageDataManager;

        private readonly int viewNum = 1;


        public GuideViewManager(Label topleftLabel, Label topRightLabel, Label bottomLeftLabel, Label bottomRightLabel, PictureBox inStage, PictureBox zoomStage, StageDataManager stageDataManager)
        {
            this.topleftLabel = topleftLabel;
            this.topRightLabel = topRightLabel;
            this.bottomLeftLabel = bottomLeftLabel;
            this.bottomRightLabel = bottomRightLabel;
            this.inStage = inStage;
            this.zoomStage = zoomStage;
            this.stageDataManager = stageDataManager;

            this.inStage.SizeChanged += InStage_SizeChanged;
            this.zoomStage.LocationChanged += ZoomStage_LocationChanged;
            UpdateCornerLabel();
        }
        public GuideViewManager(PictureBox inStage, PictureBox zoomStage, StageDataManager stageDataManager)
        {

            this.inStage = inStage;
            this.zoomStage = zoomStage;
            this.stageDataManager = stageDataManager;

            CreateLabel();

            this.inStage.SizeChanged += InStage_SizeChanged;
            this.zoomStage.LocationChanged += ZoomStage_LocationChanged;
            UpdateCornerLabel();
        }

        private void CreateLabel()
        {
            this.topleftLabel = new Label();
            this.topRightLabel = new Label();
            this.bottomLeftLabel = new Label();
            this.bottomRightLabel = new Label();

            inStage.FindForm().Controls.Add(topleftLabel);
            inStage.FindForm().Controls.Add(topRightLabel);
            inStage.FindForm().Controls.Add(bottomLeftLabel);
            inStage.FindForm().Controls.Add(bottomRightLabel);

            this.topleftLabel.BringToFront();
            this.topRightLabel.BringToFront();
            this.bottomLeftLabel.BringToFront();
            this.bottomRightLabel.BringToFront();
            this.topleftLabel.AutoSize = true;
            this.topRightLabel.AutoSize = true;
            this.bottomLeftLabel.AutoSize = true;
            this.bottomRightLabel.AutoSize = true;

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
            float magnification = zoomStage.Width / stageDataManager.GetWidth();
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

            topLeft.x = (float)Math.Round(topLeft.x, viewNum);
            topLeft.y = (float)Math.Round(topLeft.y, viewNum);
            bottomRight.x = (float)Math.Round(bottomRight.x, viewNum);
            bottomRight.y = (float)Math.Round(bottomRight.y, viewNum);

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
