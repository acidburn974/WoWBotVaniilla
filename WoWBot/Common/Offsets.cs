using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWBot.Common
{
    /// <summary>
    /// Offsets en relation avec le client 1.12.1.5875
    /// </summary>
    public static class Offsets
    {
        #region Nested type: General
        public enum General
        {
            IsInGame = 0x00B4B424
        }

        #endregion

        #region Nested type: StaticPointers
        /// <summary>
        ///   1.12.1.5875
        /// </summary>
        internal enum StaticPointers
        {
            CurrentTargetGUID = 0x74e2d8,
            LocalPlayerGUID = 0x741e30,
        }

        #endregion

        public enum ObjectManager
        {
            CurrentManagerPointer = 0x00741414,
            CurrentManagerOffset = 0xAC,
            FirstObject = 0xAC, // CurrentManagerPointer + 0xAC
            NextObject = 0x3C, // CurrentObject + 0x3C
            LocalGuid = 0xC0
        }

        public enum Object
        {
            DataPtr = 0x8,
            Type = 0x14,
            GUID = 0x30,
            Y = 0x9b8,
            X = Y + 0x4,
            Z = Y + 0x8,
            Rotation = X + 0x10,  // This seems to be wrong
            GameObjectY = 0x2C4, // *DataPtr (0x288) + 0x3c
            GameObjectX = GameObjectY + 0x4,
            GameObjectZ = GameObjectY + 0x8,
            Speed = 0xA34
        }

        public enum Player
        {
            Name = 0x827D88,
            MouseOverGUID = 0x00B4E2C8
        }

        public enum CreatureType
        {
            Unknown = 0,
            Beast,
            Dragon,
            Demon,
            Elemental,
            Giant,
            Undead,
            Humanoid,
            Critter,
            Mechanical,
            NotSpecified,
            Totem,
            NonCombatPet,
            GasCloud
        }
    }
}
