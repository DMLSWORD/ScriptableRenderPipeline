using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEditor.Experimental.Rendering
{
    public static class SerializedBitArrayUrtilities
    {
        public static bool Get8(this SerializedProperty property, uint bitIndex)
            => CheapBitArrayUtilities.Get8(bitIndex, (byte)property.FindPropertyRelative("data").intValue);
        public static bool Get16(this SerializedProperty property, uint bitIndex)
            => CheapBitArrayUtilities.Get16(bitIndex, (ushort)property.FindPropertyRelative("data").intValue);
        public static bool Get32(this SerializedProperty property, uint bitIndex)
            => CheapBitArrayUtilities.Get32(bitIndex, (uint)property.FindPropertyRelative("data").intValue);
        public static bool Get64(this SerializedProperty property, uint bitIndex)
            => CheapBitArrayUtilities.Get64(bitIndex, (ulong)property.FindPropertyRelative("data").longValue);
        public static bool Get128(this SerializedProperty property, uint bitIndex)
            => CheapBitArrayUtilities.Get128(bitIndex, (ulong)property.FindPropertyRelative("data1").intValue, (ulong)property.FindPropertyRelative("data2").intValue);

        public static void Set8(this SerializedProperty property, uint bitIndex, bool value)
        {
            byte versionedData = (byte)property.FindPropertyRelative("data").intValue;
            CheapBitArrayUtilities.Set8(bitIndex, ref versionedData, value);
            property.FindPropertyRelative("data").intValue = versionedData;
        }
        public static void Set16(this SerializedProperty property, uint bitIndex, bool value)
        {
            ushort versionedData = (ushort)property.FindPropertyRelative("data").intValue;
            CheapBitArrayUtilities.Set16(bitIndex, ref versionedData, value);
            property.FindPropertyRelative("data").intValue = versionedData;
        }
        public static void Set32(this SerializedProperty property, uint bitIndex, bool value)
        {
            int versionedData = property.FindPropertyRelative("data").intValue;
            uint trueData;
            unsafe
            {
                trueData = *(uint*)(&versionedData);
            }
            CheapBitArrayUtilities.Set32(bitIndex, ref trueData, value);
            unsafe
            {
                versionedData = *(int*)(&trueData);
            }
            property.FindPropertyRelative("data").intValue = versionedData;
        }
        public static void Set64(this SerializedProperty property, uint bitIndex, bool value)
        {
            long versionedData = property.FindPropertyRelative("data").longValue;
            ulong trueData;
            unsafe
            {
                trueData = *(ulong*)(&versionedData);
            }
            CheapBitArrayUtilities.Set64(bitIndex, ref trueData, value);
            unsafe
            {
                versionedData = *(long*)(&trueData);
            }
            property.FindPropertyRelative("data").longValue = versionedData;
        }
        public static void Set128(this SerializedProperty property, uint bitIndex, bool value)
        {
            long versionedData1 = property.FindPropertyRelative("data1").longValue;
            long versionedData2 = property.FindPropertyRelative("data2").longValue;
            ulong trueData1;
            ulong trueData2;
            unsafe
            {
                trueData1 = *(ulong*)(&versionedData1);
                trueData2 = *(ulong*)(&versionedData2);
            }
            CheapBitArrayUtilities.Set128(bitIndex, ref trueData1, ref trueData2, value);
            unsafe
            {
                versionedData1 = *(long*)(&trueData1);
                versionedData2 = *(long*)(&trueData2);
            }
            property.FindPropertyRelative("data1").longValue = versionedData1;
            property.FindPropertyRelative("data2").longValue = versionedData2;
        }
    }
}
