using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Experimental.Rendering
{
    public interface ICheapBitArray
    {
        uint capacity { get; }
        bool allFalse { get; }
        bool allTrue { get; }
        bool this[uint index] { get; set; }
        string humanizedData { get; }
    }

    [Serializable]
    public struct CheapBitArray8 : ICheapBitArray
    {
        [SerializeField]
        byte data;

        public uint capacity => 8u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == byte.MaxValue;
        public string humanizedData => String.Format("%" + capacity + "s", Convert.ToString(data, 2).Replace(' ', '0'));

        public bool this[uint index]
        {
            get => CheapBitArrayUtilities.Get8(index, data);
            set => CheapBitArrayUtilities.Set8(index, ref data, value);
        }

        public CheapBitArray8(byte initValue) => data = initValue;
        public CheapBitArray8(IEnumerable<uint> bitIndexTrue)
        {
            data = (byte)0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += (byte)(1u << (int)bitIndexTrue.ElementAt(index));
        }

        public static CheapBitArray8 operator ~(CheapBitArray8 a) => new CheapBitArray8((byte)~a.data);
        public static CheapBitArray8 operator |(CheapBitArray8 a, CheapBitArray8 b) => new CheapBitArray8((byte)(a.data | b.data));
        public static CheapBitArray8 operator &(CheapBitArray8 a, CheapBitArray8 b) => new CheapBitArray8((byte)(a.data & b.data));

        public static bool operator ==(CheapBitArray8 a, CheapBitArray8 b) => a.data == b.data;
        public static bool operator !=(CheapBitArray8 a, CheapBitArray8 b) => a.data != b.data;
        public override bool Equals(object obj) => obj is CheapBitArray8 && ((CheapBitArray8)obj).data == data;
        public override int GetHashCode() => 1768953197 + data.GetHashCode();
    }

    [Serializable]
    public struct CheapBitArray16 : ICheapBitArray
    {
        [SerializeField]
        ushort data;

        public uint capacity => 16u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == ushort.MaxValue;
        public string humanizedData => System.Text.RegularExpressions.Regex.Replace(String.Format("%" + capacity + "s", Convert.ToString(data, 2).Replace(' ', '0')), ".{8}", "$0.");


        public bool this[uint index]
        {
            get => CheapBitArrayUtilities.Get16(index, data);
            set => CheapBitArrayUtilities.Set16(index, ref data, value);
        }

        public CheapBitArray16(ushort initValue) => data = initValue;
        public CheapBitArray16(IEnumerable<uint> bitIndexTrue)
        {
            data = (ushort)0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += (ushort)(1u << (int)bitIndexTrue.ElementAt(index));
        }

        public static CheapBitArray16 operator ~(CheapBitArray16 a) => new CheapBitArray16((ushort)~a.data);
        public static CheapBitArray16 operator |(CheapBitArray16 a, CheapBitArray16 b) => new CheapBitArray16((ushort)(a.data | b.data));
        public static CheapBitArray16 operator &(CheapBitArray16 a, CheapBitArray16 b) => new CheapBitArray16((ushort)(a.data & b.data));

        public static bool operator ==(CheapBitArray16 a, CheapBitArray16 b) => a.data == b.data;
        public static bool operator !=(CheapBitArray16 a, CheapBitArray16 b) => a.data != b.data;
        public override bool Equals(object obj) => obj is CheapBitArray16 && ((CheapBitArray16)obj).data == data;
        public override int GetHashCode() => 1768953197 + data.GetHashCode();
    }

    [Serializable]
    public struct CheapBitArray32 : ICheapBitArray
    {
        [SerializeField]
        uint data;

        public uint capacity => 32u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == uint.MaxValue;
        string humanizedVersion => Convert.ToString(data, 2);
        public string humanizedData => System.Text.RegularExpressions.Regex.Replace(String.Format("%" + capacity + "s", Convert.ToString(data, 2).Replace(' ', '0')), ".{8}", "$0.");

        public bool this[uint index]
        {
            get => CheapBitArrayUtilities.Get32(index, data);
            set => CheapBitArrayUtilities.Set32(index, ref data, value);
        }

        public CheapBitArray32(uint initValue) => data = initValue;
        public CheapBitArray32(IEnumerable<uint> bitIndexTrue)
        {
            data = 0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += 1u << (int)bitIndexTrue.ElementAt(index);
        }

        public static CheapBitArray32 operator ~(CheapBitArray32 a) => new CheapBitArray32(~a.data);
        public static CheapBitArray32 operator |(CheapBitArray32 a, CheapBitArray32 b) => new CheapBitArray32(a.data | b.data);
        public static CheapBitArray32 operator &(CheapBitArray32 a, CheapBitArray32 b) => new CheapBitArray32(a.data & b.data);

        public static bool operator ==(CheapBitArray32 a, CheapBitArray32 b) => a.data == b.data;
        public static bool operator !=(CheapBitArray32 a, CheapBitArray32 b) => a.data != b.data;
        public override bool Equals(object obj) => obj is CheapBitArray32 && ((CheapBitArray32)obj).data == data;
        public override int GetHashCode() => 1768953197 + data.GetHashCode();
    }

    [Serializable]
    public struct CheapBitArray64 : ICheapBitArray
    {
        [SerializeField]
        ulong data;

        public uint capacity => 64u;
        public bool allFalse => data == 0uL;
        public bool allTrue => data == ulong.MaxValue;
        public string humanizedData => System.Text.RegularExpressions.Regex.Replace(String.Format("%" + capacity + "s", Convert.ToString((long)data, 2).Replace(' ', '0')), ".{8}", "$0.");

        public bool this[uint index]
        {
            get => CheapBitArrayUtilities.Get64(index, data);
            set => CheapBitArrayUtilities.Set64(index, ref data, value);
        }

        public CheapBitArray64(ulong initValue) => data = initValue;
        public CheapBitArray64(IEnumerable<uint> bitIndexTrue)
        {
            data = 0L;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += 1uL << (int)bitIndexTrue.ElementAt(index);
        }


        public static CheapBitArray64 operator ~(CheapBitArray64 a) => new CheapBitArray64(~a.data);
        public static CheapBitArray64 operator |(CheapBitArray64 a, CheapBitArray64 b) => new CheapBitArray64(a.data | b.data);
        public static CheapBitArray64 operator &(CheapBitArray64 a, CheapBitArray64 b) => new CheapBitArray64(a.data & b.data);

        public static bool operator ==(CheapBitArray64 a, CheapBitArray64 b) => a.data == b.data;
        public static bool operator !=(CheapBitArray64 a, CheapBitArray64 b) => a.data != b.data;
        public override bool Equals(object obj) => obj is CheapBitArray64 && ((CheapBitArray64)obj).data == data;
        public override int GetHashCode() => 1768953197 + data.GetHashCode();
    }

    [Serializable]
    public struct CheapBitArray128 : ICheapBitArray
    {
        [SerializeField]
        ulong data1;
        [SerializeField]
        ulong data2;

        public uint capacity => 128u;
        public bool allFalse => data1 == 0uL && data2 == 0uL;
        public bool allTrue => data1 == ulong.MaxValue && data2 == ulong.MaxValue;
        public string humanizedData =>
            System.Text.RegularExpressions.Regex.Replace(String.Format("%" + capacity + "s", Convert.ToString((long)data1, 2).Replace(' ', '0')), ".{8}", "$0.")
            + "." +
            System.Text.RegularExpressions.Regex.Replace(String.Format("%" + capacity + "s", Convert.ToString((long)data2, 2).Replace(' ', '0')), ".{8}", "$0.");

        public bool this[uint index]
        {
            get => CheapBitArrayUtilities.Get128(index, data1, data2);
            set => CheapBitArrayUtilities.Set128(index, ref data1, ref data2, value);
        }

        public CheapBitArray128(ulong initValue1, ulong initValue2)
        {
            data1 = initValue1;
            data2 = initValue2;
        }
        public CheapBitArray128(IEnumerable<uint> bitIndexTrue)
        {
            data1 = data2 = 0uL;
            if (bitIndexTrue == null)
                return;
            var groups = bitIndexTrue.GroupBy(idx => idx < 128u);
            for (int index = groups.First().Count() - 1; index >= 0; --index)
                data1 += 1uL << (int)bitIndexTrue.ElementAt(index);
            for (int index = groups.Last().Count() - 1; index >= 0; --index)
                data2 += 1uL << (int)bitIndexTrue.ElementAt(index);
        }

        public static CheapBitArray128 operator ~(CheapBitArray128 a) => new CheapBitArray128(~a.data1, a.data2);
        public static CheapBitArray128 operator |(CheapBitArray128 a, CheapBitArray128 b) => new CheapBitArray128(a.data1 | b.data1, a.data2 | b.data2);
        public static CheapBitArray128 operator &(CheapBitArray128 a, CheapBitArray128 b) => new CheapBitArray128(a.data1 & b.data1, a.data2 & b.data2);

        public static bool operator ==(CheapBitArray128 a, CheapBitArray128 b) => a.data1 == b.data1 && a.data2 == b.data2;
        public static bool operator !=(CheapBitArray128 a, CheapBitArray128 b) => a.data1 != b.data1 && a.data2 != b.data2;
        public override bool Equals(object obj) => (obj is CheapBitArray128) && data1.Equals(((CheapBitArray128)obj).data1) && data2.Equals(((CheapBitArray128)obj).data2);
        public override int GetHashCode()
        {
            var hashCode = 1755735569;
            hashCode = hashCode * -1521134295 + data1.GetHashCode();
            hashCode = hashCode * -1521134295 + data2.GetHashCode();
            return hashCode;
        }
    }




    public static class CheapBitArrayUtilities
    {
        //written here to not duplicate the serialized accessor and runtime accessor
        public static bool Get8(uint index, byte data) => (data & (1u << (int)index)) != 0u;
        public static bool Get16(uint index, ushort data) => (data & (1u << (int)index)) != 0u;
        public static bool Get32(uint index, uint data) => (data & (1u << (int)index)) != 0u;
        public static bool Get64(uint index, ulong data) => (data & (1uL << (int)index)) != 0uL;
        public static bool Get128(uint index, ulong data1, ulong data2) => index < 64u ? (data1 & (1uL << (int)index)) != 0uL : (data2 & (1uL << (int)index)) != 0uL;
        public static void Set8(uint index, ref byte data, bool value) => data = (byte)(value ? (data | (1u << (int)index)) : (data & ~(1u << (int)index)));
        public static void Set16(uint index, ref ushort data, bool value) => data = (ushort)(value ? (data | (1u << (int)index)) : (data & ~(1u << (int)index)));
        public static void Set32(uint index, ref uint data, bool value) => data = (value ? (data | (1u << (int)index)) : (data & ~(1u << (int)index)));
        public static void Set64(uint index, ref ulong data, bool value) => data = (value ? (data | (1uL << (int)index)) : (data & ~(1uL << (int)index)));
        public static void Set128(uint index, ref ulong data1, ref ulong data2, bool value)
        {
            if (index < 64u)
                data1 = (value ? (data1 | (1uL << (int)index)) : (data1 & ~(1uL << (int)index)));
            else
                data2 = (value ? (data2 | (1uL << (int)(index - 64u))) : (data2 & ~(1uL << (int)(index - 64u))));
        }
    }
}
