using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET
{
    public class PlayerSetComponent : Entity
    {
        public class PlayerComponentSystem : AwakeSystem<PlayerSetComponent>
        {
            public override void Awake(PlayerSetComponent self)
            {
                self.Awake();
            }
        }

        public static PlayerSetComponent Instance { get; private set; }

        private readonly Dictionary<long, Player> idPlayers = new Dictionary<long, Player>();
        public int Count => this.idPlayers.Count;
        public Dictionary<long, Player>.KeyCollection Keys => idPlayers.Keys;
        public Dictionary<long, Player>.ValueCollection Values => idPlayers.Values;

        public void Awake()
        {
            Instance = this;
        }

        public void Add(Player player)
        {
            this.idPlayers.Add(player.Id, player);
        }

        public Player Get(long id)
        {
            this.idPlayers.TryGetValue(id, out Player gamer);
            return gamer;
        }

        public void Remove(long id)
        {
            this.idPlayers.Remove(id);
        }

        public Player[] GetAll()
        {
            return this.idPlayers.Values.ToArray();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (Player player in this.idPlayers.Values)
            {
                player.Dispose();
            }

            Instance = null;
        }
    }
}