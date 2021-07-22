using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel.Command
{
    
    public class CLIOption
    {
        [Option('p', "path", Required = true, HelpText = "원본 파일 경로")]
        public string Path { get; set; }
        [Option('d', "dest", Required = true, HelpText = "복사 파일 경로")]
        public string DestPath { get; set; }
        [Option('o', "overwrite", HelpText = "덮어쓰기")]
        public bool Overwrite { get; set; }
    }
}
