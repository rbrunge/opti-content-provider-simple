using System.Globalization;

using EPiServer.Construction;
using Features.MovieCharacters.Models;
using Features.MovieCharacters.Services;

namespace Features.MovieCharacters.Infrastructure;

public class MovieCharacterContentProvider : ContentProvider
{
    private readonly IContentTypeRepository _contentTypeRepository;
    private readonly IContentRepository _contentRepository;
    private readonly IContentFactory _contentFactory;
    private readonly IdentityMappingService _identityMappingService;
    private readonly HarryPotterService _harryPotterService;

    public const string Key = nameof(MovieCharacterContentProvider);
    private List<MovieCharacterBlock> _movieCharacters = new List<MovieCharacterBlock>();

    public MovieCharacterContentProvider(
        IContentTypeRepository contentTypeRepository,
        IContentRepository contentRepository,
        IContentFactory contentFactory,
        IdentityMappingService identityMappingService,
        HarryPotterService harryPotterService)
    {
        _contentTypeRepository = contentTypeRepository;
        _contentRepository = contentRepository;
        _contentFactory = contentFactory;
        _identityMappingService = identityMappingService;
        _harryPotterService = harryPotterService;
    }

    protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
    {
        var mappedIdentity = _identityMappingService.Get(contentLink);

        var id = mappedIdentity.ExternalIdentifier.Segments[1];
        if (string.IsNullOrEmpty(id)) return null;

        var data = _harryPotterService.GetMovieCharacter(id).GetAwaiter().GetResult();
        if (data is null) return null;

        return Map(data) as IContent;
    }

    protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(
        ContentReference contentLink, string languageID, out bool languageSpecific)
    {
        // the attendees are not language specific, so this is ignored.
        languageSpecific = false;

        // get all Person objects
        var movieCharacters = _harryPotterService.GetMovieCharacters().GetAwaiter().GetResult();
        if (movieCharacters is null) return new List<GetChildrenReferenceResult>();

        return movieCharacters.Select(p =>
            new GetChildrenReferenceResult()
            {
                ContentLink =
                    _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, p.Id), true)
                        .ContentLink,
                ModelType = typeof(MovieCharacterBlock)
            }).ToList();
    }

    public MovieCharacterBlock Map(MovieCharacter data)
    {
        var contentType = _contentTypeRepository.Load(typeof(MovieCharacterBlock));

        var character = _contentFactory.CreateContent(contentType, new BuildingContext(contentType)
        {
            // as this is a flat structure, we set the parent to the provider's EntryPoint
            // by setting this in the Buildingcontext, access rights will also be inherited
            Parent = _contentRepository.Get<ContentFolder>(EntryPoint)
        }) as MovieCharacterBlock;

        if (character is null) return null;

        ((IVersionable)character).Status = VersionStatus.Published;
        ((IVersionable)character).IsPendingPublish = false;
        ((IVersionable)character).StartPublish = DateTime.Now.Subtract(TimeSpan.FromDays(14));

        var externalId = MappedIdentity.ConstructExternalIdentifier(ProviderKey, data.Id);
        MappedIdentity mappedContent = _identityMappingService.Get(externalId, true);

        ((ILocalizable)character).Language = CultureInfo.GetCultureInfo("da-DK");
        ((IContent)character).Name = data.Name;
        ((IContent)character).ContentLink = mappedContent.ContentLink;
        ((IContent)character).ContentGuid = mappedContent.ContentGuid;
        ((IChangeTrackable)character).Changed = DateTime.UtcNow;
        ((IChangeTrackable)character).CreatedBy = "";
        ((IChangeTrackable)character).Created = DateTime.UtcNow;
        ((ILocalizable)character).MasterLanguage = CultureInfo.GetCultureInfo("da-DK");
        ((ILocalizable)character).ExistingLanguages = new List<CultureInfo>();

        character.CharacterName = data.Name;
        character.CharacterUrl = _harryPotterService.GetCharacterUrl(data.Id);
        character.DateOfBirth = DateTime.TryParse(data.DateOfBirth, out var dateOfBirth) ? dateOfBirth : null;
        character.CharacterName = data.Name;
        character.AlternateNames = data.AlternateNames;
        character.Species = data.Species;
        character.Gender = data.Gender;
        character.House = data.House;
        character.Ancestry = data.Ancestry;
        character.Actor = data.Actor;

        character.MakeReadOnly();
        return character;
    }
}