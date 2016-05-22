using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class LoadJSON
{
    public string Reading(string fileName)
    {
        string url;
        url = Application.dataPath + "/Json/" + fileName + ".json";
        FileInfo loadFile = new FileInfo(url);

        StreamReader sr = new StreamReader(loadFile.OpenRead(), Encoding.UTF8);
        string json = sr.ReadToEnd();

        return json;
    }
}
