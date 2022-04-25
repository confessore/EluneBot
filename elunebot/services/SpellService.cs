using elunebot.extensions;
using elunebot.services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elunebot.services
{
    public sealed class SpellService : ISpellService
    {
        readonly IMemoryService _memory;

        public SpellService(
            IMemoryService memory)
        {
            _memory = memory;
        }

        public IReadOnlyDictionary<string, uint[]> PlayerSpells { get; private set; }

        public void UpdateSpells()
        {
            var tmpPlayerSpells = new Dictionary<string, uint[]>();
            const uint currentPlayerSpellPtr = 0x00B700F0;
            uint index = 0;
            while (index < 1024)
            {
                var currentSpellId = (currentPlayerSpellPtr + 4 * index).ReadAs<uint>();
                if (currentSpellId == 0) break;
                var entryPtr = ((0x00C0D780 + 8).ReadAs<uint>() + currentSpellId * 4).ReadAs<uint>();

                var entrySpellId = entryPtr.ReadAs<uint>();
                var namePtr = (entryPtr + 0x1E0).ReadAs<uint>();
                var name = namePtr.ReadString();

#if DEBUG
                Console.WriteLine(entrySpellId + " " + name);
#endif

                if (tmpPlayerSpells.ContainsKey(name))
                {
                    var tmpIds = new List<uint>();
                    tmpIds.AddRange(tmpPlayerSpells[name]);
                    tmpIds.Add(entrySpellId);
                    tmpPlayerSpells[name] = tmpIds.ToArray();
                }
                else
                {
                    uint[] ranks = { entrySpellId };
                    tmpPlayerSpells.Add(name, ranks);
                }
                index += 1;
            }
            PlayerSpells = tmpPlayerSpells;
        }

        /// <summary>
        /// cast a spell by name
        /// </summary>
        /// <param name="parName">name of the spell</param>
        /// <param name="parRank">rank of the spell</param>
        public void Cast(string parName, int parRank = -1)
        {
            var spellEscaped = parName.Replace("'", "\\'");
            var rankText = parRank != -1 ? $"(Rank {parRank})" : "";
            var spellCastString = $"CastSpellByName('{spellEscaped}{rankText}')";
            _memory.DoString(spellCastString);
        }

        /// <summary>
        /// get the rank of the spell
        /// </summary>
        /// <param name="spell">the spell name</param>
        /// <returns></returns>
        public int GetSpellRank(string spell)
        {
            if (!PlayerSpells.ContainsKey(spell)) return 0;
            return PlayerSpells[spell].Length;
        }
    }
}
