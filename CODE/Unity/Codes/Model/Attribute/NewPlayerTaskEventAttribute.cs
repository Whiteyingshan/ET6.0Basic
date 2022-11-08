namespace ET
{
    public class NewPlayerTaskEventAttribute : BaseAttribute
    {
        public readonly string eventName;

        public NewPlayerTaskEventAttribute(string _eventName)
        {
            this.eventName = _eventName;
        }
    }
}