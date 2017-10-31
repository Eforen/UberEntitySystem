using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class SignatureFactory : ObjectFactory<Signature>
    {
        public override Signature CleanForReuse(Signature sig)
        {
            sig.clear();
            return sig;
        }

        public override Signature CreateNew()
        {
            return new Signature();
        }
    }
}
