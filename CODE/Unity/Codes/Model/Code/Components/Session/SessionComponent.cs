namespace ET
{
    public class SessionComponent : Entity
    {
        public static SessionComponent Inst { get; set; }
        public Session Session { get; set; }
    }
}