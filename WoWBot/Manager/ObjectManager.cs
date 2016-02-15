using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magic;

using WoWBot.Manager.Wow;

namespace WoWBot.Manager
{
    /// <summary>
    /// Gère les objets du jeu
    /// </summary>
    static class ObjectManager
    {
        private static readonly BlackMagic Wow = Common.Memory.Wow;
        private static Thread _refresher;
        private static readonly object Locker = new object();
        private static List<Wow.PObject> ObjectList { get; set; }
        private static Dictionary<ulong, PObject> ObjectDictionary { get; set; }
        public static bool IsAttached = Common.Memory.IsAttached;
        public static IntPtr BaseAddress => Common.Memory.BaseAddress;
        public static uint InGameObjectManager
        {
            get
            {
                if (!IsAttached) return 0;
                return Wow.ReadUInt((uint) BaseAddress + (uint) Common.Offsets.ObjectManager.CurrentManagerPointer);
            }
        }
        public static uint FirstObject => Common.Memory.Wow.ReadUInt(InGameObjectManager + (uint) Common.Offsets.ObjectManager.FirstObject);
        public static bool Closing { get; set; }
        public static ulong LocalGUID
        {
            get
            {
                try
                {
                    return Wow.ReadUInt64((uint)BaseAddress + (uint) Common.Offsets.StaticPointers.LocalPlayerGUID);
                }
                catch
                {
                    return ulong.MinValue;
                }
            }
        }
        public static ulong TargetGUID
        {
            get
            {
                try
                {
                    return Wow.ReadUInt64((uint)BaseAddress + (uint)Common.Offsets.StaticPointers.CurrentTargetGUID);
                }
                catch
                {
                    return ulong.MinValue;
                }
            }
        }
        public static PObject MyPlayer = new PObject(0);
        public static List<PObject> GetObjects
        {
            get
            {
                lock(Locker)
                {
                    return ObjectList.OfType<PObject>().ToList();
                }
            }
        }

        static ObjectManager()
        {
            ObjectList = new List<PObject>();
            ObjectDictionary = new Dictionary<ulong, PObject>();

            Closing = false;
            _refresher = new Thread(BackgroundWorker);
            _refresher.Start();
        }

        /// <summary>
        /// Rafraichit la liste des objets
        /// </summary>
        private static void BackgroundWorker()
        {
            while(!Closing)
            {
                // Remove invalid objects.
                foreach (var o in ObjectDictionary)
                {
                    o.Value.BaseAddress = uint.MinValue;
                }

                // Fill the new list.
                ReadObjectList();

                // Clear out old references.
                List<ulong> toRemove = (from o in ObjectDictionary where !o.Value.IsValid select o.Key).ToList();

                foreach (ulong guid in toRemove)
                {
                    ObjectDictionary.Remove(guid);
                }

                // All done! Just make sure we pass up a valid list to the ObjectList.
                ObjectList = (from o in ObjectDictionary where o.Value.IsValid select o.Value).ToList();

                Thread.Sleep(700);
            }
        }

        /// <summary>
        /// Lit la liste des objets du jeu
        /// </summary>
        private static void ReadObjectList()
        {
            var currentObject = new PObject(FirstObject);
            //var currentObject = FirstObject;
            //var nextObject = FirstObject;

            while(currentObject.BaseAddress != uint.MinValue && currentObject.BaseAddress % 2 == uint.MinValue)
            {
                if(currentObject.GUID == LocalGUID)
                {
                    MyPlayer.BaseAddress = currentObject.BaseAddress;
                }

                if(!ObjectDictionary.ContainsKey(currentObject.GUID))
                {
                    PObject obj = null;
                    // Add the object based on it's *actual* type. Note: WoW's Object descriptors for OBJECT_FIELD_TYPE
                    // is a bitmask. We want to use the type at 0x14, as it's an 'absolute' type.
                    /* switch (currentObject.Type)
                    {
                        // Belive it or not, the base Object class is hardly used in WoW.
                        case (int)Constants.ObjectType.Object:
                            obj = new PObject(currentObject.BaseAddress);
                            break;
                        case (int)Constants.ObjectType.Unit:
                            obj = new PUnit(currentObject.BaseAddress);
                            break;
                        case (int)Constants.ObjectType.Player:
                            obj = new PPlayer(currentObject.BaseAddress);
                            break;
                        case (int)Constants.ObjectType.GameObject:
                            obj = new PGameObject(currentObject.BaseAddress);
                            break;
                        case (int)Constants.ObjectType.Item:
                            obj = new PItem(currentObject.BaseAddress);
                            break;
                        case (int)Constants.ObjectType.Container:
                            obj = new PContainer(currentObject.BaseAddress);
                            break;
                        // These two aren't used in most bots, as they're fairly pointless.
                        // They are AI and area triggers for NPCs handled by the client itself.
                        case (int)Constants.ObjectType.AiGroup:
                        case (int)Constants.ObjectType.AreaTrigger:
                            break;
                    } */
                    if(obj != null)
                    {
                        ObjectDictionary.Add(currentObject.GUID, obj);
                    }

                }
                else
                {
                    ObjectDictionary[currentObject.GUID].BaseAddress = currentObject.BaseAddress;
                }

                currentObject.BaseAddress = Wow.ReadUInt(currentObject.BaseAddress + (uint)Common.Offsets.ObjectManager.NextObject);
            }
        }

        /// <summary>
        /// Gets the object by GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>PObject instance</returns>
        public static PObject GetObjectByGUID(ulong guid)
        {
            lock(Locker)
            {
                return ObjectList.OfType<PObject>().Where(wowObject => wowObject.GUID.Equals(guid)).FirstOrDefault();
            }
        }
    }
}
