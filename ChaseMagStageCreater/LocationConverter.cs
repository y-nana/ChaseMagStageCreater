using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChaseMagStageCreater
{
    class LocationConverter
    {
        // をUnity内の座標をアプリケーション上の座標へ変換する
        public static Point StagePositionToLocation(Vector2 pos, Size size, BasePoint basePoint, Point stageBaseLocation, float magnification)
        {

            Point location = stageBaseLocation;
            // 原点へ
            //location.Offset((int)Math.Round(stage.Size.Width / 2.0f), stage.Size.Height);

            PointF delta = Point.Empty;
            switch (basePoint)
            {
                case BasePoint.Top:
                    delta = new PointF(-size.Width / 2.0f, 0.0f);

                    break;
                case BasePoint.Center:
                    delta = new PointF(-size.Width / 2.0f, -size.Height / 2.0f);
                    break;
                case BasePoint.Bottom:
                    delta = new PointF(-size.Width / 2.0f, -size.Height);
                    break;

            }
            // アプリケーション上の座標へ変換

            location.Offset((int)Math.Round(pos.x * magnification), -(int)Math.Round(pos.y * magnification));
            location.Offset(Point.Round(delta));
            //PointF pointf = new PointF(pos.x + delta.X, pos.y + delta.Y);
            return location;
        }



        // アプリケーション上の座標をUnity内の座標へ変換する
        public static Vector2 LocationToStagePosition(Point point, Size size, BasePoint basePoint, Point stageBaseLocation, float magnification)
        {
            //Point location = stageBaseLocation;
            // 原点へ
            //location.Offset((int)Math.Round(stage.Size.Width / 2.0f), stage.Size.Height);

            switch (basePoint)
            {
                case BasePoint.Top:
                    point.Offset((int)Math.Round(size.Width / 2.0f), 0);

                    break;
                case BasePoint.Center:
                    point.Offset((int)Math.Round(size.Width / 2.0f), (int)Math.Round(size.Height / 2.0f));

                    break;
                case BasePoint.Bottom:
                    point.Offset((int)Math.Round(size.Width / 2.0f), size.Height);

                    break;

            }

            return LocationToStagePositionIncrement(point,stageBaseLocation,magnification);
            /*
            Vector2 deltaMove = new Vector2(point.X - location.X, point.Y - location.Y);

            // ステージ上のポジションへ変換
            Vector2 returnValue = new Vector2(
                deltaMove.x / magnification,
                deltaMove.y / magnification
                );
            // 0.5刻みにする
            float increment = 0.5f;
            returnValue = new Vector2(IncrementOfValue(returnValue.x, increment), -IncrementOfValue(returnValue.y, increment));
            return returnValue;
            */
        }


        // アプリケーション上の座標をUnity内の座標へ変換する

        public static Vector2 LocationToStagePosition(Point point, Point stageBaseLocation, float magnification)
        {
            Point location = stageBaseLocation;


            Vector2 deltaMove = new Vector2(point.X - location.X, point.Y - location.Y);

            // ステージ上のポジションへ変換
            Vector2 returnValue = new Vector2(
                deltaMove.x / magnification,
                deltaMove.y / magnification
                );
           return returnValue;

        }
        public static Vector2 LocationToStagePositionIncrement(Point point, Point stageBaseLocation, float magnification)
        {
            // ステージ上のポジションへ変換
            Vector2 returnValue = LocationToStagePosition(point, stageBaseLocation, magnification);
            // 0.5刻みにする
            float increment = 0.5f;
            returnValue = new Vector2(IncrementOfValue(returnValue.x, increment), -IncrementOfValue(returnValue.y, increment));
            return returnValue;

        }
        // 指定の値刻みの値を返す
        public static float IncrementOfValue(float value, float increment)
        {

            float r = (float)(Math.Floor(value / increment) * increment);
            if (r < value)
                r += increment;
            return r;


        }
    }
}
