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
            var exitCode = 0;
            try
            {
                var enc = Encoding.GetEncoding(ConfigurationManager.AppSettings["encoding"]);
                var parentDir = Path.Combine(Path.GetDirectoryName(args[0]), DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
                Directory.CreateDirectory(parentDir);

                var startCol = int.Parse(ConfigurationManager.AppSettings["keyCodeStartCol"]);
                var len = int.Parse(ConfigurationManager.AppSettings["keyCodeLength"]);
                var outputFileNameWithPlaceHolderOfKeyCode = ConfigurationManager.AppSettings["outputFileNameWithPlaceHolderOfKeyCode"];


                var allLines = File.ReadAllLines(args[0], enc);

                var keyCodes = allLines.GroupBy(line => line.Substring(startCol - 1, len).Trim());

                foreach (var keyCode in keyCodes)
                {
                    var filePath = Path.Combine(parentDir, string.Format(outputFileNameWithPlaceHolderOfKeyCode, keyCode.Key));

                    var lines = allLines.Where(line => line.Substring(startCol - 1, len).Trim() == keyCode.Key);
                    File.AppendAllLines(filePath, lines, enc);
                }

            }
            catch(Exception ex)
            {
                exitCode = -1;
                Console.WriteLine(ex.ToString());
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                Environment.Exit(exitCode);
            }

        }
    }
}
