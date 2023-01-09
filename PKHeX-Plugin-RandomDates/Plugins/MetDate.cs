using PKHeX_Plugin_RandomDates.Helpers.Pokemon;

namespace PKHeX_Plugin_RandomDates.Plugins;

internal class MetDate : DateRandomizerPlugin
{
    public override string Name     => "Randomize Met Dates";
    public override int    Priority => 1;

    protected override void AddPluginControl(ToolStripDropDownItem menuDropDown)
    {
        var ctrl = new ToolStripMenuItem(Name);
        ctrl.Click += (_, _) => RandomizePkmMetDates();
        ctrl.Name  =  "Menu_MetDate";
        menuDropDown.DropDownItems.Add(ctrl);
    }

    private void RandomizePkmMetDates()
    {
        Debug.WriteLine($"{Constants.LOGGING_PREFIX} Starting randomizing Met Dates...");

        var pkmMetDate = new PkmMetDate(SaveFileEditor.SAV);

        pkmMetDate.RandomizeDate();

        SaveFileEditor.ReloadSlots();
        MessageBox.Show($"Randomized {pkmMetDate.Counter} Pokémon Met Dates!",
            "Met Dates Randomized!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}