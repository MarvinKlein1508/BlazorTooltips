using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorTooltips
{
    public partial class Tooltip : IAsyncDisposable
    {
        [Parameter] public TooltipOptions Options { get; set; } = new TooltipOptions();
        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public string Title { get; set; } = String.Empty;
        [Parameter] public bool Html { get; set; } = true;
        [Parameter] public TooltipPlacement Placement { get; set; } = TooltipPlacement.Top;

        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AllOtherAttributes { get; set; } = new Dictionary<string, object>();
        private Guid _id;

        private bool _rendered = false;
        private IJSObjectReference? jsModule { get; set; }

        public Tooltip()
        {
            _id = Guid.NewGuid();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _rendered = true;
                jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorTooltips/bootstrap.tooltip.js");
                await Setup(Options);
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            if (_rendered)
                await Update();
        }

        public async ValueTask DisposeAsync()
        {
            await Destroy();
        }

        private async Task Setup(TooltipOptions options)
        {
            if (jsModule is not null)
            {
                await jsModule.InvokeVoidAsync("setup", new object[] { _id, GetOptions().ToJSON() });
            }
        }

        private async Task Destroy()
        {
            if (jsModule is not null)
            {
                try
                {
                    await jsModule.InvokeVoidAsync("destroy", _id);
                    await jsModule.DisposeAsync();
                }
                catch (TaskCanceledException)
                {
                }
                catch (JSDisconnectedException)
                {
                }
            }
        }

        private TooltipOptions GetOptions()
        {
            Options.Html = Html;
            Options.Title = Title;
            Options.Placement = Placement;
            return Options;
        }

        private async Task Update()
        {
            if (jsModule is not null)
            {
                await jsModule.InvokeVoidAsync("update", new object[] { _id, GetOptions().ToJSON() });
            }
        }

    }
}