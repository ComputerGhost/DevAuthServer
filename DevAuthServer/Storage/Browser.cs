namespace DevAuthServer.Storage;

public class Browser
{
    private HttpRequest _request;
    private HttpResponse _response;

    public Browser(HttpRequest request, HttpResponse response)
    {
        _request = request;
        _response = response;
    }

    public string? UserId
    {
        get
        {
            return _request.Cookies["user-id"];
        }
        set
        {
            if (value == null)
            {
                _response.Cookies.Delete("user-id");
            }
            else
            {
                _response.Cookies.Append("user-id", value);
            }
        }
    }
}
