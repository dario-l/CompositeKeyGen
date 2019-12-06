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

            const byte instanceId = 1;
            const ushort tenantId = 10;
            const long sequenceId = 100;

            var generator = new KeyGenerator(instanceId, config);
            Console.WriteLine(generator.Config);
            Console.WriteLine(generator.Config.GetSummary());

            var oryginal = generator.Create(tenantId, sequenceId);
            CompositeKey reconstructed = generator.Reconstruct(oryginal);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("For given values       : InstanceId:{0}\tTenantId:{1}\tSequenceId:{2}",
                instanceId, tenantId, sequenceId);
            Console.WriteLine("Created key will be    : {0}", oryginal);
            Console.WriteLine("Reconstructed from key : InstanceId:{0}\tTenantId:{1}\tSequenceId:{2}",
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
