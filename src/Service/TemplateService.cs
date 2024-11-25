namespace MailService.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Mustache;
    public class TemplateService
    {
        private readonly Template _template;
        public TemplateService(string resourceName)
        {
            using (var stream = GetExecutingAssemblySafely().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception($"Resource {resourceName} load error.");
                }

                using (var reader = new StreamReader(stream))
                {
                    var templateS = reader.ReadToEnd();

                    _template = Template.Compile(templateS);
                }
            }
        }
        public string GetHtmlBody(IDictionary<string, string> args)
        {
            return _template.Render(args);
        }

        public static TemplateService Create()
        {
            return new TemplateService($"{GetExecutingAssemblySafely().GetName().Name}.Template.EmailTemplate.html");
        }

        [RequiresUnreferencedCode("This method requires reflection and should not be trimmed.")]
        public static Assembly GetExecutingAssemblySafely()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
