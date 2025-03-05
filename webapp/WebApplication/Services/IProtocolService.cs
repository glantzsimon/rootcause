﻿using K9.DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace K9.WebApplication.Services
{
    public interface IProtocolService : ICacheableService
    {
        Protocol Find(int id);
        Protocol FindPrevious(int id);
        Protocol FindNext(int id);
        Protocol Find(Guid id);
        Protocol FindAutoGenerated(int clientId);
        Protocol GetFullProtocol(Protocol protocolItem);
        Protocol Duplicate(int id);
        Protocol GetProtocolWithProtocolSections(int id);
        Protocol GetProtocolWithProtocolSections(Guid id);
      
        void DeleteChildRecords(int id);
        void AddDefaultSections(int id);
        void UpdateSectionDetails(Protocol protocol);
        void UpdateProtocolProductsAndProductPackQuantities(Protocol protocol);
        void CheckProductsAndProductPacksDoNotOverlap(Protocol protocol);


        List<Protocol> List(bool retrieveFullProtocol = false);
    }
}