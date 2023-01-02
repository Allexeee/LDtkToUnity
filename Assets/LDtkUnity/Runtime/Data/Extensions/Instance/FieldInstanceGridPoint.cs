﻿using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class FieldInstanceGridPoint
    {
        /// <value>
        /// Grid-based coordinate
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityCoord => new Vector2Int((int)Cx, (int)Cy);
    }
}