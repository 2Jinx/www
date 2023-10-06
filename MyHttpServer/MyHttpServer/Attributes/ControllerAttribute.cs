namespace MyHttpServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        public string Type { get; }
        public ControllerAttribute(string type) => Type = type;
    }
}

