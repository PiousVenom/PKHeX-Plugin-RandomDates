namespace PKHeX_Plugin_RandomDates.Helpers.Pokemon;

internal class PkmEggMetDate
{
    internal PkmEggMetDate(SaveFile save)
    {
        Save    = save;
        Counter = 0;
    }

    private  SaveFile Save    { get; }
    internal int      Counter { get; private set; }

    internal void RandomizeDate()
    {
        StartProcess();
    }

    private void StartProcess()
    {
        if (!Save.HasBox) throw new Exception("Save File has no boxes.");

        for (var i = 0; i < Save.BoxCount; i++)
        {
            Debug.WriteLine($"{Constants.LOGGING_PREFIX} Starting box: {i + 1}");
            ProcessBox(i);
        }
    }

    private void ProcessBox(int box)
    {
        if ((uint)box >= Save.BoxCount)
            throw new ArgumentOutOfRangeException(nameof(box), "Value was more than BoxCount");

        Debug.WriteLine($"{Constants.LOGGING_PREFIX} Starting box: {box + 1}");

        var data = Save.GetBoxData(box);

        ProcessPokemon(data);

        if (Counter > 0) Save.SetBoxData(data, box);
    }

    private void ProcessPokemon(IEnumerable<PKM> data)
    {
        var rand = new Random();

        foreach (var pokemon in data)
        {
            if (pokemon.Species <= 0 || pokemon is { WasEgg: false, IsEgg: false }) continue;

            Debug.WriteLine($"\n\n{Constants.LOGGING_PREFIX} Starting update on: {pokemon.Nickname}");

            Debug.WriteLine(
                $"{Constants.LOGGING_PREFIX} Starting Egg Met Date for {pokemon.Nickname}: {pokemon.EggMetDate}");

            var releaseDate = new ReleaseDate(Save.Version);

            var baseDate = releaseDate.Date;

            var days = (DateTime.Today - baseDate).Days;

            var newEggMetDate = baseDate.AddDays(rand.Next(days));

            pokemon.Egg_Day   = newEggMetDate.Day;
            pokemon.Egg_Year  = newEggMetDate.Year - 2000;
            pokemon.Egg_Month = newEggMetDate.Month;

            Debug.WriteLine(
                $"{Constants.LOGGING_PREFIX} New Egg Met Date for {pokemon.Nickname}: {pokemon.EggMetDate}");

            Counter++;
        }
    }
}