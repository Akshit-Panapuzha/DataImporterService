using System.Configuration;
using System.IO;
using DataProcessor;
namespace DataImporter
{
    public class FileMover
    {
        string sourceFolder;
        string archiveFolder;
        string[] sourceFileNames;
        ProcessXML processXML;
        public FileMover()
        {
            processXML = new ProcessXML();
        }

        public int StartService()
        {
            sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            archiveFolder = ConfigurationManager.AppSettings["ArchiveFolder"];
            sourceFileNames = Directory.GetFiles(sourceFolder);
            return CopyFileFromFolder(sourceFileNames);
        }

        private int CopyFileFromFolder(string[] source)
        {
            foreach (string fileName in source)
            {
                string name = Path.GetFileName(fileName);
                if (!File.Exists(archiveFolder + "\\" + name))
                {
                    if(Path.GetExtension(fileName) == ".xml")
                    {
                        File.Copy(fileName, archiveFolder + "\\" + name);
                        //reads all text data from file into fileContent
                        string fileContent = File.ReadAllText(fileName);
                        processXML.Process(fileContent);
                        //deleting file after copying 
                        File.Delete(fileName);
                    }
                }
            }
            return 1;
        }
    }
}
