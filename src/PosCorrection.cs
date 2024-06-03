using GBX.NET;
using GBX.NET.Engines.Game;

class PosCorection {
    public PosCorection(){}
    public virtual Vec3 block(CGameCtnBlock ctnBlock){
        return Vec3.Zero;
    }
    public virtual Vec3 item(CGameCtnAnchoredObject ctnItem){
        return Vec3.Zero;
    }
}
class DiagBlockcorrection : PosCorection{
    public DiagBlockcorrection() : base(){}

    public override Vec3 block(CGameCtnBlock ctnBlock){
        switch (ctnBlock.Direction){
            case Direction.North:
                return new Vec3(0,0,0);
            case Direction.East:
                return new Vec3(0,0,-32);
            case Direction.South:
                return new Vec3(-64,0,-32);
            case Direction.West:
                return new Vec3(-64,0,0);
        }
        return Vec3.Zero;
    }
}