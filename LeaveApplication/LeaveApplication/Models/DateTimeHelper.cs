﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace LeaveApplication.Models
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Convert Sql server Datetime format
        /// </summary>
        /// <param name="_DateTime"></param>
        /// <returns></returns>
        static public string dd_MM_yyyy_HH_mm_tt(string _DateTime)
        {

            return DateTime.Parse(_DateTime).ToString("dd/MM/yyyy HH:mm: tt");
        }
        /// <summary>
        /// Required Format dd/MM/yyyy h:mm tt
        /// </summary>
        /// <param name="_DateTime"></param>
        /// <returns></returns>
        public static string ToDateTime(string _DateTime)
        {
            return DateTime.ParseExact(_DateTime, "dd/MM/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture).ToString();
        }

        public static string yyyy_mm_dd(string _DateTime)
        {
            return DateTime.ParseExact(_DateTime, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
        }
        static public string ToDate(string _DateTime)
        {
            return DateTime.Parse(_DateTime).ToShortDateString();
        }
        static public string ToTime(string _DateTime)
        {
            return DateTime.Parse(_DateTime).ToShortTimeString();
        }
        static public string dd_MM_yyyy(string _DateTime)
        {
            return DateTime.Parse(_DateTime).ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// Return date on the bases of time zone assigned in webconfig
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDate()
        {
            DateTime serverTime = DateTime.Now;

            DateTime utcTime = serverTime.ToUniversalTime();

            // convert it to Utc using timezone setting of server computer

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["TimeZone"]);

            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            return localTime;
        }
       
    }
}