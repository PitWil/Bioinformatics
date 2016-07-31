using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using Bioinformatics.Persistence.Interfaces;
using Newtonsoft.Json;

namespace Bioinformatics.Persistence.Services
{
    public class ProteinGraphFileRepository : IProteinGraphRepository
    {
        private readonly string _path;

        public ProteinGraphFileRepository(string path)
        {
            _path = path;
        }

        public Result SaveProteinGraph(Graph g)
        {
            var result = new Result();
            var serializedGraph =
                JsonConvert.SerializeObject(g, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            try
            {
                using (var sw = new StreamWriter(_path, true))
                {
                    sw.WriteLine(serializedGraph);
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return result;
            }
            result.Successed = true;
            return result;
        }

        public DataResult<Graph> GetProteinGraphById(string id)
        {
            var result = new DataResult<Graph>();
            var allResult = GetAllProteinGraph();
            if (!allResult.Successed)
            {
                result.ErrorMessage = allResult.ErrorMessage;
                return result;
            }
            var graph = allResult.Data.Where(a => a.Id == id).ToList();
            if (graph.Count == 0)
            {
                result.ErrorMessage = "Graph does not exist!";
                return result;
            }
            result.Data = graph[0];
            return result;
        }

        public DataResult<List<Graph>> GetAllProteinGraph()
        {
            var result = new DataResult<List<Graph>>();
            List<string> allLines = null;
            try
            {
                allLines = File.ReadAllLines(_path).ToList();
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return result;
            }

            result.Data = new List<Graph>();
            try
            {
                foreach (var line in allLines)
                {
                    result.Data.Add(JsonConvert.DeserializeObject<Graph>(line));
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Data = null;
                return result;
            }

            result.Successed = true;
            return result;
        }
    }
}