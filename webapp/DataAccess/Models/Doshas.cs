using K9.DataAccessLayer.Enums;

namespace K9.DataAccessLayer.Models
{
    public class Doshas
    {
        private const double triDoshaBoundary = 31;
        private const double dualBoundary = 42;
        private const double singleBoundary = 54;

        public double VataDoshaScore { get; set; }
        public double PittaDoshaScore { get; set; }
        public double KaphaDoshaScore { get; set; }

        public int GetVataDoshaPercentage() => (int)(VataDoshaScore );
        public int GetPittaDoshaPercentage() => (int)(PittaDoshaScore );
        public int GetKaphaDoshaPercentage() => (int)(KaphaDoshaScore);

        public EDosha GetDosha()
        {
            if (VataDoshaScore >= triDoshaBoundary && PittaDoshaScore >= triDoshaBoundary && KaphaDoshaScore >= triDoshaBoundary)
            {
                return EDosha.Tridoshic;
            }
            if (KaphaDoshaScore >= dualBoundary && PittaDoshaScore >= dualBoundary)
            {
                return EDosha.PittaKapha;
            }
            if (KaphaDoshaScore >= dualBoundary && VataDoshaScore >= dualBoundary)
            {
                return EDosha.KaphaVata;
            }

            if (VataDoshaScore > PittaDoshaScore && VataDoshaScore > KaphaDoshaScore)
            {
                return EDosha.Vata;
            }
            if (PittaDoshaScore > VataDoshaScore && PittaDoshaScore > KaphaDoshaScore)
            {
                return EDosha.Pitta;
            }
            if (KaphaDoshaScore > VataDoshaScore && KaphaDoshaScore > PittaDoshaScore)
            {
                return EDosha.Kapha;
            }

            return EDosha.Tridoshic;
        }
    }
}
