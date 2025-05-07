using System;

namespace CharacterProcess.Gimmicks
{
    public interface ICharacterGimmick
    {
    }

    public class CharacterTagAttibute : Attribute
    {
        private string _tag;
        public CharacterTagAttibute(string tag)
        {
            _tag = tag;
        }
    }
    public abstract class BaseCharacterProcessor : OperationalProcessor<CharacterDataSettings>
    {
    }

}