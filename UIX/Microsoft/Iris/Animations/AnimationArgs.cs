// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.AnimationArgs
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Animations
{
    internal struct AnimationArgs
    {
        public ViewItem ViewItem;
        public Vector3 OldPosition;
        public Vector2 OldSize;
        public Vector3 OldScale;
        public Rotation OldRotation;
        public float OldAlpha;
        public Vector3 NewPosition;
        public Vector2 NewSize;
        public Vector3 NewScale;
        public Rotation NewRotation;
        public float NewAlpha;
        public Vector3 OldEye;
        public Vector3 OldAt;
        public Vector3 OldUp;
        public float OldZn;
        public Vector3 NewEye;
        public Vector3 NewAt;
        public Vector3 NewUp;
        public float NewZn;

        public AnimationArgs(Camera cam)
        {
            this.ViewItem = null;
            this.OldPosition = Vector3.Zero;
            this.OldSize = Vector2.Zero;
            this.OldScale = Vector3.Zero;
            this.OldRotation = Rotation.Default;
            this.OldAlpha = 0.0f;
            this.NewPosition = Vector3.Zero;
            this.NewSize = Vector2.Zero;
            this.NewScale = Vector3.Zero;
            this.NewRotation = Rotation.Default;
            this.NewAlpha = 0.0f;
            this.OldEye = cam.Eye;
            this.NewEye = cam.Eye;
            this.OldAt = cam.At;
            this.NewAt = cam.At;
            this.OldUp = cam.Up;
            this.NewUp = cam.Up;
            this.OldZn = cam.Zn;
            this.NewZn = cam.Zn;
        }

        public AnimationArgs(
          ViewItem vi,
          Vector3 oldPosition,
          Vector2 oldSize,
          Vector3 oldScale,
          Rotation oldRotation,
          float oldAlpha,
          Vector3 newPosition,
          Vector2 newSize,
          Vector3 newScale,
          Rotation newRotation,
          float newAlpha)
        {
            this.ViewItem = vi;
            this.OldPosition = oldPosition;
            this.OldSize = oldSize;
            this.OldScale = oldScale;
            this.OldRotation = oldRotation;
            this.OldAlpha = oldAlpha;
            this.NewPosition = newPosition;
            this.NewSize = newSize;
            this.NewScale = newScale;
            this.NewRotation = newRotation;
            this.NewAlpha = newAlpha;
            this.OldEye = Vector3.Zero;
            this.NewEye = Vector3.Zero;
            this.OldAt = Vector3.Zero;
            this.NewAt = Vector3.Zero;
            this.OldUp = Vector3.Zero;
            this.NewUp = Vector3.Zero;
            this.OldZn = 0.0f;
            this.NewZn = 0.0f;
        }

        public AnimationArgs(
          ViewItem vi,
          Vector3 oldPosition,
          Vector2 oldSize,
          Vector3 oldScale,
          Rotation oldRotation,
          float oldAlpha)
          : this(vi, oldPosition, oldSize, oldScale, oldRotation, oldAlpha, oldPosition, oldSize, oldScale, oldRotation, oldAlpha)
        {
        }

        public AnimationArgs(ViewItem vi)
          : this(vi, vi.VisualPosition, vi.VisualSize, vi.VisualScale, vi.VisualRotation, vi.VisualAlpha, vi.VisualPosition, vi.VisualSize, vi.VisualScale, vi.VisualRotation, vi.VisualAlpha)
        {
        }
    }
}
