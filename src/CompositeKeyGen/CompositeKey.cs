namespace CompositeKeyGen
{
    public struct CompositeKey
    {
        public int InstanceId { get; }
        public int TenantId { get; }
        public long SequenceId { get; }

        private CompositeKey(int instanceId, int tenantId, long sequenceId)
        {
            InstanceId = instanceId;
            TenantId = tenantId;
            SequenceId = sequenceId;
        }

        internal static CompositeKey Create(int instanceId, int tenantId, long sequenceId)
            => new CompositeKey(instanceId, tenantId, sequenceId);
    }
}