using EPiServer.Shell;
using Features.MovieCharacters.Models;

namespace Features.MovieCharacters;

[UIDescriptorRegistration]
public class MovieCharacterUIDescriptor : UIDescriptor<MovieCharacterBlock>
{
    public MovieCharacterUIDescriptor() : base("icon-document")
    {
        DefaultView = CmsViewNames.AllPropertiesView;
        DisabledViews = new List<string>
        {
            CmsViewNames.OnPageEditView,
            CmsViewNames.PreviewView
        };
    }
}