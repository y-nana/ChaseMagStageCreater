using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{
    // ズームを管理するクラス
    public class ZoomManager
    {

        public float zoomValue { get; private set; }     // 現在何倍か
        public bool isZoomed { get; private set; }
        public float zoomLocation { get; private set; }      // どこが中心か
        public Vector2 viewStagePictureSize { get; private set; }
        private Vector2 baseStageSize;

        public readonly float maxZoomValue = 2.0f;

        private readonly float defaultPercent = 1.0f;

        private readonly float wheelPercentValue = 0.1f;

        private readonly float moveValue = 40;

        public ZoomManager(Vector2 baseStageSize)
        {
            zoomValue = defaultPercent;
            this.baseStageSize = baseStageSize;
            viewStagePictureSize = baseStageSize;
            isZoomed = false;

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
                isZoomed = false;
                zoomValue -= wheelPercentValue;
                if (zoomValue < defaultPercent)
                {
                    zoomValue = defaultPercent;
                }

                zoomLocation = 0.0f;

            }
            ZoomAplly();
        }

        //ズームする
        public void ZoomChange(bool ZoomIn)
        {

            if (ZoomIn)
            {
                isZoomed = true;
                zoomValue = maxZoomValue;

            }
            else
            {
                isZoomed = false;
                zoomValue = defaultPercent;
                zoomLocation = 0.0f;

            }
            ZoomAplly();
        }



        private void ZoomAplly()
        {
            viewStagePictureSize = new Vector2(baseStageSize.x * zoomValue, baseStageSize.y * zoomValue);
        }

        // ズームをリセットする
        public void ResetZoom(Vector2 size)
        {
            baseStageSize = size;
            zoomLocation = 0.0f;

            isZoomed = false;
            zoomValue = defaultPercent;
            ZoomAplly();
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

        public void ChangeFocus(float value)
        {
            if (!isZoomed)
            {
                return;
            }

            zoomLocation += value;

            CheckRange();
        }

        public void SetForcus(float value)
        {
            if (!isZoomed)
            {
                return;
            }

            zoomLocation = value;
            CheckRange();

        }
        private void CheckRange()
        {
            // 表示範囲外まで移動しないようにする
            if (zoomLocation > (viewStagePictureSize.x - baseStageSize.x) * 0.5f)
            {
                zoomLocation = (viewStagePictureSize.x - baseStageSize.x) * 0.5f;
            }
            else if (zoomLocation < -(viewStagePictureSize.x - baseStageSize.x) * 0.5f)
            {
                zoomLocation = -(viewStagePictureSize.x - baseStageSize.x) * 0.5f;

            }
        }












    }
}
