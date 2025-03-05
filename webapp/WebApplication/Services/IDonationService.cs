﻿using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public interface IDonationService : IBaseService
    {
        void CreateDonation(Donation donation, Client client);
        int GetFundsReceivedToDate();
    }
}