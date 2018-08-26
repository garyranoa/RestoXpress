using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace UHack.Web.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Checks to be sure a phone number contains 10 digits as per American phone numbers.  
        /// If 'IsRequired' is true, then an empty string will return False. 
        /// If 'IsRequired' is false, then an empty string will return True.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="IsRequired"></param>
        /// <returns></returns>
        public static bool ValidatePhoneNumber(this string phone, bool IsRequired)
        {
            if (string.IsNullOrEmpty(phone) & !IsRequired)
                return true;

            if (string.IsNullOrEmpty(phone) & IsRequired)
                return false;

            var cleaned = phone.RemoveNonNumeric();
            if (IsRequired)
            {
                if (cleaned.Length == 10)
                    return true;
                else
                    return false;
            }
            else
            {
                if (cleaned.Length == 0)
                    return true;
                else if (cleaned.Length > 0 & cleaned.Length < 10)
                    return false;
                else if (cleaned.Length == 10)
                    return true;
                else
                    return false; // should never get here
            }
        }

        /// <summary>
        /// Removes all non numeric characters from a string
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string RemoveNonNumeric(this string phone)
        {
            return Regex.Replace(phone, @"[^0-9]+", "");
        }

        public static string ReformatPhone(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;

            phone = phone.RemoveNonNumeric();
            return string.Format("({0}) {1}-{2}",
                    phone.Substring(0, 3),
                    phone.Substring(3, 3),
                    phone.Substring(6));
        }

        public static int GetTotalDigits(this string str)
        {
            int flag = 0;
            foreach (char c in str.ToCharArray())
            {
                if (Char.IsDigit(c))
                    flag++;
            }
            return flag;
        }

        public static int GetTotalChar(this string str)
        {
            int flag = 0;
            foreach (char c in str.ToCharArray())
            {
                if (Char.IsLetter(c))
                    flag++;
            }
            return flag;
        }
    }
}
