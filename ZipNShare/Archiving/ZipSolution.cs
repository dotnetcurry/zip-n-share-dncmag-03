using A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare.Archiving
{
    public class ZipSolution
    {
        static void Test(string[] args)
        {
            string rootFolder = ".\\";
            string archiveName = "Archive.zip";

            if (args.Length > 0)
            {
                rootFolder = args[0];
            }
            if (args.Length > 1)
            {
                archiveName = args[1];
            }
            List<ZipExclusion> exceptions = new List<ZipExclusion>();
            exceptions.Add(new ZipExclusion
            {
                ExclusionType = ExclusionType.File,
                Expression = ".user"
            });
            exceptions.Add(new ZipExclusion
            {
                ExclusionType = ExclusionType.File,
                Expression = ".suo"
            });
            exceptions.Add(new ZipExclusion
            {
                ExclusionType = ExclusionType.Folder,
                Expression = @"\bin"
            });
            exceptions.Add(new ZipExclusion
            {
                ExclusionType = ExclusionType.Folder,
                Expression = @"\obj"
            });
            exceptions.Add(new ZipExclusion
            {
                ExclusionType = ExclusionType.Folder,
                Expression = @"\packages"
            });

            int filesAdded = CreateArchive(rootFolder,
                exceptions, archiveName, rootFolder, true);
            Console.WriteLine(String.Format(" {0} file(s) added ",
                filesAdded));
            Console.ReadLine();
        }

        public static int CreateArchive(string solutionFolder,
                IList<ZipExclusion> exceptions, string archiveName, string outputFolder, bool overwriteArchive)
        {
            int filesCount = 0;
            string folderFullPath = Path.GetFullPath(solutionFolder + @"..\");
            if (!archiveName.EndsWith(".zip"))
            {
                archiveName = archiveName + ".zip";
            }
            string archivePath = Path.Combine(folderFullPath, archiveName);
            if (File.Exists(archivePath))
            {
                if (overwriteArchive)
                {
                    File.Delete(archivePath);
                }
                else
                {
                    Console.WriteLine(string.Format(@"Archive {0} already exists. 
                        Aborting!", archivePath));
                    return 0;
                }
            }
            IEnumerable<string> files = Directory.EnumerateFiles(solutionFolder,
                    "*.*", SearchOption.AllDirectories);
            using (ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                foreach (string file in files)
                {
                    if (!Excluded(file, exceptions))
                    {
                        try
                        {
                            var addFile = Path.GetFullPath(file);
                            if (addFile != archivePath)
                            {
                                addFile = addFile.Substring(folderFullPath.Length);
                                Console.WriteLine("Adding " + addFile);
                                archive.CreateEntryFromFile(file, addFile);
                                filesCount++;
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(@"Failed to add {0} due to error : 
                            {1} \n Ignoring it!", file, ex.Message);
                        }
                    }
                }
            }
            return filesCount;
        }

        private static bool Excluded(string file, IList<ZipExclusion> exceptions)
        {
            List<string> folderNames = (from folder in exceptions
                                        where folder.ExclusionType == ExclusionType.Folder
                                        select folder.Expression).ToList<string>();
            try
            {
                List<ZipExclusion> exs = (from zex in exceptions
                                         where zex.Expression == Path.GetExtension(file)
                                         select zex).ToList<ZipExclusion>();
                if (exs.Count == 0)
                {
                    foreach (string folderException in folderNames)
                    {
                        if (Path.GetDirectoryName(file).Contains(folderException))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
    }
}
