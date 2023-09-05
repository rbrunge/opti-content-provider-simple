using System.ComponentModel.DataAnnotations;

namespace Features.MovieCharacters.Models;

[ContentType(
    DisplayName = "Movie Character",
    Description = "",
    GUID = "bfe49fb0-b3f3-454a-9cf5-e10cdddfbf5a",
    AvailableInEditMode = false)]
[AvailableContentTypes(Availability.None, Include = new Type[] { })]
public class MovieCharacterBlock : BlockData
{
    [Editable(false)]
    public virtual string CharacterName { get; set; } = null!;

    [Editable(false)]
    public virtual Url CharacterUrl { get; set; } = null!;

    [Editable(false)]
    public virtual DateTime? DateOfBirth { get; set; }

    [Editable(false)]
    public virtual IList<string> AlternateNames { get; set; }

    [Editable(false)]
    public virtual string Species { get; set; }

    [Editable(false)]
    public virtual string Gender { get; set; }

    [Editable(false)]
    public virtual string House { get; set; }

    [Editable(false)]
    public virtual string Ancestry { get; set; }

    [Editable(false)]
    public virtual string Actor { get; set; }
}