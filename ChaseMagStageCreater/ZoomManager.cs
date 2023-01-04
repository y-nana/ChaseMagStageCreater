using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{
    public class ZoomManager
    {

        private float zoomValue;      // 現在何倍か（％）
        public float zoomLocation { get; private set; }      // どこが中心か
        public Vector2 viewStagePictureSize { get; private set; }
        private Vector2 baseStageSize;

        private readonly float wheelPercentValue = 0.1f;
        private readonly float maxZoomValue = 2.0f;
        private readonly float minZoomValue = 1.0f;

        private readonly float defaultPercent = 1.0f;


        public ZoomManager(Vector2 baseStageSize)
        {
            zoomValue = defaultPercent;
            this.baseStageSize = baseStageSize;
            viewStagePictureSize = baseStageSize;
            

        }


        //ズームする
        public void ZoomChange(int deltaWheel, float locationX)
        {
            this.zoomLocation = locationX;

            if (deltaWheel > 0)
            {
                
                zoomValue += wheelPercentValue;
                if (zoomValue > maxZoomValue)
                {
                    zoomValue = maxZoomValue;
                }

            }
            else if(deltaWheel < 0)
            {
                zoomValue -= wheelPercentValue;
                if (zoomValue < minZoomValue)
                {
                    zoomValue = minZoomValue;
                }
            }
            ZoomAplly();
        }



        private void ZoomAplly()
        {
            viewStagePictureSize = new Vector2(baseStageSize.x * zoomValue, baseStageSize.y * zoomValue);
        }

        public void ResetZoom(Vector2 size)
        {
            baseStageSize = size;
            zoomValue = defaultPercent;
            ZoomAplly();
        }
         
















    }
}
