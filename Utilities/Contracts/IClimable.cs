using UnityEngine;

namespace Assets.Scripts.Utilities.Contracts
{
    public interface IClimable
    {
        /// <summary>
        /// Point where the player should hang during the mantle.
        /// </summary>
        Vector3 GetHangPoint();

        /// <summary>
        /// Point where the player ends up standing after climbing.
        /// </summary>
        Vector3 GetStandPoint();
    }
}
