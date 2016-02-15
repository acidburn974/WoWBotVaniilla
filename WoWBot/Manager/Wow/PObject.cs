using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWBot.Manager.Wow
{
    /// <summary>
    /// Contains all information for a WowObject.
    /// </summary>
    class PObject
    {
        public uint BaseAddress { get; set; }
        public bool IsValid
        {
            get
            {
                if(BaseAddress != UInt32.MinValue)
                {
                    return true;
                }
                return false;
            }
        }
        public ulong GUID
        {
            get
            {
                if(IsValid)
                {
                    return Common.Memory.Wow.ReadUInt64(BaseAddress + (uint) Common.Offsets.Object.GUID);
                }
                return ulong.MinValue;
            }
        }
        public uint Type
        {
            get
            {
                return Common.Memory.Wow.ReadUInt(BaseAddress + (uint) Common.Offsets.Object.Type);
            }
        }
        public float X
        {
            get
            {
                try
                {
                    return Common.Memory.Wow.ReadFloat(this.BaseAddress + (uint)Common.Offsets.Object.X);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public float Y
        {
            get
            {
                try
                {
                    return Common.Memory.Wow.ReadFloat(this.BaseAddress + (uint)Common.Offsets.Object.Y);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public float Z
        {
            get
            {
                try
                {
                    return Common.Memory.Wow.ReadFloat(this.BaseAddress + (uint)Common.Offsets.Object.Z);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// En radian
        /// </summary>
        public float Facing
        {
            get
            {
                try
                {
                    return Common.Memory.Wow.ReadFloat(this.BaseAddress + (uint)Common.Offsets.Object.Rotation);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public bool IsMe
        {
            get { return GUID == ObjectManager.MyPlayer.GUID ? true : false; }
        }

        public PObject(uint baseAddress)
        {
            BaseAddress = baseAddress;
        }

        /// <summary>
        /// Update the Object Base Address
        /// </summary>
        /// <param name="baseAddress"></param>
        public void UpateBaseAddress(uint baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}
