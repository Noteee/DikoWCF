using System;

public class FileDao
{
    private string fileName;
    private string fileExtension;
    private string absolutePath;
    private string owner;

    public void FileDao(string filename,string fileExtension,string absolutePath,string owner)
    {
        this.fileName = fileName;
        this.fileExtension = fileExtension;
        this.absolutePath = absolutePath;
        this.owner = owner;
    }
	
}
