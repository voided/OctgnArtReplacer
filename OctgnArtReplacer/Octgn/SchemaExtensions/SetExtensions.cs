using OctgnArtReplacer.Octgn.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Octgn.SchemaExtensions
{
    static class SetExtensions
    {
        public static Card GetCardByName(this Set set, string cardName, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return set.GetCardsByName(cardName, comparison)
                .FirstOrDefault();
        }
        public static IEnumerable<Card> GetCardsByName(this Set set, string cardName, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return set.Cards
                .Where(c => string.Equals(c.Name, cardName, comparison));
        }

        public static Card GetCardByMultiverseId(this Set set, int multiverseId)
        {
            return set.Cards
                .Where(c => c.GetMultiverseId() != null)
                .FirstOrDefault(c => c.GetMultiverseId() == multiverseId);
        }
    }
}
