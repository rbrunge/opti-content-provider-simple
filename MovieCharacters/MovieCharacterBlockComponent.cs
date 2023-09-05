using EPiServer.Web.Mvc;
using Features.MovieCharacters.Models;
using Microsoft.AspNetCore.Mvc;

namespace Features.MovieCharacters;

public class MovieCharacterBlockComponent : AsyncBlockComponent<MovieCharacterBlock>
{
    protected override async Task<IViewComponentResult> InvokeComponentAsync(MovieCharacterBlock currentContent)
    {
        return await Task.FromResult(View("~/Features/MovieCharacters/MovieCharacterBlock.cshtml", currentContent));
    }
}