﻿using System.IO;
using System.Xml;
using log4net;

namespace EdFi.LoadTools.Engine.InterchangePipeline
{
    public class FindReferencesStep : IInterchangePipelineStep
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FindReferencesStep).Name);
        private readonly IXmlReferenceCacheProvider _referenceCacheProvider;

        public FindReferencesStep(IXmlReferenceCacheProvider referenceCacheProvider)
        {
            _referenceCacheProvider = referenceCacheProvider;
        }

        public bool Process(string sourceFileName, Stream stream)
        {
            var total = 0;
            var referenceCache = _referenceCacheProvider.GetXmlReferenceCache(sourceFileName);
            using (var reader = new XmlTextReader(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) continue;
                    var refId = reader.GetAttribute("ref");

                    if (!string.IsNullOrEmpty(refId))
                    {
                        referenceCache.LoadReference(refId);
                        total++;
                    }
                }
                Log.Info($"{total} references to {referenceCache.NumberOfReferences} resources found");
            }
            return true;
        }
    }
}
