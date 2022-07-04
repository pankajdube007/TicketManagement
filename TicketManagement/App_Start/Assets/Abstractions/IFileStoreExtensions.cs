using System.Linq;

public static class IFileStoreExtensions
{
    /// <summary>
    /// Combines multiple path parts using the path delimiter semantics of the abstract virtual file store.
    /// </summary>
    /// <param name="paths">The path parts to combine.</param>
    /// <returns>The full combined path.</returns>
    public static string Combine(this IFileStore fileStore, params string[] paths)
    {
        if (paths.Length == 0)
            return null;

        var normalizedParts =
            paths
                .Select(x => fileStore.NormalizePath(x))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

        var combined = string.Join("/", normalizedParts);

        // Preserve the initial '/' if it's present.
        //if (paths[0]?.StartsWith("/") == true)//C# 6.0
        //combined = "/" + combined;
        if (paths[0] != null)
        {
            if (paths[0].StartsWith("/") == true)
            {
                combined = "/" + combined;
            }
        }

        return combined;
    }

    /// <summary>
    /// Normalizes a path using the path delimiter semantics of the abstract virtual file store.
    /// </summary>
    /// <remarks>
    /// Backslash is converted to forward slash and any leading or trailing slashes
    /// are removed.
    /// </remarks>
    public static string NormalizePath(this IFileStore fileStore, string path)
    {
        if (path == null)
            return null;

        return path.Replace('\\', '/').Trim('/');
    }
}