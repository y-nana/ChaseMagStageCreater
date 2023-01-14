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

        
    }


}
