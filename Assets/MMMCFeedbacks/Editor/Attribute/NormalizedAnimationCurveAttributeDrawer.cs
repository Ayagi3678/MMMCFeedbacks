using MMMCFeedbacks.Core;

namespace MMMCFeedbacks.Editor
{using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(NormalizedAnimationCurveAttribute))]
public class NormalizedAnimationCurveAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (NormalizedAnimationCurveAttribute) attribute;

        if (property.propertyType != SerializedPropertyType.AnimationCurve)
        {
            // AnimationCurve以外のフィールドにアトリビュートが付けられていた場合のエラー表示
            position = EditorGUI.PrefixLabel(position, label);
            var preIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(position, "Use NormalizedAnimationCurveAttribute with AnimationCurve.");
            EditorGUI.indentLevel = preIndent;
            return;
        }

        using (var ccs = new EditorGUI.ChangeCheckScope())
        {
            EditorGUI.PropertyField(position, property, label, true);
            if (ccs.changed)
            {
                if (attr.NormalizeValue)
                {
                    property.animationCurveValue = NormalizeValue(property.animationCurveValue);
                }

                if (attr.NormalizeTime)
                {
                    property.animationCurveValue = NormalizeTime(property.animationCurveValue);
                }
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    // アニメーションカーブの値（縦軸）を正規化する
    private static AnimationCurve NormalizeValue(AnimationCurve curve)
    {
        var keys = curve.keys;
        
        for (var i = 0; i < keys.Length; ++i)
        {
            keys[i].value = Mathf.Clamp01(keys[i].value);
        }

        curve.keys = keys;
        return curve;
    }

    // アニメーションカーブの時間（横軸）を正規化する
    private static AnimationCurve NormalizeTime(AnimationCurve curve)
    {
        var keys = curve.keys;
        for (var i = 0; i < keys.Length; ++i)
        {
            keys[i].time = Mathf.Clamp01(keys[i].time);
        }

        curve.keys = keys;
        return curve;
    }
}
#endif
}