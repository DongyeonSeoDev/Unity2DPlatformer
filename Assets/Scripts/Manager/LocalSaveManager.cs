using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Text;

[Serializable]
public class Json<T>
{
    public List<T> list;
}

[Serializable]
public class SaveData
{
    public string version;
    public string key;
    public string iv;
}

public class LocalSaveManager : MonoBehaviour
{
    private Rijndael myRijndael;
    private static LocalSaveManager instance = null;

    private readonly static string fileName = "Save";
    private static string filePath;

    public static LocalSaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("LocalSaveManager instance가 없습니다.");
                return null;
            }

            return instance;
        }
    }

    private static string GetFilePath()
    {
        if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("fileName이 없습니다.");
            return null;
        }

        return Application.persistentDataPath + "/" + fileName;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 LocalSaveManager가 실행되고 있습니다.");
            Destroy(this);
            return;
        }

        instance = this;

        object data = Resources.Load("SaveData/SaveData");
        SaveData saveData = JsonUtility.FromJson<SaveData>(Convert.ToString(data));

        myRijndael = Rijndael.Create();

        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;
        myRijndael.BlockSize = 128;

        myRijndael.Key = SetKeyOrIV(saveData.key);
        myRijndael.IV = SetKeyOrIV(saveData.iv);
    }

    private byte[] SetKeyOrIV(string keyOrIv)
    {
        byte[] value = Encoding.UTF8.GetBytes(keyOrIv);
        byte[] bytes = new byte[16];

        int length = value.Length;

        if (length > bytes.Length)
        {
            length = bytes.Length;
        }

        Array.Copy(value, bytes, length);

        return bytes;
    }

    public static void Save(Stage[] stages)
    {
        filePath = GetFilePath();

        if (filePath == null)
        {
            return;
        }

        StageSaveData[] stageSaveData = new StageSaveData[stages.Length];

        for (int i = 0; i < stages.Length; i++)
        {
            stageSaveData[i] = StageSaveData.FromStageToStageSaveData(stages[i]);
        }

        string jsonData = JsonUtility.ToJson(new Json<StageSaveData>() { list = stageSaveData.ToList() });
        byte[] encryptedSavegame = instance.Encrypt(jsonData, instance.myRijndael.Key, instance.myRijndael.IV);

        File.WriteAllBytes(filePath, encryptedSavegame);
        Debug.Log($"세이브 완료 (경로: {filePath})");
    }

    private byte[] Encrypt(string message, byte[] Key, byte[] IV)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        byte[] text = Encoding.UTF8.GetBytes(message);

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        rijndaelCipher.Key = Key;
        rijndaelCipher.IV = IV;

        ICryptoTransform cryptoTransform = rijndaelCipher.CreateEncryptor();

        return cryptoTransform.TransformFinalBlock(text, 0, text.Length);
    }

    public static void Load(Stage[] stages)
    {
        filePath = GetFilePath();

        if (filePath == null)
        {
            return;
        }

        if (File.Exists(filePath))
        {
            byte[] decryptedSavegame = File.ReadAllBytes(filePath);
            string jsonString = instance.Decrypt(decryptedSavegame, instance.myRijndael.Key, instance.myRijndael.IV);

            StageSaveData[] stageSaveData = JsonUtility.FromJson<Json<StageSaveData>>(jsonString).list.ToArray();

            for (int i = 0; i < stages.Length; i++)
            {
                if (stageSaveData.Length <= i)
                {
                    return;
                }

                StageSaveData.FromStageSaveDataToStage(stages[i], stageSaveData[i]);
            }

            Debug.Log($"로드 완료 (경로: {filePath})");
        }
        else
        {
            Debug.Log("파일이 존재하지 않음");
        }
    }

    private string Decrypt(byte[] message, byte[] Key, byte[] IV)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        rijndaelCipher.Key = Key;
        rijndaelCipher.IV = IV;

        byte[] text = rijndaelCipher.CreateDecryptor().TransformFinalBlock(message, 0, message.Length);

        return Encoding.UTF8.GetString(text);
    }
}