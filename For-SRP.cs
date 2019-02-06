    class Player
    {
        private float _health;
        private float _armor;
        private int _id;

        public float Health => _health;

        public Player(float health, float armor, int id)
        {
            _health = health;
            _armor = armor;
            _id = id;

            if (File.Exists(($"user_{_id}.data")))
            {
                var data = File.ReadAllText($"user_{_id}.data");
                _health = float.Parse(data);
            }
        }

        public void ApplyDamage(float damage)
        {
            float healthDelta = damage - _armor;
            _health -= healthDelta;
            _armor /= 2;

            Console.WriteLine($"Вы получили урона - {healthDelta}");

            File.WriteAllText($"user_{_id}.data", _health.ToString());
        }
    }