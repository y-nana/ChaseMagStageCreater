using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ChaseMagStageCreater.Resources
{
    class StageDataManager
    {

        private StageData stageData;

        private List<PictureBox> pictures;

        public StageDataManager()
        {
            pictures = new List<PictureBox>();
            stageData = new StageData();
        }



    }

}
