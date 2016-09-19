
namespace Dynamic.GeographicCalcService
{
    public class DirectionClass
    {
        private string InternalDirection = string.Empty;

        public string Direction
        {
            get
            {
                return this.ToString();
            }
            set
            {
                Set(value);
            }
        }

        public DirectionClass()
        {
        }

        public DirectionClass(string Direction)
        {
            Set(Direction);
        }

        private void Set(string Direction)
        {
            InternalDirection = string.Empty;
            string temp = Direction.Trim().ToUpper();
            if (temp.Length >= 1)
            {
                temp = temp.Substring(0, 1).ToUpper();
            }
            if (temp == "N" || temp == "S" || temp == "E" || temp == "W")
            {
                InternalDirection = temp;
            }
            else
            {
                InternalDirection = string.Empty;
            }
        }

        public override string ToString()
        {
            return InternalDirection;
        }


        public bool IsValidEastWest
        {
            get
            {
                if (InternalDirection == "E" || InternalDirection == "W")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsValidNorthSouth
        {
            get
            {
                if (InternalDirection == "N" || InternalDirection == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}