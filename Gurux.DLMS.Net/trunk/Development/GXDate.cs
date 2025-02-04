//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// More information of Gurux products: http://www.gurux.org
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Gurux.DLMS.Enums;
using System.Globalization;

namespace Gurux.DLMS
{
    /// <summary>
    /// This class is used because in COSEM object model some fields from date time can be ignored.
    /// Default behavior of DateTime do not allow this.
    /// </summary>
    public class GXDate : GXDateTime
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDate()
            : base()
        {
            Skip = DateTimeSkips.Hour | DateTimeSkips.Minute | DateTimeSkips.Second | DateTimeSkips.Ms;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDate(DateTime value)
            : base(value)
        {
            Skip = DateTimeSkips.Hour | DateTimeSkips.Minute | DateTimeSkips.Second | DateTimeSkips.Ms;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDate(GXDateTime value)
            : base(value)
        {
            Skip = value.Skip | DateTimeSkips.Hour | DateTimeSkips.Minute | DateTimeSkips.Second | DateTimeSkips.Ms;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="date">Date string.</param>
        public GXDate(string date)
            : this(date, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="date">Date string.</param>
        public GXDate(string date, System.Globalization.CultureInfo culture)
            : base()
        {
            Skip = DateTimeSkips.Hour | DateTimeSkips.Minute | DateTimeSkips.Second | DateTimeSkips.Ms;
            if (date != null)
            {
                int year = 2000, month = 1, day = 1;
#if !WINDOWS_UWP
                string dateSeparator = culture.DateTimeFormat.DateSeparator, timeSeparator = culture.DateTimeFormat.TimeSeparator;
                List<string> shortDatePattern = new List<string>(culture.DateTimeFormat.ShortDatePattern.Split(new string[] { CultureInfo.InvariantCulture.DateTimeFormat.DateSeparator, dateSeparator, " " }, StringSplitOptions.RemoveEmptyEntries));
#else
                //In UWP Standard Date and Time Format Strings are used.
                string dateSeparator = Internal.GXCommon.GetDateSeparator(), timeSeparator = Internal.GXCommon.GetTimeSeparator();
                List<string> shortDatePattern = new List<string>("yyyy-MM-dd".Split(new string[] { dateSeparator, " " }, StringSplitOptions.RemoveEmptyEntries));
#endif
                Skip |= DateTimeSkips.DayOfWeek;
                string[] values = date.Trim().Split(new string[] { dateSeparator, timeSeparator, " " }, StringSplitOptions.None);
                int cnt = shortDatePattern.Count;
                if (!string.IsNullOrEmpty(culture.DateTimeFormat.PMDesignator))
                {
                    if (date.IndexOf(culture.DateTimeFormat.PMDesignator) != -1)
                    {
                        ++cnt;
                    }
                    else if (date.IndexOf(culture.DateTimeFormat.AMDesignator) != -1)
                    {
                        ++cnt;
                    }
                }               
                int offset = 3;
                for (int pos = 0; pos != shortDatePattern.Count; ++pos)
                {
                    bool skip = false;
                    if (values[pos] == "*")
                    {
                        skip = true;
                    }
                    if (shortDatePattern[pos].ToLower().StartsWith("yy"))
                    {
                        if (skip)
                        {
                            Skip |= DateTimeSkips.Year;
                        }
                        else
                        {
                            year = int.Parse(values[pos]);
                        }
                    }
                    else if (shortDatePattern[pos].ToLower().StartsWith("m"))
                    {
                        if (skip)
                        {
                            Skip |= DateTimeSkips.Month;
                        }
                        else
                        {
                            month = int.Parse(values[pos]);
                        }
                    }
                    else if (shortDatePattern[pos].ToLower().StartsWith("d"))
                    {
                        if (skip)
                        {
                            Skip |= DateTimeSkips.Day;
                        }
                        else
                        {
                            day = int.Parse(values[pos]);
                        }
                    }
                    else
                    {
                        //This is OK. There might be day name in some cultures.
                        ++offset;
                    }
                }
                DateTime dt = culture.Calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
                this.Value = new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDate(int year, int month, int day)
            : base(year, month, day, -1, -1, -1, -1)
        {
        }
    }
}
