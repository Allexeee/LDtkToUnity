﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /// <summary>
    /// This is a component that stores the field instances for entities/levels, Conveniently converted for use in Unity.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_FIELDS)]
    public partial class LDtkFields : MonoBehaviour
    {
        internal const string PROPERTY_FIELDS = nameof(_fields);
        [SerializeField] private LDtkField[] _fields;
        
        private readonly Dictionary<string, int> _keys = new Dictionary<string, int>();

        private void Awake()
        {
            CacheFields();
        }
        
        private void CacheFields()
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                LDtkField field = _fields[i];
                _keys.Add(field.Identifier, i);
            }
        }

        internal void SetFieldData(LDtkField[] fields)
        {
            _fields = fields;
        }
        
        internal bool TryGetField(string identifier, out LDtkField field)
        {
            if (_keys.ContainsKey(identifier))
            {
                int key = _keys[identifier];
                field = _fields[key];
                return field != null;
            }

            field = _fields.FirstOrDefault(fld => fld.Identifier == identifier);
            return field != null;
        }
        
        internal bool IsFieldOfType(string identifier, LDtkFieldType type)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.Type == type;
        }
        
        internal bool IsFieldArray(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.IsArray;
        }

        private T GetFieldSingle<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                Debug.LogError($"LDtk: No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return default;
            }
            
            LDtkFieldElement element = field.GetSingle();
            return selector.Invoke(element);
        }
        
        private bool TryGetFieldSingle<T>(string identifier, LDtkElementSelector<T> selector, out T value)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = default;
                return false;
            }
            
            LDtkFieldElement element = field.GetSingle();
            value = selector.Invoke(element);
            return true;
        }

        private T[] GetFieldArray<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                Debug.LogError($"LDtk: No array field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return default;
            }
            
            LDtkFieldElement[] elements = field.GetArray();
            return elements.Select(selector.Invoke).ToArray();
        }
        
        private bool TryGetFieldArray<T>(string identifier, LDtkElementSelector<T> selector, out T[] value)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = default;
                return false;
            }
            
            LDtkFieldElement[] elements = field.GetArray();
            value = elements.Select(selector.Invoke).ToArray();
            return true;
        }
    }
}