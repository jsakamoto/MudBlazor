﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Extensions;
using MudBlazor.Utilities;

namespace MudBlazor
{
    public partial class MudTabs : MudComponentBase
    {
        protected string TabsClassnames =>
            new CssBuilder("mud-tabs")
            .AddClass($"mud-tabs-vertical", Vertical)
            .AddClass(Class)
            .Build();

        protected string ToolbarClassnames => 
            new CssBuilder("mud-tabs-toolbar")
            .AddClass($"mud-tabs-rounded", Rounded)
            .AddClass($"mud-tabs-vertical", Vertical)
            .AddClass($"mud-tabs-toolbar-{Color.ToDescriptionString()}", Color != Color.Default)
            .AddClass($"mud-border-right", Border)
            .AddClass($"mud-paper-outlined", Outlined)
            .AddClass($"mud-elevation-{Elevation.ToString()}" , Elevation != 0)
            .Build();

        protected string WrapperClassnames =>
            new CssBuilder("mud-tabs-toolbar-wrapper")
            .AddClass($"mud-tabs-centered", Centered)
            .AddClass($"mud-tabs-vertical", Vertical)
            .Build();

        protected string PanelsClassnames =>
            new CssBuilder("mud-tabs-panels")
            .AddClass($"mud-tabs-vertical", Vertical)
            .Build();

        /// <summary>
        /// If true, sets the border-radius to theme default.
        /// </summary>
        [Parameter] public bool Rounded { get; set; }

        /// <summary>
        /// If true, sets a border.
        /// </summary>
        [Parameter] public bool Border { get; set; }

        /// <summary>
        /// If true, toolbar will be outlined.
        /// </summary>
        [Parameter] public bool Outlined { get; set; }

        /// <summary>
        /// If true, centers the tabitems.
        /// </summary>
        [Parameter] public bool Centered { get; set; }

        /// <summary>
        /// If true, displays the MudTabs verticaly.
        /// </summary>
        [Parameter] public bool Vertical { get; set; }

        /// <summary>
        /// The color of the component. It supports the theme colors.
        /// </summary>
        [Parameter] public Color Color { get; set; } = Color.Default;

        /// <summary>
        /// Child content of component.
        /// </summary>
        [Parameter] public int Elevation { set; get; } = 0;

        /// <summary>
        /// If true, disables ripple effect.
        /// </summary>
        [Parameter] public bool DisableRipple { get; set; }

        /// <summary>
        /// Child content of component.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Custom class/classes for TabPanel
        /// </summary>
        [Parameter] public string TabPanelClass { get; set; }

        public MudTabPanel ActivePanel { get; set; }

        private int _activePanelIndex = 0;
        /// <summary>
        /// The current active panel index. Also with Bidirectional Binding
        /// </summary>
        [Parameter]
        public int ActivePanelIndex
        {
            get => _activePanelIndex;
            set
            {
                if (_activePanelIndex != value)
                {
                    _activePanelIndex = value;
                    ActivatePanel(_activePanelIndex);
                    ActivePanelIndexChanged.InvokeAsync(value);
                }
            }
        }

        /// <summary>
        /// Fired when ActivePanelIndex changes.
        /// </summary>
        [Parameter]
        public EventCallback<int> ActivePanelIndexChanged { get; set; }

        public List<MudTabPanel> Panels = new List<MudTabPanel>();

        internal void AddPanel(MudTabPanel tabPanel)
        {
            Panels.Add(tabPanel);
            if (Panels.Count == 1)
                ActivePanel = tabPanel;
            StateHasChanged();

        }

        string GetTabClass(MudTabPanel panel)
        {
            var TabClass = new CssBuilder("mud-tab")
              .AddClass($"mud-tab-active", when: () => panel == ActivePanel)
              .AddClass($"mud-disabled", panel.Disabled)
              .AddClass($"mud-ripple" ,!DisableRipple)
              .AddClass(TabPanelClass)
            .Build();

            return TabClass;
        }
        void ActivatePanel(MudTabPanel panel, MouseEventArgs ev)
        {
            if(!panel.Disabled)
            {
                ActivePanel = panel;
                ActivePanelIndex = Panels.IndexOf(panel);
                if (ev != null) 
                    ActivePanel.OnClick.InvokeAsync(ev);
            }
        }

        public void ActivatePanel(MudTabPanel panel)
        {
            ActivatePanel(panel, null);
        }

        public void ActivatePanel(int index)
        {
            MudTabPanel panel = Panels[index];
            ActivatePanel(panel, null);
        }

        public void ActivatePanel(object id)
        {
            MudTabPanel panel = Panels.Where((p) => p.ID == id).FirstOrDefault();
            if (panel != null)
                ActivatePanel(panel, null);
        }


    }
}
