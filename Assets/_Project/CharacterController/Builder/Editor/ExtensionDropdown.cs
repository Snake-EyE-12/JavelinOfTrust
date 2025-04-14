using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

public class ExtensionDropdown : AdvancedDropdown
{
    private List<string> options;
    private Action<string> onSelected;

    public ExtensionDropdown(AdvancedDropdownState state, List<string> options, Action<string> onSelected)
        : base(state)
    {
        this.options = options;
        this.onSelected = onSelected;
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("Extensions");

        foreach (var option in options)
        {
            root.AddChild(new AdvancedDropdownItem(option));
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        onSelected?.Invoke(item.name);
    }
}