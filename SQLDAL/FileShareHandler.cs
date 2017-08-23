using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    public class FileShareHandler
    {
        private string fileName;
        private string fileExtension;
        private string filePath;
        private string fileSize;


        public FileShareHandler(string fileName, string fileExtension, string filePath, string fileSize)
        {
            this.fileName = fileName;
            this.fileExtension = fileExtension;
            this.filePath = filePath;
            this.fileSize = fileSize;
       
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        public string FileSize
        {
            get { return FileSize; }
            set { fileSize = value; }
        }
    }
}
