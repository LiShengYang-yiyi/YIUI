using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    internal static class CatalogTools
    {
        /// <summary>
        /// 序列化（JSON文件）
        /// </summary>
        public static void SerializeToJson(string savePath, DefaultBuildinFileCatalog catalog)
        {
            string json = JsonUtility.ToJson(catalog, true);
            FileUtility.WriteAllText(savePath, json);
        }

        /// <summary>
        /// 反序列化（JSON文件）
        /// </summary>
        public static DefaultBuildinFileCatalog DeserializeFromJson(string jsonContent)
        {
            return JsonUtility.FromJson<DefaultBuildinFileCatalog>(jsonContent);
        }

        /// <summary>
        /// 序列化（二进制文件）
        /// </summary>
        public static void SerializeToBinary(string savePath, DefaultBuildinFileCatalog catalog)
        {
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                // 创建缓存器
                BufferWriter buffer = new BufferWriter(CatalogDefine.FileMaxSize);

                // 写入文件标记
                buffer.WriteUInt32(CatalogDefine.FileSign);

                // 写入文件版本
                buffer.WriteUTF8(CatalogDefine.FileVersion);

                // 写入文件头信息
                buffer.WriteUTF8(catalog.PackageName);
                buffer.WriteUTF8(catalog.PackageVersion);

                // 写入资源包列表
                buffer.WriteInt32(catalog.Wrappers.Count);
                for (int i = 0; i < catalog.Wrappers.Count; i++)
                {
                    var fileWrapper = catalog.Wrappers[i];
                    buffer.WriteUTF8(fileWrapper.BundleGUID);
                    buffer.WriteUTF8(fileWrapper.FileName);
                }

                // 写入文件流
                buffer.WriteToStream(fs);
                fs.Flush();
            }
        }

        /// <summary>
        /// 反序列化（二进制文件）
        /// </summary>
        public static DefaultBuildinFileCatalog DeserializeFromBinary(byte[] binaryData)
        {
            // 创建缓存器
            BufferReader buffer = new BufferReader(binaryData);

            // 读取文件标记
            uint fileSign = buffer.ReadUInt32();
            if (fileSign != CatalogDefine.FileSign)
                throw new Exception("Invalid catalog file !");

            // 读取文件版本
            string fileVersion = buffer.ReadUTF8();
            if (fileVersion != CatalogDefine.FileVersion)
                throw new Exception($"The catalog file version are not compatible : {fileVersion} != {CatalogDefine.FileVersion}");

            DefaultBuildinFileCatalog catalog = new DefaultBuildinFileCatalog();
            {
                // 读取文件头信息
                catalog.FileVersion = fileVersion;
                catalog.PackageName = buffer.ReadUTF8();
                catalog.PackageVersion = buffer.ReadUTF8();

                // 读取资源包列表
                int fileCount = buffer.ReadInt32();
                catalog.Wrappers = new List<DefaultBuildinFileCatalog.FileWrapper>(fileCount);
                for (int i = 0; i < fileCount; i++)
                {
                    var fileWrapper = new DefaultBuildinFileCatalog.FileWrapper();
                    fileWrapper.BundleGUID = buffer.ReadUTF8();
                    fileWrapper.FileName = buffer.ReadUTF8();
                    catalog.Wrappers.Add(fileWrapper);
                }
            }

            return catalog;
        }
    }
}