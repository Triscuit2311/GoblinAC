using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Goblin.Client.Countermeasures;

public class DrawManager
{
    DrawManager()
    {
        //TODO: Add event to take a player ID from the server and start drawing cheater tags
    }
    
    [Tick]
    public Task OnTick()
    {
        return Task.FromResult(0);
    }
    enum pBones
    {
        SKEL_ROOT = 0,
        SKEL_Head = 31086
    }
    void DrawCheaterTag()
    {
        var rootVec3 = API.GetPedBoneCoords(API.PlayerPedId(), (int)pBones.SKEL_ROOT, 0, 0, 0);
        var headVec3 = API.GetPedBoneCoords(API.PlayerPedId(), (int)pBones.SKEL_Head, 0, 0, 0);
        headVec3.Z += 0.05f;
        var tagVec = headVec3;
        tagVec.Z += 0.5f;
        var lineTopVec = headVec3;
        lineTopVec.Z += 500f;
            
        Vector3 bottom = new Vector3(rootVec3.X, rootVec3.Y, rootVec3.Z - 1.1f);
        Vector3 top = new Vector3(rootVec3.X, rootVec3.Y, rootVec3.Z + 0.9f);

        PointF top_pt = Rendering.SafeW2S(top);
        PointF bot_pt = Rendering.SafeW2S(bottom);
            
        Rendering.DrawText3ds(tagVec, $"Cheater");

        var height = Math.Abs(top_pt.Y - bot_pt.Y);
        var width = height / 4;
        API.DrawRect(top_pt.X,top_pt.Y + (height / 2), width, height,255, 0,0,50);
        Rendering.Draw3dSpaceLine(headVec3,lineTopVec);
    }
}