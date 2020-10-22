public class Player
{
    private float _armor;

    public event EventHandler<PlayerInfo> OnLoad;
    public event Action OnSave;

    public Player(float health, float armor, int id)
    {
        Health = health;
        _armor = armor;
        Id = id;

        Load();
    }

    public int Id { get; }

    public float Health { get; private set; }

    private void Load()
    {
        var playerInfo = new PlayerInfo();

        OnLoad?.Invoke(this, playerInfo);
        Health = playerInfo.Health;
    }
    
    public void ApplyDamage(float damage)
    {
        var healthDelta = damage - _armor;
        Health -= healthDelta;
        _armor /= 2;

        Console.WriteLine($"Вы получили урона - {healthDelta}");
        OnSave?.Invoke();
    }
}

public class PlayerInfo : EventArgs
{
    public float Health { get; set; }
}

public class FileStorage : IDisposable
{
    private readonly Player _player;
    private static readonly List<int> Players = new List<int>();

    public FileStorage(Player player)
    {
        _player = player;

        try
        {
            if (Players.Contains(player.Id))
                throw new InvalidOperationException($"Для игрока с ID={_player.Id} уже создано файловое хранилище!");

            Players.Add(_player.Id);
            _player.OnLoad += OnLoad;
            _player.OnSave += OnSave;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void OnSave()
    {
        File.WriteAllText($"user_{_player.Id}.data", _player.Health.ToString(CultureInfo.InvariantCulture));
    }

    private void OnLoad(object sender, PlayerInfo playerInfo)
    {
        var data = string.Empty;

        if (File.Exists(($"user_{_player.Id}.data")))
        {
            data = File.ReadAllText($"user_{_player.Id}.data");
        }

        if (float.TryParse(data, out var parseResult)) playerInfo.Health = parseResult;
    }

    public void Dispose()
    {
        _player.OnLoad -= OnLoad;
        _player.OnSave -= OnSave;
        Players.Remove(_player.Id);
    }
}
