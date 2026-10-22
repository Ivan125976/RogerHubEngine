using System.Reflection;

namespace RogerHubEngine.Desktop.Resources
{
    internal class Resource
    {
        private static string? _resourcePath;

        public static string? ResourcePath
        {
            get => _resourcePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                _resourcePath = value.TrimEnd('.');
            }
        }

        public static readonly Assembly resource = typeof(Resource).Assembly;

        public static string GetTextResource(string resourceName)
        {
            using Stream stream = resource.GetManifestResourceStream(_resourcePath + '.' + resourceName) ?? throw new FileNotFoundException($"Resource {resourceName} not found!");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
