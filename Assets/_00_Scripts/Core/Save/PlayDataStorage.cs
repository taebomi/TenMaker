using System;
using UnityEngine;

namespace TenMaker.Core
{
    public class PlayDataStorage
    {
        // private const int SAVE_VERSION = 1;
        // private const string FILE_PATH = "save.dat";
        // private const string ENCRYPTION_PASSWORD = "tq4CTt8aYaJ6HyfOBtSyEKpAo9ASjYCS";
        //
        // private ES3Settings _es3Settings;
        //
        // private void Awake()
        // {
        //     _es3Settings = new ES3Settings(ES3.Location.Cache, ES3.CompressionType.Gzip);
        //     _es3Settings = new ES3Settings(FILE_PATH, ES3.EncryptionType.AES, ENCRYPTION_PASSWORD, _es3Settings);
        //     ES3.CacheFile(_es3Settings);
        // }
        //
        // public void Load()
        // {
        //     var saveVersion = ES3.Load("save version", SAVE_VERSION, _es3Settings);
        //     PlayData.BestScore = ES3.Load("best score", 0, _es3Settings);
        // }
        //
        // public void Save()
        // {
        //     ES3.Save("save version", SAVE_VERSION, _es3Settings);
        //     ES3.Save("best score", PlayData.BestScore, _es3Settings);
        //     ES3.StoreCachedFile(_es3Settings);
        // }
    }
}