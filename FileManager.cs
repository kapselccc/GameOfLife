using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace GameOfLife2
{
    public class FileManager
    {
        public static void SaveStateToFile(string[] state)
        {

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Path.GetFullPath("../../../saved/")
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fs = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (string s in state)
                    {
                       fs.WriteLine(s);
                    }
                }
            }
        }

        public static string[] LoadStateFromFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetFullPath("../../../saved/")
            };
            string[] state;

            if (openFileDialog.ShowDialog() == true)
            {
                state = File.ReadAllLines(openFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("Unable to open file!");
                return Array.Empty<string>();
            }

            return state;
        }
    }
}
