using System.Collections;
using System.Text.RegularExpressions;

namespace TemplatingEngine
{
    public class TemplateParser
    {
        public string Render(string template, object data)
        {
            var renderedTemplate = template;

            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
            {
                var regex = new Regex($"@{property.Name}");
                renderedTemplate = regex.Replace(renderedTemplate, property.GetValue(data).ToString());
            }

            renderedTemplate = ProcessForLoops(renderedTemplate, data);

            return renderedTemplate;
        }

        private string ProcessForLoops(string template, object data)
        {
            var regex = new Regex(@"@for\((.*?)\)(.*?)@endfor", RegexOptions.Singleline);
            var matches = regex.Matches(template);

            foreach (Match match in matches)
            {
                var loopData = match.Groups[1].Value;
                var content = match.Groups[2].Value;

                var property = data.GetType().GetProperty(loopData);
                if (property != null)
                {
                    var value = property.GetValue(data);

                    if (value is IEnumerable enumerable)
                    {
                        var loopContent = "";
                        foreach (var item in enumerable)
                        {
                            loopContent += content.Replace("@item", item.ToString());
                        }
                        template = template.Replace(match.Value, loopContent);
                    }
                }
            }

            return template;
        }
    }
}
