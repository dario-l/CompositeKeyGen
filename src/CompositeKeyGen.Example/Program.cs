using System;
using System.Linq;

namespace CompositeKeyGen.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MaskConfig(instanceBits: 5, tenantBits: 14);

            Console.WriteLine("Max long       : {0}\t{1}", long.MaxValue, ToBinary(long.MaxValue));
            Console.WriteLine("Max sequenceId : {0}\t\t{1}", config.SequenceMask, ToBinary(config.SequenceMask));
            Console.WriteLine("Max int        : {0}\t\t{1}", int.MaxValue, ToBinary(int.MaxValue));
            Console.WriteLine();

            byte instanceId = (byte)config.InstanceMask;
            ushort tenantId = (ushort)config.TenantMask;
            long maxSequenceId = config.SequenceMask;

            var generator = new KeyGenerator(instanceId, config);
            Console.WriteLine(generator.Config);
            Console.WriteLine();
            Console.WriteLine(generator.Config.GetSummary());

            var oryginal = generator.Create(tenantId, maxSequenceId);
            var reconstructed = generator.Reconstruct(oryginal);

            Console.WriteLine();
            Console.WriteLine("Oryginal      : \tInstanceId:{0}\tTenantId:{1}\tId:{2}",
                instanceId, tenantId, maxSequenceId);
            Console.WriteLine("Reconstructed : \tInstanceId:{0}\tTenantId:{1}\tId:{2}",
                reconstructed.InstanceId, reconstructed.TenantId, reconstructed.SequenceId);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static string ToBinary(long value)
        {
            var binaryString = Convert.ToString(value, 2).PadLeft(64, '0');
            var parts = binaryString.Select((c, i) => new { c, i })
                .GroupBy(x => x.i / 8)
                .Select(x => string.Join(string.Empty, x.Select(y => y.c)))
                .ToList();

            return string.Join(" ", parts);
        }
    }
}
