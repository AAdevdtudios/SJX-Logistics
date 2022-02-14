namespace SjxLogistics.Models.Responses
{
    public class ServiceResponses<T>
    {
        public int StatusCode { get; set; }
        public string Messages { get; set; }
        public string Tokken { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
