﻿using CodeChallenge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class CompensationDataSeeder
    {
        private CompensationContext _compensationContext;
        private const string COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public CompensationDataSeeder(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
        }

		public async Task Seed(bool reseed)
		{
            if(reseed && _compensationContext.Compensations.Any())
            {
                _compensationContext.Compensations.RemoveRange(_compensationContext.Compensations);
				await _compensationContext.SaveChangesAsync();
			}

			if(!_compensationContext.Compensations.Any())
			{
				List<Compensation> compensations = LoadCompensations();
                _compensationContext.Compensations.AddRange(compensations);

                await _compensationContext.SaveChangesAsync();
            }
        }

        private List<Compensation> LoadCompensations()
        {
            using(FileStream fs = new FileStream(COMPENSATION_SEED_DATA_FILE, FileMode.Open))
            using(StreamReader sr = new StreamReader(fs))
            using(JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Compensation> compensations = serializer.Deserialize<List<Compensation>>(jr);

                return compensations;
            }
        }
    }
}
