using System;

public class FileDao
{
    private string fileName;
    private string fileExtension;
    private string absolutePath;
    private string fileSize;
    
   
    public void FileDao(string filename,string fileExtension,string absolutePath,string fileSize)
    {
        this.fileName = fileName;
        this.fileExtension = fileExtension;
        this.absolutePath = absolutePath;
        this.fileSize = fileSize;
    }
	
}
