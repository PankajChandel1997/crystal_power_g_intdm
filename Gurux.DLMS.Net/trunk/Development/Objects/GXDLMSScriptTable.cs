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
using System.Xml.Serialization;
using Gurux.DLMS.Internal;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;

namespace Gurux.DLMS.Objects
{
    /// <summary>
    /// Script table objects contain a table of script entries. Each entry consists of a script identifier
    /// and a series of action specifications.
    /// Online help:
    /// http://www.gurux.fi/Gurux.DLMS.Objects.GXDLMSSchedule
    /// </summary>
    public class GXDLMSScriptTable : GXDLMSObject, IGXDLMSBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSScriptTable()
        : base(ObjectType.ScriptTable)
        {
            Scripts = new List<GXDLMSScript>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ln">Logical Name of the object.</param>
        public GXDLMSScriptTable(string ln)
        : base(ObjectType.ScriptTable, ln, 0)
        {
            Scripts = new List<GXDLMSScript>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ln">Logical Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSScriptTable(string ln, ushort sn)
        : base(ObjectType.ScriptTable, ln, sn)
        {
            Scripts = new List<GXDLMSScript>();
        }

        [XmlIgnore()]
        public List<GXDLMSScript> Scripts
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, Scripts };
        }

        #region IGXDLMSBase Members


        byte[] IGXDLMSBase.Invoke(GXDLMSSettings settings, ValueEventArgs e)
        {
            e.Error = ErrorCode.ReadWriteDenied;
            return null;
        }

        int[] IGXDLMSBase.GetAttributeIndexToRead(bool all)
        {
            List<int> attributes = new List<int>();
            //LN is static and read only once.
            if (all || string.IsNullOrEmpty(LogicalName))
            {
                attributes.Add(1);
            }
            //Scripts
            if (all || !base.IsRead(2))
            {
                attributes.Add(2);
            }
            return attributes.ToArray();
        }

        /// <inheritdoc cref="IGXDLMSBase.GetNames"/>
        string[] IGXDLMSBase.GetNames()
        {
            return new string[] { Internal.GXCommon.GetLogicalNameString(), "Scripts" };
        }

        int IGXDLMSBase.GetAttributeCount()
        {
            return 2;
        }

        int IGXDLMSBase.GetMethodCount()
        {
            return 1;
        }

        /// <inheritdoc cref="IGXDLMSBase.GetDataType"/>
        public override DataType GetDataType(int index)
        {
            if (index == 1)
            {
                return DataType.OctetString;
            }
            if (index == 2)
            {
                return DataType.Array;
            }
            throw new ArgumentException("GetDataType failed. Invalid attribute index.");
        }

        object IGXDLMSBase.GetValue(GXDLMSSettings settings, ValueEventArgs e)
        {
            if (e.Index == 1)
            {
                return GXCommon.LogicalNameToBytes(LogicalName);
            }
            if (e.Index == 2)
            {
                int cnt = Scripts.Count;
                GXByteBuffer data = new GXByteBuffer();
                data.SetUInt8((byte)DataType.Array);
                //Add count
                GXCommon.SetObjectCount(cnt, data);
                foreach (GXDLMSScript it in Scripts)
                {
                    data.SetUInt8((byte)DataType.Structure);
                    data.SetUInt8(2); //Count
                    GXCommon.SetData(settings, data, DataType.UInt16, it.Id); //Script_identifier:
                    data.SetUInt8((byte)DataType.Array);
                    data.SetUInt8((byte)it.Actions.Count); //Count
                    foreach (GXDLMSScriptAction a in it.Actions)
                    {
                        data.SetUInt8((byte)DataType.Structure);
                        //Count
                        data.SetUInt8(5);
                        //service_id
                        GXCommon.SetData(settings, data, DataType.Enum, a.Type);
                        if (a.Target == null)
                        {
#pragma warning disable CS0618  
                            //class_id
                            GXCommon.SetData(settings, data, DataType.UInt16, a.ObjectType);
                            //logical_name
                            GXCommon.SetData(settings, data, DataType.OctetString, GXCommon.LogicalNameToBytes(a.LogicalName));
#pragma warning restore CS0618
                        }
                        else
                        {
                            //class_id
                            GXCommon.SetData(settings, data, DataType.UInt16, a.Target.ObjectType);
                            //logical_name
                            GXCommon.SetData(settings, data, DataType.OctetString, GXCommon.LogicalNameToBytes(a.Target.LogicalName));
                        }
                        //index
                        GXCommon.SetData(settings, data, DataType.Int8, a.Index);
                        //parameter
                        DataType tp = a.ParameterDataType;
                        if (tp == DataType.None)
                        {
                            tp = GXCommon.GetValueType(a.Parameter);
                        }
                        GXCommon.SetData(settings, data, tp, a.Parameter);
                    }
                }
                return data.Array();
            }
            e.Error = ErrorCode.ReadWriteDenied;
            return null;
        }

        void IGXDLMSBase.SetValue(GXDLMSSettings settings, ValueEventArgs e)
        {
            if (e.Index == 1)
            {
                LogicalName = GXCommon.ToLogicalName(e.Value);
            }
            else if (e.Index == 2)
            {
                Scripts.Clear();
                //Fix Xemex bug here.
                //Xemex meters do not return array as they shoul be according standard.
                if (e.Value is Object[] && ((Object[])e.Value).Length != 0)
                {
                    if (((Object[])e.Value)[0] is Object[])
                    {
                        foreach (Object[] item in (Object[])e.Value)
                        {
                            GXDLMSScript script = new GXDLMSScript();
                            script.Id = Convert.ToInt32(item[0]);
                            Scripts.Add(script);
                            foreach (Object[] arr in (Object[])item[1])
                            {
                                GXDLMSScriptAction it = new GXDLMSScriptAction();
                                it.Type = (ScriptActionType)Convert.ToInt32(arr[0]);
                                ObjectType ot = (ObjectType)Convert.ToInt32(arr[1]);
                                String ln = GXCommon.ToLogicalName(arr[2]);
                                it.Target = settings.Objects.FindByLN(ot, ln);
                                if (it.Target == null)
                                {
#pragma warning disable CS0618
                                    it.ObjectType = ot;
                                    it.LogicalName = ln;
#pragma warning restore CS0618
                                }

                                it.Index = Convert.ToInt32(arr[3]);
                                it.Parameter = arr[4];
                                if (it.Parameter != null)
                                {
                                    it.ParameterDataType = GXDLMSConverter.GetDLMSDataType(it.Parameter);
                                }
                                script.Actions.Add(it);
                            }
                        }
                    }
                    else //Read Xemex meter here.
                    {
                        GXDLMSScript script = new GXDLMSScript();
                        script.Id = Convert.ToInt32(((Object[])e.Value)[0]);
                        Scripts.Add(script);
                        Object[] arr = (Object[])((Object[])e.Value)[1];
                        GXDLMSScriptAction it = new GXDLMSScriptAction();
                        it.Type = (ScriptActionType)Convert.ToInt32(arr[0]);
                        ObjectType ot = (ObjectType)Convert.ToInt32(arr[1]);
                        String ln = GXCommon.ToLogicalName(arr[2]);
                        it.Target = settings.Objects.FindByLN(ot, ln);
                        if (it.Target == null)
                        {
#pragma warning disable CS0618
                            it.ObjectType = ot;
                            it.LogicalName = ln;
#pragma warning restore CS0618
                        }

                        it.Index = Convert.ToInt32(arr[3]);
                        it.Parameter = arr[4];
                        script.Actions.Add(it);
                    }
                }
            }
            else
            {
                e.Error = ErrorCode.ReadWriteDenied;
            }
        }

        void IGXDLMSBase.Load(GXXmlReader reader)
        {
            Scripts.Clear();
            if (reader.IsStartElement("Scripts", true))
            {
                while (reader.IsStartElement("Script", true))
                {
                    GXDLMSScript it = new GXDLMSScript();
                    Scripts.Add(it);
                    it.Id = reader.ReadElementContentAsInt("ID");
                    if (reader.IsStartElement("Actions", true))
                    {
                        while (reader.IsStartElement("Action", true))
                        {
                            GXDLMSScriptAction a = new Objects.GXDLMSScriptAction();
                            a.Type = (ScriptActionType)reader.ReadElementContentAsInt("Type");
                            ObjectType ot = (ObjectType)reader.ReadElementContentAsInt("ObjectType");
                            string ln = reader.ReadElementContentAsString("LN");
                            a.Index = reader.ReadElementContentAsInt("Index");
                            a.Target = reader.Objects.FindByLN(ot, ln);
                            if (a.Target == null)
                            {
                                a.Target = GXDLMSClient.CreateObject(ot);
                                a.Target.LogicalName = ln;
                            }
                            a.ParameterDataType = (DataType)reader.ReadElementContentAsInt("ParameterDataType");
                            a.Parameter = reader.ReadElementContentAsObject("Parameter", null);
                        }
                        reader.ReadEndElement("Actions");
                    }
                }
                reader.ReadEndElement("Scripts");
            }
        }

        void IGXDLMSBase.Save(GXXmlWriter writer)
        {
            if (Scripts != null)
            {
                writer.WriteStartElement("Scripts");
                foreach (GXDLMSScript it in Scripts)
                {
                    writer.WriteStartElement("Script");
                    writer.WriteElementString("ID", it.Id.ToString());
                    writer.WriteStartElement("Actions");
                    foreach (GXDLMSScriptAction a in it.Actions)
                    {
                        writer.WriteStartElement("Action");
                        writer.WriteElementString("Type", ((int)a.Type).ToString());
                        if (a.Target == null)
                        {
                            writer.WriteElementString("ObjectType", (int)ObjectType.None);
                            writer.WriteElementString("LN", "0.0.0.0.0.0");
                            writer.WriteElementString("Index", "0");
                            writer.WriteElementString("ParameterDataType", (int)DataType.None);
                            writer.WriteElementObject("Parameter", "");
                        }
                        else
                        {
                            writer.WriteElementString("ObjectType", (int)a.Target.ObjectType);
                            writer.WriteElementString("LN", a.Target.LogicalName);
                            writer.WriteElementString("Index", a.Index);
                            writer.WriteElementObject("Parameter", a.Parameter);
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();//Actions
                    writer.WriteEndElement();//Script
                }
                writer.WriteEndElement();//Scripts
            }
        }

        void IGXDLMSBase.PostLoad(GXXmlReader reader)
        {
        }

        #endregion
    }
}
