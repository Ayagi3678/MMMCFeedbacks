using UnityEditor;
using UnityEngine;

namespace MMMCFeedbacks.Editor
{
    public static class EditorStyling
    {
        public static readonly GUIStyle grayBoldLabel;
        public static readonly GUIStyle smallTickbox;
        public static readonly GUIStyle headerLabel;

        static readonly Color headerBackgroundDark;
        static readonly Color headerBackgroundLight;
        static readonly Color headerBackgroundLineDark;
        static readonly Color headerBackgroundLineLight;
        
        public static Color HeaderBackground => EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight;
        public static Color HeaderBackgroundLine => EditorGUIUtility.isProSkin ? headerBackgroundLineDark : headerBackgroundLineLight;


        static EditorStyling()
        {
            smallTickbox = new GUIStyle("ShurikenToggle");
            headerLabel = new GUIStyle("BoldLabel")
            {
                fontSize = 13
            };
            grayBoldLabel = new GUIStyle("BoldLabel")
            {
                fontSize = 12,
                normal = {textColor = Color.gray}
            };
            headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
            headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);
            headerBackgroundLineDark = new Color(.12f, .12f, .12f);
            headerBackgroundLineLight = new Color(.4f, .4f, .4f);
        }
    }
}