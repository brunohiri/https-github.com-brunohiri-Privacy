using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;

namespace Privacy.Business.Util
{
    /// <summary>
    /// Métodos da biblioteca DotNetZip.
    /// http://dotnetzip.codeplex.com/
    /// </summary>
    public static class ZipUtil
    {
        /// <summary>
        /// Dezipa uma entrada (arquivo) dentro de um zip.
        /// </summary>
        /// <param name="entry"> </param>
        /// <returns></returns>
        private static byte[] UnzipEntry(ZipEntry entry)
        {
            using (var msOutput = new MemoryStream())
            {
                entry.Extract(msOutput);
                return msOutput.ToArray();
            }
        }

        /// <summary>
        /// Zipa um array de bytes.
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="entryName"> </param>
        /// <returns></returns>
        public static byte[] Zip(byte[] dados, string entryName)
        {
            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.BestCompression;
                zip.AddEntry(entryName, dados);
                using (var ms = new MemoryStream())
                {
                    zip.Save(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Dezipa uma entrada (arquivo) dentro de um zip.
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public static byte[] Unzip(byte[] dados, string entryName)
        {
            using (var msInput = new MemoryStream(dados))
            using (var zip = ZipFile.Read(msInput))
            {
                return UnzipEntry(zip.Entries.First(o => o.FileName == entryName));
            }
        }

        /// <summary>
        /// Dezipa um zip que possua somente uma entrada (arquivo).
        /// Caso não possua somente uma entrada (arquivo), joga exception.
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="entryName"> </param>
        /// <returns></returns>
        public static byte[] Unzip(byte[] dados, out string entryName)
        {
            using (var msInput = new MemoryStream(dados))
            using (var zip = ZipFile.Read(msInput, new ReadOptions { Encoding = Encoding.ASCII }))
            {
                if (zip.Entries.Count != 1)
                    throw new Exception($"O arquivo zip deve possuir apenas um arquivo. Foram encontrados {zip.Entries.Count}");

                var entry = zip.Entries.First();
                entryName = entry.FileName.Replace("?", ""); // Remove caracteres especiais
                return UnzipEntry(entry);
            }
        }

        /// <summary>
        /// Dezipa um zip que possua somente uma entrada (arquivo).
        /// Caso não possua somente uma entrada (arquivo), joga exception.
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static byte[] Unzip(byte[] dados)
        {
            string entryName;
            return Unzip(dados, out entryName);
        }

        /// <summary>
        /// Zipa uma coleção de arquivos em array de bytes.
        /// </summary>
        /// <param name="arquivos"></param>
        /// <returns></returns>
        public static byte[] Zip(Dictionary<string, byte[]> arquivos)
        {
            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.BestCompression;
                foreach (var a in arquivos)
                    zip.AddEntry(a.Key, a.Value);
                
                using (var ms = new MemoryStream())
                {
                    zip.Save(ms);
                    return ms.ToArray();
                }
            }
        }

    }
}
