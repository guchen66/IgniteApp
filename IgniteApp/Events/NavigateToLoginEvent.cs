namespace IgniteApp.Events
{
    public class NavigateToLoginEvent
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}