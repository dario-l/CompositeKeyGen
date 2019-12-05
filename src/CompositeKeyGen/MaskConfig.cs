using System;
using System.Runtime.CompilerServices;

namespace CompositeKeyGen
{
    public class MaskConfig
    {
        public const byte MaxBits = 63;

        public byte InstanceBits { get; }
        public byte TenantBits { get; }
        public byte SequenceBits { get; }

        public long SequenceMask { get; }
        public long TenantMask { get; }
        public long InstanceMask { get; }

        public MaskConfig(byte instanceBits = 5, byte tenantBits = 16)
        {
            InstanceBits = instanceBits;
            TenantBits = tenantBits;
            var sBits = MaxBits - instanceBits - tenantBits;
            if (sBits < 0) throw new Exception($"No bits left for sequence. Maximum bits to use is {MaxBits}.");
            SequenceBits = (byte)sBits;

            InstanceMask = GetMask(InstanceBits);
            TenantMask = GetMask(TenantBits);
            SequenceMask = GetMask(SequenceBits);
        }

        public static MaskConfig Default { get; } = new MaskConfig();

        public override string ToString() => $"Config: {MaxBits + 1} bits => | 1 | {InstanceBits} for instanceId | {TenantBits} for tenantId | {SequenceBits} for sequenceId |";

        public string GetSummary()
        {
            return
                $"Max values:\r\n\tInstanceId:\t{InstanceMask}\r\n\tTenantId:\t{TenantMask}\r\n\tSequenceId:\t{SequenceMask}\r\n\r\n\tSequenceId diff to int.MaxValue: {(SequenceMask / int.MaxValue):0.0}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetMask(byte bits) => (1L << bits) - 1;
    }
}