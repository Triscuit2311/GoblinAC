using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Goblin.Client.Countermeasures;

internal class Rendering : BaseScript
{
    internal static PointF SafeW2S(Vector3 pt3d)
    {
        var screenX = 0.0f;
        var screenY = 0.0f;
        API.World3dToScreen2d(pt3d.X, pt3d.Y, pt3d.Z, ref screenX, ref screenY);
        return new PointF(screenX, screenY);
    }

    internal static void Draw3dSpaceLine(Vector3 origin, Vector3 destination)
    {
        API.DrawLine(origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 255, 0, 0, 255);
    }

    internal static void Draw3dSpaceSphere(Vector3 origin, float size, Color color)
    {
        API.DrawSphere(origin.X, origin.Y, origin.Z, size, color.R,color.G,color.B,color.A);
    }

    internal static void DrawText3ds(Vector3 pos, string msg, bool centered = true)
    {
        var screenPos = Screen.WorldToScreen(pos);
        var text = new Text(msg,
            screenPos,
            0.35f);
        text.Centered = centered;
        text.Draw();
    }

}