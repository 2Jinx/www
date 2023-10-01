using System;
namespace MyHttpServer.Controllers
{
    [AttributeUsage(AttributeTargets.All)]
    public class Controller : Attribute
    {
        public string Type { get; }
        public Controller() { }
        public Controller(string type) => Type = type;
    }
}

