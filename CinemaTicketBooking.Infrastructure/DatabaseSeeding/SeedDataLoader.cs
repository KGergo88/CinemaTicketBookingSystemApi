using System.Text.Json;
using HandlebarsDotNet;

namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

internal class SeedDataLoader
{
    public static SeedData LoadFromJson(string path)
    {
        if (!Path.Exists(path))
            throw new ArgumentException($"The path \"{path}\" does not exist!");

        var jsonContent = LoadJsonContent(path);

        var seedData = JsonSerializer.Deserialize<SeedData>(jsonContent);
        if (seedData is null)
            throw new ArgumentException($"Could not serialize JSON file: {path}");

        return seedData;
    }

    private static string LoadJsonContent(string path)
    {
        var fileContent = File.ReadAllText(path);

        if (path.EndsWith(".json"))
            return fileContent;

        if (path.EndsWith(".hbs") || path.EndsWith(".handlebars"))
            return RenderJsonTemplate(fileContent);

        throw new ArgumentException($"The path \"{path}\" shall point to a valid JSON file ending with .json" +
                                    $" or a Handlebars template ending with .hbs or .handlebars!");
    }

    private static string RenderJsonTemplate(string jsonTemplate)
    {
        Handlebars.RegisterHelper("GuidNew", (writer, context, parameters) =>
        {
            var guid = SeedDataTemplateHelpers.GuidNew();
            writer.WriteSafeString(guid);
        });

        Handlebars.RegisterHelper("GuidNamed", (writer, context, parameters) =>
        {
            if (parameters.Length != 1)
                throw new ArgumentException("GuidNamed helper requires a single parameter: \"name\"");

            var guidName = parameters[0].ToString();
            if (string.IsNullOrWhiteSpace(guidName))
                throw new ArgumentException("GuidNamed helper requires a non-empty \"name\" parameter.");

            var namedGuid = SeedDataTemplateHelpers.GuidNamed(guidName);
            writer.WriteSafeString(namedGuid);
        });

        var compiledTemplate = Handlebars.Compile(jsonTemplate);
        var renderedTemplate = compiledTemplate(null);
        return renderedTemplate;
    }

    private class SeedDataTemplateHelpers
    {
        public static Dictionary<string, Guid> NamedGuids = new();

        public static string GuidNew()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GuidNamed(string guidName)
        {
            if (!NamedGuids.ContainsKey(guidName))
                NamedGuids.Add(guidName, Guid.NewGuid());

            return NamedGuids[guidName].ToString();
        }
    }
}
