using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using PalMarket.Common.Helpers;
using System.Configuration;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PalMarket.Model;
using System.Data;

namespace PalMarket.Common.Helpers
{
    public class Utilities
    {
        /// <summary>
        /// Gets a formatted DateTime object  
        /// </summary>
        /// <param name="value">Date value as string</param>
        /// <param name="format">Format of parsing the date value</param>
        /// <returns>Formatted DateTime object</returns>
        public static DateTime? GetDateFormattedValue(string value, bool isNullable = true, string format = "dd/MM/yyyy")
        {
            if (string.IsNullOrEmpty(value) && isNullable)
            {
                return null;
            }
            DateTime date;
            DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);
            return date;
        }

        public static string GetDateFormat()
        {
            return "dd/MM/yyyy";
        }
        public static string GetRTLDateFormat()
        {
            return "dd-MM-yyyy";
        }
        public static string DateToStringRTL(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(GetRTLDateFormat()) : null;
        }
        public static string DateToString(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(GetDateFormat()) : null;
        }
        public static string GetDateTimeFormat()
        {
            return "dd/MM/yyyy HH:mm";
        }
        /// <summary>
        /// Gets full domain of the current request
        /// </summary>
        /// <returns>Request full domain</returns>
        public static string GetRequestFullDomain()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        public static int CalculateAge(DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Checks if the password is valid
        /// </summary>
        /// <param name="password">Password to be validated</param>
        /// <returns>Boolean</returns>
        public static bool ValidatePassword(string password)
        {
            // Password should be minimum 8 characters (configured) and should contain: letters, numbers and special characters (*,#,@,!, & … etc.)
            int minLength = 8;
            string minLengthStr = ConfigurationManager.AppSettings["PasswordMinLength"];
            int.TryParse(minLengthStr, out minLength);
            Regex exp = new Regex("(?=.*[0-9])((?=.*[a-z])|(?=.*[A-Z]))(?=.*[ ~*!@#$%^&+=()\\-_{}[\\];:'\"\\|<>,.?\\/]).{" + minLength + ",}");
            return exp.IsMatch(password);
        }

        /// <summary>
        /// Generates password
        /// </summary>
        /// <returns>Password</returns>
        public static string GeneratePassword()
        {
            string minLengthStr = ConfigurationManager.AppSettings["PasswordMinLength"];
            int.TryParse(minLengthStr, out int minLength);
            string password = GeneratePassword(minLength, 4);
            char[] passwordChars = password.ToCharArray();
            if (int.TryParse(passwordChars[0].ToString(), out int num))
            {
                password = "a" + password;
            }
            if (int.TryParse(passwordChars[passwordChars.Count() - 1].ToString(), out num))
            {
                password = password + "z";
            }
            return password;
        }

        /// <summary>
        /// Generates pin code
        /// </summary>
        /// <returns>PIN code</returns>
        public static string GeneratePinCode()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new Random();
            return rdm.Next(min, max).ToString();
        }

        /// <summary>
        /// Creates a pseudo-random password containing the number of character classes
        /// defined by complexity, where 2 = alpha, 3 = alpha+num, 4 = alpha+num+special.
        /// </summary>
        public static string GeneratePassword(int length, int complexity)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider csp =
            new System.Security.Cryptography.RNGCryptoServiceProvider();
            // Define the possible character classes where complexity defines the number
            // of classes to include in the final output.
            char[][] classes =
            {
                @"abcdefghijklmnopqrstuvwxyz".ToCharArray(),
                @"ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
                @"0123456789".ToCharArray(),
                @"#$&*+?@".ToCharArray(),
                //@" !""#$%&'()*+,./:;<>?@[\]^_{|}~".ToCharArray(),
            };

            complexity = Math.Max(1, Math.Min(classes.Length, complexity));
            if (length < complexity)
                throw new ArgumentOutOfRangeException("length");

            // Since we are taking a random number 0-255 and modulo that by the number of
            // characters, characters that appear earilier in this array will recieve a
            // heavier weight. To counter this we will then reorder the array randomly.
            // This should prevent any specific character class from recieving a priority
            // based on it's order.
            char[] allchars = classes.Take(complexity).SelectMany(c => c).ToArray();
            byte[] bytes = new byte[allchars.Length];
            csp.GetBytes(bytes);
            for (int i = 0; i < allchars.Length; i++)
            {
                char tmp = allchars[i];
                allchars[i] = allchars[bytes[i] % allchars.Length];
                allchars[bytes[i] % allchars.Length] = tmp;
            }

            // Create the random values to select the characters
            Array.Resize(ref bytes, length);
            char[] result = new char[length];

            while (true)
            {
                csp.GetBytes(bytes);
                // Obtain the character of the class for each random byte
                for (int i = 0; i < length; i++)
                    result[i] = allchars[bytes[i] % allchars.Length];

                // Verify that it does not start or end with whitespace
                if (Char.IsWhiteSpace(result[0]) || Char.IsWhiteSpace(result[(length - 1) % length]))
                    continue;

                string testResult = new string(result);
                // Verify that all character classes are represented
                if (0 != classes.Take(complexity).Count(c => testResult.IndexOfAny(c) < 0))
                    continue;

                return testResult;
            }
        }

        /// <summary>
        /// Encodes text as base64
        /// </summary>
        /// <param name="plainText">Text to be encoded</param>
        /// <returns>Base64 code</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Generates a token for a record
        /// </summary>
        /// <param name="id">Record id</param>
        /// <returns>Token</returns>
        public static string GenerateToken(string id)
        {
            // Generate a token which is compound from the record id and expiration
            return AESEncryption.Encrypt(id + "_" + DateTime.Now.AddMinutes(GetTokenExpiration()));
        }

        /// <summary>
        /// Gets base URL
        /// </summary>
        /// <returns>Base URL</returns>
        public static string GetBaseUrl()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                            HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
        }

        /// <summary>
        /// Gets request URL
        /// </summary>
        /// <returns>Request URL</returns>
        public static string GetRequestUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }

        public static string GetRequestIPAddress()
        {
            string ipAdd = null;

            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAdd = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else
            {
                ipAdd = HttpContext.Current.Request.UserHostAddress.ToString();
            }
            return ipAdd;
        }

        public static string GetFileExtension(string fileName)
        {
            return System.IO.Path.GetExtension(fileName).Substring(1).ToLower();
        }

        public static int GetMaxRequestLength()
        {
            int maxRequestLength = 0;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            if (section != null)
            {
                maxRequestLength = section.MaxRequestLength * 1024;
            }

            return maxRequestLength;
        }
        public static void OpenAndAddTextToWordDocument(string filepath, string txt)
        {
            // Open a WordprocessingDocument for editing using the filepath.
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            // Assign a reference to the existing document body.
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            // Add new text.
            Paragraph para = body.InsertBeforeSelf(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text(txt));

            // Close the handle explicitly.
            wordprocessingDocument.Close();
        }

        public static bool CheckFileExtension(string ext)
        {
            string[] allowedExt = new string[] { "doc", "docx", "pdf" };
            return allowedExt.FirstOrDefault(a => a.ToLower() == ext.ToLower()) != null;
        }

        public static bool IsImageExtension(string ext)
        {
            string[] allowedExt = new string[] { "jpg", "jpeg", "gif", "png" };
            return allowedExt.FirstOrDefault(a => a.ToLower() == ext.ToLower()) != null;
        }

        /// <summary>
        /// Generates from & to filter dates
        /// </summary>
        /// <param name="fromDate">Output from date</param>
        /// <param name="toDate">Output to date</param>
        /// <param name="period">Period filter</param>
        /// <param name="from">Input from date</param>
        /// <param name="to">Input to date</param>
        /// <returns></returns>
        public static void GenerateFilterDates(out DateTime? fromDate, out DateTime? toDate, PeriodFilter period = PeriodFilter.ThisMonth, DateTime? from = null, DateTime? to = null)
        {
            if (period == PeriodFilter.Custom)
            {
                fromDate = from.MinTime();
                toDate = to.MaxTime();
            }
            else if (period == PeriodFilter.Today)
            {
                fromDate = DateTime.Today.MinTime();
                toDate = DateTime.Today.MaxTime();
            }
            else if (period == PeriodFilter.ThisWeek)
            {
                fromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                toDate = DateTime.Now;
            }
            else if (period == PeriodFilter.ThisMonth)
            {
                fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                toDate = DateTime.Now.Date.MaxTime();
            }
            else if (period == PeriodFilter.LastMonth)
            {
                fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                toDate = fromDate.Value.AddMonths(1).AddTicks(-1);
            }
            else if (period == PeriodFilter.ThisYear)
            {
                fromDate = new DateTime(DateTime.Today.Year, 1, 1);
                toDate = DateTime.Now;
            }
            else if (period == PeriodFilter.LastYear)
            {
                fromDate = new DateTime(DateTime.Today.Year - 1, 1, 1);
                toDate = (new DateTime(DateTime.Today.Year - 1, 12, 31)).MaxTime();
            }
            else if (period == PeriodFilter.LastTwoYears)
            {
                fromDate = new DateTime(DateTime.Today.Year - 1, 1, 1);
                toDate = DateTime.Now;
            }
            else if (period == PeriodFilter.LastFiveYears)
            {
                fromDate = new DateTime(DateTime.Today.Year - 4, 1, 1);
                toDate = DateTime.Now;
            }
            else
            {
                fromDate = (DateTime?)null;
                toDate = (DateTime?)null;
            }
        }

        /// <summary>
        /// Validates a token for a record
        /// </summary>
        /// <param name="id">Record id</param>
        /// <param name="token">Token</param>
        /// <returns>Boolean</returns>
        public static bool ValidateToken(string id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            string[] parts = AESEncryption.Decrypt(token).Split(new char[] { '_' });

            if (parts.Count() != 2)
            {
                return false;
            }
            else
            {
                DateTime expiry = DateTime.Now;

                if (parts[0] == id && DateTime.TryParse(parts[1], out expiry) && expiry > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string GetUserPlatform()
        {
            var ua = HttpContext.Current.Request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return HttpContext.Current.Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        public static string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        private static double GetTokenExpiration()
        {
            return 30;
        }

        public static void ExportToExcel<T>(List<T> data, string fileName = "Default.xls")
        {
            DataTable dt = ListToDataTable(data);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.AddHeader("FileName", fileName);
            HttpContext.Current.Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv=\"content-type\" content=\"text/plain; charset=UTF-8\"/></head><body>" + ConvertDataTable2HTMLTableString(dt) + "</body></html>");
        }

        private static string ConvertDataTable2HTMLTableString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table><thead><tr style='background-color:#d9d9d9; border: 1px solid black;'>");
            foreach (DataColumn c in dt.Columns)
            {
                sb.AppendFormat("<th>{0}</th>", c.ColumnName);
            }
            sb.AppendLine("</tr></thead><tbody>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr style='border:1px solid black; font-size: 14px;'>");
                foreach (object o in dr.ItemArray)
                {
                    sb.AppendFormat("<td>{0}</td>", System.Web.HttpUtility.HtmlEncode(o.ToString()));
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody></table>");
            return sb.ToString();
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
    }
}