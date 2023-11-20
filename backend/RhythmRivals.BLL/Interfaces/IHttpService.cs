namespace RhythmRivals.BLL.Interfaces;
public interface IHttpService
{
    Task<TResponse?> SendAsync<TRequest, TResponse>(string requestUrl, TRequest? requestData, HttpMethod method, params KeyValuePair<string, string>[] Headers);
    Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage message);
}
