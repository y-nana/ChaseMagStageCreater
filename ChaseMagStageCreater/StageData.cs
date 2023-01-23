using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{


    // 一つのステージのデータ
    public class StageData
    {
        public List<StagePart> stageParts { get; set; } = new List<StagePart>();
        public float width { get; set; }
        public float height { get; set; }

        public StageData() { }

        public StageData(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
        public StageData(StageData stageData)
        {
            stageParts = new List<StagePart>(stageData.stageParts);
            width = stageData.width;
            height = stageData.height;
        }

        
    }


}
