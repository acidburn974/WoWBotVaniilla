using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WoWBot.Engines
{
    /// <summary>
    /// Gère la pêche en jeu
    /// </summary>
    static class FishingEngine
    {
        /// <summary>
        /// Actif lorsque le moteur de Fishing est activé
        /// </summary>
        private static bool _isFishing = false;

        /// <summary>
        /// Demarre le thread pour lancé la pêche
        /// </summary>
        internal static Thread _fishingThread = new Thread(Work);

        /// <summary>
        /// Static Constructor
        /// </summary>
        static FishingEngine()
        {
        }

        /// <summary>
        /// Execute la fonction pour pêcher
        /// </summary>
        private static void Work()
        {
            
        }

        /// <summary>
        /// Démarre / arrête le moteur de Fishing
        /// </summary>
        public static void ToggleFish()
        {
            if (_isFishing)
            {
                _fishingThread.Join();
            }
            else
            {
                _fishingThread.Start();
            }
        }


    }
}
