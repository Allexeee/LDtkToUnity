﻿using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class FieldInstanceGridPoint
    {
        /// <value>
        /// Grid-based coordinate
        /// </value>
        [JsonIgnore] public Vector2Int UnityCoord => new Vector2Int((int)Cx, (int)Cy);
    }
}