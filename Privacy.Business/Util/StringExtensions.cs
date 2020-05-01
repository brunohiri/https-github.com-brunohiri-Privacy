using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace Privacy.Business.Util
{
    /// <summary>
    /// Métodos de extensão.
    /// </summary>
    public static class StringExtensions
    {
        #region Campos
        static readonly Regex MultipleSpacesRegex = new Regex(@"\s{2,}|(\r\n|\r\n\t|\r|\n|\t)|\&nbsp\;", RegexOptions.Compiled);
        static readonly Regex NotDigitsRegex = new Regex("[^0-9]", RegexOptions.Compiled);
        static readonly Regex NotASCII = new Regex("[^\x20-\x7E]", RegexOptions.Compiled);
        private static readonly CultureInfo ci = CultureInfo.InvariantCulture;
        #endregion

        #region Métodos
        public static string AddPrefix(this string s, string prefix) => $"{prefix.TrimAndEmptyNull()}{s}";

        /// <summary>
        /// Verifica se a string é IsNullOrWhiteSpace e joga exception.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static void ThrowIfNullOrWhiteSpace(this string value, string variableName) => ThrowIfNull(value, variableName);

        public static void ThrowIfNullOrWhiteSpaceOrEmpty(this string value, string variableName) => ThrowIfNull(value, variableName);

        public static string ThrowIfNull(this string value, string variableName) => (value.IsNullOrEmpty()) ? ThrowIfNull(value, variableName, string.Empty) : value;

        public static string ThrowIfNull(this string value, string variableName, string msg) => (value.IsNullOrEmpty()) ? throw new ArgumentNullException($"{ msg.TrimAndEmptyNull() } Valor nulo: {variableName}".Trim()) : value;

        /// <summary>
        /// Formata uma string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(this string value, params object[] args) => string.Format(value, args);

        public static string FormatOrEmpty(this long? value, string format) => value.FormatOrNull(format) ?? string.Empty;

        public static string FormatOrNull(this long? value, string format) => value.Value.Format(format) ?? null;

        public static string Format(this long value, string format) => string.Format(format, value);

        /// <summary>
        /// Realiza trim na string. Caso seja empty, transforma em null.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimAndNullEmpty(this string s) => (s.IsNullOrEmpty()) ? null : s.Trim();

        /// <summary>
        /// Realiza trim na string. Caso seja null, transforma em empty.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimAndEmptyNull(this string s) => s.TrimAndNullEmpty() ?? string.Empty;

        /// <summary>
        /// Transforma uma sequencia de dois espaços ou mais em um único espaço.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveMultipleSpaces(this string s) => Regex.Replace(MultipleSpacesRegex.Replace(s, " "), @"\>\s\<", "><");

        public static string RemoveAllSpaces(this string s) => (!s.IsNullOrEmpty()) ? Regex.Replace(MultipleSpacesRegex.Replace(s, " "), @"\s", string.Empty) : string.Empty;

        /// <summary>
        /// Remove caracteres não ASCII.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string KeepOnlyASCII(this string s) => NotASCII.Replace(s, string.Empty);

        /// <summary>
        /// Substitui caracteres acentuados e remove caracteres não permitidos.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="notAllowedRegEx"></param>
        /// <returns></returns>
        public static string NormalizeAndReplace(this string s, string notAllowedRegEx)
        {
            if (s.IsNullOrEmpty()) return string.Empty;

            string str = s.Normalize(NormalizationForm.FormD);

            return Regex.Replace(str, notAllowedRegEx, string.Empty);
        }

        public static string ExtractDateTimeFormat(this string s, string formatReturn) => ExtractDateTimeOrNull(s)?.ToString(formatReturn);

        public static DateTime ExtractDateTime(this string s, string format = null, CultureInfo culture = null) => (!s.IsNullOrEmpty()) ? (DateTime)ExtractDateTimeOrNull(s, format, culture) : throw new Exception("Data não informada!");

        public static DateTime? ExtractDateTimeOrNull(this string s, string format = null, CultureInfo culture = null)
        {
            if (s.IsNullOrEmpty()) { return null; }

            string value = s.KeepOnlyASCII();
            IList<string> formats = new string[] { "d'/'M'/'yyyy", "d'/'M'/'yy", "yyyy'-'M'-'d", "yy'-'M'-'d", "ddMMyyyy", "ddMMyy", "yyyyMMdd", "yyMMdd", "dd'/'MM'/'yyyy", "dd'/'MM'/'yy", "MM-dd-yyyy", "MM-dd-yy", "yyyy-MM-dd", "yy-MM-dd", "d'-'M'-'yyyy", "d'-'M'-'yy", "d/M/yyyy - H:mm:ss", "d/M/yyyy H:mm:ss" };

            if (!format.IsNullOrEmpty() && !formats.Contains(format)) { formats.Add(format); }

            culture = culture ?? ci;

            try
            {
                if (!DateTime.TryParseExact(value, formats.ToArray(), culture, DateTimeStyles.None, out DateTime date))
                {
                    if (!DateTime.TryParseExact(value, formats.ToArray(), culture, DateTimeStyles.AllowWhiteSpaces, out date))
                    {
                        if (!DateTime.TryParseExact(value, formats.ToArray(), Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out date))
                        {
                            if (!DateTime.TryParseExact(value, formats.ToArray(), Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out date))
                            {
                                if (!DateTime.TryParse(value, out date))
                                { throw new Exception($"Formato de data inválido. Formatos suportados: 'dd/mm/aaaa', 'aaaa-mm-dd', 'ddmmaaaa' ou 'aaaammdd'. Valor informado: '{s}'"); }
                            }
                        }
                    }
                }
                return date;
            }
            catch (Exception)
            { throw new Exception($"Formato de data inválido. Formatos suportados: 'dd/mm/aaaa', 'aaaa-mm-dd', 'ddmmaaaa' ou 'aaaammdd'. Valor informado: '{s}'"); }
        }

        /// <summary>
        /// Converte para um Boolean.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ExtractBool(this string s)
        {
            s = s.TrimAndNullEmpty()?.ToUpperInvariant() ?? string.Empty;

            if (s.IsNullOrEmpty()) return false;

            string[] lstValida = new string[] { "1", "S", "TRUE", "T", "VERDADEIRO", "V", "SIM" };
            if (lstValida.Contains(s)) { return true; }
            else
            {
                lstValida = new string[] { "0", "N", "FALSE", "F", "FALSO", "F", "NAO", "NÃO" };
                if (lstValida.Contains(s)) { return false; }
                else { throw new Exception($"Não foi possível converter o valor em bool. Valor: {s}."); }
            }
        }

        public static bool? ExtractBoolOrNull(this string s) => (!s.IsNullOrEmpty()) ? (bool?)ExtractBool(s) : null;

        public static bool ExtractBoolTrhowIfNull(this string s, string label) => (!s.IsNullOrEmpty()) ? s.ExtractBool() : throw new ArgumentNullException($"Valor nulo. Campo: {label}");

        public static long? ExtractLongOrNull(this string s) => (!s.IsNullOrEmpty()) ? (long.TryParse(s.KeepOnlyNumbers(), out long retorno)) ? (long?)retorno : throw new InvalidCastException($"Valor inválido para conversão! Valor encontrado: {s}") : null;

        public static long ExtractLong(this string s) => s.ExtractLongOrNull() ?? 0;

        public static byte? ExtractByteOrNull(this string s) => (!s.IsNullOrEmpty()) ? ((byte.TryParse(s.KeepOnlyNumbers(), out byte retorno)) ? (byte?)retorno : throw new InvalidCastException("Valor inválido para conversão!")) : null;

        public static byte ExtractByteOrZero(this string s) => ExtractByteOrNull(s) ?? 0;

        public static byte ExtractByteTrhowIfNull(this string s, string label) => (!s.IsNullOrEmpty()) ? s.ExtractByteOrZero() : throw new ArgumentNullException($"Valor nulo. Campo: {label}");

        public static int? ExtractInt32OrNull(this string s) => (!s.IsNullOrEmpty()) ? (int.TryParse(s.KeepOnlyNumbers(), out int retorno)) ? (int?)retorno : throw new InvalidCastException($"Valor inválido para conversão! Valor encontrado: {s}") : null;

        public static int ExtractInt32(this string s) => s.ExtractInt32OrNull() ?? 0;

        public static short? ExtractShortOrNull(this string s) => (!s.IsNullOrEmpty()) ? (short.TryParse(s.KeepOnlyNumbers(), out short retorno)) ? (short?)retorno : throw new InvalidCastException($"Valor inválido para conversão! Valor: {s}") : null;

        public static short ExtractShort(this string s) => s.ExtractShortOrNull() ?? 0;

        public static decimal? ExtractDecimalOrNull(this string s, CultureInfo culture = null)
        {
            if (s.IsNullOrEmpty()) { return null; }

            if (!decimal.TryParse(s, NumberStyles.AllowDecimalPoint, culture ?? ci, out decimal retorno))
            {
                if (!decimal.TryParse(s, NumberStyles.Float, culture ?? ci, out retorno))
                {
                    if (!decimal.TryParse(s, out retorno))
                    { throw new InvalidCastException($"Valor inválido para conversão! Valor:{s}"); }
                }
            }

            return retorno;
        }

        public static decimal ExtractDecimal(this string s, CultureInfo culture = null) => s.ExtractDecimalOrNull(culture) ?? 0M;

        public static string KeepOnlyNumbersOrNull(this string s) => s.IsNullOrEmpty() ? null : NotDigitsRegex.Replace(s, string.Empty);

        public static string GZipDecompressString(this string data) => ByteExtensions.GZipDecompressToString(Encoding.UTF8.GetBytes(data));

        /// <summary>
        /// Remove todos os caracteres que não sejam números.
        /// Caso a string seja null, retorna string.Empty.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string KeepOnlyNumbers(this string s) => NotDigitsRegex.Replace(s, string.Empty);

        /// <summary>
        /// Retorna X caracteres à esquerda da string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string s, int length) => (length >= s.Length) ? s : s.Substring(0, length);

        /// <summary>
        /// Retorna X caracteres à direita da string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string s, int length) => (length >= s.Length) ? s : s.Substring(s.Length - length, length);

        /// <summary>
        /// Deserializa objeto de uma string em Base64 usando BinaryFormatter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectData"></param>
        /// <returns></returns>
        public static T DeserializeFromBase64<T>(this string objectData) => (T)DeserializeFromBase64(objectData, typeof(T));

        /// <summary>
        /// Deserializa objeto de uma string em Base64 usando BinaryFormatter.
        /// </summary>
        /// <param name="objectData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFromBase64(this string objectData, Type type)
        {
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(objectData)))
            { return new BinaryFormatter().Deserialize(stream); }
        }

        public static T DeserializeXmlToObj<T>(this string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(input))
            { return (T)ser.Deserialize(sr); }
        }

        public static string SerializeObjToXml<T>(this T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Compacta uma string usando GZip.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] GZipCompressString(this string s) => Encoding.UTF8.GetBytes(s).GZipCompress();

        /// <summary>
        /// Compacta uma string usando GZip e converte o resultado para Base64.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GZipBase64CompressString(this string s) => Convert.ToBase64String(GZipCompressString(s));

        /// <summary>
        /// Descompacta uma string em Base64 usando GZip.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GZipBase64DecompressString(this string s) => Convert.FromBase64String(s).GZipDecompressToString();

        /// <summary>
        /// Descompacta uma string em Base64 e deserializa um objeto.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64ObjectData"></param>
        /// <returns></returns>
        public static T DecompressAndDeserializeFromBase64<T>(this string base64ObjectData) => ZipUtil.Unzip(Convert.FromBase64String(base64ObjectData)).Deserialize<T>();


        /// <summary>
        /// Retorna a primeira ocorrência de uma string entre outras duas.
        /// Retorna null caso o início ou fim não tenham sido encontrados.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startDelimiter"></param>
        /// <param name="endDelimiter"></param>
        /// <param name="lastOccurrence"> </param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string ReturnStringBetweenDelimiters(this string input, string startDelimiter, string endDelimiter = null, bool lastOccurrence = false, int? startIndex = null, bool withDelimiter = false)
        {
            int start = lastOccurrence // Acha início
                            ? input.LastIndexOf(startDelimiter, startIndex ?? input.Length - 1, StringComparison.Ordinal)
                            : input.IndexOf(startDelimiter, startIndex ?? 0, StringComparison.Ordinal);

            if (start < 0) { return string.Empty; }

            start = start + startDelimiter.Length;

            int end;
            if (string.IsNullOrEmpty(endDelimiter)) { end = input.Length; }// Acha fim
            else
            {
                end = input.IndexOf(endDelimiter, start, StringComparison.Ordinal);
                if (end < 0) { return string.Empty; }
            }

            if (!withDelimiter) // Retorna sem delimitadores
            { return input.Substring(start, end - start); }

            // Retorna com delimitadores
            return string.Format("{0}{1}{2}", startDelimiter, input.Substring(start, end - start), endDelimiter);
        }

        public static string DecodeW1252(this string s) => Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(s));

        public static string DecodeUnicode(this string s)
        {
            StringBuilder retorno = new StringBuilder(s);
            Regex rx = new Regex(@"\\[uU]00([a-fA-F0-9]{2})");
            MatchCollection unicodes = rx.Matches(s);

            if (unicodes != null && unicodes.Count > 0)
            {
                unicodes.Cast<Match>().Select(match => match.Value).Distinct().ToList().ForEach(item =>
                {
                    retorno.Replace(item, ((char)int.Parse(item.Substring(2), NumberStyles.HexNumber)).ToString());
                });
            }
            return retorno.ToString();
        }

        /// <summary>
        /// Retira espaços e acentuação de propriedades JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string NormalizaJSON(this string json)
        {
            StringBuilder retorno = new StringBuilder(json);
            Regex rx = new Regex(@"\{\""(.*?)\""\:");
            MatchCollection propriedades = rx.Matches(json);

            if (propriedades != null && propriedades.Count > 0)
            {
                propriedades.Cast<Match>().Select(match => match.Groups[1].Value).Distinct().ToList().ForEach(item =>
                {
                    retorno.Replace(item, Regex.Replace(item, @"\s|[^\w\d]", string.Empty).NormalizaText());
                });
            }

            return retorno.ToString().Replace(@"\/", "/");
        }

        /// <summary>
        /// Retira Acentuação
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string NormalizaText(this string s) => HttpUtility.UrlEncode(s, Encoding.GetEncoding(28597)).Replace("+", " ");

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static string TrataValorHtml(this string valorHtml) => WebUtility.HtmlDecode(valorHtml).RemoveMultipleSpaces().Trim();

        // http://stackoverflow.com/questions/121511/reading-xml-with-an-into-c-sharp-xmldocument-object
        /// <summary>
        /// Faz replace do caracter "&" que dá erro no xmldocument.loadxml
        /// </summary>
        /// <param name="valorXml"></param>
        /// <returns></returns>
        public static string TratarValorXml(this string valorXml) => Regex.Replace(valorXml, "&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)", "&amp;");

        public static string TrataValorHtml(this byte[] html, string encodingName = null) => Encoding.GetEncoding(encodingName ?? "ISO-8859-1").GetString(html).RemoveMultipleSpaces().TrataValorHtml();

        public static string UriDataEscape(this string s) => Uri.EscapeDataString(s);

        public static string UriEscape(this string s) => Uri.EscapeUriString(s);

        public static string RemoveScriptsHtml(this string htmlOriginal)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlOriginal);

            //Remove potentially harmful elements
            HtmlNodeCollection nc = doc.DocumentNode.SelectNodes("//script|//link|//iframe|//frameset|//frame|//applet|//object|//embed");
            if (nc != null)
            { nc.ToList().ForEach(node => { node.ParentNode.RemoveChild(node, false); }); }

            //remove hrefs to java/j/vbscript URLs
            nc = doc.DocumentNode.SelectNodes("//a[starts-with(translate(@href, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'javascript')]|//a[starts-with(translate(@href, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'jscript')]|//a[starts-with(translate(@href, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'vbscript')]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                { node.SetAttributeValue("href", "#"); }
            }

            //remove img with refs to java/j/vbscript URLs
            nc = doc.DocumentNode.SelectNodes("//img[starts-with(translate(@src, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'javascript')]|//img[starts-with(translate(@src, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'jscript')]|//img[starts-with(translate(@src, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'vbscript')]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                { node.SetAttributeValue("src", "#"); }
            }

            //remove on<Event> handlers from all tags
            nc = doc.DocumentNode.SelectNodes("//*[@onclick or @onmouseover or @onfocus or @onblur or @onmouseout or @ondoubleclick or @onload or @onunload]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                {
                    node.Attributes.Remove("onFocus");
                    node.Attributes.Remove("onBlur");
                    node.Attributes.Remove("onClick");
                    node.Attributes.Remove("onMouseOver");
                    node.Attributes.Remove("onMouseOut");
                    node.Attributes.Remove("onDoubleClick");
                    node.Attributes.Remove("onLoad");
                    node.Attributes.Remove("onUnload");
                }
            }

            // remove any style attributes that contain the word expression (IE evaluates this as script)
            nc = doc.DocumentNode.SelectNodes("//*[contains(translate(@style, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'expression')]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                { node.Attributes.Remove("stYle"); }
            }

            return doc.DocumentNode.WriteTo();
        }

        /// <summary>
        /// Remove tags html de um texto.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string RemoveTagsHtml(this string texto) => Regex.Replace(texto, "<.+?>", string.Empty);

        public static string CompletaZerosEsquerda(this string numero, int qtdCaracter)
        {
            long numeroInt = numero.ExtractLong();

            if (numeroInt < 0) { numeroInt *= -1; }

            return numero.ToString(ci).PadLeft(qtdCaracter, '0');
        }

        /// <summary>
        /// Remove tags display:none e visibility:hidden.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string RemoverDisplayNone(this string htmlOriginal)
        {
            return htmlOriginal.Replace("visibility:hidden;", string.Empty).Replace("display:none;", string.Empty);
        }

        /// <summary>
        /// Padronizar Sexo para MASCULINO/FEMININO/INDEFINIDO.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string PadronizarSexo(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (texto.ToArray().Count() == 1 ? (texto.Equals("M") ? "MASCULINO" : texto.Equals("F") ? "FEMININO" : texto.Equals("I") ? "INDEFINIDO" : "INDEFINIDO") : texto);
        }

        /// <summary>
        /// Padronizar CPF sem pontuação e com 11 caracteres.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string PadronizarCPF(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString("00000000000"));
        }

        /// <summary>
        /// Padronizar CNPJ sem pontuação e com 14 caracteres.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string PadronizarCNPJ(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString("00000000000000"));
        }

        /// <summary>
        /// Padronizar Telefone sem máscaras tanto fixo quanto Celular.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string PadronizarTelefone(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (texto.KeepOnlyNumbers().ToString().Length == 10 ? Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString("0000000000") : Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString("000000000000"));
        }

        /// <summary>
        /// Padronizar CEP sem máscara e com 8 caractéres.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string PadronizarCEP(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString("00000000"));
        }


        /// <summary>
        /// Formatar CPF com máscara.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarCPF(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"000\.000\.000\-00"));
        }

        /// <summary>
        /// Formatar CPF com máscara criptografado.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarCPFCriptografado(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"000\.000\.000\-00").Left(5) + "XX.XX-" + Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"000\.000\.000\-00").Right(4));
        }

        /// <summary>
        /// Formatar CNPJ com máscara.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarCNPJ(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"00\.000\.000\/0000\-00"));
        }

        /// <summary>
        /// Formatar CNPJ com máscara criptografado.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarCNPJCriptografado(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"00\.000\.000\/0000\-00").Left(4) + "XX.XXX/XXX" + Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"00\.000\.000\/0000\-00").Right(4));
        }

        /// <summary>
        /// Formatar Telefone com máscaras tanto Fixo quanto Celular.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarTelefone(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (texto.KeepOnlyNumbers().ToString().Length == 10 ? Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"\(00\) 0000\-0000") : Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"\(00\) 00000\-0000"));
        }

        /// <summary>
        /// Formatar CEP com máscara.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FormatarCEP(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : (Convert.ToUInt64(texto.KeepOnlyNumbers().ToString()).ToString(@"00000\-000"));
        }

        /// <summary>
        /// Remover acentuação da string.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string RemoverAcentuacao(this string texto)
        {
            string valor = string.Empty;
            if (texto != null && texto.Length > 0)
            {
                valor = texto;
                valor = Regex.Replace(valor, "[áàâãááààââääããåå]", "a", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÁÀÂÃÁÁÀÀÂÂÄÄÃÃÅÅ]", "A", RegexOptions.Singleline);
                //Acentos no e e E;
                valor = Regex.Replace(valor, "[éêééèèêêëë]", "e", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÉÊÉÉÈÈÊÊËË]", "E", RegexOptions.Singleline);
                //Acentos no i e I
                valor = Regex.Replace(valor, "[ííììîîïï]", "i", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÍÍÌÌÎÎÏÏ]", "I", RegexOptions.Singleline);
                //Acentos no o e O
                valor = Regex.Replace(valor, "[óôõóóòòôôööõ]", "o", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÓÔÕÓÓÒÒÔÔÖÖÕ]", "O", RegexOptions.Singleline);
                //Acentos no u e U;
                valor = Regex.Replace(valor, "[úüüúúùùùûûü]", "u", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÚÜÜÚÚÙÙÙÛÛÜ]", "U", RegexOptions.Singleline);
                //Acentos no n e N;
                valor = Regex.Replace(valor, "[ñ]", "n", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[Ñ]", "N", RegexOptions.Singleline);
                //Acentos no ç e Ç;
                valor = Regex.Replace(valor, "[ç]", "c", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[Ç]", "C", RegexOptions.Singleline);
                //Acentos no y e Y;
                valor = Regex.Replace(valor, "[ýýÿ]", "y", RegexOptions.Singleline);
                valor = Regex.Replace(valor, "[ÝÝŸ]", "Y", RegexOptions.Singleline);
            }
            return (valor);
        }

        /// <summary>
        /// Formatar IdFila para o formato definido.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToIdFila(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? null : Convert.ToInt64(texto).ToString("000000");
        }
        #endregion
    }
}