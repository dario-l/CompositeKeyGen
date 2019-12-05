using System;

namespace CompositeKeyGen
{
    public class KeyGenerator
    {
        private readonly long _instanceId;
        private readonly byte _shiftForInstanceId;
        private readonly byte _shiftForTenantId;

        public KeyGenerator(int instanceId, MaskConfig cfg = null)
        {
            Config = cfg ?? MaskConfig.Default;

            if (instanceId > Config.InstanceMask)
                throw new ArgumentOutOfRangeException(
                    $"InstanceId overflow. Must be between 0 and {Config.InstanceMask} (inclusive).");

            _instanceId = instanceId;
            _shiftForInstanceId = (byte)(MaskConfig.MaxBits - Config.InstanceBits);
            _shiftForTenantId = (byte)(_shiftForInstanceId - Config.TenantBits);
        }

        public MaskConfig Config { get; }

        public long Create(int tenantId, long sequenceId)
        {
            if (tenantId > Config.TenantMask)
                throw new ArgumentOutOfRangeException(nameof(tenantId),
                    $"TenantId overflow. Must be between 0 and {Config.TenantMask} (inclusive).");

            if (sequenceId > Config.SequenceMask)
                throw new ArgumentOutOfRangeException(nameof(sequenceId),
                    $"SequenceId overflow. Must be between 0 and {Config.SequenceMask} (inclusive).");

            unchecked
            {
                return (_instanceId << _shiftForInstanceId)
                    + ((long)tenantId << _shiftForTenantId)
                    + (sequenceId);
            }
        }

        public CompositeKey Reconstruct(long id)
        {
            var instanceId = (int)((id >> _shiftForInstanceId) & Config.InstanceMask);
            var tenantId = (int)((id >> _shiftForTenantId) & Config.TenantMask);
            var sequenceId = id & Config.SequenceMask;

            return CompositeKey.Create(instanceId, tenantId, sequenceId);
        }
    }
}