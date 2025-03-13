using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class FileDataHandler
{
    string dataDirPath = "";
    string dataFileName = "";
    bool isEncrypted;
    string encryptedWord = "word";
    public FileDataHandler(string dataDirPath, string dataFileName, bool isEncrypted)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.isEncrypted = isEncrypted;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if(isEncrypted)
                {
                    dataToLoad = Encrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            
            catch (Exception e)
            {
                Debug.LogError("Error occured when load data from file: " + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data,true);
            if (isEncrypted)
            {
                dataToStore = Encrypt(dataToStore);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
                
            }
            
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when save data to file: " +  fullPath + "\n" + e);
        }
    }

    string Encrypt(string data)
    {
        string res = "";
        for(int i=0;i<data.Length;i++)
        {
            res += (char)(data[i] ^ encryptedWord[i % encryptedWord.Length]);
        }
        return res;
    }
}
