namespace emmad.Interface
{
    public interface IEmail
    {
        public void Send(string to, string subject, string html, string from = null);
    }
}
