# allocations
Simple test of C# allocations vs enumeration through already allocated list (CPU bound operation)

Actual test can be found [here](https://github.com/DovydasNavickas/allocations/blob/master/src/Allocations/Startup.cs#L32-L78);

And results I'm seeing are:
```

No allocations. Loop through objects with constant time async action
580066 ticks (238 ms)

Allocate list via Linq. Select tasks of constant time async action and await WhenAll
5069842 ticks (2081 ms)

Allocate list manually. Add tasks to list manually and await WhenAll
4384613 ticks (1800 ms)

```

You should be able to see similar ones yourself.