namespace elunebot.services.interfaces
{
    public interface ISpellService
    {
        void UpdateSpells();
        void Cast(string name, int rank = -1);
        int GetSpellRank(string spell);
    }
}
