using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Magic;

namespace WoWBot.Common
{
    static class Memory
    {
        /// <summary>
        /// Memory Reading Public Instance
        /// </summary>
        public static BlackMagic Wow { get; } = new BlackMagic();

        /// <summary>
        /// Verifie que BM est attaché à un processus
        /// </summary>
        public static bool IsAttached => Wow.ProcessId > 0;

        /// <summary>
        /// Adresse de base du jeu
        /// </summary>
        public static IntPtr BaseAddress
        {
            get
            {
                if (IsAttached)
                {
                    return Wow.MainModule.BaseAddress;
                }
                return (IntPtr) null;
            }
        }

        /// <summary>
        /// Attache BM à un processus
        /// </summary>
        /// <param name="processId"></param>
        public static void AttachToProcess(int processId)
        {
            Wow.OpenProcessAndThread(processId);
        }
    }
}
