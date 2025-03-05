﻿using K9.DataAccessLayer.Models;
using System.Collections.Generic;

namespace K9.WebApplication.Services
{
    public interface IConsultationService : IBaseService
    {
        int CreateConsultation(Consultation consultation, Client client, int? userId = null, bool isComplementary = false);
        Consultation Find(int id);
        UserConsultation FindUserConsultation(int consultationId, int userId);
        Slot FindSlot(int id);
        void SelectSlot(int consultationId, int slotId);
        void CreateFreeSlots();
        List<Slot> GetAvailableSlots();
    }
}