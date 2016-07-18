namespace Kip.Sample
{
    public class MediaSizeViewModel
    {
        public MediaSizeViewModel(Option option)
        {
            Option = option;
        }

        public string DisplayName
        {
            get
            {
                return Option.Get(Psk.DisplayName).AsString() ?? Option.Name.LocalName;
            }
        }

        public int Width
        {
            get
            {
                return (Option.Get(Psk.MediaSizeWidth)?.AsValue()?.AsInt()).GetValueOrDefault(0);
            }
        }

        public int Height
        {
            get
            {
                return (Option.Get(Psk.MediaSizeHeight)?.AsValue()?.AsInt()).GetValueOrDefault(0);
            }
        }

        public Option Option
        {
            get;
        }
    }
}
