using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.IO;

namespace ChaseMagStageCreater
{
    class JsonManager
    {

        public static void InportData()
        {

        }

        public static void ExportData(string path, StageData data)
        {


            // jsonに変換
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            string jsonstr = JsonSerializer.Serialize(data, options);
            // 保存先を開く
            StreamWriter writer = new StreamWriter(path, false);
            // jsonデータ書き込み
            writer.WriteLine(jsonstr);
            // バッファのクリア、クローズ
            writer.Flush();
            writer.Close();

        }




    }
}
