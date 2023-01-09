namespace PKHeX_Plugin_RandomDates.Helpers.Pokemon;

internal class PkmMetDate
{
    internal PkmMetDate(SaveFile save)
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

        for (var i = 0; i < Save.BoxCount; i++) ProcessBox(i);
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
            if (pokemon.Species <= 0) continue;

            Debug.WriteLine($"\n\n{Constants.LOGGING_PREFIX} Starting update on: {pokemon.Nickname}");
            Debug.WriteLine($"{Constants.LOGGING_PREFIX} Starting Met Date for {pokemon.Nickname}: {pokemon.MetDate}");

            var releaseDate = new ReleaseDate(Save.Version);
            var baseDate    = PokemonEggCheck(pokemon, releaseDate.Date);

            var days = (DateTime.Today - baseDate).Days;

            var newMetDate = baseDate.AddDays(rand.Next(days));

            pokemon.Met_Day   = newMetDate.Day;
            pokemon.Met_Year  = newMetDate.Year - 2000;
            pokemon.Met_Month = newMetDate.Month;

            Debug.WriteLine($"{Constants.LOGGING_PREFIX} New Met Date for {pokemon.Nickname}: {pokemon.MetDate}");

            Counter++;
        }
    }

    /// <summary>
    ///     Returns either the game's release date or the EggMetDate of the Pokémon, whichever is more recent
    /// </summary>
    /// <param name="pokemon"></param>
    /// <param name="gameReleaseDate"></param>
    /// <returns></returns>
    // ReSharper disable once MemberCanBeMadeStatic.Local
#pragma warning disable CA1822
    private DateTime PokemonEggCheck(PKM pokemon, DateTime gameReleaseDate)
#pragma warning restore CA1822
    {
        if (pokemon.EggMetDate is not null)
        {
            Debug.WriteLine($"{Constants.LOGGING_PREFIX} {pokemon.Nickname} was an egg");
            Debug.WriteLine($"{Constants.LOGGING_PREFIX} Setting Base date set to Egg Met Date: {pokemon.EggMetDate}");

            return (DateTime)pokemon.EggMetDate;
        }

        Debug.WriteLine($"{Constants.LOGGING_PREFIX} {pokemon.Nickname} was not an egg");

        return gameReleaseDate;
    }
}