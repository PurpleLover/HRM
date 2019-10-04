using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CommonHelper.String
{
    public static class StringUtilities
    {

        public static List<SelectListItem> AddDefault(this List<SelectListItem> obj, string messageInit)
        {
            if (string.IsNullOrEmpty(messageInit))
            {
                messageInit = "--Chọn--";
            }
            if (obj == null)
            {
                obj = new List<SelectListItem>();
            }

            if (!obj.Any(x => x.Text.Equals(messageInit)))
            {
                obj.Add(new SelectListItem()
                {
                    Text = messageInit,
                    Value = "",
                    Selected=obj.Any(x=>x.Selected)?false:true
                });
            }
            return obj;
        }
        public static DateTime? ToDateTime(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            var date = input.Split('/');
            if (string.IsNullOrEmpty(date[0]) || string.IsNullOrEmpty(date[1]) || string.IsNullOrEmpty(date[2]))
            {
                return null;
            }
            var day = int.Parse(date[0]).ToString("00");
            var month = int.Parse(date[1]).ToString("00");
            var year = int.Parse(date[2]).ToString("0000");
            return DateTime.ParseExact(string.Format("{0}/{1}/{2}", day, month, year), "dd/MM/yyyy", null);
        }

        public static IEnumerable<T> ToListNumber<T>(this string input, char splitKey = ',') where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (!string.IsNullOrEmpty(input))
            {
                var list = input.Split(splitKey);
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        yield return (T)Convert.ChangeType(item, typeof(T));
                    }
                }
            }
        }

        public static Nullable<T> ToNullableNumber<T>(this string input) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (string.IsNullOrEmpty(input) || !input.All(char.IsDigit))
            {
                return null;
            }
            var result = (T)Convert.ChangeType(input, typeof(T));
            return result;
        }

        public static T ToNumber<T>(this string input) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (string.IsNullOrEmpty(input) || !input.All(char.IsDigit))
            {
                return default(T);
            }
            var result = (T)Convert.ChangeType(input, typeof(T));
            return result;
        }

        public static string GetErrors(this ModelStateDictionary modelState)
        {
            string result = string.Empty;
            foreach (var item in modelState)
            {
                var state = item.Value;
                if (state.Errors.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in state.Errors)
                    {
                        sb.Append(error.ErrorMessage);
                    }
                    result = sb.ToString();
                }
            }
            return result;
        }

        public static DateTime? ToEndDay(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            var parts = input.Split('/');
            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
            {
                return null;
            }
            int day, month, year;
            day = month = year = 0;

            if (!int.TryParse(parts[0], out day) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out year))
            {
                return null;
            }
            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", day.ToString("00"), month.ToString("00"), year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);
        }

        public static DateTime? ToStartDay(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            var parts = input.Split('/');
            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
            {
                return null;
            }
            int day, month, year;
            day = month = year = 0;

            if (!int.TryParse(parts[0], out day) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out year))
            {
                return null;
            }
            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", day.ToString("00"), month.ToString("00"), year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);
        }
        /// <summary>
        /// Kiểm tra thao tác có nằm trong danh sách thao tác mà user có quyền không
        /// </summary>
        /// <param name="list_thaotac">Danh sách thao tác của user đang đăng nhập</param>
        /// <param name="ma_thaotac">Thao tác muốn kiểm tra quyền</param>
        /// <returns></returns>

        private static readonly string[] VietnameseSigns = new string[]

        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"

        };
        public static string RemoveSign4VietnameseString(string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }

        public static string GetFileNameFormart(this string str)
        {
            var result = RemoveSign4VietnameseString(str);
            result = result.Trim();
            result = result.Replace(' ', '_');
            return result;
        }
        public static bool ToBoolByOnOff(this string obj)
        {
            if (!string.IsNullOrEmpty(obj) && obj.ToLower().Equals("On".ToLower()))
            {
                return true;
            }
            return false;

        }
        public static bool? ToBoolByOnOffNULL(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(obj) && obj.ToLower().Equals("On".ToLower()))
            {
                return true;
            }
            return false;

        }
        public static DateTime? ToDateTimeFromMonth(this string obj)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(obj))
            {
                var date = obj.Split('/');
                if (date != null)
                {
                    if (!string.IsNullOrEmpty(date[0]) && !string.IsNullOrEmpty(date[1]))
                    {

                        var month = int.Parse(date[0]).ToString("00");
                        var year = int.Parse(date[1]).ToString("0000");
                        return DateTime.ParseExact(string.Format("{0}/{1}/{2}", "01", month, year), "dd/MM/yyyy", null);
                    }
                }
                return null;
            }
            else
            {
                return null;
            }

        }


        public static DateTime? ToDateTimeFromYear(this string obj)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(obj))
            {
                return DateTime.ParseExact(string.Format("{0}/{1}/{2}", "01", "01", obj), "dd/MM/yyyy", null);

            }
            else
            {
                return null;
            }
            //}catch(Exception ex){
            //    return null;
            // }
        }

        public static DateTime? ToEndYear(this DateTime? obj)
        {
            if (obj.HasValue)
            {
                var day = DateTime.DaysInMonth(obj.Value.Year, 12).ToString("00");

                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", day, "12", obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }

        public static DateTime? ToEndMonth(this DateTime? obj)
        {
            if (obj.HasValue)
            {
                var day = DateTime.DaysInMonth(obj.Value.Year, obj.Value.Month).ToString("00");

                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", day, obj.Value.Month.ToString("00"), obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }
        public static DateTime ToEndMonth(this DateTime obj)
        {

            var day = DateTime.DaysInMonth(obj.Year, obj.Month).ToString("00");

            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", day, obj.Month.ToString("00"), obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);


        }
        public static DateTime? ToEndDay(this DateTime? obj)
        {
            if (obj.HasValue)
            {


                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", obj.Value.Day.ToString("00"), obj.Value.Month.ToString("00"), obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }
        public static DateTime ToEndDay(this DateTime obj)
        {


            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", obj.Day.ToString("00"), obj.Month.ToString("00"), obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

        }
        public static DateTime? ToStartDay(this DateTime? obj)
        {
            if (obj.HasValue)
            {


                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", obj.Value.Day.ToString("00"), obj.Value.Month.ToString("00"), obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }
        public static DateTime ToStartDay(this DateTime obj)
        {

            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", obj.Day.ToString("00"), obj.Month.ToString("00"), obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

        }
        public static DateTime? ToStartMonth(this DateTime? obj)
        {
            if (obj.HasValue)
            {


                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", "01", obj.Value.Month.ToString("00"), obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }
        public static DateTime ToStartMonth(this DateTime obj)
        {


            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", "01", obj.Month.ToString("00"), obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

        }
        public static DateTime ToEndYear(this DateTime obj)
        {

            var day = DateTime.DaysInMonth(obj.Year, 12).ToString("00");

            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 23:59:59", day, "12", obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);


        }
        public static DateTime? ToStartYear(this DateTime? obj)
        {
            if (obj.HasValue)
            {

                return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", "01", "01", obj.Value.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);

            }
            else
            {
                return null;
            }
        }
        public static DateTime ToStartYear(this DateTime obj)
        {

            return DateTime.ParseExact(string.Format("{0}/{1}/{2} 00:00:00", "01", "01", obj.Year.ToString("0000")), "dd/MM/yyyy HH:mm:ss", null);


        }

        public static short? ToShortOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return short.Parse(obj);
            }
            else
            {
                return null;
            }
        }

        public static int? ToIntOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return int.Parse(obj);
            }
            else
            {
                return null;
            }
        }
        public static long? ToLongOrNULL(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return long.Parse(obj);
            }
            else
            {
                return null;
            }
        }
        public static short ToShortOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return short.Parse(obj);
            }
            else
            {
                return 0;
            }
        }

        public static int ToIntOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return int.Parse(obj);
            }
            else
            {
                return 0;
            }
        }
        public static bool ToBoolOrFalse(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return bool.Parse(obj);
            }
            else
            {
                return false;
            }
        }
        public static long ToLongOrZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                return long.Parse(obj);
            }
            else
            {
                return 0;
            }
        }

        public static float ToFloatOrZero(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return float.Parse(obj);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {

                return 0;
            }

        }
        public static float? ToFloatOrNull(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return float.Parse(obj);
                }
                else
                {
                    return null;
                }
            }
            catch
            {

                return null;
            }

        }
        public static decimal ToDecimalOrZero(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return decimal.Parse(obj);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {

                return 0;
            }
        }
        public static decimal? ToDecimalOrNull(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return decimal.Parse(obj);
                }
                else
                {
                    return null;
                }
            }
            catch
            {

                return null;
            }
        }
        public static List<long> ToListLong(this string obj, char split_key)
        {
            List<long> listLong = new List<long>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listLong.Add(long.Parse(item));
                        }
                    }
                }
            }
            return listLong;
        }
        public static List<int> ToListInt(this string obj, char split_key)
        {
            List<int> listInt = new List<int>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listInt.Add(int.Parse(item));
                        }
                    }
                }
            }
            return listInt;
        }
        public static List<string> ToListStringLower(this string obj, char split_key)
        {
            List<string> listInt = new List<string>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listInt.Add(item);
                        }
                    }
                }
            }
            return listInt;
        }


        public static List<short> ToListShort(this string obj, char split_key)
        {
            List<short> listInt = new List<short>();
            if (!string.IsNullOrEmpty(obj))
            {
                var list = obj.Split(split_key);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            listInt.Add(short.Parse(item));
                        }
                    }
                }
            }
            return listInt;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1 || firstWeek > 50)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static DateTime GetStartOfWeek(int year, int month, int weekofmonth)
        {
            //lấy ngày bắt đầu của tuần trong tháng
            var day = weekofmonth * 7 - 6;
            var StartDate = new DateTime(year, month, day);
            var weekOfYear = GetIso8601WeekOfYear(StartDate);
            return FirstDateOfWeek(year, weekOfYear, CultureInfo.CurrentCulture);
        }

        public static DateTime GetEndOfWeek(DateTime startOfWeek)
        {
            return startOfWeek.AddDays(6);
        }
        public static string Truncate(string input = "", int length = 0)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length <= length)
                {
                    return input;
                }
                else
                {
                    return input.Substring(0, length) + "...";
                }
            }
            return string.Empty;
        }

        public static string GetSummary(this string input, int length = 0)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length <= length)
                {
                    return input;
                }
                else
                {
                    return input.Substring(0, length) + "...";
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Chuyển số tiền sang dạng chữ
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ChuyenSo(this string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn, ", "triệu, ", "tỉ, " };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

            var length = number.Length;
            number += "ss";
            var doc = new StringBuilder();
            var rd = 0;

            var i = 0;
            while (i < length)
            {
                //So chu so o hang dang duyet
                var n = (length - i + 2) % 3 + 1;

                //Kiem tra so 0
                var found = 0;
                int j;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] == '0') continue;
                    found = 1;
                    break;
                }

                //Duyet n chu so
                int k;
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        var ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3)
                                    doc.Append(cs[0]);
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0')
                                        doc.Append("lẻ");
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                switch (n - j)
                                {
                                    case 3:
                                        doc.Append(cs[1]);
                                        break;
                                    case 2:
                                        doc.Append("mười");
                                        ddv = 0;
                                        break;
                                    case 1:
                                        k = (i + j == 0) ? 0 : i + j - 1;
                                        doc.Append((number[k] != '1' && number[k] != '0') ? "mốt" : cs[1]);
                                        break;
                                }
                                break;
                            case '5':
                                doc.Append((i + j == length - 1) ? "lăm" : cs[5]);
                                break;
                            default:
                                doc.Append(cs[number[i + j] - 48]);
                                break;
                        }

                        doc.Append(" ");

                        //Doc don vi nho
                        if (ddv == 1)
                            doc.Append(dv[n - j - 1] + " ");
                    }
                }


                //Doc don vi lon
                if (length - i - n > 0)
                {
                    if ((length - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (length - i - n) / 9; k++)
                                doc.Append("tỉ, ");
                        rd = 0;
                    }
                    else
                        if (found != 0) doc.Append(dv[((length - i - n + 1) % 9) / 3 + 2] + " ");
                }

                i += n;
            }
            var result = (length == 1) && (number[0] == '0' || number[0] == '5') ? cs[number[0] - 48] : doc.ToString();
            result = result.Trim().Trimtxt();
            if (result[result.Length - 1] == ',')
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        public static string Trimtxt(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {

                str = str.TrimStart();
                str = str.TrimEnd();
                str = str.Trim();
                while (str.Contains("  "))
                {
                    str = str.Replace("  ", " ");
                }
            }
            else
            {
                str = string.Empty;
            }


            return str;
        }
        public static string ToDateTimeTextZero(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var date = obj.Split('/');
                if (date != null)
                {
                    if (date.Length == 3)
                    {
                        return obj;
                    }

                    if (date.Length == 2)
                    {
                        return "00/" + obj;
                    }

                    if (date.Length == 1)
                    {
                        return "00/00/" + obj;
                    }
                }
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }

        }
        public static string ToSafeFileName(this string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        public static double ToDoubleOrZero(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return double.Parse(obj);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public static double? ToDoublelOrNull(this string obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    return double.Parse(obj);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string ConvertToVN(this string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }
        public static string GetChuCaiDauTrongHoTen(this string obj)
        {
            obj = obj.Trim().Trimtxt();
            obj = obj.ConvertToVN();
            var tmpStr = obj.Split(' ');
            string result = string.Empty;
            if (!string.IsNullOrEmpty(obj))
            {

                if (tmpStr.Length == 1)
                {

                    if (tmpStr[0].Length == 1)
                    {
                        result += tmpStr[0][0];
                        result += tmpStr[0][0];
                        result += tmpStr[0][0];
                    }
                    else
                    {
                        result += tmpStr[0][0];
                        result += tmpStr[0][1];
                        result += tmpStr[0][tmpStr[0].Length - 1];
                    }

                }
                else if (tmpStr.Length == 2)
                {
                    result += tmpStr[0][0];
                    result += tmpStr[1][0];
                    result += tmpStr[1][tmpStr[1].Length - 1];
                }
                else
                {
                    result += tmpStr[0][0];
                    result += tmpStr[tmpStr.Length - 2][0];
                    result += tmpStr[tmpStr.Length - 1][0];
                }
            }
            return result.ToUpper();
        }
    }
}
