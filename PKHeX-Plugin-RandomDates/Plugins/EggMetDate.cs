using PKHeX_Plugin_RandomDates.Helpers.Pokemon;

namespace PKHeX_Plugin_RandomDates.Plugins;

internal class EggMetDate : DateRandomizerPlugin
{
    public override string Name     => "Randomize Egg Met Dates";
    public override int    Priority => 2;

    protected override void AddPluginControl(ToolStripDropDownItem menuDropDown)
    {
        var ctrl = new ToolStripMenuItem(Name);
        ctrl.Click += (_, _) => RandomizePkmEggMetDates();
        ctrl.Name  =  "Menu_EggMetDate";
        menuDropDown.DropDownItems.Add(ctrl);
    }

    private void RandomizePkmEggMetDates()
    {
        Debug.WriteLine($"{Constants.LOGGING_PREFIX} Starting randomizing Egg Met Dates...");

        var pkmEggMetDate = new PkmEggMetDate(SaveFileEditor.SAV);

        pkmEggMetDate.RandomizeDate();

        SaveFileEditor.ReloadSlots();
        MessageBox.Show($"Randomized {pkmEggMetDate.Counter} Pokémon Egg Met Dates!",
            "Met Dates Randomized!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}