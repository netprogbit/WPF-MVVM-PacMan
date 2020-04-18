using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface ICast : ICharacter
    {
        void Move(int pacManX, int pacManY);
        bool TouchedPacMan(int pacManX, int pacManY);
    }
}
