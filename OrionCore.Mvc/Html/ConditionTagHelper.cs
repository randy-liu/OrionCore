using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Orion.Mvc.Html
{
    /// <summary></summary>
    [HtmlTargetElement(Attributes = nameof(Condition))]
    public class ConditionTagHelper : TagHelper
    {
        /// <summary></summary>
        public bool Condition { get; set; }

        /// <summary></summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition) { output.SuppressOutput(); }
        }
    }
}
