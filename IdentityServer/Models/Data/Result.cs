namespace IdentityServer.Models.Data
{
    public class Result<T>
    {
        public Result(T Data)
        {
            this.Data = Data;
        }

        public Result(bool Success, List<string> Messages)
        {
            this.Success = Success;
            this.Messages = Messages;
        }

        public Result(bool Success, T Data, List<string> Messages)
        {
            this.Success = Success;
            this.Data = Data;
            this.Messages = Messages;
        }

        public Result(T Data, List<string> Messages)
        {
            this.Data = Data;
            this.Messages = Messages;
        }

        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public List<string>? Messages { get; set; }
    }
}
