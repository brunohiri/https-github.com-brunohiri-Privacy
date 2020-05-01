using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Privacy.Business.Util
{
    /// <summary>
    /// Métodos de extensão.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Deserializa objeto de um array de bytes usando BinaryFormatter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectData"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] objectData) => (T)Deserialize(objectData, typeof(T));

        /// <summary>
        /// Deserializa objeto de um array de bytes usando BinaryFormatter.
        /// </summary>
        /// <param name="objectData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(this byte[] objectData, Type type)
        {
            using (var stream = new MemoryStream(objectData))
            { return new BinaryFormatter().Deserialize(stream); }
        }

        /// <summary>
        /// Compacta usando GZip.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                { zip.Write(data, 0, data.Length); }

                return ms.ToArray();
            }
        }

        public static byte[] GZipCompressOptimal(this byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionLevel.Optimal))
                { zip.Write(data, 0, data.Length); }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Descompacta usando GZip.
        /// </summary>
        /// <param name="data"> </param>
        /// <returns></returns>
        public static byte[] GZipDecompress(this byte[] data)
        {
            using (var zip = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                using (var ms = new MemoryStream())
                {
                    zip.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Descompacta uma string usando GZip e encoding UTF8.
        /// </summary>
        /// <param name="data"> </param>
        /// <returns></returns>
        public static string GZipDecompressToString(this byte[] data) => Encoding.UTF8.GetString(GZipDecompress(data));

        /// <summary>
        /// Compacta retornando uma string usando GZip e encoding UTF8.
        /// </summary>
        /// <param name="data"> </param>
        /// <returns></returns>
        public static string GZipCompressToString(this byte[] data) => Encoding.UTF8.GetString(GZipCompress(data));

        /// <summary>
        /// Retorna um stream com os bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Stream GetStream(this byte[] data)
        {
            var ms = new MemoryStream();

            ms.Write(data, 0, data.Length);
            ms.Position = 0;

            return ms;
        }

        /// <summary>
        /// Valida se o valor é nulo e retorno exception ou o próprio valor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static byte ThrowIfNull(this byte? value, string variableName) => value ?? throw new NullReferenceException($"Valor nulo: {variableName}");

        /// <summary>
        /// Valida se é nulo ou zerado e retorna exception ou o próprio valor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static byte ThrowIfNullOrZero(this byte? value, string variableName) => (ThrowIfNull(value, variableName) == 0) ? throw new ArgumentOutOfRangeException($"Valor zerado para: {variableName}") : (byte)value;

        /// <summary>
        /// Retorna o valor informado caso seja null ou zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static byte ReturnIfNullOrZero(this byte? value, byte secondValue) => ((value ?? 0) == 0) ? secondValue : value.Value;

        /// <summary>
        /// Retorna o valor informado caso seja null ou zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static byte ReturnZeroIfNull(this byte? value) => value.ReturnIfNullOrZero(0);

        /// <summary>
        /// Retorna o valor informado caso seja zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static byte ReturnIfZero(this byte value, byte secondValue) => (value == 0) ? secondValue : value;

        /// <summary>
        /// Retorna o valor informado caso seja zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static byte ReturnIfZero(this byte? value, byte secondValue) => ReturnIfZero((value ?? 0), 0);
    }
}