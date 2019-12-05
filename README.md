# CompositeKeyGen
Allows to create composite key encoded as Int64 (long).

*Usually you should use these identifiers in domain objects (aggregates).*

Each key is composed from 3 elements:
- instanceId - could be server identifier
- tenantId - could be tenant identifier
- sequenceId - incrementing identifier from other source

## Usage
This should be done once at application startup:
``` c#
// Define configuration
var config = new MaskConfig(instanceBits: 5, tenantBits: 14);

// Create key generator
var serverId = _application.Configuration.GetServerId(); // this is your code :)
var generator = new KeyGenerator(serverId, config);
```

Configuration defined above will give you the following maximum values:
```
   Max InstanceId:     31
   Max TenantId:       16383
   Max SequenceId:     17592186044415
```

When you need to create key:
``` c#
// Create key providing tenantId and sequenceId
var nextSequenceId = _hiloGenerator.GetNext(); // this is your code :)
var id = generator.Create(tenantId, nextSequenceId);
```

When you need to verify key for given tenantId:
``` c#
var reconstructed = generator.Reconstruct(id);
if (reconstructed.TenantId != _userContext.TenantId) throw...
```

