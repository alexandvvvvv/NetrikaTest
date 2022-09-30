﻿using Microsoft.Extensions.Options;
using Netrika.Services.MedicalOrganizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetrikaTest.Services.MedicalOrganizations
{
    public interface IMedicalOrganizationsService
    {
        Task<MedicalOrganization> Get(Guid id);
        Task<IReadOnlyCollection<MedicalOrganization>> List(string? filter, int? skip = null, int? take = null, string? orderBy = null);
    }

    public class MedicalOrganizationsService : IMedicalOrganizationsService
    {
        private readonly IMedicalOrganizationsCache _cache;
        private readonly int _throttle;

        public MedicalOrganizationsService(IMedicalOrganizationsCache cache, IOptions<MedicalOrganizationsParams> options)
        {
            _cache = cache;
            _throttle = options.Value.Throttling;
        }

        public async Task<MedicalOrganization> Get(Guid id)
        {
            var organizations = await _cache.List();
            var result = organizations.FirstOrDefault(x => x.Id == id);
            if (result is null)
            {
                throw new Exception($"Organization with id {id} not found");
            }
            return result;
        }

        public async Task<IReadOnlyCollection<MedicalOrganization>> List(string? filter, int? skip, int? take, string? orderBy)
        {
            //todo validate params
            var result = await _cache.List();

            //var searchChunks = name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var query = result.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query
                    .AsParallel()
                    .Where(x =>
                    {
                        return x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase);
                    });
            }

            query = orderBy?.ToLower() switch
            {
                "name" => query.OrderBy(x => x.Name),
                "id" => query.OrderBy(x => x.Id),
                _ => query
            };

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            query = query.Take(take ?? _throttle);

            return query.ToList();
        }
    }
}
