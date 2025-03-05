using K9.Base.DataAccessLayer.Models;
using K9.SharedLibrary.Models;

namespace K9.DataAccessLayer.Models
{
    public abstract class ScorableBase : ObjectBase, IObjectBase, IScorableBase
    {
        public int Score { get; set; }
        
        public int RelativeScore { get; set; }
    }
}
