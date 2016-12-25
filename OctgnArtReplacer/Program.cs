using Nito.AsyncEx;
using OctgnArtReplacer.Octgn.Mtg;
using OctgnArtReplacer.Replacer;
using OctgnArtReplacer.Replacer.Images;
using OctgnArtReplacer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async Task MainAsync(string[] args)
        {
            if (args.Length < 3)
            {
                Log.Info("Usage:");
                Log.Info("  OctgnArtReplacer \"path\\to\\image\\source\" \"path\\for\\output\" \"set name\"");
                return;
            }

            var artReplacer = new ArtReplacer(
                imageSourcePath: args[0],
                imageOutputPath: args[1],
                setName: args[2]
            );

            VerificationResult verificationResult = await artReplacer.Verify();

            if (verificationResult == VerificationResult.Fail)
                return;

            if (verificationResult == VerificationResult.Warn)
            {
                Log.Warning("Press any key to continue, or Ctrl+C to cancel");
                Console.ReadKey();
            }

            await artReplacer.Run();
        }
    }
}