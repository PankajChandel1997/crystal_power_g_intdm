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
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Gurux.DLMS;
using Gurux.DLMS.Enums;

namespace Gurux.DLMS.ManufacturerSettings
{
    public enum StartProtocolType
    {
        IEC = 1,
        DLMS = 2
    }

#if !WINDOWS_UWP
    [Serializable]
#endif
    public class GXManufacturer
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXManufacturer()
        {
            StartProtocol = StartProtocolType.IEC;
            ObisCodes = new GXObisCodeCollection();
            Settings = new List<GXAuthentication>();
            ServerSettings = new List<GXServerAddress>();
        }

        /// <summary>
        /// Manufacturer Identification.
        /// </summary>
        /// <remarks>
        /// Device returns manufacturer ID when connection to the meter is made.
        /// </remarks>
        public String Identification
        {
            get;
            set;
        }

        /// <summary>
        /// Real name of the manufacturer.
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Is Logical name referencing used.
        /// </summary>
        [DefaultValue(false)]
        [XmlElement("UseLN")]
        public bool UseLogicalNameReferencing
        {
            get;
            set;
        }           

        /// <summary>
        /// Manufacturer spesific OBIS codes.
        /// </summary>
#if !WINDOWS_UWP
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
#endif
        public GXObisCodeCollection ObisCodes
        {
            get;
            set;
        }

        /// <summary>
        /// Is IEC 62056-47 supported.
        /// </summary>
        [DefaultValue(false)]
        public bool UseIEC47
        {
            get;
            set;
        }

        /// <summary>
        /// Is IEC 62056-21 skipped when using serial port connection.
        /// </summary>
        [DefaultValue(StartProtocolType.IEC)]
        public StartProtocolType StartProtocol
        {
            get;
            set;
        }

        /// <summary>
        /// Web address where is more information.
        /// </summary>
        public string WebAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Address info.
        /// </summary>
        public string Address
        {
            get;
            set;
        }


        /// <summary>
        /// Additional info.
        /// </summary>
        public string Info
        {
            get;
            set;
        }


        /// <summary>
        /// Used extension class.
        /// </summary>
        public string Extension
        {
            get;
            set;
        }

        /// <summary>
        /// Is manufacturer setting removed.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [XmlIgnore()]
        public bool Removed
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize settings.
        /// </summary>
        // [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<GXAuthentication> Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Server settings
        /// </summary>
        public List<GXServerAddress> ServerSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Used GMAC Security type.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [DefaultValue(Gurux.DLMS.Enums.Security.None)]
        public Gurux.DLMS.Enums.Security Security
        {
            get;
            set;
        }

        /// <summary>
        /// System Title.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [DefaultValue(null)]
        public byte[] SystemTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Server System Title.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [DefaultValue(null)]
        public byte[] ServerSystemTitle
        {
            get;
            set;
        }


        /// <summary>
        /// GMAC block cipher key.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [DefaultValue(null)]
        public byte[] BlockCipherKey
        {
            get;
            set;
        }

        /// <summary>
        /// GMAC authentication key.
        /// </summary>
#if !WINDOWS_UWP
        [Browsable(false)]
#endif
        [DefaultValue(null)]
        public byte[] AuthenticationKey
        {
            get;
            set;
        }

        public GXAuthentication GetAuthentication(Authentication authentication)
        {
            foreach (GXAuthentication it in Settings)
            {
                if (it.Type == authentication)
                {
                    return it;
                }
            }
            return null;
        }

        public GXAuthentication GetActiveAuthentication()
        {
            foreach (GXAuthentication it in Settings)
            {
                if (it.Selected)
                {
                    return it;
                }
            }
            if (Settings.Count != 0)
            {
                return Settings[0];
            }
            return null;
        }

        public GXServerAddress GetServer(HDLCAddressType type)
        {
            foreach (GXServerAddress it in this.ServerSettings)
            {
                if (it.HDLCAddress == type)
                {
                    return it;
                }
            }
            return null;
        }

        public GXServerAddress GetActiveServer()
        {
            foreach (GXServerAddress it in this.ServerSettings)
            {
                if (it.Selected)
                {
                    return it;
                }
            }
            if (this.ServerSettings.Count != 0)
            {
                return this.ServerSettings[0];
            }
            return null;
        }
    }
}