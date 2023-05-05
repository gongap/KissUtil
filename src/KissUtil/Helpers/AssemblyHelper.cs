using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace KissUtil.Helpers;

public static class AssemblyHelper
{
    /// <summary>
    /// Loads the assemblies.
    /// </summary>
    /// <param name="folderPath">The folder path.</param>
    /// <param name="searchOption">The search option.</param>
    /// <param name="relatedFile">The related file.</param>
    /// <param name="notRelatedFile">The not related file.</param>
    /// <returns></returns>
    public static List<Assembly> LoadAssemblies(string folderPath, SearchOption searchOption, string relatedFile = null, string notRelatedFile = null)
    {
        var result = new List<Assembly>();
        var assemblyFiles = GetAssemblyFiles(folderPath, searchOption, relatedFile, notRelatedFile).Distinct();
        foreach (var assemblyFile in assemblyFiles)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFile);
                if (!result.Contains(assembly))
                {
                    result.Add(assembly);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return result;
    }

    /// <summary>
    /// 获取过滤器程序集
    /// </summary>
    /// <param name="assemblyNames">The assembly names.</param>
    /// <param name="relatedFile">The related file.</param>
    /// <param name="notRelatedFile">The not related file.</param>
    /// <returns></returns>
    private static IEnumerable<string> GetFilterAssemblies(string[] assemblyNames, string relatedFile = null, string notRelatedFile = null)
    {
        var pattern =$"^Microsoft.\\w*|^System.\\w*|^Autofac.\\w*|^runtime.\\w*|^Newtonsoft.Json.\\w*{(string.IsNullOrEmpty(notRelatedFile) ? string.Empty : $"|{notRelatedFile}")}";
        var notRelatedRegex = new Regex(pattern,RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var relatedRegex = new Regex(relatedFile, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (!string.IsNullOrEmpty(relatedFile))
        {
            return assemblyNames.Where( name => !notRelatedRegex.IsMatch(name) && relatedRegex.IsMatch(name));
        }

        return assemblyNames.Where( name => !notRelatedRegex.IsMatch(name));
    }

    /// <summary>
    /// 获取所有程序集
    /// </summary>
    /// <param name="folderPath">The folder path.</param>
    /// <param name="searchOption">The search option.</param>
    /// <param name="relatedFile">The related file.</param>
    /// <param name="notRelatedFile">The not related file.</param>
    /// <returns></returns>
    public static IEnumerable<string> GetAssemblyFiles(string folderPath, SearchOption searchOption, string relatedFile = null, string notRelatedFile = null)
    {
        var pattern =$"^Microsoft.\\w*|^System.\\w*|^Autofac.\\w*{(string.IsNullOrEmpty(notRelatedFile) ? string.Empty : $"|{notRelatedFile}")}";
        var notRelatedRegex = new Regex(pattern,RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (!string.IsNullOrEmpty(relatedFile))
        {
            var relatedRegex = new Regex(relatedFile, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Directory.EnumerateFiles(folderPath, "*.dll", searchOption).Select(Path.GetFullPath) .Where(a => !notRelatedRegex.IsMatch(a) && relatedRegex.IsMatch(a));
        }

        return Directory.EnumerateFiles(folderPath, "*.dll", searchOption).Select(Path.GetFullPath).Where(a => !notRelatedRegex.IsMatch(Path.GetFileName(a)));
    }

    /// <summary>
    /// Gets all types.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    public static IReadOnlyList<Type> GetAllTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types;
        }
    }
}
