using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;

namespace ファイル分割
{
    class Program
    {
        static void Main(string[] args)
        {
            var enc = Encoding.GetEncoding(ConfigurationManager.AppSettings["encoding"]);
            var parentDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));

            var startCol = int.Parse(ConfigurationManager.AppSettings["keyCodeStartCol"]);
            var len = int.Parse(ConfigurationManager.AppSettings["keyCodeLength"]);
            var fileName = ConfigurationManager.AppSettings["outputFileName"];


            var allLines = File.ReadAllLines(args[0], enc);

            var keyCodes = allLines.GroupBy(line => line.Substring(startCol - 1, len).Trim());
            
            foreach(var keyCode in keyCodes)
            {
                var dir = Path.Combine(parentDir, keyCode.Key);
                Directory.CreateDirectory(dir);
                var filePath = Path.Combine(dir, fileName);

                var lines = allLines.Where(line => line.Substring(startCol - 1, len).Trim() == keyCode.Key);
                File.AppendAllLines(filePath, lines, enc);
            }

        }
    }
}
