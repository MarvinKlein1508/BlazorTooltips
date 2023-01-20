using System.Dynamic;

namespace BlazorTooltips
{
    public class TooltipOptions
    {
        public bool Animation { get; set; } = true;
        public string Container { get; set; } = String.Empty;
        public int Delay { get; set; } = 0;
        public bool Html { get; set; } = true;
        public TooltipPlacement Placement { get; set; } = TooltipPlacement.Top;
        public string Selector { get; set; } = String.Empty;
        public string Template { get; set; } = "<div class='tooltip' role='tooltip'><div class='tooltip-arrow'></div><div class='tooltip-inner'></div></div>";
        public string Title { get; set; } = String.Empty;
        public TooltipTrigger[] Trigger { get; set; } = new TooltipTrigger[] { TooltipTrigger.Hover };
        public TooltipPlacement[] FallbackPlacements { get; set; } = new TooltipPlacement[] { TooltipPlacement.Top, TooltipPlacement.Right, TooltipPlacement.Bottom, TooltipPlacement.Left };
        public string Boundary { get; set; } = "clippingParents";
        public string CustomClass { get; set; } = String.Empty;
        public bool Sanitize { get; set; } = true;
        public int OffsetSkidding { get; set; } = 0;
        public int OffsetDistance { get; set; } = 0;

        public string ArrayToJson<T>(T[] array)
        {
            string result = "[";
            List<string> values = new List<string>();
            foreach (T o in array)
            {
                if (o is not null)
                {
                    values.Add($"\"{o.ToString()?.ToLower()}\"");
                }
            }

            result += String.Join(',', values);

            result += "]";
            return result;
        }

        public string ToJSON()
        {
            dynamic tmp = new ExpandoObject();
            tmp.animation = Animation;
            tmp.delay = Delay;
            tmp.html = Html;
            tmp.placement = Placement.ToString().ToLower();
            tmp.template = Template;
            tmp.title = Title;
            tmp.trigger = String.Join(' ', Trigger.Distinct().Select(x => x.ToString().ToLower()));
            tmp.fallbackPlacements = FallbackPlacements.Select(x => x.ToString().ToLower()).ToArray();
            tmp.boundary = Boundary;
            tmp.customClass = CustomClass;
            tmp.sanitize = Sanitize;
            tmp.closest = false;

            if (String.IsNullOrWhiteSpace(Container))
            {
                tmp.container = false;
            }
            else
            {
                tmp.container = Container;
            }

            if (String.IsNullOrWhiteSpace(Selector))
            {
                tmp.selector = false;
            }
            else
            {
                tmp.selector = Selector;
            }
            
            return System.Text.Json.JsonSerializer.Serialize(tmp);
        }
    }
}