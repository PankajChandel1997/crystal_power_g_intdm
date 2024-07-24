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

using Gurux.DLMS.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Gurux.DLMS.Objects
{
    /// <summary>
    /// Read serialized COSEM object from the file.
    /// </summary>
    public class GXXmlReader : IDisposable
    {
        XmlReader reader = null;

        /// <summary>
        /// Collection of read objects.
        /// </summary>  
        public GXDLMSObjectCollection Objects
        {
            get;
            internal set;
        }


        public void Dispose()
        {
            if (reader != null)
            {
#if !WINDOWS_UWP
                reader.Close();
#endif
                reader = null;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stream">Stream.</param>
        internal GXXmlReader(Stream stream)
        {
            reader = XmlReader.Create(stream);
            Objects = new GXDLMSObjectCollection();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filename"></param>
        internal GXXmlReader(string filename)
        {
            reader = XmlReader.Create(filename);
            Objects = new GXDLMSObjectCollection();
        }

        public string Name
        {
            get
            {
                return reader.Name;
            }
        }

        private void GetNext()
        {
            while (reader.NodeType == XmlNodeType.Comment || reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        public bool EOF
        {
            get
            {
                return reader.EOF;
            }
        }

        public bool Read()
        {
            return reader.Read();
        }

        public void ReadEndElement(string name)
        {
            GetNext();
            if (reader.NodeType == XmlNodeType.EndElement && string.Compare(name, reader.Name, true) == 0)
            {
                reader.Read();
                GetNext();
            }
        }

        public bool IsStartElement(string name, bool getNext)
        {
            GetNext();
            bool ret = reader.IsStartElement(name);
            if (getNext && (ret || (reader.NodeType == XmlNodeType.EndElement && string.Compare(name, reader.Name, true) == 0)))
            {
                reader.Read();
                if (!ret)
                {
                    ret = IsStartElement(name, getNext);
                }
            }
            GetNext();
            return ret;
        }

        public bool IsStartElement()
        {
            return reader.IsStartElement();
        }

        public string GetAttribute(int index)
        {
            return reader.GetAttribute(index);
        }

        public int ReadElementContentAsInt(string name)
        {
            return ReadElementContentAsInt(name, 0);
        }

        public int ReadElementContentAsInt(string name, int defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                int ret = reader.ReadElementContentAsInt();
                GetNext();
                return ret;
            }
            return defaultValue;
        }

        public long ReadElementContentAsLong(string name)
        {
            return ReadElementContentAsLong(name, 0);
        }

        public long ReadElementContentAsLong(string name, long defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                long ret = reader.ReadElementContentAsLong();
                return ret;
            }
            return defaultValue;
        }

        public UInt64 ReadElementContentAsULong(string name)
        {
            return ReadElementContentAsULong(name, 0);
        }

        public UInt64 ReadElementContentAsULong(string name, UInt64 defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                UInt64 ret = Convert.ToUInt64(reader.ReadElementContentAsString());
                return ret;
            }
            return defaultValue;
        }


        public double ReadElementContentAsDouble(string name, double defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                string str = reader.ReadElementContentAsString();
                double ret = double.Parse(str, CultureInfo.InvariantCulture);
                return ret;
            }
            return defaultValue;
        }

        object[] ReadArray()
        {
            List<object> list = new List<object>();
            while (IsStartElement("Item", false))
            {
                list.Add(ReadElementContentAsObject("Item", null));
            }
            return list.ToArray();
        }

        public object ReadElementContentAsObject(string name, object defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                object ret;
                DataType tp = (DataType)Enum.Parse(typeof(DataType), reader.GetAttribute(0));
                if (tp == DataType.Array)
                {
                    reader.Read();
                    GetNext();
                    ret = ReadArray();
                    ReadEndElement(name);
                    return ret;
                }
                else
                {
                    string str = reader.ReadElementContentAsString();
                    if (tp == DataType.OctetString)
                    {
                        ret = GXDLMSTranslator.HexToBytes(str);
                    }
                    else if (tp == DataType.DateTime)
                    {
                        ret = new GXDateTime(str, CultureInfo.InvariantCulture);
                    }
                    else if (tp == DataType.Date)
                    {
                        ret = new GXDate(str, CultureInfo.InvariantCulture);
                    }
                    else if (tp == DataType.Time)
                    {
                        ret = new GXTime(str, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        ret = Convert.ChangeType(str, GXDLMSConverter.GetDataType(tp));
                    }
                }
                while (!(reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement))
                {
                    reader.Read();
                }
                return ret;
            }
            return defaultValue;
        }

        public string ReadElementContentAsString(string name)
        {
            return ReadElementContentAsString(name, null);
        }

        public string ReadElementContentAsString(string name, string defaultValue)
        {
            GetNext();
            if (string.Compare(name, reader.Name, true) == 0)
            {
                string ret = reader.ReadElementContentAsString();
                GetNext();
                return ret;
            }
            return defaultValue;
        }

        public override string ToString()
        {
            if (reader != null)
            {
                return reader.NodeType + ", Name=\"" + reader.Name + "\"";
            }
            return base.ToString();
        }
    }
}
