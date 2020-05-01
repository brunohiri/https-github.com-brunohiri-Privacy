using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Privacy.Business.Util
{
    public static class Util
    {
        #region Campos
        static readonly Regex MultipleSpacesRegex = new Regex(@"\s{2,}|(\r\n|\r\n\t|\r|\n|\t)|\&nbsp\;", RegexOptions.Compiled);
        #endregion

        #region Métodos

        public static string ReadResponse(this HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                var encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(response.CharacterSet) && ((response.CharacterSet.Split('.')?.Length ?? 1) >= 1)) // Adicionada regra por conta do Sintegr CE (charSet=pt_BR.UTF-8)
                    if (response.CharacterSet.ToUpper() == "PT_BR.UTF-8")
                    { encoding = Encoding.GetEncoding("ISO-8859-1"); }
                    else
                    { encoding = Encoding.GetEncoding(response.CharacterSet.Replace("\"", "")); }

                using (var reader = new StreamReader(responseStream, encoding))
                { return reader.ReadToEnd(); }
            }
        }

        public static long GetTime()
        {
            long retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (long)(t.TotalMilliseconds + 0.5);
            return retval;
        }


        public static string TratarDocumento(string Documento)
        {
            return Documento.Replace("-", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty).Trim();
        }

        public static string GerarUid()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16);
        }

        /// <summary>
        /// Realiza a validação do CNPJ
        /// </summary>
        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        #region TratarNomeArquivo

        private const string CaracteresInvalidos = @"[\s\.\,\@\\\+\*\?\[\^\]\$\(\)\{\}\=\!\""\'\#\&\/\;\<\>\|\:-]";
        public static string TratarNomeArquivo(string FileName, int i)
        {
            return "SD_" + Regex.Replace(Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty),
                    CaracteresInvalidos,
                    string.Empty, RegexOptions.Singleline) + "_"
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString()
                    + Path.GetExtension(FileName);
        }

        public static string TratarNomeArquivoSaida(string FileName, string Extensao)
        {
            return string.Format("{0}_PROCESSADO{1}", Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty), Extensao);
        }

        public static string TratarNomeArquivoZip(string FileName)
        {
            return string.Format("{0}.zip", Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty));
        }
        #endregion

        #region Compactar e Descompactar
        public static bool CompactarArquivo(string nomeArquivoZip, string nomeArquivo)
        {
            bool retorno = false;
            try
            {
                if (System.IO.File.Exists(nomeArquivoZip))
                {
                    System.IO.File.Delete(nomeArquivoZip);
                }

                using (ZipOutputStream strmZipOutputStream = new ZipOutputStream(System.IO.File.Create(nomeArquivoZip)))
                {

                    FileStream fs = System.IO.File.OpenRead(nomeArquivo);
                    ZipEntry ze = new ZipEntry(Path.GetFileName(fs.Name));
                    int len = int.Parse(fs.Length.ToString());
                    byte[] b1 = new byte[len];
                    fs.Read(b1, 0, b1.Length);

                    strmZipOutputStream.PutNextEntry(ze);
                    strmZipOutputStream.Write(b1, 0, b1.Length);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                    retorno = true;
                }
            }
            catch (Exception)
            {
                retorno = false;
                throw;
            }
            return retorno;
        }

        public static bool DescompactarArquivo(string caminhoPendente, string nomeArquivoZip, ref string nomeArquivoRetorno, ref string nomeArquivoConteudoZip)
        {
            bool descompactado = false;
            string caminhoRelativoArquivo = string.Empty;
            string caminhoDescompactacao;
            string caminhoArquivoASerDescompactado;

            try
            {

                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(@caminhoPendente, @nomeArquivoZip)))
                {
                    ZipFile arquivoZip = null;
                    try
                    {
                        arquivoZip = new ZipFile(fs);
                        byte[] buffer = new byte[4096];
                        int i = 0;

                        foreach (ZipEntry zipEntry in arquivoZip)
                        {
                            if (!zipEntry.IsFile)
                                continue;

                            string flname = Path.GetFileNameWithoutExtension(zipEntry.Name);
                            string flextension = Path.GetExtension(zipEntry.Name);
                            string file = flname.Replace(".", "") + flextension;

                            caminhoRelativoArquivo = TratarNomeArquivo(file, i);
                            nomeArquivoConteudoZip += caminhoRelativoArquivo + "|";
                            caminhoArquivoASerDescompactado = Path.Combine(@caminhoPendente, @caminhoRelativoArquivo);
                            caminhoDescompactacao = Path.GetDirectoryName(@caminhoArquivoASerDescompactado);

                            if (caminhoDescompactacao.Length > 0 &&
                                !Directory.Exists(caminhoDescompactacao))
                                Directory.CreateDirectory(caminhoDescompactacao);

                            Stream zipStream = arquivoZip.GetInputStream(zipEntry);
                            using (FileStream streamWriter = System.IO.File.Create(caminhoArquivoASerDescompactado))
                            {
                                StreamUtils.Copy(zipStream, streamWriter, buffer);
                                streamWriter.Flush();
                            }

                            i++;
                        }
                    }
                    finally
                    {
                        if (arquivoZip != null)
                        {
                            arquivoZip.IsStreamOwner = true;
                            arquivoZip.Close();
                            nomeArquivoRetorno = caminhoRelativoArquivo;
                            descompactado = true;
                        }
                    }


                    fs.Close();
                }
            }
            catch (Exception)
            {
                caminhoRelativoArquivo = string.Empty;
                descompactado = false;
                throw;
            }

            return descompactado;
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".rar", "application/x-rar-compressed, application/octet-stream" },
                {".zip", "application/zip"}//application/octet-stream, application/x-zip-compressed, multipart/x-zip
            };
        }

        #endregion
        #endregion
    }
}

