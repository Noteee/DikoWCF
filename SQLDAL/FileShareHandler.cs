using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    [DataContract]
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
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        [DataMember]
        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
        [DataMember]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        [DataMember]
        public string FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
    }
}
