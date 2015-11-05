using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Any object that the player can look at and press a key to interact with is an activator.
    /// </summary>
    public interface Activator
    {
        void OnActivate(Player player);
    }
}
