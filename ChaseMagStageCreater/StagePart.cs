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
        PoleScaffold,
        Clip
    }
    // ステージ内のオブジェクトのデータ
    public class StagePart
    {
        public StagePartsCategory category { get; set; }
        public Vector2 position { get; set; }

        public Vector2 sizeMagnification { get; set; } = new Vector2(1.0f, 1.0f);
        public bool isNorth { get; set; }

        public StagePart()
        {
            this.category = StagePartsCategory.Scaffold;
            this.position = Vector2.zero;
            this.sizeMagnification = Vector2.one;
            this.isNorth = false;
        }

        public StagePart(StagePartsCategory category, Vector2 position, Vector2 sizeMagnification, bool isNorth)
        {
            this.category = category;
            this.position = position;
            this.sizeMagnification = sizeMagnification;
            this.isNorth = isNorth;
        }

        public static bool IsSetPole(StagePartsCategory category)
        {
            return category == StagePartsCategory.JumpRamp 
                || category == StagePartsCategory.Wall
                || category == StagePartsCategory.PoleScaffold;

        }

    }
}
