namespace emmad.Parameter
{
    public class OrganisationParameters
    {
        public int page { get; set; } = 1;
        private int maxSize = 20;
        private int _size = 10;
        public int size 
        {
            get
            {
                return _size;
            } 
            set
            {
                _size = (value > maxSize) ? maxSize : value;
            }
        }
    }
}
