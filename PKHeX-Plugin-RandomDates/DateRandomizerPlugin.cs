global using PKHeX.Core;
global using PKHeX_Plugin_RandomDates.Utils;
global using System.Diagnostics;

namespace PKHeX_Plugin_RandomDates;

public abstract class DateRandomizerPlugin : IPlugin
{
    #region Plugin Interface requirements

    /// <summary>
    ///     Main plugin variables
    /// </summary>
    public abstract string Name { get; }

    public abstract int Priority { get; }

    /// <summary>
    ///     Initialized during plugin load
    /// </summary>
    public ISaveFileProvider SaveFileEditor { get; private set; } = null!;

    private IPKMView PkmEditor { get; set; } = null!;

    /// <summary>
    ///     Initialize the plugin
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="Exception"></exception>
    public void Initialize(params object[] args)
    {
        Debug.WriteLine($"{Constants.LOGGING_PREFIX} Loading {Name}");

        SaveFileEditor = (ISaveFileProvider)(Array.Find(args, z => z is ISaveFileProvider) ??
                                             throw new Exception("SaveFileEditor error"));

        PkmEditor = (IPKMView)(Array.Find(args, z => z is IPKMView)
                               ?? throw new Exception("PkmEditor error"));

        var menu = (ToolStrip)(Array.Find(args, z => z is ToolStrip)
                               ?? throw new Exception("ToolStrip error"));

        LoadMenuStrip(menu);
    }

    /// <summary>
    ///     Let's plugin know a save file is loaded
    /// </summary>
    public virtual void NotifySaveLoaded()
    {
        Debug.WriteLine($"{Name} was notified that a Save File was just loaded.");
    }

    /// <summary>
    ///     This plugin should never use this function
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public virtual bool TryLoadFile(string filePath)
    {
        Debug.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
        return false; // no action taken
    }

    /// <summary>
    ///     Retrieves the menu strip from PKHeX
    ///     This allows the plugin to add any menu items required
    /// </summary>
    /// <param name="menuStrip"></param>
    /// <exception cref="ArgumentException"></exception>
    private void LoadMenuStrip(ToolStrip menuStrip)
    {
        var items = menuStrip.Items;

        if (items.Find(Constants.PARENT_MENU_PARENT, false)[0] is not ToolStripDropDownItem tools)
            throw new ArgumentException("MenuStrip not found", nameof(menuStrip));

        var             toolStripItemCollection = tools.DropDownItems;
        ToolStripItem[] toolStripItems          = toolStripItemCollection.Find(Constants.PARENT_MENU_NAME, false);
        var             toolStripMenuItem       = GetMenuItem(tools, toolStripItems);

        AddPluginControl(toolStripMenuItem);
    }

    private static ToolStripMenuItem GetMenuItem(ToolStripDropDownItem tools, IReadOnlyList<ToolStripItem> search)
    {
        if (search.Count != 0) return (ToolStripMenuItem)search[0];

        var toolStripMenuItem = CreatePluginGroupItem();
        tools.DropDownItems.Insert(0, toolStripMenuItem);

        return toolStripMenuItem;
    }

    private static ToolStripMenuItem CreatePluginGroupItem()
    {
        return new ToolStripMenuItem(Constants.PARENT_MENU_TEXT)
        {
            Name = Constants.PARENT_MENU_NAME
        };
    }

    /// <summary>
    ///     Add required menu items to the menu strip
    /// </summary>
    /// <param name="dropDownMenu"></param>
    protected abstract void AddPluginControl(ToolStripDropDownItem dropDownMenu);

    #endregion Plugin Interface requirements
}