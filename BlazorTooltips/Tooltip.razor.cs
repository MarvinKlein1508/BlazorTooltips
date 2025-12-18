using System.Dynamic;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTooltips
{
    public partial class Tooltip : IAsyncDisposable
    {
        private Guid _id;
        private bool _rendered = false;
        private IJSObjectReference? _jsModule { get; set; }
        #region Parameters

        /// <summary>
        /// Sets the text of the tooltip.
        /// </summary>
        [Parameter, EditorRequired]
        public string Title { get; set; } = String.Empty;
        /// <summary>
        /// Enables HTML Content in the tooltip. 
        /// <para>
        /// Default is <c>true</c>.
        /// </para>
        /// </summary>
        [Parameter] public bool Html { get; set; } = true;
        /// <summary>
        /// Sets the content which the tooltip should wrap around.
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }
        /// <summary>
        /// Sets the default positioning of the tooltip.
        /// <para>
        /// Default is <see cref="TooltipPlacement.Top"/>
        /// </para>
        /// </summary>
        [Parameter] public TooltipPlacement Placement { get; set; } = TooltipPlacement.Top;
        /// <summary>
        /// Sets the bootstrap custom class property
        /// 
        /// <para>
        /// Default is <c>string.Empty</c>
        /// </para>
        /// 
        /// <para>
        /// Requires at least Bootstrap 5.2 or later.
        /// </para>
        /// </summary>
        [Parameter] public string CustomClass { get; set; } = string.Empty;
        /// <summary>
        /// Apply a CSS fade transition to the tooltip
        /// <para>
        /// Default is <c>true</c>
        /// </para>
        /// </summary>
        [Parameter] public bool Animation { get; set; } = true;
        /// <summary>
        /// Delay showing and hiding the tooltip (ms)—doesn’t apply to manual trigger type. If a number is supplied, delay is applied to both hide/show.   
        /// </summary>
        [Parameter] public int Delay { get; set; } = 0;
        [Parameter] public string Container { get; set; } = string.Empty;
        [Parameter] public string Selector { get; set; } = string.Empty;
        [Parameter] public string Template { get; set; } = "<div class='tooltip' role='tooltip'><div class='tooltip-arrow'></div><div class='tooltip-inner'></div></div>";
        [Parameter] public TooltipTrigger[] Trigger { get; set; } = new TooltipTrigger[] { TooltipTrigger.Hover };
        [Parameter] public TooltipPlacement[] FallbackPlacements { get; set; } = new TooltipPlacement[] { TooltipPlacement.Top, TooltipPlacement.Right, TooltipPlacement.Bottom, TooltipPlacement.Left };
        [Parameter] public string Boundary { get; set; } = "clippingParents";
        [Parameter] public bool Sanitize { get; set; } = true;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AllOtherAttributes { get; set; } = new Dictionary<string, object>();
        #endregion
        public Tooltip()
        {
            _id = Guid.NewGuid();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _rendered = true;
                _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Omniacore.BlazorTooltips/bootstrap.tooltip.js");
                await Setup();
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            if (_rendered)
            {
                await Update();
            }
        }

        public async ValueTask DisposeAsync()
        {
            try { await Destroy(); }
            catch (JSDisconnectedException) { }
            catch (ObjectDisposedException) { }
        }

        private async Task Setup()
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("setup", new object[] { _id, OptionsToJSON() });
            }
        }

        private async Task Destroy()
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("destroy", _id);
                await _jsModule.DisposeAsync();
            }
        }

        private string OptionsToJSON()
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

            return JsonSerializer.Serialize(tmp);
        }

        private async Task Update()
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("update", new object[] { _id, OptionsToJSON() });
            }
        }

    }
}