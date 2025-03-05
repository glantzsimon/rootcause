namespace K9.DataAccessLayer.Constants
{
    public static class FormatConstants
    {
        public const string AppointmentDisplayDateTimeFormat = "dd MMMM yyyy, HH:mm";
        public const string AppointmentDisplayDateFormat = "dd MMM yyyy";
        public const string AppointmentDisplayTimeFormat = "HH:mm";
        public const string ApiDateTimeFormat = "yyyy-MM-dd";
    }

    public static class ConversionConstants
    {
        public const double BahtToDollarsRate = 0.0282;
        public const double BahtToBritishPoundsRate = 0.0262;
        public const int DefaultCurrencyRounding = 0;
    }
}