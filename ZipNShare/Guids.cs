// Guids.cs
// MUST match guids.h
using System;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare
{
    static class GuidList
    {
        public const string guidZipNSharePkgString = "36772697-6705-40dd-a4b7-ca0ca8afa721";
        public const string guidZipNShareCmdSetString = "332804e2-af89-41e1-9933-ae9fde29e728";
        public const string guidToolWindowPersistanceString = "88e597af-02fa-4648-8af8-120cc6053675";

        public static readonly Guid guidZipNShareCmdSet = new Guid(guidZipNShareCmdSetString);
    };
}