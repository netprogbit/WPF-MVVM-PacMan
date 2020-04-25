using Core.Enums;

namespace Core.Abstractions
{
  public interface ICast : ICharacter
  {
    Occupation Owner { get; set; }
    void Move(int pacManX, int pacManY);
    bool TouchedPacMan(int pacManX, int pacManY);
  }
}
