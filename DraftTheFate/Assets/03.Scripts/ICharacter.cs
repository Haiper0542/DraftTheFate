public interface ICharacter
{
    void TakeDamage(int damage);
    void TakeHeal(int heal);

    void StartTurn();
    void EndTurn();

    void Death();
}