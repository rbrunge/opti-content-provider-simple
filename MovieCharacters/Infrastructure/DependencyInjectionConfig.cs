using EPiServer.Core.Internal;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Features.MovieCharacters.Services;

namespace Features.MovieCharacters.Infrastructure;

public static class MovieCharacterContentProviderExtensions
{
    public static void AddMovieCharaterContentProvider(this IServiceCollection services)
    {
        services.AddTransient<HarryPotterService>();
        services.AddScoped<MovieCharacterContentProvider>();
        services.AddHttpClient<HarryPotterService>(client =>
        {
            client.BaseAddress = new Uri(HarryPotterService.ApiEndpoint);
        });
        services.Configure<ContentOptions>(o => o.AddProvider<MovieCharacterContentProvider>(nameof(MovieCharacterContentProvider),
            config =>
        {
            var contentRepository = services.BuildServiceProvider().GetRequiredService<IContentRepository>();
            var entrypoint = GetEntryPoint(contentRepository!, "MovieCharacters").ContentLink;
            config[ContentProviderParameter.EntryPoint] = entrypoint!.ToString();
            config[ContentProviderParameter.Capabilities] = "None";
        }));
    }

    private static ContentFolder GetEntryPoint(IContentRepository contentRepository, string name)
    {
        var folder = contentRepository.GetBySegment(ContentReference.GlobalBlockFolder, name, LanguageSelector.AutoDetect()) as ContentFolder;
        if (folder == null)
        {
            folder = contentRepository.GetDefault<ContentFolder>(ContentReference.GlobalBlockFolder);
            folder.Name = name;
            contentRepository.Save(folder, SaveAction.Publish, AccessLevel.NoAccess);
        }
        return folder;
    }
}