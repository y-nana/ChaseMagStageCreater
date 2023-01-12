using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{

    public enum StagePartsCategory
    {
        Scaffold,
        JumpRamp,
        Wall,
        NormalWall,
        ItemBox,

    }

    // ステージ内のオブジェクトのデータ
    public class StagePart
    {
        public StagePartsCategory category { get; set; }
        public Vector2 position { get; set; }

        public Vector2 sizeMagnification { get; set; } = new Vector2(1.0f, 1.0f);
        public bool isNorth { get; set; }

    }

    // 一つのステージのデータ
    public class StageData
    {
        public List<StagePart> stageParts { get; set; } = new List<StagePart>();
        public float width { get; set; }
        public float height { get; set; }

        
    }

    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
