namespace MVC.Service
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }
    }
}