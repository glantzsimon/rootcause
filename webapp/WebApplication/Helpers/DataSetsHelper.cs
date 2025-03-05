﻿using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Respositories;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace K9.WebApplication.Helpers
{
    public class GetToTheRootDataSetsHelper : IDataSetsHelper
    {
        private readonly DbContext _db;
        private readonly IDataSets _datasets;
        private Type _resourceType { get; set; }

        public GetToTheRootDataSetsHelper(DbContext db, IDataSets datasets)
        {
            _db = db;
            _datasets = datasets;
        }

        public string GetAllDataSetsJson()
        {
            try
            {
                var jsonDictionary = _datasets.Collection
                    .Where(e => e.Key != null && e.Value != null)
                    .GroupBy(kvp => kvp.Key.Name)
                    .ToDictionary(
                        g => g.Key,
                        g => g.First().Value
                    );

                return JsonConvert.SerializeObject(jsonDictionary);
            }
            catch (Exception e)
            {
            }

            return "[]";
        }

        public List<ListItem> GetDataSet<T>(bool refresh = false, string nameExpression = "Name", string valueExpression = "Name", bool includeDeleted = false, Type resourceType = null) where T : class, IObjectBase
        {
            List<ListItem> dataset = null;
            if (refresh || !_datasets.Collection.ContainsKey(typeof(T)))
            {
                dataset = GetItemList<T>(nameExpression, valueExpression, includeDeleted, resourceType);
                if (dataset != null)
                {
                    if (_datasets.Collection.ContainsKey(typeof(T)))
                    {
                        try
                        {
                            _datasets.Collection[typeof(T)] = dataset;
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    else
                    {
                        AddDataSetToCollection<T>(dataset);
                    }
                }
            }
            return dataset;
        }

        public List<ListItem> GetDataSetFromEnum<T>(bool refresh = false, Type resourceType = null)
        {
            List<ListItem> dataset = null;
            var enumType = typeof(T);

            if (refresh || !_datasets.Collection.ContainsKey(enumType))
            {
                var dictionary = resourceType ??
                                 (enumType.Namespace.Contains("K9.Base") ? typeof(Base.Globalisation.Dictionary) : typeof(Globalisation.Dictionary));

                var values = Enum.GetValues(enumType).Cast<T>();
                dataset = new List<ListItem>(values.Select(e =>
                {
                    var enumValue = e as Enum;
                    var id = Convert.ToInt32(e);
                    var descriptionAttribute = enumValue.GetAttribute<EnumDescriptionAttribute>();

                    descriptionAttribute.ResourceType = dictionary;
                    var name = descriptionAttribute.GetDescription();

                    return new ListItem(id, name);
                }));

                if (!_datasets.Collection.ContainsKey(enumType))
                {
                    AddDataSetToCollection<T>(dataset);
                }
                else
                {
                    try
                    {
                        _datasets.Collection[enumType] = dataset;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return dataset;
        }

        public void AddDataSetToCollection<T>(List<ListItem> items)
        {
            if (!_datasets.Collection.ContainsKey(typeof(T)))
            {
                try
                {
                    _datasets.Collection.Add(typeof(T), items);
                }
                catch (Exception e)
                {
                }
            }
        }

        public SelectList GetSelectList<T>(int? selectedId, bool refresh = false, string nameExpression = "Name",
            string valueExpression = "Name", bool includeDeleted = false, Type resourceType = null) where T : class, IObjectBase
        {
            return new SelectList(GetDataSet<T>(refresh, nameExpression, "Name", includeDeleted, resourceType), "Id", "Name", selectedId);
        }

        public SelectList GetSelectListFromEnum<T>(int selectedId, bool refresh = false, Type resourceType = null)
        {
            return new SelectList(GetDataSetFromEnum<T>(refresh, resourceType), "Id", "Name", selectedId);
        }

        public string GetName<T>(int? selectedId, bool refresh = false, string nameExpression = "Name") where T : class, IObjectBase
        {
            if (!selectedId.HasValue)
            {
                return string.Empty;
            }

            IRepository<T> repo = new BaseRepository<T>(_db);
            return repo.GetName<T>(selectedId.Value, nameExpression);
        }

        private List<ListItem> GetItemList<T>(string nameExpression, string valueExpression, bool includeDeleted = false, Type resourceType = null) where T : class, IObjectBase
        {
            IRepository<T> repo = new BaseRepository<T>(_db);
            var whereClause = !includeDeleted ? " WHERE [IsDeleted] = 0" : "";
            var items = repo.CustomQuery<ListItem>($"SELECT [Id], {nameExpression} AS [Name], {valueExpression} AS [Value] FROM [{typeof(T).Name}]{whereClause} ORDER BY {nameExpression}");

            if (nameExpression == "Name")
            {
                var type = typeof(T);
                var descriptionAttribute = type.GetCustomAttribute<DescriptionAttribute>();

                if (descriptionAttribute?.UseLocalisedString == true)
                {
                    var dictionary = resourceType ?? typeof(Base.Globalisation.Dictionary);
                    foreach (var item in items)
                    {
                        try
                        {
                            item.Name = dictionary.GetValueFromResource(item.Name);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            return items;
        }
    }

}