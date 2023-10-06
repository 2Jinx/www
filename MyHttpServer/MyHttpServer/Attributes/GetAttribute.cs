using System;
namespace MyHttpServer.Attributes
{
    public class GetAttribute: Attribute, IHttpMethodAttribute
    {
        public GetAttribute(string actionName)
        {
            ActionName = actionName;
        }

        public string ActionName { get; }
    }
}

