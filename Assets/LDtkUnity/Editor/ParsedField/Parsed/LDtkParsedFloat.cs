﻿using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedFloat : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;

        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFloat;

        public object ImportString(object input)
        {
            if (_process == null)
            {
                Debug.LogError("LDtk: Didn't process a field value, field data may be missing");
                return 0f;
            }
            
            //floats can be legally null
            if (input == null)
            {
                return 0f;
            }

            float value = Convert.ToSingle(input);
            value = _process.Postprocess(value);
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;//todo
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}