using System.Net;
using System.Text;

public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly string _customJsonResponse;

    private readonly HttpStatusCode _statusCode;

    public StubHttpMessageHandler(string customJsonResponse, HttpStatusCode statusCode)
    {
        _customJsonResponse = customJsonResponse;
        _statusCode = statusCode;

    }

    //override the transport layer so we never send data over TCP, we just intercept the "http send" to return our custom json response
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = new StringContent(
                _customJsonResponse,
                Encoding.UTF8,
                //set content-type header
                "application/json"
            )
        };

        return httpResponse;

    }
}