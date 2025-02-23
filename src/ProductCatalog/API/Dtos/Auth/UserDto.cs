namespace ProductCatalog.API.Auth.Dtos;

public record UserDto
{
    public string DisplayName { get; set; }
    public string Token { get; set; }
    public string Image { get; set; }
    public string Username { get; set; }
}
