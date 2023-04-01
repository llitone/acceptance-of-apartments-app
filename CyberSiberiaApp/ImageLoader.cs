using System;
using System.IO;

public class ImageLoader
{
    private Random _random = new Random();
    private const string name_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private string _get_file_name(string name)
    {
        string result_name = "";
        int slice_start;
        for (int i = 0; i < 30; i++)
        {
            slice_start = this._random.Next(0, name_chars.Length - 1);
            result_name += name_chars.Substring(slice_start, 1);
        }
        return result_name + "." + name.Split('.')[name.Split('.').Length - 1];
    }

    public string convert_image(string filename)
    {
        string name = this._get_file_name(filename);
        if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "_images"))
        {
            Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "/_images");
        }
        File.Copy(filename, string.Format(System.AppDomain.CurrentDomain.BaseDirectory + "/_images/{0}", name));
        return name;
    }
}
