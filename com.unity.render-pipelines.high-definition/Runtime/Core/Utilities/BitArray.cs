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
        bool this[int index] { get; set; }
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

        public bool this[int index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = (byte)(data | (1u << (int)index));
                else
                    data = (byte)(data & ~(1u << (int)index));
            }
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


        public bool this[int index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = (ushort)(data | (1u << (int)index));
                else
                    data = (ushort)(data & ~(1u << (int)index));
            }
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

        public bool this[int index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = data | (1u << (int)index);
                else
                    data = data & ~(1u << (int)index);
            }
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

        public bool this[int index]
        {
            get => (data & (1uL << (int)index)) != 0uL;
            set
            {
                if (value)
                    data = data | (1uL << (int)index);
                else
                    data = data & ~(1uL << (int)index);
            }
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
        CheapBitArray64 data1;
        [SerializeField]
        CheapBitArray64 data2;

        public uint capacity => 128u;
        public bool allFalse => data1.allFalse && data2.allFalse;
        public bool allTrue => data1.allTrue && data2.allTrue;
        public string humanizedData => data1.humanizedData + "." + data2.humanizedData;

        public bool this[int index]
        {
            get => (index < 64u) ? data1[index] : data2[index - 64];
            set
            {
                if (index < 64u)
                    data1[index] = value;
                else
                    data2[index - 64] = value;
            }
        }

        public CheapBitArray128(ulong initValue1, ulong initValue2)
        {
            data1 = new CheapBitArray64(initValue1);
            data2 = new CheapBitArray64(initValue2);
        }
        public CheapBitArray128(IEnumerable<uint> bitIndexTrue)
        {
            if (bitIndexTrue == null)
            {
                data1 = new CheapBitArray64(0uL);
                data2 = new CheapBitArray64(0uL);
                return;
            }
            var groups = bitIndexTrue.GroupBy(idx => idx < 128u);
            data1 = new CheapBitArray64(groups.First());
            data2 = new CheapBitArray64(groups.Last());
        }
        private CheapBitArray128(CheapBitArray64 initValue1, CheapBitArray64 initValue2)
        {
            data1 = initValue1;
            data2 = initValue2;
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
}
