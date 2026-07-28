using Microsoft.Iris.Data;
using Microsoft.Iris.Render.Text;

namespace Microsoft.Zune;

public abstract class MemoryFonts
{
    private static readonly MemoryFonts Impl =
#if WINDOWS
        new Win32MemoryFonts();
#else
        new SixLaborsMemoryFonts();
#endif
    
    public abstract bool TryLoadFromResourceCore(string resourceDllName, string fontResourceName);

    public static bool TryLoadFromResource(string resourceDllName, string fontResourceName) =>
        Impl.TryLoadFromResourceCore(resourceDllName, fontResourceName);
}

internal class SixLaborsMemoryFonts : MemoryFonts
{
    public override bool TryLoadFromResourceCore(string resourceDllName, string fontResourceName)
    {
        var resourceAssemblyName = resourceDllName[..^".dll".Length];
        var loaded = FontResourceLoader.LoadFromModuleResource(resourceAssemblyName, fontResourceName);

        if (loaded)
            return true;
        
        var resource = ResourceManager.AcquireResource($"res://{resourceAssemblyName}!{fontResourceName}");
        loaded = FontResourceLoader.LoadFromBuffer(resource.Buffer, (int)resource.Length);

        return loaded;
    }
}